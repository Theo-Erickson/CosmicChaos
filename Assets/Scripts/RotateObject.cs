using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour {
    public bool xRotate;
    public bool yRotate;
    public bool zRotate;

    public bool moving = true;
    public bool clockwise = true;
    public Vector3 rotationSpeed = new Vector3(1,1,1);

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        if (moving) {
            if (xRotate && yRotate && zRotate) {
                if (clockwise) {
                    this.transform.Rotate(new Vector3(-rotationSpeed.x, -rotationSpeed.y, -rotationSpeed.z));
                } else {
                    this.transform.Rotate(new Vector3(rotationSpeed.x, rotationSpeed.y, rotationSpeed.z));
                }


            } else if (xRotate && yRotate) {
                if (clockwise) {
                    this.transform.Rotate(new Vector3(-rotationSpeed.x, -rotationSpeed.y, 0));
                } else {
                    this.transform.Rotate(new Vector3(rotationSpeed.x, rotationSpeed.y, 0));
                }

            } else if (xRotate && zRotate) {
                if (clockwise) {
                    this.transform.Rotate(new Vector3(-rotationSpeed.x, 0, -rotationSpeed.z));
                } else {
                    this.transform.Rotate(new Vector3(rotationSpeed.x, 0, rotationSpeed.z));
                }


            } else if (yRotate && zRotate) {
                if (clockwise) {
                    this.transform.Rotate(new Vector3(0, -rotationSpeed.y, -rotationSpeed.z));
                } else {
                    this.transform.Rotate(new Vector3(0, rotationSpeed.y, rotationSpeed.z));
                }

            } else if (xRotate) {
                if (clockwise) {
                    this.transform.Rotate(new Vector3(-rotationSpeed.x, 0, 0));
                } else {
                    this.transform.Rotate(new Vector3(rotationSpeed.x, 0, 0));
                }

            } else if (yRotate) {
                if (clockwise) {
                    this.transform.Rotate(new Vector3(0, -rotationSpeed.y, 0));
                } else {
                    this.transform.Rotate(new Vector3(0, rotationSpeed.y, 0));
                }
            }else if (zRotate) {
                if (clockwise) {
                    this.transform.Rotate(new Vector3(0, 0, -rotationSpeed.z));
                } else {
                    this.transform.Rotate(new Vector3(0, 0, rotationSpeed.z));
                }
            }
        }
	}
    public void OnTriggerStay(Collider col) {
        if (col.gameObject.tag == "Player") {
            col.transform.parent = transform;
        }
    }

    public void OnTriggerExit(Collider col) {
        if (col.gameObject.tag == "Player") {
            col.transform.parent = null;

        }
    }
}
