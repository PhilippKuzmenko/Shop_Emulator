using UnityEngine;
using UnityEngine.AI;

public enum BuyerStay
{
    ChangeProducts,
    GoToCashBox,
    GoToHome
}
[RequireComponent(typeof(NavMeshAgent))]
public class Human : MonoBehaviour
{
    public static float BuyPointSwithTime = 30f;
    public float Products;
    public float ChangeTime;

    private float startSpeed;
    private float startAngularSpeed;

    private Shop shop;
    private NavMeshAgent agent;
    
    private float BuyPointSwithTimeout = BuyPointSwithTime;
    private Vector3 BuyPointPosition;
    private CashBox ChangedCashBox;
    private Vector3 HomePosition;
    private BuyerStay Stay = BuyerStay.ChangeProducts;

    private bool WasInCashBox = false;
    
    public void Initialize(int ProductCount, float changeTime)
    {
        Products = ProductCount;
        ChangeTime = changeTime;
    }
    void Start()
    {
        shop = FindObjectOfType<Shop>().GetComponent<Shop>();
        agent = GetComponent<NavMeshAgent>();
        startSpeed = agent.speed;
        startAngularSpeed = agent.angularSpeed;
        GoToBuyPoint();
    }
    void Update()
    {
        agent.speed = startSpeed * shop.GetTimeScale();
        agent.angularSpeed = startAngularSpeed * shop.GetTimeScale();
        if (GetTargetComplated(2f, BuyPointPosition) && Stay == BuyerStay.ChangeProducts)
        { 
            if (ChangeTime < 0f)
            {
                ChangedCashBox = shop.GetOptimalCashBox();
                agent.destination = ChangedCashBox.transform.position;
                Stay = BuyerStay.GoToCashBox;
                return;
            }
            if(BuyPointSwithTimeout < 0f)
            {
                GoToBuyPoint();
                BuyPointSwithTimeout = BuyPointSwithTime;
            }
            ChangeTime -= Time.deltaTime * shop.GetTimeScale();
            BuyPointSwithTimeout -= Time.deltaTime * shop.GetTimeScale();
        } else if(Stay == BuyerStay.GoToCashBox && !WasInCashBox && GetTargetComplated(2f, ChangedCashBox.transform.position))
        {
            ChangedCashBox.Queue.Add(this);
            WasInCashBox = true;
        } else if(Stay == BuyerStay.GoToHome && GetTargetComplated(2f, HomePosition))
        {
            Destroy(gameObject);
        }
    }

    public bool GetTargetComplated(float ComplateDistance, Vector3 Target)
    {
        return (Vector3.Distance(transform.position, Target) <= ComplateDistance) && (agent.remainingDistance <= ComplateDistance) && Vector3.Distance(agent.destination, Target) < 1.5f;
    }
    public void GoToHome()
    {
        HomePosition = shop.ExitPoint.position;
        agent.destination = HomePosition;
        Stay = BuyerStay.GoToHome;
    }
    void GoToBuyPoint()
    {
        BuyPointPosition = shop.GetRandomBuyPoint();
        agent.destination = BuyPointPosition;
    }
}
