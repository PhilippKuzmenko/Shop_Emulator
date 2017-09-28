using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class ShopSettings
{
	public float MinTimeToNewBuyer = 30;
	public float MaxTimeToNewBuyer = 60 * 10;
    public float MinChangeTime = 60;
    public float MaxChangeTime = 60 * 15;
	public int MinProducts = 5;
	public int MaxProducts = 45;
	[Header("Time")]
	public int StartHour = 7;
	public int EndHour = 20;
	public float TimeScale = 1;
}
public enum ShopStatus
{
    Open,
    Closing,
    Closed
}
public class ShopInfo
{
    [Header("Time")]
    public int Hours;
    public int Minutes;
    public float Seconds;
    [Header("Timers")]
    public float TimeToNewBuyer = 1;
    [Header("Main")]
    public bool Pause = true;
    public ShopStatus Status;
}
public enum Language
{
    English,
    Russian
}
[System.Serializable]
public class LocalizableText
{
    [TextArea]
    public string onEnglish;
    [TextArea]
    public string onRussian;

    public LocalizableText(string english, string russian)
    {
        onEnglish = english;
        onRussian = russian;
    }

    public string GetText(Language language)
    {
        if(language == Language.English)
        {
            return onEnglish;
        } else if (language == Language.Russian)
        {
            return onRussian;
        } else
        {
            return "Unknown language!";
        }
    }
}
public class Shop : MonoBehaviour
{
    /*
    Баги:
    (Замечено в ускорении времени)
    -Покупатели могут застрявать между собой
    -Покупатель удаляется не успев выйти из магазина
    -В очередь добавляется один и тот же покупатель
    -Покупятель не удаляется из кассы(Missing)
    (Постоянно)
    -Когда время ~12:00:00 и более - мерцает солнце(Vector3 проблема)
    */
    [Header("Interface")]
    public HideableObject StatsWindow;
    public Transform SettingsContainer;
    public Transform StatsContainer;
    [Header("Objects")]
    public Human BuyerPref;
    public Transform ExitPoint;

    public Language changedLanguage = Language.Russian;
	public ShopSettings settings = new ShopSettings();
    public ShopInfo info = new ShopInfo();
    List<BuyPoint> BuyPoints = new List<BuyPoint>();
    List<CashBox> cashBoxes = new List<CashBox>();
    //Time local
    static LocalizableText hoursLocal = new LocalizableText("h.", "ч.");
    static LocalizableText minutesLocal = new LocalizableText("m.", "м.");
    static LocalizableText secondsLocal = new LocalizableText("s.", "с.");

    void Start()
    {
        BuyPoints.AddRange(FindObjectsOfType<BuyPoint>());
        cashBoxes.AddRange(FindObjectsOfType<CashBox>());
        UpdateDynamicSettings();
        UpdateCashBoxStats(true);
    }
    void Update()
    {
        BuyerSpawnModule();
        TimeModule();
        StatusUpdateModule();
        UpdateCashBoxStats(false);
    }

