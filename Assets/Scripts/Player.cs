using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {
    [Header ("Movement")]
    public float moveSpeed = 1.0f;
    [Tooltip("Whether the player can shift themselves")]
    public bool canShift = true;

    [Header("Scanning")]
    public GameObject scannerFilter;
    public bool aimingAtInteractibleThing = false;

    [Header("World Switching")]
    public KeyCode shiftWorldKey = KeyCode.LeftShift;
    public int currentWorld = 1;
    public GameObject world1;
    public GameObject world2;

    [Header("Object Referencing")]
    public GameObject GUI;
    public DetectionPulse dPulse;
    public Vector3 respawnPoint;
    public GameManager GM;
    public KeyCode pauseKey = KeyCode.Escape;


    // Use this for initialization
    private void Awake(){
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Start () {
        //If you don't link it in the editor, it will try to find it where it expects it to be
        if(GUI == null) { this.transform.Find("GUI"); }
        if(dPulse == null) { this.transform.Find("Detection Sphere").GetComponent<DetectionPulse>(); }

        if(world1 == null) { GameObject.Find("World 1"); }
        if(world1 == null) { GameObject.Find("World 2"); }

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
        if (Input.GetKeyDown(shiftWorldKey)) {
            if (canShift) {
                ShiftWorlds();
            }else {
                StartCoroutine(DisplayFadingText("Middle", "Cannot Shift: Otherworldly Object Intererence", 2.0f));
            }
        }

        GUI.transform.Find("Top Left").GetComponent<Text>().text = dPulse.Mode.ToString();
        //.GetComponent<Text>().text = dPulse.Mode.ToString();

        if (Input.GetKeyDown(pauseKey)) {
            if (GM.paused) {
                DisplayText("Middle", "PAUSED");
            } else {
                DisplayText("Middle", "");
            }
        }
    }

    //
    public void ShiftWorlds() {
        if(currentWorld == 1) { currentWorld = 2; } else { currentWorld = 1; }
        world1.GetComponent<PhaseInteraction>().ToggleChildren();
        world2.GetComponent<PhaseInteraction>().ToggleChildren();
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
        GUI.transform.Find(whichTextField).GetComponent<Text>().text = whatToSay;
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
        GUI.transform.Find(whichTextField).GetComponent<Text>().text = whatToSay;
        yield return new WaitForSeconds(delayBeforeDissappearInSeconds);
        GUI.transform.Find(whichTextField).GetComponent<Text>().text = "";
    }

}
