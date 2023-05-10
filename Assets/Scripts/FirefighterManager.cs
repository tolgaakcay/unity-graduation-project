using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
using System.Text;
using UnityEngine.UI;
using System.IO;



public class FirefighterManager : MonoBehaviour
{
    public Vector3 startPosition;
    public float startTime;
    public float endTime;
    public int counter = 0;
    public string total_agent;
    public Text finalcounter;
    public Text textBox;
    public string fileLocation = @".\coord_time.csv";
 
    public GameObject[] extinguishers;
    public GameObject[] fires;
    public Vector3 _firePosition;
    private NavMeshPath _path;
    public Animator anim;
    public NavMeshAgent agent;
    public SimulationManager simManager;
    [SerializeField] private ParticleSystem _fireExtinguisherParticles;
    
    [SerializeField] private Transform _objGrabPosTransform;

    public Vector3 random_destination_1;
    public Vector3 random_destination_2;
    public Vector3 random_destination_final;

    private GameObject _fireObj;
    GrabPoint extinguisherChild;

        public void WriteSimulationData()    
    {
        StreamWriter sw = new StreamWriter(fileLocation, true);
        string data = (startPosition.x * 1000).ToString("F0") + "," + (startPosition.z * 1000).ToString("F0") + "," + (startPosition.y * 1000).ToString("F0") + "," + Math.Round((endTime-startTime)).ToString();
        sw.WriteLine(data);
        sw.Flush();
        sw.Close();       
    }

    public enum FirefighterState
    {
        Idle,
        GoingToExtinguisher,
        GoingToFire,
        Extinguish,
        Evacuation
    }

    public FirefighterState state;

    void Start()
    {
        state = FirefighterState.Idle;
        extinguishers = GameObject.FindGameObjectsWithTag("extinguisher");
        fires = GameObject.FindGameObjectsWithTag("fire");
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        simManager = GameObject.FindWithTag("SimulationManager").GetComponent<SimulationManager>();
        _fireExtinguisherParticles = GetComponentInChildren<ParticleSystem>();
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

        if (simManager.isSimStarted)
        {
            if (fires.Length > 0)
            {
                if (state == FirefighterState.Idle)
                {
                    anim.SetTrigger("isStatic");
                    GameObject _extinguisher = FindNearestGameObject(extinguishers);
                    extinguisherChild = _extinguisher.GetComponentInChildren<GrabPoint>();
                    GoToExtinguisher(extinguisherChild.transform.position);
                }
            }

        }
        if (agent.velocity!=new Vector3(0,0,0) && agent.remainingDistance>0.1f)
        {
            anim.SetTrigger("isRunning");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "extinguisher")
        {
            Debug.Log("EXTINGUISHERRRRRR");
            anim.SetTrigger("isStatic");
            StartCoroutine(Stop(other.gameObject));
            other.gameObject.tag = "grabbed";
        }
        if (other.gameObject.tag == "fire")
        {
            anim.SetTrigger("isStatic");
            agent.SetDestination(transform.position);
            StartCoroutine(Extinguish(_fireObj));
        }
        if (other.gameObject.tag == "evacuation")
        {
            anim.SetTrigger("isStatic");
            this.gameObject.tag = "final";
        }
    }

    IEnumerator Stop(GameObject obj)
    {
        // Debug.Log("Waiting for 5 seconds");
        // Debug.Log(Time.time);
        yield return new WaitForSeconds(5);
        // Debug.Log(Time.time);
        GrabExtinguisher(obj);
        yield return new WaitForSeconds(1);
        GoToExtinguish();
    }

    void GrabExtinguisher(GameObject obj)
    {
        if (obj.TryGetComponent(out ObjectGrabbable grabbable))
        {
            Debug.Log(this.name + " grabbed " + obj.name);
            grabbable.DisableCollider();
            grabbable.Grab(_objGrabPosTransform);
        }
    }

    void GoToExtinguisher(Vector3 _extinguisherPos)
    {
        Debug.Log("Going to extinguisher child");
        _path = new NavMeshPath();
        NavMesh.CalculatePath(transform.position, _extinguisherPos, UnityEngine.AI.NavMesh.AllAreas, _path);
        state = FirefighterState.GoingToExtinguisher;
        agent.SetDestination(_extinguisherPos);
        anim.SetTrigger("isRunning");

        // if (agent.remainingDistance != 0 && agent.remainingDistance < 3)
        // {
        //     GoToExtinguish();
        // }
    }

    void GoToExtinguish()
    {
        Debug.Log("Going to extinguish");
        state = FirefighterState.GoingToFire;
        _fireObj = FindNearestGameObject(fires);
        // anim.SetTrigger("goingToExtinguish");
        if (_fireObj != null)
            agent.SetDestination(_fireObj.transform.position);
            Debug.DrawLine(new Vector3(transform.position.x, 1f, transform.position.z), 
                           new Vector3(_fireObj.transform.position.x, 1f, _fireObj.transform.position.z), Color.red, 5f);
        


        // if (agent.remainingDistance != 0 && agent.remainingDistance < 3f)
        // {
        //     Extinguish(_fireObj);
        // }
    }

    IEnumerator Stop(int sec)
    {
        anim.SetTrigger("isStatic");
        // Debug.Log("Waiting for 5 seconds");
        // Debug.Log(Time.time);
        yield return new WaitForSeconds(sec);
    }

    IEnumerator Extinguish(GameObject fire)
    {
        state = FirefighterState.Extinguish;
        // anim.SetTrigger("extinguish");
        _fireExtinguisherParticles.Play();
        
        int index = Array.IndexOf(fires, fire);
        if (index != -1)
        {
            GameObject[] newFires = new GameObject[fires.Length - 1];
            Array.Copy(fires, 0, newFires, 0, index);
            Array.Copy(fires, index + 1, newFires, index, fires.Length - index - 1);
            fires = newFires;
            yield return new WaitForSeconds(5);
            Destroy(fire);
            _fireExtinguisherParticles.Stop();
        }

        if (fires.Length == 0)
        {
            state = FirefighterState.Evacuation;
            anim.SetTrigger("isRunning");
            Evacuate();
        }
        else
        {
            GoToExtinguish();
        }
    }


    void Evacuate()
    {
        Debug.Log("Evacuating");
        random_destination_1 = new Vector3(UnityEngine.Random.Range(220.0f, 228.0f), 5, UnityEngine.Random.Range(112.0f, 125.0f));
        random_destination_2 = new Vector3(UnityEngine.Random.Range(109.5f, 97.0f), 1, UnityEngine.Random.Range(60.0f, 52.0f));
                
        // Add new variables to store the distances between the agent and each destination
        float distance_to_destination_1 = Vector3.Distance(random_destination_1, transform.position);
        float distance_to_destination_2 = Vector3.Distance(random_destination_2, transform.position);

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
    }

    GameObject FindNearestGameObject(GameObject[] fires)
    {
        float closestDistance = Mathf.Infinity;
        GameObject closestObject = null;

        foreach (GameObject obj in fires)
        {
            float distance = Vector3.Distance(obj.transform.position, this.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestObject = obj;
            }
        }

        return closestObject;
    }

}
