using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSwitch : MonoBehaviour {
    public float maxTurn = 180f;
    public float rotateSpeed = 1.0f;
    public float progress = 0.0f;

    public GameObject Door;

    void OnTriggerStay(Collider col) {
        if (Input.GetKey(KeyCode.Space)) {
            if(progress < maxTurn) {
                progress++;
                this.transform.Rotate(new Vector3(0f, 0f, rotateSpeed));
                Door.transform.position = new Vector3(Door.transform.position.x, Door.transform.position.y + rotateSpeed/60, Door.transform.position.z);
            }
        } else {
            if(progress > 0) {
                progress--;
                this.transform.Rotate(new Vector3(0f, 0f, -rotateSpeed));
                Door.transform.position = new Vector3(Door.transform.position.x, Door.transform.position.y - rotateSpeed/60, Door.transform.position.z);
            }
        }
    }
}
