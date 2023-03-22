using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationCharacterManager_6 : MonoBehaviour {

    public GameObject character;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayHit;

            if (Physics.Raycast(ray, out rayHit))
            {
                if (rayHit.collider.tag == "Floor")
                {
                    Instantiate(character, rayHit.point, Quaternion.identity, this.gameObject.transform);
                }
            }
        }
		
	}
}
