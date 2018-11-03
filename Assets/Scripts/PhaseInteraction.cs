using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseInteraction : MonoBehaviour {
    public bool visible;
    public GameObject player;
    public bool enableOnClick = true;

	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnMouseOver() {
        if (Input.GetMouseButtonDown(0)) {
            if (enableOnClick) {
                changePhase();
            }
        }
        if (Input.GetMouseButtonDown(1)) {
            print(this.gameObject.name);
        }
    }

    public void changeMyPhase() {
        print("change my phase");
        this.GetComponent<Renderer>().enabled = !this.GetComponent<Renderer>().enabled;
    }

    public void changePhase() {
        print("change phase: " + this.transform.name);
        List<GameObject> children = new List<GameObject>();

        //get all the children of the otherWorld.
        for (int i = 0; i < this.transform.parent.childCount; i++) {
            children.Add(this.transform.parent.GetChild(i).gameObject);
        }

        if (visible) {
            //disable from top to bottom
            StartCoroutine(toggleObjectsOverTime(children, true, visible, 0.2f));
        } else {
            //disable from bottom to top
            StartCoroutine(toggleObjectsOverTime(children, false, visible, 0.2f));
        }
        visible = !visible;
    }

    public IEnumerator toggleObjectsOverTime(List<GameObject> objects, bool forward, bool enable, float decayTime) {
        yield return new WaitForSeconds(decayTime);
        if (forward) {
            if (objects[0].GetComponent<MeshRenderer>() != null) {
                objects[0].GetComponent<MeshRenderer>().enabled = enable;
            }
            if (objects.Count > 1) {
                objects.RemoveAt(0);
                StartCoroutine(toggleObjectsOverTime(objects, forward, enable, decayTime));
            }
        } else {
            if (objects[objects.Count - 1].GetComponent<MeshRenderer>() != null) {
                objects[objects.Count - 1].GetComponent<MeshRenderer>().enabled = enable;
            }
            if (objects.Count > 1) {
                objects.RemoveAt(objects.Count - 1);
                StartCoroutine(toggleObjectsOverTime(objects, forward, enable, decayTime));
            }
        }
    }



}
