using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class SimpleTextTranslator : MonoBehaviour
{
    public LocalizableText localizationText;
    Text text;
    Shop shop;

    void Start()
    {
        text = GetComponent<Text>();
        shop = FindObjectOfType<Shop>();
        UpdateLocalization();
    }

    public void UpdateLocalization()
    {
        if(shop != null)
        {
            text.text = shop.GetLocalization(localizationText);
        }
    }
}
