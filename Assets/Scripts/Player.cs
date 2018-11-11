using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {
    [Header ("Movement")]
    public float moveSpeed = 1.0f;
    [Tooltip("Whether the player can shift themselves")]
    public bool canShift = true;
    public bool speedrunnerMode = false;

   [Header("Movement Sounds")]
    private AudioSource footStepSound;
    private bool forwardFootStepSoundOn = false;
    private bool horizontalFootStepSoundOn = false;

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
        footStepSound = GetComponent<AudioSource>();
    }

    void Start () {
        if (speedrunnerMode) {
            moveSpeed *= 5;
        }
        //If you don't link it in the editor, it will try to find it where it expects it to be
        if(GUI == null) { this.transform.Find("GUI"); }
        if(dPulse == null) { this.transform.Find("Detection Sphere").GetComponent<DetectionPulse>(); }

        if(world1 == null) { GameObject.Find("World 1"); }
        if(world1 == null) { GameObject.Find("World 2"); }

        // Controls if sound is already playing
        forwardFootStepSoundOn = false;
        horizontalFootStepSoundOn = false;

        //initial spawn point
        respawnPoint = this.transform.position;
    }

    // Update is called once per frame
    void Update() {
        
        //stop player from shifting while pulse is active
        canShift = !transform.Find("Detection Sphere").GetComponent<DetectionPulse>().expanding;
        //If you fall through the floor or press "R"
        if (this.transform.position.y < -20 || Input.GetKeyDown(KeyCode.R)) {
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


        //Slider for the UI for later use if we need it. Right now just counts down your detection pulse energy just as a proof of concept
        Slider UISlider = GUI.transform.Find("Slider").GetComponent<Slider>();
        if (dPulse.GetComponent<DetectionPulse>().infiniteEnergy) {
            UISlider.gameObject.SetActive(false);
        }else {
            UISlider.gameObject.SetActive(true);
            UISlider.value = (int)((dPulse.energy / dPulse.maxEnergy) * UISlider.maxValue);
        }

        if (GM.paused) {
            footStepSound.Stop();
        }

        if (Input.GetKeyDown(pauseKey)) {
            if (!GM.paused) {
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
        footStepSound.pitch = 1.0f;
        //movement
        // Forward and Back movement
        if (Input.GetAxis("Vertical") > 0) {
            footStepSound.pitch += Random.Range(-.2f, .2f);
            transform.localPosition += transform.forward * moveSpeed * Time.deltaTime;
            PlayFootstepSound(ref forwardFootStepSoundOn);
        }
        else if (Input.GetAxis("Vertical") < 0) {
            footStepSound.pitch += Random.Range(-.2f, .2f);
            transform.localPosition -= transform.forward * moveSpeed * Time.deltaTime;
            PlayFootstepSound(ref forwardFootStepSoundOn);
        } else{
            PauseFootstepSound(ref forwardFootStepSoundOn);
        }

        // Side to Side movement
        if (Input.GetAxis("Horizontal") > 0) {
            footStepSound.pitch += Random.Range(-.2f, .2f);
            transform.localPosition += transform.right * moveSpeed * Time.deltaTime;
            PlayFootstepSound(ref horizontalFootStepSoundOn);
        } else if (Input.GetAxis("Horizontal") < 0) {
            footStepSound.pitch += Random.Range(-.2f, .2f);
            transform.localPosition -= transform.right * moveSpeed * Time.deltaTime;
            PlayFootstepSound(ref horizontalFootStepSoundOn);
        } else {
            PauseFootstepSound(ref horizontalFootStepSoundOn);
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

    // Plays the Footstep audio
    private void PlayFootstepSound(ref bool directionBool)
    {
        if (!forwardFootStepSoundOn && !horizontalFootStepSoundOn)
        {
            footStepSound.Play();
            directionBool = true;
        }
    }

    // Pauses the FootStep audio
    private void PauseFootstepSound(ref bool directionBool)
    {
        if (directionBool)
        {
            footStepSound.Pause();
            directionBool = false;
        }
    }

}
