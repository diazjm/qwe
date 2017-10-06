using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
	
public class Player : MonoBehaviour
{
	
	const float Z_AXIS = 0;
	const float SKIN_WIDTH = .1f;
	
	private float horizontalMove;
	private float verticalMove;

	private Vector2 gravityVector = Vector2.down; //this will be set by the level settings
	private float gravity = .5f;
	private float moveSpeed = .1f;
	
	private float hitBoxWidth;
	private float hitBoxHeight;
	
	private GameObject topLeft;
	private GameObject topRight;
	private GameObject bottomLeft;
	private GameObject bottomRight;





	public float debugScale;
	public Vector2 debugRayAngle = new Vector2(1, 1);
	public bool debug = true;
	
	FourCorners fourCorners;
	private struct FourCorners
	{
		public Vector2 topLeft;
		public Vector2 topRight;
		public Vector2 bottomLeft;
		public Vector2 bottomRight;
	}
	
	private struct HitboxCornersAndDimensions
	{
		
	}
	
	
	// Use this for initialization
	void Start()
	{
		fourCorners = new FourCorners();
		fourCorners.topLeft = new Vector2(this.GetComponent<Collider2D>().bounds.min.x, this.GetComponent<Collider2D>().bounds.max.y);
		fourCorners.topRight = new Vector2(this.GetComponent<Collider2D>().bounds.max.x, this.GetComponent<Collider2D>().bounds.max.y);
		fourCorners.bottomLeft = new Vector2(this.GetComponent<Collider2D>().bounds.min.x, this.GetComponent<Collider2D>().bounds.min.y);
		fourCorners.bottomRight = new Vector2(this.GetComponent<Collider2D>().bounds.max.x, this.GetComponent<Collider2D>().bounds.min.y);
		
		hitBoxWidth = fourCorners.bottomRight.x - fourCorners.bottomLeft.x;
		hitBoxHeight = fourCorners.topRight.y - fourCorners.bottomRight.y;
		
		topLeft = new GameObject("topLeft");
		topRight = new GameObject("topRight");
		bottomLeft = new GameObject("bottomLeft");
		bottomRight = new GameObject("bottomRight");
		topLeft.transform.SetParent(this.transform);
		topRight.transform.SetParent(this.transform);
		bottomLeft.transform.SetParent(this.transform);
		bottomRight.transform.SetParent(this.transform);
		
		topLeft.transform.localPosition = new Vector3(-hitBoxWidth / 2, hitBoxHeight / 2, Z_AXIS);
		topRight.transform.localPosition = new Vector3(hitBoxWidth / 2, hitBoxHeight / 2, Z_AXIS);
		bottomLeft.transform.localPosition = new Vector3(-hitBoxWidth / 2, -hitBoxHeight / 2, Z_AXIS);
		bottomRight.transform.localPosition = new Vector3(hitBoxWidth / 2, -hitBoxHeight / 2, Z_AXIS);
		
	}
	
