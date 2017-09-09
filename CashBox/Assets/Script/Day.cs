using UnityEngine;
using UnityEngine.UI;

public class Day : MonoBehaviour
{
    public Text TimeText;
    private int Hours;
    private int Minutes;
    private float Seconds;
	
    void Update()
    {
        UpdateTimer();
        UpdateRotation();
        SetTimeTextValue();
    }

    void UpdateTimer()
    {
        Seconds += Time.deltaTime;
        if (Seconds >= 60)
        {
            Seconds -= 60;
            Minutes++;
        }
        if (Minutes >= 60)
        {
            Minutes -= 60;
            Hours++;
        }
        if (Hours >= 24)
        {
            Hours -= 24;
        }
    }
    void UpdateRotation()
    {
        float TimeInHours = Hours + (Minutes / 60f) + (Seconds / 3600f);
        float RotX = ((TimeInHours / 24f) * 360f) - 90f;
        float RotY = transform.rotation.eulerAngles.y;
        float RotZ = transform.rotation.eulerAngles.z;
        transform.rotation = Quaternion.Euler(RotX, RotY, RotZ);
    }
    void SetTimeTextValue()
    {
        string Time = "";
        if(Hours >= 10)
        {
            Time += Hours.ToString();
        } else
        {
            Time += "0" + Hours.ToString();
        }
        Time += ":";
        if (Minutes >= 10)
        {
            Time += Minutes.ToString();
        }
        else
        {
            Time += "0" + Minutes.ToString();
        }
        Time += ":";
        if (Seconds >= 10)
        {
            Time += ((int)Seconds).ToString();
        }
        else
        {
            Time += "0" + ((int)Seconds).ToString();
        }
        TimeText.text = Time;
    }
}
