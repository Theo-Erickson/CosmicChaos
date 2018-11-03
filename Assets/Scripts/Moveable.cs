using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moveable : MonoBehaviour {
    [TextArea]
    public string Notes = "Handles objects going towards a set position. " +
        "\n Best used for pulling things towards you";

    public bool moving;
    public Vector3 destination;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (moving) {
            this.transform.position = Vector3.MoveTowards(this.transform.position, destination, 1.5f);
            if (Vector3.Distance(this.transform.position, destination) < this.transform.localScale.x + 1.0) {
                moving = false;
            }

        }
    }
}
