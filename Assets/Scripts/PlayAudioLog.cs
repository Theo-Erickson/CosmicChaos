using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioLog : MonoBehaviour {
    // This script takes an Audio Clip, plays it, and then destroys he object.
    // The script is only played when the interact button is pressed while moused over


    [SerializeField] private KeyCode interactButton;     // Button to be pressed
    [SerializeField] private AudioClip audioLog;         // Audio Clip
    [SerializeField] private Transform playerTransform;  // Player Reference
    [SerializeField] private float playerDistFromSwitch; // Distance player can interact with object

    private bool playedAudioLog;

    private void Start()
    {
        playedAudioLog = false;
    }

    private void OnMouseOver()
    {
        PlayAudioLogWhenSelected();
    }

    private void PlayAudioLogWhenSelected()
    {
        if (Input.GetKey(interactButton) && (Vector3.Distance(playerTransform.position, transform.position) <= playerDistFromSwitch))
        {
            //player shifting worlds is disabled until they pick up an audio log
            if (GameObject.Find("Player").GetComponent<Player>().canShift == false) {
                GameObject.Find("Player").GetComponent<Player>().canShift = true;
                GameObject.Find("Player").GetComponent<Player>().ShiftWorlds();
                GameObject.Find("Player").GetComponent<Player>().DisplayFadingText("Middle", "SHIFT", 6.0f);
            }
            if (!playedAudioLog)
            {
                print(audioLog.name);
                AudioScript.instance.playSFX(audioLog.name);

                Destroy(this.gameObject);

                playedAudioLog = true;
            }

        }
    }
}
