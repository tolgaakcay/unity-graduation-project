using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.UI;
using System.IO;


public class HeatmapManager : MonoBehaviour
{
    public float startTime;
    public float endTime;
    public string total_agent;

    public string heatmapDataPath = @"./heatmap_data.csv";

    public Text finalcounter;
    public Text textBox;

    // public void WriteHeatMapSourceData()
    // {
    //     StreamWriter sw = new StreamWriter(heatmapDataPath, true);
    //     string data = (this.transform.position.x).ToString("F0") + "," + (this.transform.position.z).ToString("F0");
    //     sw.WriteLine(data);
    //     sw.Flush();
    //     sw.Close();     
    // }

    // void Start()
    // {
    //     InvokeRepeating("WriteHeatMapSourceData", 2, 0.3F);
    // }

    // void Update()
    // {
    //     if (transform.position[0] >= 114.0 & transform.position[0] <= 114.5 || transform.position[0] >= 217.0 & transform.position[0] <= 217.5)
    //     {
            
    //         if (gameObject.tag == "Agent")
    //         {
    //             {
    //                 transform.gameObject.tag = "final";
    //                 GameObject[] gameObjects;
    //                 gameObjects = GameObject.FindGameObjectsWithTag("Agent");
    //                 finalcounter.text = ("Kalan aktor sayisi:"+gameObjects.Length.ToString()+"/"+total_agent);
    //                 endTime = Time.time;
    //                 CancelInvoke("WriteHeatMapSourceData");
    //             }
    //         }
    //     }
    // }


}
