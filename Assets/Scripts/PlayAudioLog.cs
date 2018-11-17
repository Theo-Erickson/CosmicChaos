using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioLog : MonoBehaviour {

    [SerializeField] private KeyCode interactButton;
    [SerializeField] private AudioClip audioLog;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float playerDistFromSwitch;

    private bool playedAudioLog;

    private void Start()
    {
        playedAudioLog = false;
    }

    private void OnMouseOver()
    {
        if(Input.GetKey(interactButton) && (Vector3.Distance(playerTransform.position, transform.position) <= playerDistFromSwitch))
        {
            if (!playedAudioLog)
            {
                print(audioLog.name);
                AudioScript.instance.playSFX(audioLog.name);

                playedAudioLog = true;
            }
            
        }
    }
}
