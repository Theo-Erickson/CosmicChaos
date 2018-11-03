using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beacon : MonoBehaviour {
    public bool clickable = true;

    public SphereCollider sCollider;
    public DetectionPulse dPulse;

	// Use this for initialization
	void Start () {
        sCollider = this.gameObject.AddComponent<SphereCollider>();
        dPulse = this.gameObject.AddComponent<DetectionPulse>();
        
        if (!clickable) {
            this.GetComponent<Renderer>().material.color = Color.black;
        } else {
            this.GetComponent<Renderer>().material.color = Color.green;
        }
    }
	
	// Update is called once per frame
	void Update() {
	}

    void OnMouseOver() {
        if (Input.GetMouseButtonDown(0)) {
            if (clickable) {
                dPulse.expanding = !dPulse.expanding;
            }
        }
        if (Input.GetMouseButtonDown(1)) {
            clickable = !clickable;
            if (!clickable) {
                this.GetComponent<Renderer>().material.color = Color.black;
            } else {
                this.GetComponent<Renderer>().material.color = Color.green;
            }
        }
    }
}
