using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerUiController : MonoBehaviour
{
    GameObject TimerImage;//
    
    // Start is called before the first frame update
    void Start()
    {
        TimerImage = GameObject.Find("TimerIcon");//ClockImageの読み込み
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TimerCount (float TimeValue, float TimeLimit) 
    {
    TimerImage.GetComponent<Image>().fillAmount = TimeValue / TimeLimit;
    }

}
