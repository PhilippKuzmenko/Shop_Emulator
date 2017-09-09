using UnityEngine;
using System.Collections;

public enum BuyerStay
{
    ChangeProducts,
    ChangeCashBox,
    GoToCashBox,
    GoToHome
}
[RequireComponent(typeof(NavMeshAgent))]
public class Human : MonoBehaviour
{
    public float Products = 10.0f;
    public float TimeToChange = 10.0f;

    private Shop shop;
    private NavMeshAgent NavMeshAg;
    private Transform BuyPoint;
    private CashBox ChangedCashBox;
    private BuyerStay Stay = BuyerStay.ChangeProducts;
    
    void Start()
    {
        shop = FindObjectOfType<Shop>().GetComponent<Shop>();
        NavMeshAg = GetComponent<NavMeshAgent>();
        //
        GoToBuyPoint();
    }
    void Update()
    {
        if(GetTargetComplated(2f) && Stay == BuyerStay.ChangeProducts)
        {
            if (TimeToChange > 0f)
            {
                TimeToChange -= Time.deltaTime;
            } else
            {
                Stay = BuyerStay.ChangeCashBox;
            }
        } else if(Stay == BuyerStay.ChangeCashBox)
        {
            ChangedCashBox = shop.GetOptimalCashBox();
            NavMeshAg.destination = ChangedCashBox.transform.position; 
            Stay = BuyerStay.GoToCashBox;
        } else if(Stay == BuyerStay.GoToCashBox && GetTargetComplated(2f))
        {
            ChangedCashBox.Queue.Add(this);
        } else if(Stay == BuyerStay.GoToHome)
        {
            NavMeshAg.destination = shop.ExitPoint.position;
            if(GetTargetComplated(2f))
            {
                Destroy(gameObject);
            }
        }
    }

    public bool GetTargetComplated(float ComplateDistance)
    {
        return NavMeshAg.remainingDistance <= ComplateDistance;
    }
    public void GoToHome()
    {
        Stay = BuyerStay.GoToHome;
    }
    void GoToBuyPoint()
    {
        BuyPoint = shop.GetRandomBuyPoint();
        NavMeshAg.destination = BuyPoint.position;
    }
}
