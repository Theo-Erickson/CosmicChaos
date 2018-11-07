using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public Text scannerType;
    public GameObject player;

    private void Awake()
    {
        LockCursor();
    }

    void Start() {
    }

    void Update() {    
    }

    private void OnApplicationFocus(bool focus)
    {
        if(focus == true)
        {
            LockCursor();
        }
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;
    }
}