    void StatusUpdateModule()
    {
        if(info.Status == ShopStatus.Closed)
        {

        } else if(info.Status == ShopStatus.Closing && FindObjectOfType<Human>() == null)
        {
            info.Status = ShopStatus.Closed;
            settings.TimeScale = 0;
            StatsWindow.Use(false);
        } else if (info.Status == ShopStatus.Open && info.Hours >= settings.EndHour)
        {
            info.Status = ShopStatus.Closing;
        }
    }
    void BuyerSpawnModule()
    {
        if (info.Status == ShopStatus.Open)
        {
            if (info.TimeToNewBuyer <= 0f)
            {
                info.TimeToNewBuyer = GetRandomTimeToNewBuyer();
                Human buyer = Instantiate(BuyerPref, ExitPoint.position, Quaternion.identity);
                buyer.Initialize(GetRandomProductsCount(), GetRandomChangeTime());
            }
            info.TimeToNewBuyer -= Time.deltaTime * GetTimeScale();
        }
    }
    void TimeModule()
    {
        info.Seconds += Time.deltaTime * GetTimeScale();
        if (info.Seconds >= 60)
        {
            int count = (int)(info.Seconds / 60);
            info.Seconds -= 60 * count;
            info.Minutes+=count;
        }
        if (info.Minutes >= 60)
        {
            int count = (int)(info.Minutes / 60);
            info.Minutes -= 60 * count;
            info.Hours+=count;
        }
    }
    //
    public string GetLocalization(LocalizableText text)
    {
        return text.GetText(changedLanguage);
    }
    //
    public void SwapLanguages()
    {
        if(changedLanguage == Language.English)
        {
            changedLanguage = Language.Russian;
        } else
        {
            changedLanguage = Language.English;
        }
        List<SimpleTextTranslator> texts = new List<SimpleTextTranslator>();
        texts.AddRange(FindObjectsOfType<SimpleTextTranslator>());
        for(int i = 0; i < texts.Count; i++)
        {
            texts[i].UpdateLocalization();
        }
    }
    //
    public void NormalizeTime(int seconds, out int Hours, out int Minutes, out int Seconds)
    {
        Hours = seconds / 3600;
        Minutes = (seconds - (Hours * 3600)) / 60;
        Seconds = seconds - ((Hours * 3600) + (Minutes * 60));
    }
    public string PrintTime(int hours, int minutes, int seconds)
    {
        return hours.ToString("00") + GetLocalization(hoursLocal) + " " + minutes.ToString("00") + GetLocalization(minutesLocal) + " " + seconds.ToString("00") + GetLocalization(secondsLocal);
    }
    public string PrintTime(int Seconds)
    {
        int hours, minutes, seconds;
        NormalizeTime(Seconds, out hours, out minutes, out seconds);
        return hours.ToString() + GetLocalization(hoursLocal) + " " + minutes.ToString() + GetLocalization(minutesLocal) + " " + seconds.ToString() + GetLocalization(secondsLocal);
    }
    //
    public void UpdateDynamicSettings()
    {
        List<int> DynamicIndexes = new List<int>();
        for(int i = 0; i < SettingsContainer.childCount; i++)
        {
            if(SettingsContainer.GetChild(i).GetComponent<DynamicSetting>() != null)
            {
                DynamicIndexes.Add(i);
            }
        }
        //Delete
        for (int i = 0; i < DynamicIndexes.Count; i++)
        {
            Destroy(SettingsContainer.GetChild(i).gameObject);
        }
        //
        for(int i = 0; i < cashBoxes.Count; i++)
        {
            DynamicSetting dynamicSetting = Instantiate(Resources.Load<DynamicSetting>("Prefabs/CashBoxSetting"));
            //Event initialize
            dynamicSetting.inputField.onValueChanged.AddListener(cashBoxes[i].SetPPM); //Internet help me :) (Delegate)
            dynamicSetting.inputField.gameObject.AddComponent<StartValueApplyer>(); //Костыль?
            //Visual initialize
            dynamicSetting.transform.SetParent(SettingsContainer);
            dynamicSetting.transform.localScale = Vector3.one;
        }
    }
    public void UpdateCashBoxStats(bool SpawnNew)
    {
        if(SpawnNew)
        {
            //Delete
            for (int i = 0; i < StatsContainer.childCount; i++)
            {
                Destroy(StatsContainer.GetChild(i).gameObject);
            }
            //Spawn
            for (int i = 0; i < cashBoxes.Count; i++)
            {
                CashBoxStats stats = Instantiate(Resources.Load<CashBoxStats>("Prefabs/CashBoxStats"));
                //Visual initialize
                stats.transform.SetParent(StatsContainer);
                stats.transform.localScale = Vector3.one;
            }  
        }
        for (int i = 0; i < StatsContainer.childCount; i++)
        {
            CashBoxStats stats = StatsContainer.GetChild(i).GetComponent<CashBoxStats>();
            if(stats != null && cashBoxes.Count > i)
            {
                stats.UpdateStats(i, (int)cashBoxes[i].Stats.ProductsDone, cashBoxes[i].Stats.NumberOfBuyers, cashBoxes[i].Stats.MaxQueueCount, (int)cashBoxes[i].Stats.WorkTime, (int)cashBoxes[i].Stats.SleepTime, cashBoxes[i].Stats.BuyersPerMinute(), cashBoxes[i].Stats.EfficiencyRate());
            }
        }
    }
    //
    public void SetMinTimeToNewBuyer(InputField Caller)
    {
        float.TryParse(Caller.text, out settings.MinTimeToNewBuyer);
    }
    public void SetMaxTimeToNewBuyer(InputField Caller)
    {
        float.TryParse(Caller.text, out settings.MaxTimeToNewBuyer);
    }
    public void SetMinChangeTime(InputField Caller)
    {
        float.TryParse(Caller.text, out settings.MinChangeTime);
    }
    public void SetMaxChangeTime(InputField Caller)
    {
        float.TryParse(Caller.text, out settings.MaxChangeTime);
    }
    public void SetMinProducts(InputField Caller)
    {
        int.TryParse(Caller.text, out settings.MinProducts);
    }
    public void SetMaxProducts(InputField Caller)
    {
        int.TryParse(Caller.text, out settings.MaxProducts);
    }
    public void SetStartHour(InputField Caller)
    {
        int.TryParse(Caller.text, out settings.StartHour);
        info.Hours = settings.StartHour;
        info.Minutes = 0;
        info.Seconds = 0;
    }
    public void SetEndHour(InputField Caller)
    {
        int.TryParse(Caller.text, out settings.EndHour);
    }
    public void SetTimeScale(InputField Caller)
    {
        float.TryParse(Caller.text, out settings.TimeScale);
    }
    //
    public float GetRandomTimeToNewBuyer()
    {
        return Random.Range(settings.MinTimeToNewBuyer, settings.MaxTimeToNewBuyer);
    }
    public float GetRandomChangeTime()
    {
        return Random.Range(settings.MinChangeTime, settings.MaxChangeTime);
    }
    public int GetRandomProductsCount()
    {
        return Random.Range(settings.MinProducts, settings.MaxProducts);
    }
    //
    public Vector3 GetRandomBuyPoint()
    {
        int Index = Random.Range(0, BuyPoints.Count);
        return BuyPoints[Index].GetRandomPoint();
    }
    public CashBox GetOptimalCashBox()
    {
        int OptimalIndex = 0; 
        for(int i = 0; i < cashBoxes.Count; i++)
        {
            if(cashBoxes[i].Queue.Count < cashBoxes[OptimalIndex].Queue.Count)
            {
                OptimalIndex = i;
            }
        }
        return cashBoxes[OptimalIndex];
    }
    //
    public float GetTimeScale()
    {
        if (info.Pause)
        {
            return 0;
        } else
        {
            return settings.TimeScale;
        }
    }
}
