using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public GameObject player;
    public GameObject GUI;

    public static GameManager instance = null;

    public bool paused = false;

    private Player playerScript;

    void Awake() {
        /*
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(this.gameObject);
        }
        */
        if (player == null) { GameObject.Find("Player"); }
        playerScript = player.GetComponent<Player>();
        if (GUI == null) { GameObject.Find("GUI"); }
    }

    void Start() {
        Object.DontDestroyOnLoad(this.gameObject);
    }

    void Update() {
        LockCursor();
        changeCursor();
    }

    public void LockCursor() {
        if (!paused) {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        } else {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            paused = !paused;
        }
        if (paused) {
            Time.timeScale = 0;
        } else {
            Time.timeScale = 1;
        }
    }

    private void OnApplicationFocus(bool focus) {
        if (focus == true) {
            LockCursor();
        }
    }

    public void changeCursor() {
        Image mainCursor = GUI.transform.Find("Reticle").GetComponent<Image>();
        Image XCursor = GUI.transform.Find("X Overlay").GetComponent<Image>();
        /* OLD CURSORS
        if (playerScript.currentWorld == 1 && !playerPulse.expanding) {
            mainCursor.sprite = Resources.Load<Sprite>("Cursors/Cursor 1");
        } else if (playerScript.currentWorld == 1 && playerPulse.expanding) {
            mainCursor.sprite = Resources.Load<Sprite>("Cursors/Cursor 2");
        } else if (playerScript.currentWorld == 2 && !playerPulse.expanding) {
            mainCursor.sprite = Resources.Load<Sprite>("Cursors/Cursor 3");
        } else if (playerScript.currentWorld == 2 && playerPulse.expanding) {
            mainCursor.sprite = Resources.Load<Sprite>("Cursors/Cursor 4");
        }
      
        if (playerScript.aimingAtInteractibleThing) {
            XCursor.enabled = false;
        } else {
            XCursor.enabled = true;
        }
        */

        if (playerScript.currentWorld == 1) {
            LoadAnimCursor(mainCursor, "NormalIdle", 8);
        //    LoadAnimCursor(mainCursor, "NormalScanner", 8);
        } else if (playerScript.currentWorld == 2) {
            LoadAnimCursor(mainCursor, "OtherIdle", 8);
        //    LoadAnimCursor(mainCursor, "OtherScanner", 8);
        }

        if (playerScript.currentWorld == 1 && !playerScript.aimingAtInteractibleThing) {
            LoadAnimCursor(XCursor, "NormalUnInteract", 8);
        } else if (playerScript.currentWorld == 1 && playerScript.aimingAtInteractibleThing) {
            LoadAnimCursor(XCursor, "NormalInteract", 8);
        } else if (playerScript.currentWorld == 2 && !playerScript.aimingAtInteractibleThing) {
            LoadAnimCursor(XCursor, "OtherUniteract", 8);
        } else if (playerScript.currentWorld == 2 && playerScript.aimingAtInteractibleThing) {
            LoadAnimCursor(XCursor, "OtherInteract", 8);
        }
    }

    private void LoadAnimCursor(Image cursor, string name, int numberOfFrames) {
        for (int i = 0; i < numberOfFrames; i++) {
            cursor.GetComponent<AnimatedCursor>().frames[i] = Resources.Load<Sprite>("Cursors/"+name+"/" + i);
        }
    }
}