using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public GameObject [] bots;
    public int controlBot = 0;
    public GameObject cam;

    void Start() {
        bots[0].GetComponentInChildren<Camera>().enabled = true;
        bots[0].GetComponentInChildren<LookAtMouse>().enabled = true;
        bots[0].GetComponentInChildren<Player>().enabled = true;

        bots[1].GetComponentInChildren<Camera>().enabled = false;
        bots[1].GetComponentInChildren<LookAtMouse>().enabled = false;
        bots[1].GetComponentInChildren<Player>().enabled = false;

    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.F)) {
            controlBot++;
            controlBot = controlBot % bots.Length; 

            for (int i = 0; i < bots.Length; i++) {
                bots[i].GetComponentInChildren<Camera>().enabled = false;
                bots[i].GetComponentInChildren<LookAtMouse>().enabled = false;
                bots[i].GetComponentInChildren<Player>().enabled = false;

            }

            enableBot(controlBot);
        }
    }


    void enableBot(int controlBot) {
        bots[controlBot].GetComponentInChildren<Camera>().enabled = true;
        bots[controlBot].GetComponentInChildren<LookAtMouse>().enabled = true;
        bots[controlBot].GetComponentInChildren<Player>().enabled = true ;

    }
}
