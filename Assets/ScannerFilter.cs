using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScannerFilter : MonoBehaviour {

    public DetectionPulse pulse;

    public Material solidityScan;
    public Material visibilityScan;
    public Material bothScan;

	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        //disable filter from revealing blocks in front of you when scanner is enabled
        if(pulse.myCollider.radius >= pulse.oldRadius) {
            this.GetComponent<BoxCollider>().enabled = false;
        } else {
            this.GetComponent<BoxCollider>().enabled = true;
        }

        if (pulse.expanding) {
            GetComponent<Renderer>().enabled = true;

            if (pulse.Mode == DetectionPulse.visOrSolidity.solidity) {
                GetComponent<Renderer>().material = solidityScan;
            }else if (pulse.Mode == DetectionPulse.visOrSolidity.visibility) {
                GetComponent<Renderer>().material = visibilityScan;
            }else if (pulse.Mode == DetectionPulse.visOrSolidity.both) {
                GetComponent<Renderer>().material = bothScan;
            }
        } else {
            GetComponent<Renderer>().enabled = false;
        }
	}

    void OnTriggerEnter(Collider col) {
        if(col.tag == "phased") {
            col.gameObject.GetComponent<PhaseInteraction>().setVisibility(true);
        }
    }

    void OnTriggerExit(Collider col) {
        if (col.tag == "phased") {
            col.gameObject.GetComponent<PhaseInteraction>().setVisibility(false);
        }
    }

}
