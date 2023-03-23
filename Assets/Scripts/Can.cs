using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Can : MonoBehaviour
{

//     public int Health = 100;

//     private void OnTriggerStay(Collider other) {


//         if(other.gameObject.tag == "fire"){
//             Health -- ;
//             StartCoroutine(Example());
//             Debug.Log(Health);
//         }

//     }

//     IEnumerator Example() // bu da yeni metod olarak altta tanımlansın
// {
//     yield return new WaitForSecondsRealtime(5);
// }

public int Health = 100;


public bool invokeStarted = false;

void DecreaseHealth(){
    if(invokeStarted){
        Health--;
        Debug.Log("Health: " + Health);
    }
}

void OnTriggerEnter(Collider other){
    if(other.gameObject.tag == "fire" && !invokeStarted){
        invokeStarted = true;
        InvokeRepeating("DecreaseHealth", 1.0f, 1.0f);
    }
}

void OnTriggerExit(Collider other){
    if(other.gameObject.tag == "fire"){
        invokeStarted = false;
        CancelInvoke("DecreaseHealth");
    }
}

}