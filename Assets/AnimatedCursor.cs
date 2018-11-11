using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimatedCursor : MonoBehaviour {

    public Sprite[] frames = new Sprite[8];  
    public int framesPerSecond = 10;
    public int index = 0;

    void Update() {

        index = (int)(Time.time * framesPerSecond) % frames.Length;
        //index = index % frames.Length;
        print(index);
        this.GetComponent<Image>().sprite = frames[index];
    }
}
