using System;

public class GamePurchaser
{
    public static void BuyProduct(string id, Action callback)
    {
        Purchaser.Instance.BuyProductID(id, OnPurchased);
        void OnPurchased(string productID)
        {
            if (!string.IsNullOrEmpty(productID))
            {
                callback?.Invoke();
                GameDatas.SetBeginnerQuestProgress(BeginnerQuestID.FIRST_DEPOSIT, 1);
            }
        }
    }
}
