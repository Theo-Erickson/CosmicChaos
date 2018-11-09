using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseEnemy : MonoBehaviour {
    public bool active;
    public int world;
    public int defaultWorld;

    public GameObject aggroSpace;

    [Header("Phased Enemy Properties")]
    public bool returnWithPlayer;
    [Tooltip(" > 2 is suggested")]public float maxReturnDistance;
    [Tooltip(" < 2 is suggested")] public float damageDistance;
    public GameObject player;

    private Player playerScript;
    private bool playerInAggroSpace;

    // Use this for initialization
    void Start () {
		if(player == null) { player = GameObject.Find("Player");}
        playerScript = player.GetComponent<Player>();
    }
	
	// Update is called once per frame
	void Update () {
        playerInAggroSpace = aggroSpace.GetComponent<AggroSpace>().playerInside;
        if (playerInAggroSpace)
        {
            print(Vector3.Distance(this.transform.position, player.transform.position));
            //hunt player in the same world
            if (world == player.GetComponent<Player>().currentWorld && active)
            {
                this.transform.position = Vector3.MoveTowards(this.transform.position, player.transform.position, 0.01f);
            }

            //go along with player if they are close enough
            if (Input.GetKeyDown(KeyCode.LeftShift) && playerScript.canShift && Vector3.Distance(this.transform.position, player.transform.position) < maxReturnDistance && returnWithPlayer)
            {
                swapWorld();
            }
            //do damage if very close to the player
            if(Vector3.Distance(this.transform.position, player.transform.position) < damageDistance)
            {
                doDamage();
            }
        }
	}


    public void swapWorld() {
        if (world == 1) { world = 2; } else { world = 1; }
        if (world == 1) this.transform.parent = playerScript.world1.transform;
        if (world == 2) this.transform.parent = playerScript.world2.transform;
    }

    public void doDamage()
    {
        print("feel sanity dropping");
    }
}