	// Update is called once per frame
	void Update()
	{
		//reacquire corner posiitions
		// TODO can probably get rid of this
		fourCorners.topLeft = topLeft.transform.position;
		fourCorners.topRight = topRight.transform.position;
		fourCorners.bottomLeft = bottomLeft.transform.position;
		fourCorners.bottomRight = bottomRight.transform.position;

		// First look for horizontal collisions
		horizontalMove = HandleHorizontalCollisions(this.transform, horizontalMove, fourCorners.topLeft, fourCorners.topRight, fourCorners.bottomLeft, fourCorners.bottomRight);

		// Add horizontal move distance to the raycast origin point to handle the correct vertical collisions
		fourCorners.topLeft += new Vector2(horizontalMove,0);
		fourCorners.topRight += new Vector2(horizontalMove,0);
		fourCorners.bottomLeft += new Vector2(horizontalMove,0);
		fourCorners.bottomRight += new Vector2(horizontalMove,0);

		// Then look for veritcal collisions
		verticalMove = HandleVerticalCollisions(this.transform, verticalMove, fourCorners.topLeft, fourCorners.topRight, fourCorners.bottomLeft, fourCorners.bottomRight);

		// Add vertical move distance to the raycast origin point to handle the correct vertical collisions
		fourCorners.topLeft += new Vector2(0,verticalMove);
		fourCorners.topRight += new Vector2(0,verticalMove);
		fourCorners.bottomLeft += new Vector2(0,verticalMove);
		fourCorners.bottomRight += new Vector2(0,verticalMove);

		//OLD CODE
//		RaycastHit2D downwardsLeftHit = Physics2D.Raycast(fourCorners.bottomLeft, Vector2.down, Mathf.Abs(verticalMove));
//		RaycastHit2D downwardsRightHit = Physics2D.Raycast(fourCorners.bottomRight, Vector2.down, Mathf.Abs(verticalMove));
//		if (downwardsLeftHit.collider != null || downwardsRightHit.collider != null)
//		{
//			
//			if (downwardsLeftHit.collider != null)
//			{
//				if (downwardsRightHit.collider != null)
//				{
//					if (downwardsLeftHit.distance < downwardsRightHit.distance)
//					{
//						verticalMove = -downwardsLeftHit.distance + SKIN_WIDTH;
//					}
//					else
//					{
//						verticalMove = -downwardsRightHit.distance + SKIN_WIDTH;
//					}
//				}
//				else
//				{
//					verticalMove = -downwardsLeftHit.distance + SKIN_WIDTH;
//				}
//			}
//			else
//			{
//				verticalMove = -downwardsRightHit.distance + SKIN_WIDTH;
//			}
//		}

		
		
		
		//Rotation calcs
		// *****************

		Debug.DrawRay(fourCorners.topLeft, debugRayAngle);


		//When doing the final movement, we should first do either the vertical or horizontal raycasts, move, and then do the raycast for the other direction before doing the final movement
		this.gameObject.transform.Translate(horizontalMove, verticalMove, Z_AXIS); //this does a movement relative to the object

		
		//this.gameObject.transform.Rotate(fourCorners.topLeft, Vector2.Angle(downwardsLeftHit.normal, downwardsRightHit.normal));
		
		//After this, do a calculation to set the angle of the object
		
		downwardsLeftHit = Physics2D.Raycast(fourCorners.bottomLeft, -this.transform.up, hitBoxHeight);
		downwardsRightHit = Physics2D.Raycast(fourCorners.bottomRight, -this.transform.up, hitBoxHeight);
		
		Debug.Log("distance from bottom left corner:" + downwardsLeftHit.distance);
		Debug.Log("angle to change to:" + Mathf.Rad2Deg * Mathf.Atan(downwardsLeftHit.distance / hitBoxWidth));
		
		if (downwardsLeftHit.distance != downwardsRightHit.distance)
		{
			this.gameObject.transform.RotateAround(fourCorners.bottomRight, Vector3.forward, Mathf.Atan(downwardsLeftHit.distance / hitBoxWidth) * Mathf.Rad2Deg);
		}
		
		if (debug)
		{
			Debug.DrawRay(fourCorners.topRight, debugRayAngle);
			Vector3 da2 = this.transform.right;
			//da2.Scale(new Vector3(debugScale,debugScale,debugScale));
			Debug.DrawRay(fourCorners.bottomRight, da2 * debugScale);


		}
		
		// Reset all movement values after an update
		verticalMove = 0;
		horizontalMove = 0;
	}
	
	// Update called for physics calculations
	void FixedUpdate()
	{
		// Apply gravity
		verticalMove = verticalMove - gravity;
		
		//Apply movement
		horizontalMove = horizontalMove + (Input.GetAxisRaw("Horizontal") * moveSpeed);
	}
	
	float HandleHorizontalCollisions(Transform objectTransform, float moveDistance, Vector2 hitBoxTopLeft, Vector2 hitBoxTopRight, Vector2 hitBoxBottomLeft, Vector2 hitBoxBottomRight)
	{
		float raycastDistance = Mathf.Abs(moveDistance);
		float moveDistanceUntilCollision = raycastDistance;
		Vector2 objectRightwardsVector = objectTransform.right;
		Vector2 objectLeftwardsVector = -objectRightwardsVector;

		if (moveDistance > 0) //the object is moving rightwards
		{
			//do 3 raycasts from the edges and centerpoint of the hitbox
			RaycastHit2D[] collisions = new RaycastHit2D[3];
			collisions[0] = Physics2D.Raycast(hitBoxTopRight, objectRightwardsVector, raycastDistance);
			collisions[1] = Physics2D.Raycast(hitBoxBottomRight, objectRightwardsVector, raycastDistance);
			collisions[2] = Physics2D.Raycast(Vector2.Lerp(hitBoxTopRight, hitBoxBottomRight, .05), objectRightwardsVector, raycastDistance);
			//TODO DEBUG
			Debug.DrawRay(hitBoxTopRight, objectRightwardsVector*raycastDistance);
			Debug.DrawRay(hitBoxBottomRight, objectRightwardsVector*raycastDistance);
			Debug.DrawRay(Vector2.Lerp(hitBoxTopRight, hitBoxBottomRight, .05), objectRightwardsVector*raycastDistance);
			
			foreach (RaycastHit2D collision in collisions)
			{
				if (collision) //collision has been detected
				{
					if (collision.distance < moveDistanceUntilCollision)
					{
						//save the shortest collision distance as the return value
						moveDistanceUntilCollision = collision.distance;
					}
				}
			}
			
			return moveDistanceUntilCollision;
		}
		else // else it is moving leftwards or not at all
		{
			//do 3 raycasts from the edges and centerpoint of the hitbox
			RaycastHit2D[] collisions = new RaycastHit2D[3];
			collisions[0] = Physics2D.Raycast(hitBoxTopLeft, objectLeftwardsVector, raycastDistance);
			collisions[1] = Physics2D.Raycast(hitBoxBottomLeft, objectLeftwardsVector, raycastDistance);
			collisions[2] = Physics2D.Raycast(Vector2.Lerp(hitBoxTopLeft, hitBoxBottomLeft, .05), objectLeftwardsVector, raycastDistance);
			//TODO DEBUG
			Debug.DrawRay(hitBoxTopLeft, objectLeftwardsVector*raycastDistance);
			Debug.DrawRay(hitBoxBottomLeft, objectLeftwardsVector*raycastDistance);
			Debug.DrawRay(Vector2.Lerp(hitBoxTopLeft, hitBoxBottomLeft, .05), objectLeftwardsVector*raycastDistance);

			foreach (RaycastHit2D collision in collisions)
			{
				if (collision) //collision has been detected
				{
					if (collision.distance < moveDistanceUntilCollision)
					{
						//save the shortest collision distance as the return value
						moveDistanceUntilCollision = collision.distance;
					}
				}
			}
			
			return -moveDistanceUntilCollision;
		}
	}
	
