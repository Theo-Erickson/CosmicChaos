using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jitter : MonoBehaviour {
    public float jitterSpeed = 1;
    [SerializeField] private bool smoothMovement = false;
    [SerializeField] private Transform[] teleportPositions;
    [SerializeField]private float countdown;
    private Vector3 oldPosition;
    private GameObject player;
    // Use this for initialization

    public float distanceFromPlayer;
    
    void Start () {
        countdown = countdown * Time.deltaTime;
        player = GameObject.Find("Player");
        oldPosition = this.transform.position;
    }

    void OnEnable() {
        oldPosition = this.transform.position;
    }

    void OnDisable() {
        oldPosition = this.transform.position;
    }

    // Update is called once per frame
    void Update() {
        distanceFromPlayer = Vector3.Distance(this.transform.position, player.transform.position);


        this.transform.position = oldPosition;

        //if you don't specify specific positions for it to teleport to, just move randomly.
        //THIS IS SUGGESTED
        //if (teleportPositions.Length == 0) {
            countdown++;
            this.transform.position = new Vector3(this.transform.position.x + Random.Range(-jitterSpeed, jitterSpeed), this.transform.position.y + Random.Range(-jitterSpeed, jitterSpeed), this.transform.position.z + Random.Range(-jitterSpeed, jitterSpeed) );
         //   if(countdown % 600 == 0) { jitterSpeed++; }
        /*} else {
            if (countdown <= 0) {
                countdown = jitterSpeed;
                this.transform.position = teleportPositions[Random.Range(0, teleportPositions.Length)].position;
            } else {
                countdown--;
            }
        }*/
    }
}
