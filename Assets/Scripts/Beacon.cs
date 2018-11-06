using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class is intended to manage activating a detection pulse on this object from affar
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

    void OnMouseOver() {
        if (Input.GetMouseButtonDown(0)) {
            if (clickable) {
                dPulse.Activate();
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
