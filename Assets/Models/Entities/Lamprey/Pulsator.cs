using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pulsator : MonoBehaviour {

	public Transform pulse;
	public float rot = 10f;
	public float scaling = 1.1f;
	public float iterationRate = 0.02f;
	public float scaleBase = 0.4f;
	public float pulsateRange = 1f;
	float virgo = 0;
	Vector3 scalar;


	// Use this for initialization
	void Start () {
		pulse = GetComponent<Transform> ();
		scalar = pulse.localScale;
	}

	// Update is called once per frame
	void FixedUpdate () {
		pulse.transform.Rotate (0, 0, rot);
		scalar.x = scaleBase + Mathf.Abs(Mathf.Sin (virgo)/pulsateRange) * scaling;
		scalar.y = scaleBase + Mathf.Abs(Mathf.Sin (virgo)/pulsateRange) * scaling;
		scalar.z = scaleBase + Mathf.Abs( Mathf.Cos (virgo)/pulsateRange) * scaling;

		virgo += iterationRate;
		pulse.localScale = scalar;
	}

	void ChangeBase(float newBase)
	{
		scaleBase = newBase;
	}
	void ChangeRange(float newRange)
	{
		pulsateRange = newRange;
	}
	void ChangeRot(float newRot)
	{
		rot = newRot;
	}
	void ChangeScaling(float newScale)
	{
		scaling = newScale;
	}
	void ChangeIter(float newIter)
	{
		iterationRate = newIter;
	}
}
