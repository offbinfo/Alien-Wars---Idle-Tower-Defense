using UnityEngine;
using TMPro;

public class PurchaserPrice : MonoBehaviour
{
    TextMeshProUGUI pricetText;

    private void Awake ()
    {
        pricetText = GetComponent<TextMeshProUGUI> ();
    }
    public void Setup (string id)
    {
        pricetText.text = Purchaser.Instance.GetPriceString (id);
    }
}
