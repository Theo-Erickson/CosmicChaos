﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingDoorTrigger : MonoBehaviour {
    // This script should be attatched to a trigger. 
    // It causes a given object to fall down to a set y position when triggered
    [SerializeField] private Transform fallingObject;
    [SerializeField] private float finalHeight;
    [SerializeField] private float fallingSpeed;

    private bool doorStartFalling = false;

    private void Update()
    {
        if (doorStartFalling && fallingObject.position.y > finalHeight)
        {
            fallingObject.Translate(Vector3.down * fallingSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            doorStartFalling = true;
        }
    }
}