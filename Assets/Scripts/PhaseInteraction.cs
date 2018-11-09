using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

//Anything that can shift needs this class
public class PhaseInteraction : MonoBehaviour {
    public enum ObjectType { Scenery, Entity, Root};
    [Header("Phased Object Type")]
    public ObjectType Type = ObjectType.Scenery;
    [Tooltip("This is here for if any future script wants to check which world this object belongs to."+ 
             "At the moment, this is not used for any code, but rather to just keep track of things")]
    public int worldAllegiance;
    [Tooltip("can you click on this to do something. SEE void OnMouseOver()")]
    public bool clickable = true;

    [Header("Visibility/Rendering")]
    [Tooltip("Determines whether or not to render an object")]
    public bool visible;
    [Tooltip("Non-solid objects will appear partially transparent, see transparentMaterialAlpha")]
    public bool solid;
    [Tooltip("Whether an object can be walked through")]
    public bool collides;
    public float solidMaterialAlpha = 1.0f;
    [Tooltip("when this.solid = false, show texture slightly transparent")]
    public float transparentMaterialAlpha = 0.5f;


    [Header("Player Reference")]
    public GameObject player;



    private bool defaultVisibility;
    private bool defaultSolidity;
    private bool defaultCollision;


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
                ToggleMyself();
            }
            if (Input.GetMouseButtonDown(1)) {
                ToggleChildren();
                ToggleMyself();
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
        } else {
            ChangeAlpha(GetComponent<Renderer>().material, transparentMaterialAlpha);
        }


        //can you collide with the object
        if (collides) {
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

    public void ToggleChildrenBoth(bool forward) {
        ToggleChildrenSolidity();
        ToggleChildrenVisibility();
    }

    //for all children of this object, make the world shift
    //Scenery: make them invisible and nonColiding
    //Enemy: if they are close to player, bring them to your new world, otherwise make them invisible and non-coliding  
    public void ToggleChildren() {
        if (this.transform.childCount > 0) {
            List<GameObject> children = new List<GameObject>();
            for (int i = 0; i < this.transform.childCount; i++) {
                children.Add(this.transform.GetChild(i).gameObject);
                if (children[i].transform.childCount > 0) {
                    children[i].GetComponent<PhaseInteraction>().ToggleChildren();
                }
                if (children[i].GetComponent<PhaseInteraction>() != null) {
                    children[i].GetComponent<PhaseInteraction>().ToggleMyself();
                }
            }
        } else {
            ToggleMyself();
            //Toggle your Visibility
        }
        
    }


    //for all the children of this object, Toggle their solidity one by one
    public void ToggleChildrenSolidity() {

        if (this.transform.childCount > 0) {
            List<GameObject> children = new List<GameObject>();
            for (int i = 0; i < this.transform.childCount; i++) {
                children.Add(this.transform.GetChild(i).gameObject);
                if (children[i].transform.childCount > 0) {
                    children[i].GetComponent<PhaseInteraction>().ToggleChildrenSolidity();
                }
                if (children[i].GetComponent<PhaseInteraction>() != null) {
                    children[i].GetComponent<PhaseInteraction>().ToggleMySolidityOverTime(0.2f);
                }
            }
        } else {
            StartCoroutine(ToggleMySolidityOverTime(0.2f));
            //Toggle your Solidity
        }
    }

    //for all children of this object, modify visibility one by one
    public void ToggleChildrenVisibility() {
        if (this.transform.childCount > 0) {
            List<GameObject> children = new List<GameObject>();
            for (int i = 0; i < this.transform.childCount; i++) {
                children.Add(this.transform.GetChild(i).gameObject);
                if (children[i].transform.childCount > 0) {
                    children[i].GetComponent<PhaseInteraction>().ToggleChildrenVisibility();
                }
                if (children[i].GetComponent<PhaseInteraction>() != null) {
                    children[i].GetComponent<PhaseInteraction>().ToggleMyVisibility();
                }
            }
        } else {
            ToggleMyVisibility();
            //Toggle your Visibility
        }
    }

    //if reveal: show what a world change would look like, ELSE: disable previewing
    public void PeekWorldChange(bool previewing) {
        if (previewing) {
            //if the world that THIS object belongs to is the same as the player, make it non solid but still visible and collision. IE you are in world 1 and this block is also in world 1
            if (this.worldAllegiance == player.GetComponent<Player>().currentWorld) {
                print("peek my world solidity toggle");
                setSolidity(false);
                //everything in other world should become visible
            } else {
                print("peek other world visbility toggle");
                setVisibility(true);
            }
        } else {
            //if the world that THIS object belongs to is the same as the player, make it non solid but still visible and collision. IE you are in world 1 and this block is also in world 1
            if (this.worldAllegiance == player.GetComponent<Player>().currentWorld) {
                print("peek my world solidity toggle");
                setSolidity(true);
                //everything in other world should become visible
            } else {
                print("peek other world visbility toggle");
                setVisibility(false);
            }
        }
    }


    //Make whatever changes you need to as a response to the world shift for your object type
    //Scenery: make them invisible and nonColiding
    //Enemy: if they are close to player, bring them to your new world, otherwise make them invisible and non-coliding  
    public void ToggleMyself() {
        if(worldAllegiance == 1) { worldAllegiance = 2; } else { worldAllegiance = 1; }
        if(this.Type == ObjectType.Scenery) {
            //print("ToggleMyself Scenery");
            ToggleMyVisibility();
            ToggleMyCollision();
        }else if (this.Type == ObjectType.Entity) {
            ToggleMyVisibility();
            ToggleMyCollision();
            //handled in the enemy Step Forward Code
        }else if(this.Type == ObjectType.Root) {
            //do nothing lol
        }
    }

    public void ToggleBoth() {
        ToggleMySolidity();
        ToggleMyVisibility();
    }


    public void ToggleMySolidity() {
        this.solid = !this.solid;
    }

    public void ToggleMyVisibility() {
        this.visible = !this.visible;
    }

    public void ToggleMyCollision() {
        this.collides = !this.collides;
    }


    public IEnumerator ToggleMySolidityOverTime(float delay) {
        yield return new WaitForSeconds(delay);
        this.solid = !this.solid;
    }

    public IEnumerator ToggleMyVisibilityOverTime(float delay) {
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
