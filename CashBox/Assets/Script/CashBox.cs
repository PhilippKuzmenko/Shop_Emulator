using UnityEngine;
using System.Collections.Generic;

public class CashBox : MonoBehaviour
{
    public float ProductsPerMinute;
    public List<Human> Queue;
	
    void Update()
    {
        Work();
    }
    void Work()
    {
        if(Queue.Count > 0)
        {
            Queue[0].Products -= ProductsPerMinute / 60 * Time.deltaTime;
            if(Queue[0].Products <= 0)
            {
                Queue[0].GoToHome();
                Queue.RemoveAt(0);
            }
        }
    }
}
