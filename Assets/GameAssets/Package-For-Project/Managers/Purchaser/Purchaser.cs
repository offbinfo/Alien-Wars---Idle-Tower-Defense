using System;
using UnityEngine;
using UnityEngine.Purchasing;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine.Purchasing.Extension;
using System.IO;
//using AppsFlyerSDK;
using System.Collections.Generic;



#if UNITY_EDITOR
using UnityEditor.SceneManagement;
using UnityEditor;
#endif

[AddComponentMenu ("Unity IAP/Purchase Script")]
public class Purchaser : MonoBehaviour, IDetailedStoreListener
{
    private static IStoreController m_StoreController;          // The Unity Purchasing system.
    private static IExtensionProvider m_StoreExtensionProvider; // The store-specific Purchasing subsystems.

    public static Purchaser Instance;

    string[] allProductID_NoneConsume = null;
    string[] allProductID = null;

    Action<string> purchaseSuccess = null;

    string title;

    [RuntimeInitializeOnLoadMethod]
    static void CreateInstance()
    {
        if (!Instance)
        {
            var settings = Resources.Load<PurchaserSettings>("PurchaserSettings");
            if (settings.activate)
                new GameObject("Purchaser-System", typeof(Purchaser));
        }
    }

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;

            if (m_StoreController == null)
            {
                InitializeGameService();
                InitializePurchasing();
            }

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeGameService()
    {
        var options = new InitializationOptions().SetEnvironmentName(Application.productName);
        UnityServices.InitializeAsync(options).ContinueWith(task => OnInitialized());

        void OnInitialized()
        {
            Debug.Log("Unity game service initialized");
        }
    }

    private void InitializePurchasing()
    {
        // If we have already connected to Purchasing ...
        if (IsInitialized())
        {
            // ... we are done here.
            return;
        }

        // Create a builder, first passing in a suite of Unity provided stores.
        ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        // Add productID
        PurchaserSettings purchaserSettings = Resources.Load<PurchaserSettings>("PurchaserSettings");
        title = purchaserSettings.title;
        allProductID_NoneConsume = purchaserSettings.AllProductID_NoneConsume;
        allProductID = purchaserSettings.AllProductID;

        for (int i = 0; i < allProductID_NoneConsume.Length; i++)
        {
            string id = allProductID_NoneConsume[i];
#if UNITY_ANDROID
            builder.AddProduct(id, ProductType.Consumable);
#else
            builder.AddProduct (id, ProductType.NonConsumable);
#endif
        }

        for (int i = 0; i < allProductID.Length; i++)
        {
            string id = allProductID[i];
            builder.AddProduct(id, ProductType.Consumable);
        }


        // Kick off the remainder of the set-up with an asynchrounous call, passing the configuration
        // and this class' instance. Expect a response either in OnInitialized or OnInitializeFailed.
        UnityPurchasing.Initialize(this, builder);
    }

    private bool IsInitialized()
    {
        // Only say we are initialized if both the Purchasing references are set.

        return m_StoreController != null && m_StoreExtensionProvider != null;
    }

    public string GetPriceString(string id)
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
            return "No internet";
        if (string.IsNullOrEmpty(id))
            return "None priceBuild";

        id = title + "." + id;

