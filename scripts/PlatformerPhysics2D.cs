using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerPhysics2D : MonoBehaviour {

    // these values will be changed on a state by state and object basis
    private float initialVelocity;
    private float finalVelocity;
    private float maximumVelocity;

    private float acceleration;
    private float decceleration;

    private float SKIN_WIDTH;


    
    float CalculateCurrentVelocity(float initialVelocity, float acceleration, float maximumVelocity)
    {
        float finalVelocity;

        finalVelocity = initialVelocity * acceleration;
        if (Mathf.Abs(finalVelocity) > maximumVelocity)
        {
            if (finalVelocity < 0)
            {
                finalVelocity = -maximumVelocity;
            }
            else
            {
                finalVelocity = maximumVelocity;
            }
        }
        return finalVelocity;
    }

    // TODO expect a minimum of at least 2 raycasts to be used
    // Will return an array of raycasts with the first index holding a raycast from point1, and the final index holding a raycast from point2
    RaycastHit2D[] GenerateRaycastsBetweenTwoPoints(Vector2 point1, Vector2 point2, int numberOfRaycasts, Vector2 raycastDirection, float raycastDistance)
    {
        RaycastHit2D[] raycasts = new RaycastHit2D[numberOfRaycasts];
        float lerpRate = Vector2.Distance(point1, point2) / (numberOfRaycasts-1);

        for (int i = 0; i < numberOfRaycasts; i++)
        {
            raycasts[i] = Physics2D.Raycast(Vector2.Lerp(point1, point2, (lerpRate * i)), (raycastDirection * raycastDistance));

            // DEBUG
            Debug.DrawRay(Vector2.Lerp(point1, point2, (lerpRate * i)), (raycastDirection * raycastDistance));
        }
        return raycasts;
    }

    // This simple collison detection will return the shortest distance among an array of raycast hits
    float CollisionDetection(Vector2 point1, Vector2 point2, int numberOfRaycasts, Vector2 movementDirection, float movementDistance)
    {
        float closestCollisionDistance = movementDistance;
        RaycastHit2D[] raycasts = GenerateRaycastsBetweenTwoPoints(point1, point2, numberOfRaycasts, movementDirection, movementDistance);

        foreach (RaycastHit2D raycast in raycasts)
        {
            if (raycast) // If the raycast hit something
            {
                if (raycast.distance < closestCollisionDistance)
                {
                    closestCollisionDistance = raycast.distance;
                }
            }
        }
        return closestCollisionDistance;
    }
    float CollisionDetection(RaycastHit2D[] raycasts, float movementDistance)
    {
        float closestCollisionDistance = movementDistance;

        foreach (RaycastHit2D raycast in raycasts)
        {
            if (raycast) // If the raycast hit something
            {
                if (raycast.distance < closestCollisionDistance)
                {
                    closestCollisionDistance = raycast.distance;
                }
            }
        }
        return closestCollisionDistance;
    }

    // Will return a Vector2 that represents the movement that will occur
    Vector2 HandleMovementWithSlopes(Vector2 point1, Vector2 point2, int numberOfRaycasts, Vector2 movementDirection, float movementDistance, float climbAbleSlopeAngle, float skinWidth)
    {

        RaycastHit2D[] raycasts = GenerateRaycastsBetweenTwoPoints(point1, point2, numberOfRaycasts, movementDirection, movementDistance);
        RaycastHit2D bottomMostRaycast = raycasts[0]; // Assuming that the lowest raycast is stored in the first array slot

        float bottomMostRaycastDistance;
        float closestCollisionDistance = movementDistance;

        if (raycasts[0])
        {
            bottomMostRaycastDistance = raycasts[0].distance;

            // Determine if the closest collision occurs at the bottom most raycast
            for (int i = 1; i < raycasts.Length; i++)
            {
                if (raycasts[i]) // If the raycast hit something
                {
                    if (raycasts[i].distance < closestCollisionDistance)
                    {
                        closestCollisionDistance = raycasts[i].distance;
                    }
                }
            }
            if (closestCollisionDistance > bottomMostRaycastDistance)
            {
                //slope angle to be different based on positive or negative movement?
                //TODO verify that the slope angle calc is working correctly
                Vector2 slopeAngle = new Vector2(raycasts[0].normal.y, -raycasts[0].normal.x);
                Vector2 subMovement = movementDirection * bottomMostRaycastDistance;

                if (Vector2.Angle(movementDirection, slopeAngle) <= climbAbleSlopeAngle)
                {
                    // Handle movement again along the angle of the slope that has been hit
                    return subMovement + HandleMovementWithSlopes(point1 + subMovement, point2 + subMovement, numberOfRaycasts, slopeAngle, movementDistance - bottomMostRaycastDistance, climbAbleSlopeAngle, skinWidth);
                }
                else
                {
                    return movementDirection * bottomMostRaycastDistance;
                }
            }
            else
            {
                return movementDirection * closestCollisionDistance;
            }
        }
        else
        {
            return movementDirection * CollisionDetection(raycasts, movementDistance);
        }

    }

    //TODO this would only work for concave surfaces, would need a way to check convex
    //maybe set this to return the correct angle instead
    void levelObjectAgainstGround(RaycastHit2D bottomLeftRaycast, RaycastHit2D bottomRightRaycast)
    {

    }





    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
