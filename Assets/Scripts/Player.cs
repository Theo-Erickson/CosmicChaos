using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {
    public float moveSpeed = 1.0f;
    public bool canShift = true;

    public GameObject scannerFilter;

    public GameObject world1;
    public GameObject world2;

    public Canvas GUI;
    public DetectionPulse dPulse;
    public Vector3 respawnPoint;

    // Use this for initialization
    private void Awake(){
    }

    void Start () {
        //If you don't link it in the editor, it will try to find it where it expects it to be
        if(GUI == null) { GameObject.Find("GUI").GetComponent<Canvas>(); }
        if(dPulse == null) { GameObject.Find("Detection Sphere").GetComponent<DetectionPulse>(); }

        //initial spawn point
        respawnPoint = this.transform.position;
    }

    // Update is called once per frame
    void Update() {
        //If you fall through the floor or press "R"
        if (this.transform.position.y < 0 || Input.GetKeyDown(KeyCode.R)) {
            this.transform.position = respawnPoint;
        }

        //only allow shifting if you canShift and are pression left shift. A lot of shifty bussiness going on here -_0
        if (Input.GetKeyDown(KeyCode.LeftShift)) {
            if (canShift) {
                world1.GetComponent<PhaseInteraction>().toggleChildrenVisibility();
                world2.GetComponent<PhaseInteraction>().toggleChildrenVisibility();
            }else {
                StartCoroutine(DisplayFadingText("Middle", "Cannot Shift: Otherworldly Object Intererence", 2.0f));
            }
        }

        GameObject.Find("Top Left").GetComponent<Text>().text = dPulse.Mode.ToString();
            //.GetComponent<Text>().text = dPulse.Mode.ToString();


    }

    private void FixedUpdate() {
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
    }

    void OnCollisionEnter(Collision C) {
        
    }

    void OnTriggerEnter(Collider col) {
        
    }






    //just show the message
    public void DisplayText(string whichTextField, string whatToSay) {
        GameObject.Find(whichTextField).GetComponent<Text>().text = whatToSay;
    }

    //wait a little before showing message
    public IEnumerator DelayedDisplayText(string whichTextField, string whatToSay, float delayBeforeStartInSeconds) {
        yield return new WaitForSeconds(delayBeforeStartInSeconds);
        DisplayText(whichTextField, whatToSay);
    }

    //Wait delayBeforeStart seconds before displaying a fading text update
    public IEnumerator DelayedDisplayFadingText(string whichTextField, string whatToSay, float delayBeforeStartInSeconds, float delayBeforeDissappearInSeconds) {
        yield return new WaitForSeconds(delayBeforeStartInSeconds);
        StartCoroutine(DisplayFadingText(whichTextField, whatToSay, delayBeforeDissappearInSeconds));
    }

    //create a message at element whichTextField that says whatToSay and dissappears after some time
    public IEnumerator DisplayFadingText(string whichTextField, string whatToSay, float delayBeforeDissappearInSeconds) {
        GameObject.Find(whichTextField).GetComponent<Text>().text = whatToSay;
        yield return new WaitForSeconds(delayBeforeDissappearInSeconds);
        GameObject.Find(whichTextField).GetComponent<Text>().text = "";
    }

}
