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
    //    if (GM == null) { GM = GameObject.Find("GameManager").GetComponent<GameManager>(); }
    }

    // Update is called once per frame
    void Update() {
        //If you fall through the floor
        if (this.transform.position.y < 0) {
            this.transform.position = new Vector3(transform.position.x, 100, transform.position.z);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift)) {
            print("scan");
            scannerFilter.GetComponent<Renderer>().enabled = !scannerFilter.GetComponent<Renderer>().enabled;

            //toggleWorld(otherWorld, scan);

            List<GameObject> children = new List<GameObject>();

            //get all the children of the otherWorld.
            for (int i = 0; i < otherWorld.transform.childCount; i++) {
                children.Add(otherWorld.transform.GetChild(i).gameObject);
            }

            if (scan) {
                //disable from top to bottom
                StartCoroutine(toggleObjectsOverTime(children, true, scan, 0.2f));
            } else {
            //disable from bottom to top
                StartCoroutine(toggleObjectsOverTime(children, false, scan, 0.2f));
            }
            scan = !scan;

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

    public void enableSphere(GameObject world, float radius, float time) {

    }
    

    public IEnumerator toggleObjectsOverTime(List<GameObject> objects, bool forward, bool enable, float decayTime) {
        yield return new WaitForSeconds(decayTime);
        if (forward) {
            if (objects[0].GetComponent<MeshRenderer>() != null) {
                objects[0].GetComponent<MeshRenderer>().enabled = enable;
            }
            if (objects.Count > 1) {
                objects.RemoveAt(0);
                StartCoroutine(toggleObjectsOverTime(objects, forward, enable, decayTime));
            }
        } else {
            if(objects[objects.Count-1].GetComponent<MeshRenderer>() != null) {
                objects[objects.Count-1].GetComponent<MeshRenderer>().enabled = enable;
            }
            if (objects.Count > 1) {
                objects.RemoveAt(objects.Count-1);
                StartCoroutine(toggleObjectsOverTime(objects, forward, enable, decayTime));
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
