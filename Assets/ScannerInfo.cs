using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScannerInfo : MonoBehaviour {

    public Canvas InfoPane;

	// Use this for initialization
	void Start () {
        InfoPane = GetComponentInChildren<Canvas>();
	}
	
	// Update is called once per frame
	void Update () {
        InfoPane.gameObject.SetActive(gameObject.GetComponentInParent<PhaseInteraction>().solid);
	}
}