        return IsInitialized() ? m_StoreController.products.WithID(id).metadata.localizedPriceString : "Oops!";
    }

    public string GetPriceCurrency(string id)
    {
        id = title + "." + id;
        return IsInitialized() ? m_StoreController.products.WithID(id).metadata.isoCurrencyCode : "Oops!";
    }

    //Mua san pham thong qua id san pham
    public void BuyProductID(string productId, Action<string> purchaseSuccess)
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            purchaseSuccess?.Invoke(null);
            return;
        }

        productId = title + "." + productId;

        this.purchaseSuccess = purchaseSuccess;

        // If Purchasing has been initialized ...
        if (IsInitialized())
        {
            // ... look up the Product reference with the general product identifier and the Purchasing
            // system's products collection.
            Product product = m_StoreController.products.WithID(productId);

            // If the look up found a product for this device's store and that product is ready to be sold ...
            if (product != null && product.availableToPurchase)
            {
                Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                // ... buy the product. Expect a response either through ProcessPurchase or OnPurchaseFailed
                // asynchronously.
                m_StoreController.InitiatePurchase(product);
            }
            // Otherwise ...
            else
            {
                // ... report the product look-up failure situation
                this.purchaseSuccess(null);
                Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
            }
        }
        // Otherwise ...
        else
        {
            // ... report the fact Purchasing has not succeeded initializing yet. Consider waiting longer or
            // retrying initiailization.
            this.purchaseSuccess(null);
            InitializePurchasing();
            Debug.Log("BuyProductID FAIL. Not initialized.");
        }
    }

    // Restore purchases previously made by this customer. Some platforms automatically restore purchases, like Google.
    // Apple currently requires explicit purchase restoration for IAP, conditionally displaying a password prompt.
    public void RestorePurchases(Action<string> purchaseSuccess, Action<bool> onComplete)
    {
        // If Purchasing has not yet been set up ...
        this.purchaseSuccess = purchaseSuccess;
        if (!IsInitialized())
        {
            // ... report the situation and stop restoring. Consider either waiting longer, or retrying initialization.
            Debug.Log("RestorePurchases FAIL. Not initialized.");
            onComplete?.Invoke(false);
            return;
        }

        // If we are running on an Apple device ...
        if (Application.platform == RuntimePlatform.IPhonePlayer ||
            Application.platform == RuntimePlatform.OSXPlayer)
        {
            // ... begin restoring purchases
            Debug.Log("RestorePurchases started ...");

            // Fetch the Apple store-specific subsystem.
            var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
            // Begin the asynchronous process of restoring purchases. Expect a confirmation response in
            // the CallHellFire<bool> below, and ProcessPurchase if there are previously purchased products to restore.
            apple.RestoreTransactions((result, s) =>
           {
                // The first phase of restoration. If no more responses are received on ProcessPurchase then
                // no purchases are available to be restored.

                onComplete?.Invoke(result);
               Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
           });
        }
        // Otherwise ...
        else
        {
            // We are not running on an Apple device. No work is necessary to restore purchases.
            onComplete?.Invoke(false);
            Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
        }
    }

    //
    // --- IStoreListener
    //

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        // Purchasing has succeeded initializing. Collect our Purchasing references.
        Debug.Log("OnInitialized: PASS");

        // Overall Purchasing system, configured with products for this application.
        m_StoreController = controller;
        // Store specific subsystem, for accessing device-specific store features.
        m_StoreExtensionProvider = extensions;

        var text = "";

        PurchaserSettings purchaserSettings = Resources.Load<PurchaserSettings>("PurchaserSettings");
        var allProductID_NoneConsume = purchaserSettings.allProductID_NoneConsume;
        var allProductID = purchaserSettings.allProductID;

        for (int i = 0; i < allProductID_NoneConsume.Length; i++)
        {
            string id = allProductID_NoneConsume[i];
            if (string.IsNullOrEmpty(GetPriceString(id)))
                text += id + "\n";
        }

        for (int i = 0; i < allProductID.Length; i++)
        {
            string id = allProductID[i];
            if (string.IsNullOrEmpty(GetPriceString(id)))
                text += id + "\n";
        }

        if (!string.IsNullOrEmpty(text))
        {
            GUIUtility.systemCopyBuffer = text;
        }
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        // Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        var product = args.purchasedProduct;

        purchaseSuccess?.Invoke(product.definition.id.Replace(title + ".", ""));

/*#if !UNITY_EDITOR
        ValidatePurchaseWithAppsFlyer (product);
#endif*/

        // Return a flag indicating whether this product has completely been received, or if the application needs
        // to be reminded of this purchase at next app launch. Use PurchaseProcessingResult.Pending when still
        // saving purchased products to the cloud, and when that save is delayed.
        return PurchaseProcessingResult.Complete;
    }
