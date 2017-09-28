using UnityEngine;
using UnityEngine.UI;

public class Day : MonoBehaviour
{
    public Text TimeText;
    Shop shop;

    private Vector3 StartRotation;

    void Start()
    {
        shop = FindObjectOfType<Shop>();
        StartRotation = transform.eulerAngles;
    }
    void Update()
    {
        UpdateRotation();
        SetTimeTextValue();
    }

    void UpdateRotation()
    {
        float TimeInHours = shop.info.Hours + (shop.info.Minutes / 60f) + (shop.info.Seconds / 3600f);
        float AngleX = ((TimeInHours / 24f) * 360f) - 90f;
        float RotX = AngleX; //0-90 -> Normal
        float RotY = StartRotation.y;
        float RotZ = StartRotation.z;
        if(RotX >= 90)
        {
            RotX = 90 - (RotX - 90);
            RotY = StartRotation.y - 180;
        }
        transform.eulerAngles = new Vector3(RotX, RotY, RotZ);
    }
    void SetTimeTextValue()
    {
        Shop shop = FindObjectOfType<Shop>();
        if (shop != null)
        {
            string Time = "";
            if (shop.info.Hours >= 10)
            {
                Time = shop.info.Hours.ToString();
            }
            else
            {
                Time = "0" + shop.info.Hours.ToString();
            }
            Time += ":";
            if (shop.info.Minutes >= 10)
            {
                Time += shop.info.Minutes.ToString();
            }
            else
            {
                Time += "0" + shop.info.Minutes.ToString();
            }
            Time += ":";
            if (shop.info.Seconds >= 10)
            {
                Time += ((int)shop.info.Seconds).ToString();
            }
            else
            {
                Time += "0" + ((int)shop.info.Seconds).ToString();
            }
            TimeText.text = Time;
        }
    }
}
