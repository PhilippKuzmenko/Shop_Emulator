using UnityEngine;
using UnityStandardAssets.CinematicEffects;

[RequireComponent(typeof(DepthOfField))]
public class DepthOfFieldController : MonoBehaviour
{
    DepthOfField depth;

    void Start()
    {
        depth = GetComponent<DepthOfField>();
    }

	void Update ()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f))
        {
            depth.focus.focusPlane = Vector3.Distance(transform.position, hit.point);
        }
	}
}