/*    private void ValidatePurchaseWithAppsFlyer (Product product)
    {
#if UNITY_ANDROID
        var googleMeta = product.metadata.GetGoogleProductMetadata ();
        var price = googleMeta.localizedPrice.ToString ().Trim ();
        var currency = googleMeta.isoCurrencyCode.Trim ();

        string receipt = product.receipt.Trim ();
        var recptToJSON = (Dictionary<string, object>)AFMiniJSON.Json.Deserialize (product.receipt);
        var receiptPayload = (Dictionary<string, object>)AFMiniJSON.Json.Deserialize ((string)recptToJSON ["Payload"]);
        var purchaseData = ((string)receiptPayload ["json"]).Trim ();
        var signature = ((string)receiptPayload ["signature"]).Trim ();
    #region google key
        var googlePublicKey = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAqvTKTgw0bEQaH5Bzy+k8BkNVfUvbEIEVUeUWMmZk74B3jyNvJq2IRSQtyFRcAqoEd1p4xT6Z7LDXpN8Br0MdApUG9rqb8vOF7Cpp639ZbWQva+C5iyzucB/F7J+Etet4ENsJVbwphRpfFvhGPRuRQ5NRJmVqhn1hGd5PxeC8oq2We2r/JZv4+ANH+4/W+wkrPvAQQM56CcxVFwWrrLfGbdTYBT4iEVgm5kJnouQDx6dldXvuHXtjxlmeFzXZ1XYwNwC8mRpGPZlv0+0DFuLKqz/fQ37LZrUKupS7FYcN3zawyfGL4ePy7vQ6eODfQ/ivPhh4OYeprVBoDElGEtqryQIDAQAB";
    #endregion
        AppsFlyer.validateAndSendInAppPurchase (googlePublicKey, signature, purchaseData, price, currency, null, this);

#elif UNITY_IOS
        var appleMeta = product.metadata.GetAppleProductMetadata ();
        var id = product.definition.id.Trim ();
        var transactionId = product.transactionID.Trim ();
        var price = appleMeta.localizedPrice.ToString ().Trim ();
        var currency = appleMeta.isoCurrencyCode.Trim ();

        AppsFlyer.setUseReceiptValidationSandbox (true);
        AppsFlyer.validateAndSendInAppPurchase (id, price, currency, transactionId, null, this);
#endif
    }*/

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        purchaseSuccess?.Invoke(null);
        // A product purchase attempt did not succeed. Check failureReason for more detail. Consider sharing
        // this reason with the user to guide their troubleshooting actions.
        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
    {
        purchaseSuccess?.Invoke(null);
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
    }
}

#if UNITY_EDITOR
public static class PurchaseMenu
{
    [MenuItem("GameObject/IAP")]
    public static void CreateIAP()
    {
        Selection.activeGameObject = new GameObject("IAP", typeof(Purchaser));
        EditorSceneManager.MarkAllScenesDirty();
    }
}

[CustomEditor(typeof(Purchaser))]
public class PurchaserEditor : Editor
{
    PurchaserSettings purchaserSettings = null;
    void Awake()
    {
        purchaserSettings = Resources.Load<PurchaserSettings>("PurchaserSettings");
        if (!purchaserSettings)
        {
            Debug.Log("Create resources");

            MonoScript monoScript = MonoScript.FromMonoBehaviour(target as Purchaser);
            string file = AssetDatabase.GetAssetPath(monoScript.GetInstanceID());
            string path = file.Replace("/" + monoScript.name + ".cs", "");
            if (!AssetDatabase.IsValidFolder(path + "/Resources"))
            {
                path = AssetDatabase.CreateFolder(path, "Resources");
                path = AssetDatabase.GUIDToAssetPath(path);
                AssetDatabase.Refresh();
            }
            else
                path = path + "/Resources";

            purchaserSettings = CreateInstance<PurchaserSettings>();
            AssetDatabase.CreateAsset(purchaserSettings, path + "/PurchaserSettings.asset");
            AssetDatabase.SaveAssets();
        }
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Settings"))
        {
            Selection.activeObject = purchaserSettings;
        }
    }
}

public class PurchaserMenu
{
    [MenuItem("Project-Tools/IAP Settings")]
    public static void OpenSettings()
    {
        PurchaserSettings purchaserSettings = Resources.Load<PurchaserSettings>("PurchaserSettings");

        if (!purchaserSettings)
        {
            var parentFolder = "Assets";
            var resourcesFolderName = "Resources";
            var resourcesFolder = Path.Combine(parentFolder, resourcesFolderName);

            if (!AssetDatabase.IsValidFolder(resourcesFolder))
            {
                AssetDatabase.CreateFolder(parentFolder, resourcesFolderName);
            }

            purchaserSettings = ScriptableObject.CreateInstance<PurchaserSettings>();
            AssetDatabase.CreateAsset(purchaserSettings, Path.Combine(resourcesFolder, "PurchaserSettings.asset"));
        }

        Selection.activeObject = purchaserSettings;
    }
}
#endif
