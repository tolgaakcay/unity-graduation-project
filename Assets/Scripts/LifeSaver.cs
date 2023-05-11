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

public class LifeSaver : MonoBehaviour
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
    public Text finalcounter;
    public Text textBox;
    private NavMeshPath path;
    public SimulationManager simManager;
    public bool isEvacuated = false;
    public int counter3 = 0;
    
    



    GameObject[] patrols ;
    private NavMeshPath _path;
    private NavMeshPath _path2;
    public bool isChild=false;
    public Animator anim;
    public NavMeshAgent agent;
    public int counter2 = 0;
    public GameObject child;
    public bool pathFinished;

      public void WriteSimulationData()    
    {
        StreamWriter sw = new StreamWriter(fileLocation, true);
        string data = (startPosition.x * 1000).ToString("F0") + "," + (startPosition.z * 1000).ToString("F0") + "," + (startPosition.y * 1000).ToString("F0") + "," + Math.Round((endTime-startTime)).ToString();
        sw.WriteLine(data);
        sw.Flush();
        sw.Close();       
    }
    
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        patrols = GameObject.FindGameObjectsWithTag("patrol");
        _path = new NavMeshPath();
        
        startPosition = gameObject.transform.position;
        GameObject[] gameObjects;
        gameObjects = GameObject.FindGameObjectsWithTag("LifeSaver");
        finalcounter.text = ("Kalan aktor sayisi:" + gameObjects.Length.ToString()+"/"+ gameObjects.Length.ToString());
        total_agent = gameObjects.Length.ToString();
        simManager = GameObject.FindWithTag("SimulationManager").GetComponent<SimulationManager>();
        
      
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position[0] >= 114.0 & transform.position[0] <= 114.5 || transform.position[0] >= 217.0 & transform.position[0] <= 217.5)
        {
            
            if (gameObject.tag == "LifeSaver")
            {
                {
                    transform.gameObject.tag = "final";
                    GameObject[] gameObjects;
                    gameObjects = GameObject.FindGameObjectsWithTag("LifeSaver");
                    finalcounter.text = ("Kalan aktor sayisi:"+gameObjects.Length.ToString()+"/"+total_agent);
                    endTime = Time.time;
                    WriteSimulationData();
                }
            }
        }


        if (Input.GetKeyDown(KeyCode.Space))
        {
        NavMesh.CalculatePath(transform.position, patrols[0].transform.position, UnityEngine.AI.NavMesh.AllAreas, _path);
        agent.SetDestination(patrols[0].transform.position);
        anim.SetTrigger("isRunning");
        }
        if(isChild && counter2 == 0){
            counter2++;
             _path2 = new NavMeshPath();
            NavMesh.CalculatePath(transform.position, child.transform.position, UnityEngine.AI.NavMesh.AllAreas, _path2);
            agent.SetDestination(child.transform.position);
        }

        if (agent.remainingDistance != 0 && agent.remainingDistance < 3 && isEvacuated)
        {
            anim.SetTrigger("isStatic");
            agent.speed = 0;
        }
         if (agent.velocity!=new Vector3(0,0,0) && transform.position!=random_destination_final)
        {
            anim.SetTrigger("isRunning");
        }
        
        if(pathFinished && counter3 == 0){

            
            counter3++;
            // simManager.isSimStarted = true;
            random_destination_1 = new Vector3(UnityEngine.Random.Range(220.0f, 228.0f), 5, UnityEngine.Random.Range(112.0f, 125.0f));
            random_destination_2 = new Vector3(UnityEngine.Random.Range(109.5f, 97.0f), 1, UnityEngine.Random.Range(60.0f, 52.0f));
            // Add new variables to store the distances between the agent and each destination
            float distance_to_destination_1 = Vector3.Distance(random_destination_1, transform.position);
            float distance_to_destination_2 = Vector3.Distance(random_destination_2, transform.position);
            // If the agent is closer to destination 1, set the final destination to destination 2
            if (distance_to_destination_1 < distance_to_destination_2)
            {
                random_destination_final = random_destination_1;
                NavMesh.CalculatePath(transform.position, random_destination_1, NavMesh.AllAreas, _path);
            }
            else if (distance_to_destination_2 < distance_to_destination_1)
            {
                random_destination_final = random_destination_2;
                NavMesh.CalculatePath(transform.position, random_destination_1, NavMesh.AllAreas, _path);
            }

            agent.SetDestination(random_destination_final);
            startTime = Time.time;

            // NavMesh.CalculatePath(transform.position, patrols[0].transform.position, UnityEngine.AI.NavMesh.AllAreas, _path);
            // agent.SetDestination(patrols[0].transform.position);
            // anim.SetTrigger("isRunning");
        
        }
    }

    void OnTriggerEnter (Collider col)
    {
        if (col.gameObject.tag == "Child")
        {
            child = col.gameObject;
            isChild = true;
        }

        
    }

    void OnTriggerStay(Collider other) {
        if(other.gameObject.tag == "Child")
        {
            float childdistance = Vector3.Distance(this.transform.position,other.transform.position);

            if(childdistance < 3.0f)
            {
                
                pathFinished =true;

                 
            }
        }
    }
}
