using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Checkpoint : MonoBehaviour {
    [TextArea]
    public string Notes = "If the player collides with this, set their respawn point here";


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider col) {
        if (col.gameObject.tag == "Player") {
            GameObject.Find("Player").GetComponent<Player>().respawnPoint = this.transform.position;
            GetComponent<Light>().intensity = 10;
            GetComponent<Light>().color = Color.green;
        }
    }
}
