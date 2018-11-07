using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSwitch : MonoBehaviour {

    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform doorBeingMoved;
    [SerializeField] private KeyCode interactButton;

    [SerializeField] [Range(1f, 100f)] private float switchRotationSpeedForward;
    [SerializeField] [Range(1f, 100f)] private float switchRotationSpeedBack;

    [SerializeField] [Range(-1f, -100f)] private float doorMoveSpeedUp;
    [SerializeField] [Range(-1f, -100f)] private float doorMoveSpeedDown;

    [SerializeField] private float doorMaxHeight;
    [SerializeField] private float PlayerDistFromSwitch;

    private Vector3 doorStartPosition;
    private bool switchOn;

    private void Start()
    {
        //doorMaxHeight = doorBeingMoved.localScale.y;
        doorStartPosition = doorBeingMoved.localPosition;
    }

    private void Update()
    {
        if (!switchOn && doorBeingMoved.localPosition.y > doorStartPosition.y)
        {
            transform.Rotate(Vector3.forward * (switchRotationSpeedBack) * Time.deltaTime);
            doorBeingMoved.Translate(Vector3.up * (doorMoveSpeedDown) * Time.deltaTime);
        }
    }

    private void OnMouseOver()
    {
        if (Input.GetKey(interactButton) && (Vector3.Distance(playerTransform.position, transform.position) <= PlayerDistFromSwitch))
        {
            print("On Switch");

            switchOn = true;
            if(doorBeingMoved.localPosition.y <= (doorStartPosition.y + doorMaxHeight))
            {
                transform.Rotate(Vector3.forward * switchRotationSpeedForward * Time.deltaTime);
                doorBeingMoved.Translate(Vector3.up * doorMoveSpeedUp * Time.deltaTime);
            }
        }
        else
        {
            switchOn = false;
        }
    }

    private void OnMouseExit()
    {
        switchOn = false;
    }
}
