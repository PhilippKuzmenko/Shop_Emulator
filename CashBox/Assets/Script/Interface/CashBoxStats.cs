using UnityEngine;
using UnityEngine.UI;

public class CashBoxStats : MonoBehaviour
{
    public Text Title;
    public Text ProductsDone;
    public Text TotalBuyers;
    public Text MaxQueue;
    public Text WorkTime;
    public Text SleepTime;
    public Text BuyersPerMinute;
    public Text EfficiencyRate;

    public LocalizableText TitleText;
    public LocalizableText ProductsDoneText;
    public LocalizableText TotalBuyersText;
    public LocalizableText MaxQueueText;
    public LocalizableText WorkTimeText;
    public LocalizableText SleepTimeText;
    public LocalizableText BuyersPerMinuteText;
    public LocalizableText EfficiencyRateText;

    Shop shop;

    void Start()
    {
        FindShopIfRequired();
    }
    public void UpdateStats(int cashBoxIndex, int productsDone, int totalBuyers, int maxQueue, int workTime, int sleepTime, float buyersPerMinute, float efficiencyRate)
    {
        FindShopIfRequired();
        Title.text = TitleText.GetText(shop.changedLanguage) + cashBoxIndex;
        ProductsDone.text = ProductsDoneText.GetText(shop.changedLanguage) + productsDone.ToString();
        TotalBuyers.text = TotalBuyersText.GetText(shop.changedLanguage) + totalBuyers.ToString();
        MaxQueue.text = MaxQueueText.GetText(shop.changedLanguage) + maxQueue.ToString();
        WorkTime.text = WorkTimeText.GetText(shop.changedLanguage) + shop.PrintTime(workTime);
        SleepTime.text = SleepTimeText.GetText(shop.changedLanguage) + shop.PrintTime(sleepTime);
        BuyersPerMinute.text = BuyersPerMinuteText.GetText(shop.changedLanguage) + buyersPerMinute.ToString();
        EfficiencyRate.text = EfficiencyRateText.GetText(shop.changedLanguage) + efficiencyRate.ToString() + "%";
    }

    void FindShopIfRequired()
    {
        if (shop == null)
        {
            shop = FindObjectOfType<Shop>();
        }
    }
}
