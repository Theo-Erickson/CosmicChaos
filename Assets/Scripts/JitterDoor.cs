using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JitterDoor : MonoBehaviour {
    [SerializeField] private float jitterSpeed;
    [SerializeField] private bool smoothMovement = false;
    [SerializeField] private Transform[] teleportPositions;
    [SerializeField]private float countdown;
    private Vector3 oldPosition;
	// Use this for initialization
	void Start () {
        oldPosition = this.transform.position;
        countdown = countdown * Time.deltaTime;
	}

    // Update is called once per frame
    void Update() {
        this.transform.position = oldPosition;

        if (teleportPositions.Length == 0) {
            countdown++;
            this.transform.position = new Vector3(this.transform.position.x + Random.Range(-jitterSpeed, jitterSpeed), this.transform.position.y + Random.Range(-jitterSpeed, jitterSpeed), this.transform.position.z + Random.Range(-jitterSpeed, jitterSpeed) );
            if(countdown % 600 == 0) { jitterSpeed++; }
        } else {

            if (countdown <= 0) {
                countdown = jitterSpeed;
                this.transform.position = teleportPositions[Random.Range(0, teleportPositions.Length)].position;
            } else {
                countdown--;
            }
        }
    }
}
