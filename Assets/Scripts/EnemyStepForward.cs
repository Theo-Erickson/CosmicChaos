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
    [SerializeField] private float speed; //rate entity moves towards player

    public int world; //world enemy will notice the player in
    public bool active; //if false enemy does nothing
    public GameObject player;
    private float minDistanceFromPlayer = 1.5f; //Without this will move inside player/push player around

    private float step; //speed * deltatime, so speed can be nice whole numbers


    private void Start()
    {
        player = GameObject.Find("Player");
        step = speed * Time.deltaTime;
    }

    private void Update()
    {
        if (active)
        {
            bool playerInAggroSpace = aggroSpace.GetComponent<AggroSpace>().playerInside;
            float distanceFromPlayer = Vector3.Distance(player.transform.position, transform.position);
            int playerWorld = player.GetComponent<Player>().currentWorld;

            //checks if player is in AggroRange in the same world and is not too close
            if (playerInAggroSpace && world == playerWorld && distanceFromPlayer >= minDistanceFromPlayer)
            { 
                //Move object towards player.
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, step);
            }
            //Stub for player damage script.
            if(distanceFromPlayer <= minDistanceFromPlayer)
            {
                Debug.Log("I HOIT YOU =D");
            }
        }
    }

}
