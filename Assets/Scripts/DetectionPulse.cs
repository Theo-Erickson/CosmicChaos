﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionPulse : MonoBehaviour {
    public bool canPulse = true; //boolean to enable/disable your pulse

    public SphereCollider myCollider;

    public float maxRange = 5.0f; //how big the pulse gets
    public float expandSpeed = 1.0f; //how fast it gets there
    public float maxEnergy = 100;  //how long the pulse can sustain itself
    public bool expanding = false; //lets you know if the pulse is going out. Also works also as a "is this activated?" boolean
    public float startCost = 50; //cost to activate it
    public bool respondToKeyPress = false; //whether the pulse can be activated via key press

    public GameObject visualizer; //referrence to possible visualizer objects

    public enum visOrSolidity{ visibility, solidity, both};  //enum to keep track of what mode the scanner is in
    //shows in inspector
    public visOrSolidity Mode = 0;

    public float oldRadius;
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
            Activate();
        }

        //toggle modes and only do so when the scanner is off
        if(respondToKeyPress && Input.GetKeyDown(KeyCode.Slash) && myCollider.radius <= oldRadius) {
            if(Mode == visOrSolidity.solidity) {
                Mode = visOrSolidity.visibility;
            } else if( Mode == visOrSolidity.visibility){
                Mode = visOrSolidity.both;
            } else {
                Mode = visOrSolidity.solidity;
            }
            print("toggled to: " + Mode.ToString());
        }

        if (visualizer != null) {
            visualizer.transform.localScale = Vector3.one * myCollider.radius;
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
            if (myCollider.radius > oldRadius) {
                myCollider.radius -= expandSpeed;
            }
            //recharge
            if ( energy < maxEnergy) {
                energy++;
            }
        }
	}

    public void Activate() {
        if (canPulse) {
            expanding = !expanding;
        }
    }

    //if your sphere collider hits something else
    void OnTriggerEnter(Collider col) {
        if (Mode == visOrSolidity.solidity) {
            print("enter Solidity");
            if (col.tag == "phased") {
                col.gameObject.GetComponent<PhaseInteraction>().toggleMySolidity();
            }
        } else if(Mode == visOrSolidity.visibility){
            print("enter Visibility");
            if (col.tag == "phased") {
                col.gameObject.GetComponent<PhaseInteraction>().toggleMyVisibility();
            }
        } else {
            if (col.tag == "phased") {
                col.gameObject.GetComponent<PhaseInteraction>().toggleMyVisibility();
                col.gameObject.GetComponent<PhaseInteraction>().toggleMySolidity();
            }
        }
    }

    void OnTriggerExit(Collider col) {
        if (Mode == visOrSolidity.solidity) {
            if (col.tag == "phased") {
                col.gameObject.GetComponent<PhaseInteraction>().toggleMySolidity();
            }
        } else if (Mode == visOrSolidity.visibility) {
            if (col.tag == "phased") {
                col.gameObject.GetComponent<PhaseInteraction>().toggleMyVisibility();
            }
        } else {
            if (col.tag == "phased") {
                col.gameObject.GetComponent<PhaseInteraction>().toggleMyVisibility();
                col.gameObject.GetComponent<PhaseInteraction>().toggleMySolidity();
            }
        }
    }

}
