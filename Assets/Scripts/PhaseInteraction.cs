using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Anything that can shift needs this class
public class PhaseInteraction : MonoBehaviour {
    //determines whether or not to render an object
    public bool visible;

    //non-solid objects will be transparent and can be walked through. Solid ones will appear opaque and will collide
    public bool solid;

    private bool defaultVisibility;

    private bool defaultSolidity;

    //this is here for if any future script wants to check which world this object belongs to.
    //At the moment, this is not used for any code, but rather to just keep track of things
    public int worldAllegiance;

    public GameObject player;
    //can you click on this to do something. SEE void OnMouseOver()
    public bool clickable = true;

    public float solidMaterialAlpha = 1.0f;
    //when this.solid = false, show texture slightly transparent
    public float transparentMaterialAlpha = 0.5f;

	// Use this for initialization
	void Start () {
        if (GetComponent<Renderer>()) {
            CheckVisbility();
        }
        defaultSolidity = solid;
        defaultVisibility = visible;
        player = GameObject.Find("Player");
    }
	
	// Update is called once per frame
	void Update () {
        if (GetComponent<Renderer>()) {
            CheckVisbility();
        }
    }

    //trigger fires when you mouse over something that has a collider
    void OnMouseOver() {
        //if you hover over something that you can see and click on
        if (clickable && this.visible) {
            //let player know they can interact with it
            player.GetComponent<Player>().aimingAtInteractibleThing = true;
            if (Input.GetMouseButtonDown(0)) {
                print(this.gameObject.name+": click change");
                toggleMySolidity();
            }
            if (Input.GetMouseButtonDown(1)) {
                print("clock change children of: "+this.gameObject.name);
                StartCoroutine(toggleChildrenSolidity(0.5f));
            }
        } else {
            player.GetComponent<Player>().aimingAtInteractibleThing = false;
        }
    }

    void OnMouseExit() {
        player.GetComponent<Player>().aimingAtInteractibleThing = false;
    }

    //change transparency of this material
    public void ChangeAlpha(Material mat, float alphaValue) {
        Color oldColor = mat.color;
        Color newColor = new Color(oldColor.r, oldColor.g, oldColor.b, alphaValue);
        mat.SetColor("_Color", newColor);
    }

    //determine whether this object should be [not rendered vs solid] or [transparent vs full]
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
            ChangeAlpha(GetComponent<Renderer>().material, solidMaterialAlpha);

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
            ChangeAlpha(GetComponent<Renderer>().material, transparentMaterialAlpha);

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

    public void setDefaultSolidity() {
        this.solid = defaultSolidity;
    }

    public void setDefaultVisibility() {
        this.visible = defaultVisibility;
    }

    public void setNonDefaultSolidity() {
        this.solid = !defaultSolidity;
    }

    public void setNonDefaultVisibility() {
        this.visible = !defaultVisibility;
    }

    //modify the solidity and visibility of this
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

    //for all the children of this object, toggle their solidity one by one
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

    //for all children of this object, modify visibility one by one
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





    void OnTriggerEnter(Collider col) {
        //if the player is in this object's trigger collider, stop them from shifting. This is to prevent the player from getting stuck
        if (col.tag == "Player") {
            col.gameObject.GetComponent<Player>().canShift = false;
        }
    }

    void OnTriggerStay(Collider col) {
        //if the player stays in this object's trigger collider, stop them from shifting. This is to help prevent edge cases
        if (col.tag == "Player") {
            col.gameObject.GetComponent<Player>().canShift = false;
        }
    }

    void OnTriggerExit(Collider col) {
        if (col.tag == "Player") {
            //if the player is not in this object's trigger collider, re-enable shifting. This is because they are not in danger of trapping themselves or falling
            col.gameObject.GetComponentInChildren<Player>().canShift = true;
        }
    }
}
