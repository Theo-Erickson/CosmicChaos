using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseInteraction : MonoBehaviour {
    public bool visible;
    public bool solid;
    
    public GameObject player;
    public bool clickable = true;

    public float solidMaterialAlpha = 1.0f;
    public float transparentMaterialAlpha = 0.5f;

    private float oldMaterialAlpha; 

	// Use this for initialization
	void Start () {
        CheckVisbility();
        player = GameObject.Find("Player");
    }
	
	// Update is called once per frame
	void Update () {
        CheckVisbility();
    }

    void OnMouseOver() {
        if (Input.GetMouseButtonDown(0)) {
            if (clickable) {
                print("toggle solid");
                toggleMySolidity();
            }
        }
        if (Input.GetMouseButtonDown(1)) {
            if (clickable) {
                print("toggle child solidity");
                StartCoroutine(toggleChildrenSolidity(0.5f));
            }
        }
    }

    public void ChangeAlpha(Material mat, float alphaValue) {
        Color oldColor = mat.color;
        Color newColor = new Color(oldColor.r, oldColor.g, oldColor.b, alphaValue);
        mat.SetColor("_Color", newColor);
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
            ChangeAlpha(GetComponent<Renderer>().material, transparentMaterialAlpha);

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
            ChangeAlpha(GetComponent<Renderer>().material, solidMaterialAlpha);

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
        StartCoroutine(toggleChildrenSolidity(0.5f));
        toggleChildrenVisibility();
    }


    public IEnumerator toggleChildrenSolidity(float delay) {
        yield return new WaitForSeconds(delay);
        if (this.transform.childCount > 0) {
            List<GameObject> children = new List<GameObject>();
            for (int i = 0; i < this.transform.childCount; i++) {
                children.Add(this.transform.GetChild(i).gameObject);
                if (children[i].transform.childCount > 0) {
                    StartCoroutine(children[i].GetComponent<PhaseInteraction>().toggleChildrenSolidity(0.5f));
                }
                if (children[i].GetComponent<PhaseInteraction>() != null) {
                    StartCoroutine(children[i].GetComponent<PhaseInteraction>().toggleMySolidityOverTime(0.2f));
                }
            }
        } else {
            StartCoroutine(toggleMySolidityOverTime(0.2f));
            //toggle your Solidity
        }
    }

    public void toggleChildrenVisibility() {
        if (this.transform.childCount > 0) {
            List<GameObject> children = new List<GameObject>();
            for (int i = 0; i < this.transform.childCount; i++) {
                children.Add(this.transform.GetChild(i).gameObject);
                if (children[i].transform.childCount > 0) {
                    children[i].GetComponent<PhaseInteraction>().toggleChildrenVisibility();
                }
                if (children[i].GetComponent<PhaseInteraction>() != null) {
                    StartCoroutine(children[i].GetComponent<PhaseInteraction>().toggleMyVisibilityOverTime(0.2f));
                }
            }
        } else {
            StartCoroutine(toggleMyVisibilityOverTime(0.2f));
            //toggle your Solidity
        }
    }

    public IEnumerator toggleMySolidityOverTime(float delay) {
        yield return new WaitForSeconds(delay);
        this.solid = !this.solid;
    }

    public IEnumerator toggleMyVisibilityOverTime(float delay) {
        yield return new WaitForSeconds(delay);
        this.visible = !this.visible;
    }
}
