using UnityEngine;
using System.Collections;

public class physics : MonoBehaviour
{
	// these values will be changed on a state by state and object basis
	private float initialVelocity;
	private float finalVelocity;
	private float maximumVelocity;

	private float acceleration;
	private float decceleration;



	
	float CalculateCurrentVelocity(float initialVelocity, float acceleration, float maximumVelocity)
	{
		float finalVelocity;

		finalVelocity = initialVelocity * acceleration;
		if (Mathf.Abs(finalVelocity) > maximumVelocity)
		{
			finalVelocity = maximumVelocity;
		}
		return finalVelocity;
	}

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}

