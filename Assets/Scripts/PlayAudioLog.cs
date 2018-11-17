using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioLog : MonoBehaviour {

    [SerializeField] private KeyCode interactButton;
    private AudioSource audioLog;

    private void Start()
    {
        audioLog = GetComponent<AudioSource>();
    }
}
