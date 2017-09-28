using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideableObject : MonoBehaviour 
{
	public Vector3 UnHidedSize;
	public float time = 1;
	bool Hide = false;
	RectTransform rTransform;

	void Start()
	{
		rTransform = GetComponent<RectTransform>();
	}

	void Update()
	{
		if(Hide)
		{
			rTransform.sizeDelta = Vector3.Lerp(rTransform.sizeDelta, Vector3.zero, Time.unscaledDeltaTime / time);
		} else 
		{
			rTransform.sizeDelta = Vector3.Lerp(rTransform.sizeDelta, UnHidedSize, Time.unscaledDeltaTime / time);
		}
	}

	public void Use()
	{
		Hide = !Hide;
	}
    public void Use(bool hide)
    {
        Hide = hide;
    }
}