	float HandleVerticalCollisions(Transform objectTransform, float moveDistance, Vector2 hitBoxTopLeft, Vector2 hitBoxTopRight, Vector2 hitBoxBottomLeft, Vector2 hitBoxBottomRight)
	{
		float raycastDistance = Mathf.Abs(moveDistance);
		float moveDistanceUntilCollision = raycastDistance;
		Vector2 objectUpwardsVector = objectTransform.up;
		Vector2 objectDownwardsVector = -objectUpwardsVector;

		if (moveDistance > 0) //the object is moving upwards
		{
			//do 3 raycasts from the edges and centerpoint of the hitbox
			RaycastHit2D[] collisions = new RaycastHit2D[3];
			collisions[0] = Physics2D.Raycast(hitBoxTopLeft, objectUpwardsVector, raycastDistance);
			collisions[1] = Physics2D.Raycast(hitBoxTopRight, objectUpwardsVector, raycastDistance);
			collisions[2] = Physics2D.Raycast(Vector2.Lerp(hitBoxTopLeft, hitBoxTopRight, .05), objectUpwardsVector, raycastDistance);
			//TODO DEBUG
			Debug.DrawRay(hitBoxTopLeft, objectUpwardsVector*raycastDistance);
			Debug.DrawRay(hitBoxTopRight, objectUpwardsVector*raycastDistance);
			Debug.DrawRay(Vector2.Lerp(hitBoxTopLeft, hitBoxTopRight, .05), objectUpwardsVector*raycastDistance);

			foreach (RaycastHit2D collision in collisions)
			{
				if (collision) //collision has been detected
				{
					if (collision.distance < moveDistanceUntilCollision)
					{
						//save the shortest collision distance as the return value
						moveDistanceUntilCollision = collision.distance;
					}
				}
			}

			return moveDistanceUntilCollision;
		}
		else // else it is moving downwards or not at all
		{
			//do 3 raycasts from the edges and centerpoint of the hitbox
			RaycastHit2D[] collisions = new RaycastHit2D[3];
			collisions[0] = Physics2D.Raycast(hitBoxBottomLeft, objectDownwardsVector, raycastDistance);
			collisions[1] = Physics2D.Raycast(hitBoxBottomRight, objectDownwardsVector, raycastDistance);
			collisions[2] = Physics2D.Raycast(Vector2.Lerp(hitBoxBottomLeft, hitBoxBottomRight, .05), objectDownwardsVector, raycastDistance);
			//TODO DEBUG
			Debug.DrawRay(hitBoxBottomLeft, objectDownwardsVector*raycastDistance);
			Debug.DrawRay(hitBoxBottomRight, objectDownwardsVector*raycastDistance);
			Debug.DrawRay(Vector2.Lerp(hitBoxBottomLeft, hitBoxBottomRight, .05), objectDownwardsVector*raycastDistance);

			foreach (RaycastHit2D collision in collisions)
			{
				if (collision) //collision has been detected
				{
					if (collision.distance < moveDistanceUntilCollision)
					{
						//save the shortest collision distance as the return value
						moveDistanceUntilCollision = collision.distance;
					}
				}
			}
			
			return -moveDistanceUntilCollision;
		}
	}
	
	void SetupHitboxCornersAndDimensions()
	{
		
	}

	
}











//			for (int i = 0; i < numberOfRaycasts; i++)
//			{
//				collisions[i] = 
//			}
//
//			foreach (RaycastHit2D collision in collisions)
//			{
//
//			}



