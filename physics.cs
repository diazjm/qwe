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
            //TODO what if it is a negative velocity? need to multiple the maxVel by 1 or -1 based on whether the final velocity is negative or not
			finalVelocity = maximumVelocity;
		}
		return finalVelocity;
	}

    Physics2D[] RaycastBetweenTwoPoints(Vector2 point1, Vector2 point2, Vector2 raycastDirection, int numberOfRaycasts)
    {
        //first calc distance between the 2 points
        //divide it by the number of raycasts that need to be done - 1
        //using the result of the division, keep incrementing up between the two points shooting out raycasts
        //return the resulting array of raycasts
    }

    float DetectCollisions()
    {
        //will return the distance of the closest collision, to be used with movmeent calcs
    }

    void DetectFloorCollision()
    {
        //this method should do the work of reorianting the player after the movement has been done
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