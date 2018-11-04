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

    public GameObject visualizer;

    public enum visOrSolidity{ visibility, solidity, both};
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
            expanding = !expanding;
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
