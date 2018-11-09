/*EnemyStepForward.cs
 * Script for basic enemy AI (they move straight towards the player
 * if player is within ts aggro space)
 * Contain no pathfinding, best used on enemies in areas with no obstacle
 * walls or who ignore walls.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStepForward : MonoBehaviour {
    [SerializeField] private GameObject aggroSpace; //An empty object with a large box collider.
    [SerializeField] private float speed = 1; //rate entity moves towards player

    public bool active = true;
    public int world = 1;
    public int defaultWorld = 1;

    [Header("Phased Enemy Properties")]
    public bool returnWithPlayer = false;
    [Tooltip(" > 2 is suggested")] public float maxReturnDistance = 5;
    public GameObject player;



    private Player playerScript;

    private float minDistanceFromPlayer = 1.5f; //Without this will move inside player/push player around

    private float step; //speed * deltatime, so speed can be nice whole numbers

    bool playerInAggroSpace;
    float distanceFromPlayer;
    int playerWorld;



    private void Start(){
        if (player == null) { player = GameObject.Find("Player"); }
        playerScript = player.GetComponent<Player>();
        step = speed * Time.deltaTime;
        if(returnWithPlayer && maxReturnDistance <= 0) {
            print("WARNING: " + this.gameObject.name + " is set to phase with player but has a <0 return distance");
        }

        playerInAggroSpace = aggroSpace.GetComponent<AggroSpace>().playerInside;
        distanceFromPlayer = Vector3.Distance(player.transform.position, transform.position);
        playerWorld = player.GetComponent<Player>().currentWorld;

    }

    private void Update(){
        playerInAggroSpace = aggroSpace.GetComponent<AggroSpace>().playerInside;
        distanceFromPlayer = Vector3.Distance(player.transform.position, transform.position);
        playerWorld = player.GetComponent<Player>().currentWorld;

        if (active)
        {
           // print("p: "+playerWorld + " this: " +world + " aggro:"+playerInAggroSpace);

            //checks if player is in AggroRange in the same world and is not too close
            if (playerInAggroSpace && world == playerWorld && distanceFromPlayer >= minDistanceFromPlayer)
            { 
                //Move object towards player.
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, step);
            }
            //Stub for player damage script.
            if (distanceFromPlayer <= minDistanceFromPlayer && this.world == playerScript.currentWorld)
            {
              Debug.Log("I HOIT YOU =D");
            }

            checkIfCanSwap();

        }

        

    }


    public bool checkIfCanSwap() {
        if (Input.GetKeyDown(KeyCode.LeftShift) && playerScript.canShift) {
            if (/*this.world != playerWorld &&*/ returnWithPlayer) {
                //go along with player if they are close enough or phase out if they aren't
                print("checking distance");
                if (Vector3.Distance(this.transform.position, player.transform.position) < maxReturnDistance) {
                    swapWorld();
                    return true;
                }
            }
        }

        return false;
    }

    public void swapWorld() {
        print("swap world");
        if (world == 1) { world = 2; } else { world = 1; }
        this.GetComponent<PhaseInteraction>().ToggleMyVisibility();
        this.GetComponent<PhaseInteraction>().ToggleMyCollision();
        if (world == 1) this.transform.parent = playerScript.world1.transform;
        if (world == 2) this.transform.parent = playerScript.world2.transform;
    }

}
