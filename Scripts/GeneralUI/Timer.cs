using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public float timevalue = 90;
    [SerializeField] Text timeText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(timevalue > 0)
        {
            timevalue -= Time.deltaTime;
        }
        else
        {
            timevalue = 0;
        }
        DisplayTime(timevalue);
    }
    void DisplayTime(float timetoDislay)
    {
        if(timetoDislay < 0)
        {
            timetoDislay = 0;
        }
     float minutes = Mathf.FloorToInt(timetoDislay / 60);
     float seconds = Mathf.FloorToInt(timetoDislay % 60);
     timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }    
}
