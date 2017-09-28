using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Work In Progress
public class BuyPoint : MonoBehaviour
{
    public float Length = 5;
	
    void OnDrawGizmos()
    {
        Vector3 StartPoint = transform.TransformPoint(new Vector3(Length / -2, 0, 0));
        Vector3 EndPoint = transform.TransformPoint(new Vector3(Length / 2, 0, 0));
        Gizmos.color = Color.red;
        Gizmos.DrawLine(StartPoint, EndPoint);
    }

	public Vector3 GetRandomPoint ()
    {
		return transform.TransformPoint(new Vector3(Random.Range(Length / -2, Length / 2), 0, 0));
    }
}
