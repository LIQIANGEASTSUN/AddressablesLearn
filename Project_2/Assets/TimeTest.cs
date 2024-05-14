using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        System.DateTime dtDateTime = new System.DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
        System.DateTime nowDataTime = System.DateTime.UtcNow;

        TimeSpan timeSpan = nowDataTime - dtDateTime;
        double second = timeSpan.TotalSeconds;
        Debug.LogError("Second:" + second);

        second += 3600;
        dtDateTime = dtDateTime.AddSeconds(second).ToLocalTime();

        Debug.LogError(dtDateTime.Year + "   " + dtDateTime.Month + "    " + dtDateTime.Day + "   " + dtDateTime.Hour + "    " + dtDateTime.Minute);

        //TimeSpan timeSpine = new TimeSpan(0, 0, 3600);
        //Debug.LogError(timeSpine)
    }

    /*

    */

    // Update is called once per frame
    void Update()
    {
        
    }
}
