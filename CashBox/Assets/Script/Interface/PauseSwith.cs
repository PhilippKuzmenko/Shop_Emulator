using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseSwith : MonoBehaviour
{
    public Text ButtonText;
    public string OnPauseText = "Start";
    public string OnStartText = "Pause";

	void Start ()
    {
        Shop shop = FindObjectOfType<Shop>();
        UpdateText(shop.info.Pause);
    }

    void UpdateText(bool Pause)
    {
        if(Pause)
        {
            ButtonText.text = OnPauseText;
        } else
        {
            ButtonText.text = OnStartText;
        }
    }
    public void SwithValue()
    {
        Shop shop = FindObjectOfType<Shop>();
        bool pause = !shop.info.Pause;
        shop.info.Pause = pause;
        UpdateText(pause);
    }
}
