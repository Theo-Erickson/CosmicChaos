using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionPulse : MonoBehaviour {
    public SphereCollider myCollider;

    public float maxRange = 5.0f;
    public float expandSpeed = 1.0f;
    public float maxEnergy = 100;
    public bool expanding = false;
    public float startCost = 50;
    public bool respondToKeyPress = false;

    private float oldRadius;
    public float energy;


	// Use this for initialization
	void Start () {
        myCollider = GetComponent<SphereCollider>();
        energy = maxEnergy;
        oldRadius = myCollider.radius;
    }
	
	// Update is called once per frame
	void Update () {
        if (respondToKeyPress && Input.GetKeyDown(KeyCode.RightShift) && energy > startCost) {
            expanding = !expanding;
        }

        //you are getting bigger
        if (expanding) {
            //expand search
            if (energy > 0) {
                if (myCollider.radius <= maxRange) {
                    myCollider.radius += expandSpeed;
                }
                energy--;
            } else {
                expanding = false;
            }

        } else {
            //shrink search ball
            if (myCollider.radius > 1.0f) {
                myCollider.radius -= expandSpeed;
            }
            //recharge
            if ( energy < maxEnergy) {
                energy++;
            }
        }
	}

    void OnTriggerEnter(Collider col) {
        print("enter");
        if(col.tag == "phased") {
            col.gameObject.GetComponent<PhaseInteraction>().changeMyPhase();
        }
    }

    void OnTriggerExit(Collider col) {
        if(col.tag == "phased") {
            col.gameObject.GetComponent<PhaseInteraction>().changeMyPhase();
        }
    }

}
