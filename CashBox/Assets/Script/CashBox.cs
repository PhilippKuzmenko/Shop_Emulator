using UnityEngine;
using System.Collections.Generic;

public class cashBoxStats
{
    public float ProductsDone;
    public int NumberOfBuyers;
    public int MaxQueueCount;
    //
    public float WorkTime;
    public float SleepTime;

    public float BuyersPerMinute()
    {
        return NumberOfBuyers / ((WorkTime + SleepTime) / 60); //60 / (120 / 60) = 60 / 2 = 30
    }
    public float EfficiencyRate()
    {
        return WorkTime / (WorkTime + SleepTime) * 100f; //7 / (7 + 3) * 100 = 7 / 10 * 100 = 0.7 * 100 = 70%
    }
}
public class CashBox : MonoBehaviour
{
    public float ProductsPerMinute = 10;
    public List<Human> Queue;
    public cashBoxStats Stats = new cashBoxStats();
    private Shop shop;

    void Start()
    {
        shop = FindObjectOfType<Shop>();
    }
    public void SetPPM(string Value)
    {
        float.TryParse(Value, out ProductsPerMinute);
    }
    void Update()
    {
        Work();
    }
    void Work()
    {
        if (Queue.Count > 0)
        {
            Stats.WorkTime += Time.deltaTime * shop.GetTimeScale();
            if(Queue.Count > Stats.MaxQueueCount)
            {
                Stats.MaxQueueCount = Queue.Count;
            }
            //
            float productCount = ProductsPerMinute / 60 * Time.deltaTime * shop.GetTimeScale();
            Queue[0].Products -= productCount;
            //
            Stats.ProductsDone += productCount;
            //
            if (Queue[0].Products <= 0)
            {
                Stats.NumberOfBuyers++;
                Queue[0].GoToHome();
                Queue.RemoveAt(0);
            }
        }
        else
        {
            Stats.SleepTime += Time.deltaTime * shop.GetTimeScale();
        }
    }
}
