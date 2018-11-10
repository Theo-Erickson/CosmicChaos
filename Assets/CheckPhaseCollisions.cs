using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPhaseCollisions : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void onTriggerExit(Collider other) {
        if (other.tag == "phased") {
            GetComponentInParent<Player>().canShift = true;
            print("reset shift");
        }
    }

    void onTriggerEnter(Collider other) {
        if (other.tag == "phased") {
            print("prevent shift");
            GetComponentInParent<Player>().canShift = false;
        }
    }

    void onTriggerStay(Collider other) {
        if (other.tag == "phased") {
            print("prevent shift");
            GetComponentInParent<Player>().canShift = false;
        }
    }

    

}
