using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	const float Z_AXIS = 0;
	const float SKIN_WIDTH = .1f;

	private float horizontalMove;
	private float verticalMove;

	private float gravity = .5f;
	private float moveSpeed = .1f;

	private float hitBoxWidth;
	private float hitBoxHeight;

	private GameObject topLeft;
	private GameObject topRight;
	private GameObject bottomLeft;
	private GameObject bottomRight;

	public float debugScale;




	public Vector2 debugRayAngle = new Vector2(1,1);

	public bool debug = true;

	FourCorners fourCorners;
	private struct FourCorners {
		public Vector2 topLeft;
		public Vector2 topRight;
		public Vector2 bottomLeft;
		public Vector2 bottomRight;
	}

	private struct HitboxCornersAndDimensions{

	}
	

	// Use this for initialization
	void Start () {
		

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

		topLeft.transform.localPosition = new Vector3(-hitBoxWidth/2, hitBoxHeight/2, Z_AXIS);
		topRight.transform.localPosition = new Vector3(hitBoxWidth/2, hitBoxHeight/2, Z_AXIS);
		bottomLeft.transform.localPosition = new Vector3(-hitBoxWidth/2, -hitBoxHeight/2, Z_AXIS);
		bottomRight.transform.localPosition = new Vector3(hitBoxWidth/2, -hitBoxHeight/2, Z_AXIS);

	}

	// Update is called once per frame
	void Update () {

//		fourCorners.topLeft = new Vector2(this.GetComponent<Collider2D>().bounds.min.x, this.GetComponent<Collider2D>().bounds.max.y);
//		fourCorners.topRight = new Vector2(this.GetComponent<Collider2D>().bounds.max.x, this.GetComponent<Collider2D>().bounds.max.y);
//		fourCorners.bottomLeft = new Vector2(this.GetComponent<Collider2D>().bounds.min.x, this.GetComponent<Collider2D>().bounds.min.y);
//		fourCorners.bottomRight = new Vector2(this.GetComponent<Collider2D>().bounds.max.x, this.GetComponent<Collider2D>().bounds.min.y);

		fourCorners.topLeft = topLeft.transform.position;
		fourCorners.topRight = topRight.transform.position;
		fourCorners.bottomLeft = bottomLeft.transform.position;
		fourCorners.bottomRight = bottomRight.transform.position;


//		RaycastHit2D horizontalBottomHit = Physics2D.Raycast(fourCorners.bottomRight, Vector2.right, Mathf.Abs(horizontalMove));
//		
		// First look for horizontal collisions
//		if (horizontalMove != 0){
//			if (horizontalMove > 0){
//				horizontalBottomHit = Physics2D.Raycast(fourCorners.bottomRight, Vector2.right, Mathf.Abs(horizontalMove));
//				horizontalMove = horizontalBottomHit.distance;
//			}
//			else{
//				horizontalBottomHit = Physics2D.Raycast(fourCorners.bottomLeft, Vector2.left, Mathf.Abs(horizontalMove));
//				horizontalMove = -horizontalBottomHit.distance;
//			}
//		}
//		
		//TODO make sure to add the horizonal move to the origin point ofr the raycast
		RaycastHit2D downwardsLeftHit = Physics2D.Raycast(fourCorners.bottomLeft, Vector2.down, Mathf.Abs(verticalMove));
		RaycastHit2D downwardsRightHit = Physics2D.Raycast(fourCorners.bottomRight, Vector2.down, Mathf.Abs(verticalMove));
		//Do veritical collision calcs
		if (downwardsLeftHit.collider != null || downwardsRightHit.collider != null){

			if (downwardsLeftHit.collider != null){
				if (downwardsRightHit.collider != null){
					if (downwardsLeftHit.distance < downwardsRightHit.distance){
						verticalMove = -downwardsLeftHit.distance + SKIN_WIDTH;
					}
					else{
						verticalMove = -downwardsRightHit.distance + SKIN_WIDTH;
					}
				}
				else{
					verticalMove = -downwardsLeftHit.distance + SKIN_WIDTH;
				}
			}
			else{
				verticalMove = -downwardsRightHit.distance + SKIN_WIDTH;
			}
		}


		//Do horizontal collision calcs
		//horizontalMove



		//Rotation calcs

		Debug.DrawRay(fourCorners.topLeft, debugRayAngle);
		//When doing the final movement, we should first do either the vertical or horizontal raycasts, move, and then do the raycast for the other direction before doing the final movement
		this.gameObject.transform.Translate(horizontalMove, verticalMove, Z_AXIS);
		//this.gameObject.transform.Translate(horizontalMove, verticalMove, Z_AXIS);
		//this.gameObject.transform.tr

		//this.gameObject.transform.Rotate(fourCorners.topLeft, Vector2.Angle(downwardsLeftHit.normal, downwardsRightHit.normal));

		//After this, do a calculation to set the angle of the object

		downwardsLeftHit = Physics2D.Raycast(fourCorners.bottomLeft, Vector2.down, hitBoxHeight);
		downwardsRightHit = Physics2D.Raycast(fourCorners.bottomRight, Vector2.down, hitBoxHeight);

		Debug.Log("distance from bottom left corner:" + downwardsLeftHit.distance);
		Debug.Log("angle to change to:" +  Mathf.Rad2Deg * Mathf.Atan(downwardsLeftHit.distance/hitBoxWidth));

		if(downwardsLeftHit.distance != downwardsRightHit.distance){
			this.gameObject.transform.RotateAround(fourCorners.bottomRight, Vector3.forward, Mathf.Atan(downwardsLeftHit.distance/hitBoxWidth)*Mathf.Rad2Deg);
		}


		if(debug){
//			Debug.DrawRay(fourCorners.bottomLeft, new Vector2(0, verticalMove));
//			Debug.DrawRay(fourCorners.bottomRight, new Vector2(0, verticalMove));
//			Debug.DrawRay(fourCorners.bottomLeft, new Vector2(horizontalMove, 0));
//			Debug.DrawRay(fourCorners.bottomRight, new Vector2(horizontalMove, 0));

			Debug.DrawRay(fourCorners.topRight, debugRayAngle);
			Vector3 da2 = this.transform.right;
			//da2.Scale(new Vector3(debugScale,debugScale,debugScale));
			Debug.DrawRay(fourCorners.bottomRight, da2*debugScale);


		}

		// Reset all movement values after an update
		verticalMove = 0;
		horizontalMove = 0;
	}

	// Update called for physics calculations
	void FixedUpdate(){
		// Apply gravity
		verticalMove = verticalMove - gravity;

		//Apply movement
		horizontalMove = horizontalMove + (Input.GetAxisRaw("Horizontal") * moveSpeed);
	}

	float HandleHorizontalCollisions(float moveDistance){
		return moveDistance;
	}

	float HandleVerticalCollisions(float moveDistance){
		return moveDistance;
	}

	void SetupHitboxCornersAndDimensions(){

	}

}