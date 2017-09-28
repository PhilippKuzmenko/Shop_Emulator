using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(InputField))]
public class StartValueApplyer : MonoBehaviour 
{
	InputField field;

	void Start()
	{
		field = GetComponent<InputField>();
        field.onValueChanged.Invoke(field.text);
        Destroy(this);
    }
}
