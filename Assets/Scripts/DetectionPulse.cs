using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionPulse : MonoBehaviour {
    [Header("Interactibility")]
    [Tooltip("How the pulse can be activated and deactivated")]
    public bool canPulse = true; //boolean to enable/disable your pulse
    [Tooltip("lets you know if the pulse is going out. Also works also as a \" is this activated ? \" boolean")]
    public bool expanding = false;
    [Tooltip("whether the pulse can be activated via key press")]
    public bool respondToKeyPress = false;

    public SphereCollider myCollider;

    [Header("Key")]
    public KeyCode activatePulseKey = KeyCode.RightShift;

    [Header("Energy Values")]
    public bool infiniteEnergy = false;
    public float maxEnergy = 100;  //how long the pulse can sustain itself
    public float startCost = 50; //cost to activate it

    [Header("Range Values")]
    public float maxRange = 5.0f; //how big the pulse gets
    public float expandSpeed = 3.0f; //how fast it gets there

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
        if (infiniteEnergy) {
            maxEnergy = -1f;
            startCost = 0;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (respondToKeyPress && Input.GetKeyDown(activatePulseKey) && energy > startCost) {
            Activate();
        }

        //Toggle modes and only do so when the scanner is off
        if(respondToKeyPress && Input.GetKeyDown(KeyCode.Slash) && myCollider.radius <= oldRadius) {
            if(Mode == visOrSolidity.solidity) {
                Mode = visOrSolidity.visibility;
            } else if( Mode == visOrSolidity.visibility){
                Mode = visOrSolidity.both;
            } else {
                Mode = visOrSolidity.solidity;
            }
            print("Toggled to: " + Mode.ToString());
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
                if(!infiniteEnergy)energy--;
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

        if(myCollider.radius <= oldRadius) {
            myCollider.enabled = false;
        } else {
            myCollider.enabled = true;
        }
	}

    public void Activate() {
        if (canPulse) {
            expanding = !expanding;
        }
    }

    //if your sphere collider hits something else
    void OnTriggerEnter(Collider col) {
        
        if(col.tag == "phased") {
            col.gameObject.GetComponent<PhaseInteraction>().PeekWorldChange(true);
        }

        /*
        if (Mode == visOrSolidity.solidity) {
            print("enter Solidity");
            if (col.tag == "phased") {
                col.gameObject.GetComponent<PhaseInteraction>().setNonDefaultSolidity();
            }
        } else if(Mode == visOrSolidity.visibility){
            print("enter Visibility");
            if (col.tag == "phased") {
                col.gameObject.GetComponent<PhaseInteraction>().setNonDefaultVisibility();
            }
        } else {
            if (col.tag == "phased") {
                col.gameObject.GetComponent<PhaseInteraction>().setNonDefaultSolidity();
                col.gameObject.GetComponent<PhaseInteraction>().setNonDefaultVisibility();
            }
        }
        */
    }

    void OnTriggerExit(Collider col) {
        if (col.tag == "phased") {
            col.gameObject.GetComponent<PhaseInteraction>().PeekWorldChange(false);
        }

        /*if (Mode == visOrSolidity.solidity) {
            print("enter Solidity");
            if (col.tag == "phased") {
                col.gameObject.GetComponent<PhaseInteraction>().setDefaultSolidity();
            }
        } else if (Mode == visOrSolidity.visibility) {
            print("enter Visibility");
            if (col.tag == "phased") {
                col.gameObject.GetComponent<PhaseInteraction>().setDefaultVisibility();
            }
        } else {
            if (col.tag == "phased") {
                col.gameObject.GetComponent<PhaseInteraction>().setDefaultSolidity();
                col.gameObject.GetComponent<PhaseInteraction>().setDefaultVisibility();
            }
        }
        */
    }

}
