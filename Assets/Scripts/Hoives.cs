using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hoives : MonoBehaviour {
    public bool hasHoiives = false;

    public float resistance = 200;

    private float maxResistance;

    [SerializeField] private Material healthyTem;
    [SerializeField] private Material hoivesTem;


    // Use this for initialization
    void Start () {
        maxResistance = resistance;
	}
	
	// Update is called once per frame
	void Update () {
		if(resistance < 0) {
            this.GetComponent<Renderer>().material = hoivesTem;
            hasHoiives = true;
        } else if(!hasHoiives){
            this.GetComponent<Renderer>().material = healthyTem;
        }
	}

    void OnTriggerStay(Collider other) {
        if(other.tag == "Player") {
            resistance--;
        }
    }

    void OnTriggerExit(Collider other) {
        if(other.tag == "Player") {
            resistance = maxResistance;
        }
    }
}
