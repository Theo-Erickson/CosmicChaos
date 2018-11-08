using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseEnemy : MonoBehaviour {
    public bool active;
    public int world;
    public int defaultWorld;

    [Header("Phased Enemy Properties")]
    public bool returnWithPlayer;
    [Tooltip(" > 2 is suggested")]public float maxReturnDistance;
    public GameObject player;

    private Player playerScript;

	// Use this for initialization
	void Start () {
		if(player == null) { player = GameObject.Find("Player");}
        playerScript = player.GetComponent<Player>();
    }
	
	// Update is called once per frame
	void Update () {
        print(Vector3.Distance(this.transform.position, player.transform.position));
        //hunt player in the same world
        if (world == player.GetComponent<Player>().currentWorld && active) {
            this.transform.position = Vector3.MoveTowards(this.transform.position, player.transform.position, 0.01f);
        }

        //go along with player if they are close enough
        if (Input.GetKeyDown(KeyCode.LeftShift) && playerScript.canShift && Vector3.Distance(this.transform.position, player.transform.position) < maxReturnDistance) {
            swapWorld();
        } 
	}


    public void swapWorld() {
        if (world == 1) { world = 2; } else { world = 1; }
        if (world == 1) this.transform.parent = playerScript.world1.transform;
        if (world == 2) this.transform.parent = playerScript.world2.transform;
    }
}
