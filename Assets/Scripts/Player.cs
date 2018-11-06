using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public float moveSpeed = 1.0f;
    public bool scan = false;

    public GameObject scannerFilter;

    public GameObject otherWorld;

    private Rigidbody playerRigidbody;
    private float oldMoveSpeed;

    public Vector3 respawnPoint;

    // Use this for initialization
    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody>();
    }

    void Start () {
        //initial spawn point
        respawnPoint = this.transform.position;
    //    if (GM == null) { GM = GameObject.Find("GameManager").GetComponent<GameManager>(); }
    }

    // Update is called once per frame
    void Update() {
        //If you fall through the floor
        if (this.transform.position.y < 0) {
            this.transform.position = respawnPoint;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift)) {
            StartCoroutine(otherWorld.GetComponent<PhaseInteraction>().toggleChildrenSolidity(0.5f));
            
        }
    }


    public void toggleWorld(GameObject world, bool enable) {
        foreach (Transform child in world.transform) {
            print("toggling");
            if(child.GetComponent<MeshRenderer>() != null) {
                child.GetComponent<MeshRenderer>().enabled = enable;
            }
        }
    }

    
    void OnCollisionEnter(Collision C) {
        
    }

    void OnTriggerEnter(Collider col) {
        
    }


    private void FixedUpdate()
    {
        //movement

        if (Input.GetAxis("Vertical") > 0) {
            transform.localPosition += transform.forward * moveSpeed * Time.deltaTime;
        } else if (Input.GetAxis("Vertical") < 0) {
            transform.localPosition -= transform.forward * moveSpeed * Time.deltaTime;
        }
        if (Input.GetAxis("Horizontal") > 0) {
            transform.localPosition += transform.right * moveSpeed * Time.deltaTime;
        } else if (Input.GetAxis("Horizontal") < 0) {
            transform.localPosition -= transform.right * moveSpeed * Time.deltaTime;
        }

        /*
        if (!inverted) {
            Vector3 movement = new Vector3(Input.GetAxis("Horizontal") * moveSpeed / 10, 0.0f, Input.GetAxis("Vertical") * moveSpeed / 10);
            playerRigidbody.MovePosition(playerRigidbody.position + movement);
        } else {
            Vector3 movement = new Vector3(Input.GetAxis("Horizontal") * -moveSpeed / 10, 0.0f, Input.GetAxis("Vertical") * -moveSpeed / 10);
            playerRigidbody.MovePosition(playerRigidbody.position + movement);
        }
        */
    }
}
