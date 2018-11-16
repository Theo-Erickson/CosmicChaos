using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeVignette : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        ChangeAlpha(  this.GetComponent<Renderer>().material, 1.0f - (GetComponentInParent<Player>().shortTermSanity/GetComponentInParent<Player>().maxShortTermSanity)  );
	}

    //change transparency of this material
    public void ChangeAlpha(Material mat, float alphaValue) {
        Color oldColor = mat.color;
        Color newColor = new Color(oldColor.r, oldColor.g, oldColor.b, alphaValue);
        mat.SetColor("_Color", newColor);
    }
}
