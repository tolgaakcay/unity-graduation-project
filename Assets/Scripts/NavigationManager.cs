using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using UnityEngine.UI;
using System;
using System.Collections.Specialized;

public class NavigationManager : MonoBehaviour
{
    public Vector3 startPosition;
    public Vector3 random_destination_1;
    public Vector3 random_destination_2;
    public Vector3 random_destination_final;
    public float startTime;
    public float endTime;
    public int counter = 0;
    public float total_distance_1 = 0.0f;
    public float total_distance_2 = 0.0f;
    public string fileLocation = @".\coord_time.csv";
    public string total_agent;
    public Animator anim;
    private NavMeshAgent agent;
    public Text finalcounter;
    public Text textBox;
    private NavMeshPath path;
    public float distanceThreshold = 4.0f;
    public void WriteSimulationData()
    {
        StreamWriter sw = new StreamWriter(fileLocation, true);
        string data = (startPosition.x * 1000).ToString("F0") + "," + (startPosition.z * 1000).ToString("F0") + "," + (startPosition.y * 1000).ToString("F0") + "," + Math.Round((endTime-startTime)).ToString();
        sw.WriteLine(data);
        sw.Flush();
        sw.Close();       
    }
    void Start()
    {
        path = new NavMeshPath();
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        startPosition = gameObject.transform.position;
        GameObject[] gameObjects;
        gameObjects = GameObject.FindGameObjectsWithTag("Agent");
        finalcounter.text = ("Kalan aktor sayisi:" + gameObjects.Length.ToString()+"/"+ gameObjects.Length.ToString());
        total_agent = gameObjects.Length.ToString();
    
    }
    void Update()
    {
        if (transform.position[0] >= 114.0 & transform.position[0] <= 114.5 || transform.position[0] >= 217.0 & transform.position[0] <= 217.5)
        {
            
            if (gameObject.tag == "Agent")
            {
                {
                    transform.gameObject.tag = "final";
                    GameObject[] gameObjects;
                    gameObjects = GameObject.FindGameObjectsWithTag("Agent");
                    finalcounter.text = ("Kalan aktor sayisi:"+gameObjects.Length.ToString()+"/"+total_agent);
                    endTime = Time.time;
                    WriteSimulationData();
                }
            }
        }


        if (agent.velocity!=new Vector3(0,0,0) && transform.position!=random_destination_final)
        {
            anim.SetTrigger("isRunning");
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            random_destination_1 = new Vector3(UnityEngine.Random.Range(220.0f, 228.0f), 5, UnityEngine.Random.Range(112.0f, 125.0f));
            random_destination_2 = new Vector3(UnityEngine.Random.Range(109.5f, 97.0f), 1, UnityEngine.Random.Range(60.0f, 52.0f));
            
            for (int i = 0; i < path.corners.Length - 1; i++)
                total_distance_1 += (path.corners[i] - path.corners[i + 1]).magnitude;

            NavMesh.CalculatePath(transform.position, random_destination_2, NavMesh.AllAreas, path);
            for (int i = 0; i < path.corners.Length - 1; i++)
                total_distance_2 += (path.corners[i] - path.corners[i + 1]).magnitude;

			GameObject[] fireObjects = GameObject.FindGameObjectsWithTag("fire");
			foreach (GameObject fireObject in fireObjects)
			{
				float distance = Vector3.Distance(fireObject.transform.position, transform.position);
				if (total_distance_1 > total_distance_2 && distance >= distanceThreshold)
				{
					random_destination_final = random_destination_2;
					UnityEngine.Debug.Log("Condition 1: Total Distance 1 > Total Distance 2 AND Distance >= Distance Threshold");
				}
				else if (total_distance_1 > total_distance_2 && distance <= distanceThreshold)
				{
					random_destination_final = random_destination_1;
					UnityEngine.Debug.Log("Condition 2: Total Distance 1 > Total Distance 2 AND Distance <= Distance Threshold");
				}
				else if (total_distance_2 > total_distance_1 && distance >= distanceThreshold)
				{
					random_destination_final = random_destination_1;
					UnityEngine.Debug.Log("Condition 3: Total Distance 2 > Total Distance 1 AND Distance >= Distance Threshold");
				}
				else if (total_distance_2 > total_distance_1 && distance <= distanceThreshold)
				{
					random_destination_final = random_destination_2;
					UnityEngine.Debug.Log("Condition 4: Total Distance 2 > Total Distance 1 AND Distance <= Distance Threshold");
				}
				else
				{
					UnityEngine.Debug.Log("No condition satisfied.");
				}
			}
            agent.SetDestination(random_destination_final);
            startTime = Time.time;
        }

        if (agent.remainingDistance != 0 && agent.remainingDistance < 1)
        {
            anim.SetTrigger("isStatic");
        }

    }
}