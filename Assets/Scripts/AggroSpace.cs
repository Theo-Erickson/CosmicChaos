/*AggroSpace.cs
 * Script for the AggroSpace of an enemy
 * Scriot should be applied on an empty object with a
 * large collider taking up an entire room/zone
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggroSpace : MonoBehaviour{
    public GameObject player;
    public bool playerInside = false;

    void Start() {
        player = GameObject.Find("Player");
    }

    private void OnTriggerStay(Collider other) {
        if (other.tag == "Player") {
            playerInside = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.tag == "Player") {
            playerInside = false;
        }
    }
}