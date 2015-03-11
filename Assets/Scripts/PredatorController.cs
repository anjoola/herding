using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PredatorController : MonoBehaviour {
    private List<Rigidbody2D> _prey;

	public float REPULSION_DISTANCE = 10;
	public float TO_APPLY = 1;
	
	public static bool testing = true;
	public string name;

	
	
	private float dist;
	private Vector3 v3Offset;
	private Plane plane;
	private bool isMouseDown;
	private Vector2 prevPosition;

	// Use this for initialization
	void Start () {
		isMouseDown = false;

	}


	
//	public void OnInputDown(Vector2 mousePosition)
//	{
//		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
//		RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
//		
//		if (hit.collider != null )
//		{
//			Debug.Log (hit.collider.gameObject);
//			v3Offset = hit.collider.transform.position - ray.GetPoint (10.0f);
//		}
//	}
//
//	
//	public void OnInputUp(Vector2 mousePosition)
//	{
//		isMouseDown = false; 
//	}
//	
//	public void OnInputDrag(Vector2 mousePosition)
//	{
//		if (GlobalStateController.isPaused && !testing) return;
//		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
//		float dist;
//		plane.Raycast (ray, out dist);
//		Vector3 v3Pos = ray.GetPoint (dist);
//		transform.position = v3Pos + v3Offset; 
//		
//		Vector2 predictedDir = rigidbody2D.position - prevPosition;
//		if (predictedDir.magnitude > 2) 
//		{
//			prevPosition = rigidbody2D.position;
//			FaceTowardsHeading (predictedDir);
//		}
//	}


	
//	void OnInputDown(Vector2 mousePosition)
//	{
//		isMouseDown = true;
//		if (GlobalStateController.isPaused && !testing) return;
//		plane.SetNormalAndPosition(Camera.main.transform.forward, transform.position);
//		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
//		float dist;
//		plane.Raycast (ray, out dist);
//		v3Offset = transform.position - ray.GetPoint (dist);  
//	}


	/******** WORKING */
	void OnInputDown(Vector2 mousePosition)
	{
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
		
		if (hit.collider.transform.position == collider2D.transform.position)
		{
			Debug.Log (hit.collider.gameObject);
			v3Offset = hit.collider.transform.position - ray.GetPoint (10.0f);
			v3Offset.z = 300.0f;
			isMouseDown = true;
		}
	}

	void Update(){
		if (Input.touchCount > 0) {
			Debug.Log ("Touch: " + Input.touchCount);
			for (int i = 0; i < Input.touchCount; i++) {
				Touch t = Input.GetTouch (i);

				if (t.phase == TouchPhase.Began) {
					Debug.Log ("Began");
					OnInputDown (t.position);
				} else if (t.phase == TouchPhase.Moved || t.phase == TouchPhase.Stationary){
					Debug.Log ("Drag");
					OnInputDrag(t.position);
				} else {
					Debug.Log ("Up");
					OnInputUp(t.position);
				}
			}
		} else {

			if (Input.GetMouseButtonDown (0)) {
				OnInputDown (Input.mousePosition);
			} else if (Input.GetMouseButton (0)) {
				OnInputDrag (Input.mousePosition);
			} else if (Input.GetMouseButtonUp (0)) {
				OnInputUp (Input.mousePosition);
			}
		}
	}



	
	void OnInputUp(Vector2 mousePosition)
	{
		isMouseDown = false; 
	}
	
	void OnInputDrag(Vector2 mousePosition)
	{
		if (!isMouseDown) return;
		if (GlobalStateController.isPaused && !testing) return;
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		float dist;
		plane.Raycast (ray, out dist);
		Vector3 v3Pos = ray.GetPoint (dist);
		transform.position = v3Pos + v3Offset; 

		Vector2 predictedDir = rigidbody2D.position - prevPosition;
		if (predictedDir.magnitude > 2) 
		{
			prevPosition = rigidbody2D.position;
			FaceTowardsHeading (predictedDir);
		}
	}

	/********/




//	void OnMouseDown()
//	{
//		Debug.Log ("Down");
//		OnInputDown (Input.mousePosition);
//		prevPosition = rigidbody2D.position;
//
//
//	}



	void FaceTowardsHeading(Vector2 heading)
	{
		float rotation = -Mathf.Atan2(heading.x, heading.y)*Mathf.Rad2Deg;
		rigidbody2D.MoveRotation(rotation);
	}
	
	// Update is called once per frame
	protected void FixedUpdate () 
	{
		if (GlobalStateController.isPaused && !testing) 
		{
			return;
		}
		// Get the cohesion, alignment, and separation components of the flocking
		ExertRepulsion ();
	}


	void ExertRepulsion(){
		for (int i=0; i<GeneralPreyBoid._boids.Count; i++)
		{
			float dist = Vector2.Distance(rigidbody2D.position, GeneralPreyBoid._boids[i].position);
			Vector2 forceDir = GeneralPreyBoid._boids[i].position - rigidbody2D.position;
			
			if (dist < REPULSION_DISTANCE && dist > 0) // dist > 0 prevents including this boid
			{
				Vector2 force = forceDir * TO_APPLY * Time.fixedDeltaTime;
				GeneralPreyBoid._boids[i].AddForce(force);
			}
		}

	}
}
