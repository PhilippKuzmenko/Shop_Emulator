using UnityEngine;
using System.Collections.Generic;

public class Shop : MonoBehaviour
{
    public Human BuyerPref;
    public List<CashBox> CashBoxes;
    public List<Transform> BuyPoints;
    public Transform ExitPoint;

    public float BuyersPerMinute;
    private float TimeToNewBuyer;

    void Update()
    {
        if(TimeToNewBuyer <= 0.0f)
        {
            TimeToNewBuyer = 60.0f / BuyersPerMinute;
            Instantiate(BuyerPref, ExitPoint.position, Quaternion.identity);
        }
        TimeToNewBuyer -= Time.deltaTime;
    }
    public Transform GetRandomBuyPoint()
    {
        int Index = Random.Range(0, BuyPoints.Count);
        return BuyPoints[Index];
    }
    public CashBox GetOptimalCashBox()
    {
        int OptimalIndex = 0; 
        for(int i = 0; i < CashBoxes.Count; i++)
        {
            if(CashBoxes[i].Queue.Count < CashBoxes[OptimalIndex].Queue.Count)
            {
                OptimalIndex = i;
            }
        }
        return CashBoxes[OptimalIndex];
    }
}
