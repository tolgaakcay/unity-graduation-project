using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;

public class ShowGoldenPath : MonoBehaviour
{
    public Transform target;
    private NavMeshPath path;
    private float elapsed = 0.0f;
    void Start()
    {
        path = new NavMeshPath();
        elapsed = 0.0f;
    }

    void Update()
    {
        // Update the way to the goal every second.
        elapsed += Time.deltaTime;
        if (elapsed > 1.0f)
        {
            elapsed -= 1.0f;
            NavMesh.CalculatePath(transform.position, new Vector3(UnityEngine.Random.Range(95.0f, 105.0f), 1, UnityEngine.Random.Range(46.0f, 56.0f)), NavMesh.AllAreas, path);
        }
        for (int i = 0; i < path.corners.Length - 1; i++)
            UnityEngine.Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red);
        for (int i = 0; i < path.corners.Length - 1; i++)
            UnityEngine.Debug.Log(path.corners);

    }
}