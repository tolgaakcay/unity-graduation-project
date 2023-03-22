using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StopWatch : MonoBehaviour
{
    public float timeStart=0;
    public Text textBox;

    bool timerActive = false;

    // Use this for initialization
    void Start()
    {
        textBox.text = ("Tahliye süresi:"+ timeStart.ToString("F2"));
    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] gameObjects;
        gameObjects = GameObject.FindGameObjectsWithTag("Agent");
        if (timerActive & gameObjects.Length!=0)
        {
            timeStart += Time.deltaTime;
            textBox.text = ("Tahliye süresi:" + timeStart.ToString("F2"));
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            timerActive = !timerActive;
        }
    }
    
}