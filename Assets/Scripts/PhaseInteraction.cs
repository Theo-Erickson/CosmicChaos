using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseInteraction : MonoBehaviour {
    public bool visible;
    public bool solid;
    public Material solidMaterial;
    public Material nonSolidMaterial;

    public GameObject player;
    public bool enableOnClick = true;

	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player");
    }
	
	// Update is called once per frame
	void Update () {
        CheckVisbility();
    }

    void OnMouseOver() {
        if (Input.GetMouseButtonDown(0)) {
            if (enableOnClick) {
                print("toggle solid");
                toggleMySolidity();
            }
        }
        if (Input.GetMouseButtonDown(1)) {
            if (enableOnClick) {
                print("toggle child solidity");
                toggleChildrenSolidity(true);
            }
        }
    }


    public void CheckVisbility() {
        //can it be seen
        if (visible) {
            this.GetComponent<Renderer>().enabled = true;
            //can't be seen
        } else {
            GetComponent<Renderer>().enabled = false;
        }

        //can it be touched?
        //NOTE: For clicking interactions, we need a collider of some sort, hence why we are setting the colliders to triggers and not disabling
        if (solid) {
            GetComponent<Renderer>().material = solidMaterial;

            if (this.GetComponent<BoxCollider>()) {
                GetComponent<BoxCollider>().isTrigger = false;
            }
            if (this.GetComponent<SphereCollider>()) {
                GetComponent<SphereCollider>().isTrigger = false;
            }
            if (this.GetComponent<CapsuleCollider>()) {
                GetComponent<CapsuleCollider>().isTrigger = false;
            }

        } else {
            GetComponent<Renderer>().material = nonSolidMaterial;

            if (this.GetComponent<BoxCollider>()) {
                GetComponent<BoxCollider>().isTrigger = true;
            }
            if (this.GetComponent<SphereCollider>()) {
                GetComponent<SphereCollider>().isTrigger = true;
            }
            if (this.GetComponent<CapsuleCollider>()) {
                GetComponent<CapsuleCollider>().isTrigger = true;
            }
        }
    }

    public void setBoth(bool solidity, bool visibility) {
        setSolidity(solidity);
        setVisibility(visible);
    }

    public void setSolidity(bool solid) {
        this.solid = solid;
    }

    public void setVisibility(bool visible) {
        this.visible = visible;
    }

    public void toggleBoth() {
        toggleMySolidity();
        toggleMyVisibility();
    }


    public void toggleMySolidity() {
        this.solid = !this.solid;
    }

    public void toggleMyVisibility() {
        this.visible = !this.visible;
    }


    public void toggleChildrenBoth(bool forward) {
        toggleChildrenSolidity(forward);
        toggleChildrenVisibility(forward);
    }

    public void toggleChildrenVisibility(bool forward) {
        print("change children visibility phase: " + this.transform.name);
        //if this object has no parent
        if (this.transform.parent == null) {
            //this has no parent and no children. IE it is a singleton
            if (this.transform.childCount == 0) {
                toggleMyVisibility();
            //this IS the parent AND it has children. WHEN IT COMES TO THESE OBJECTS... YOU ARE THE FATHER!!!
            } else {
                List<GameObject> children = new List<GameObject>();
                for (int i = 0; i < this.transform.childCount; i++) {
                    children.Add(this.transform.GetChild(i).gameObject);
                }

                //change them all one by one
                StartCoroutine(toggleObjectsVisibilityOverTime(children, forward, 0.2f));

            }

        //you do have a parent
        } else {
            //get all other objects in grouped with it
            List<GameObject> children = new List<GameObject>();
            for (int i = 0; i < this.transform.parent.childCount; i++) {
                children.Add(this.transform.parent.GetChild(i).gameObject);
            }

            //change them all one by one
            StartCoroutine(toggleObjectsVisibilityOverTime(children, forward, 0.2f));
        }
    }

    public void toggleChildrenSolidity(bool forward) {
        //print("change children solidity phase: " + this.transform.name);
        //if thos object has no parent
        if (this.transform.parent == null) {
            //no parent no children
            if (this.transform.childCount == 0) {
                print("change my solidity");
                StartCoroutine(toggleMySolidityOverTime(0.2f));
                //you have children
            } else {
                List<GameObject> children = new List<GameObject>();
                //get all other objects grouped with it
                for (int i = 0; i < this.transform.childCount; i++) {
                    children.Add(this.transform.GetChild(i).gameObject);
                    if (children[i].transform.childCount > 0) {
                        for (int k = 0; k < children[i].transform.childCount; k++) {
                            if (children[i].transform.GetChild(k).GetComponent<PhaseInteraction>() != null) {
                                children[i].transform.GetChild(k).GetComponent<PhaseInteraction>().toggleChildrenSolidity(forward);
                            }
                        }
                    }
                }
                StartCoroutine(toggleObjectsSolidityOverTime(children, forward, 0.2f));
            }

        } else {
            
            List<GameObject> children = new List<GameObject>();
            //get all other objects grouped with it
            for (int i = 0; i < this.transform.parent.childCount; i++) {
                children.Add(this.transform.parent.GetChild(i).gameObject);
                if (children[i].transform.childCount > 0) {
                    for (int k = 0; k < children[i].transform.childCount; k++) {
                        if (children[i].transform.GetChild(k).GetComponent<PhaseInteraction>() != null) {
                           children[i].transform.GetChild(k).GetComponent<PhaseInteraction>().toggleChildrenSolidity(forward);
                        }
                    }
                }
            }
            StartCoroutine(toggleObjectsSolidityOverTime(children, forward, 0.2f));

            /*
            if (this.transform.childCount == 0) {
                print("do I get here");
                StartCoroutine(toggleMySolidityOverTime(0.2f));
            }
            */

        }
    }


    public IEnumerator toggleMySolidityOverTime(float delay) {
        yield return new WaitForSeconds(delay);
        toggleMySolidity();
    }

    public IEnumerator toggleMyVisibilityOverTime(float delay) {
        yield return new WaitForSeconds(delay);
        toggleMyVisibility();
    }

    public IEnumerator toggleObjectsSolidityOverTime(List<GameObject> objects, bool forward, float decayTime) {
        yield return new WaitForSeconds(decayTime);
        if (forward) {
            if (objects[0].GetComponent<PhaseInteraction>() != null) {
                objects[0].GetComponent<PhaseInteraction>().solid = !objects[0].GetComponent<PhaseInteraction>().solid;
            }
            //recursion
            if (objects.Count > 1) {
                objects.RemoveAt(0);
                StartCoroutine(toggleObjectsSolidityOverTime(objects, forward, decayTime));
            }
        } else {
            if (objects[objects.Count - 1].GetComponent<PhaseInteraction>() != null) {
                objects[objects.Count - 1].GetComponent<PhaseInteraction>().solid = !objects[objects.Count - 1].GetComponent<PhaseInteraction>().solid;
            }
            //recursion
            if (objects.Count > 1) {
                objects.RemoveAt(objects.Count - 1);
                StartCoroutine(toggleObjectsSolidityOverTime(objects, forward, decayTime));
            }
        }
    }

    public IEnumerator toggleObjectsVisibilityOverTime(List<GameObject> objects, bool forward, float decayTime) {
        yield return new WaitForSeconds(decayTime);
        if (forward) {
            if (objects[0].GetComponent<PhaseInteraction>() != null) {
                objects[0].GetComponent<PhaseInteraction>().visible = !objects[0].GetComponent<PhaseInteraction>().visible;
            }
            //recursion
            if (objects.Count > 1) {
                objects.RemoveAt(0);
                StartCoroutine(toggleObjectsVisibilityOverTime(objects, forward, decayTime));
            }
        } else {
            if (objects[objects.Count - 1].GetComponent<PhaseInteraction>() != null) {
                objects[objects.Count - 1].GetComponent<PhaseInteraction>().visible = !objects[objects.Count - 1].GetComponent<PhaseInteraction>().visible;
            }
            //recursion
            if (objects.Count > 1) {
                objects.RemoveAt(objects.Count - 1);
                StartCoroutine(toggleObjectsVisibilityOverTime(objects, forward, decayTime));
            }
        }
    }



}
