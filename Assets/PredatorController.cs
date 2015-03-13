using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PredatorController : MonoBehaviour {
    private List<Rigidbody2D> _prey;

	public float REPULSION_DISTANCE = 10;
	public float TO_APPLY = 1;
	
	public static bool testing = false;

	
	
	private float dist;
	private Vector3 v3Offset;
	private Plane plane;
	private bool isMouseDown;

	// Use this for initialization
	void Start () {
		isMouseDown = false;

	}

	void OnInputDown(Vector2 mousePosition)
	{
		isMouseDown = true;
		if (GlobalStateController.shouldPause() && !testing) return;
		plane.SetNormalAndPosition(Camera.main.transform.forward, transform.position);
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		float dist;
		plane.Raycast (ray, out dist);
		v3Offset = transform.position - ray.GetPoint (dist);  
	}
	
	void OnInputUp(Vector2 mousePosition)
	{
		isMouseDown = false; 
	}
	
	void OnInputDrag(Vector2 mousePosition)
	{
		if (GlobalStateController.shouldPause() && !testing) return;
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		float dist;
		plane.Raycast (ray, out dist);
		Vector3 v3Pos = ray.GetPoint (dist);
		transform.position = v3Pos + v3Offset; 
	}


	private Vector2 prevPosition;

	void OnMouseDown()
	{
		Debug.Log ("Down");
		OnInputDown (Input.mousePosition);
		prevPosition = rigidbody2D.position;
	}

	void FaceTowardsHeading(Vector2 heading)
	{
		float rotation = -Mathf.Atan2(heading.x, heading.y)*Mathf.Rad2Deg;
		rigidbody2D.MoveRotation(rotation);
	}
	
	void OnMouseUp()
	{
		OnInputUp (Input.mousePosition);
	}
	
	void OnMouseDrag()
	{
		OnInputDrag (Input.mousePosition);
		Vector2 predictedDir = rigidbody2D.position - prevPosition;
		if (predictedDir.magnitude > 2) 
		{
			prevPosition = rigidbody2D.position;
			FaceTowardsHeading (predictedDir);
		}
	}
	
	// Update is called once per frame
	protected void FixedUpdate () 
	{
		if (GlobalStateController.shouldPause() && !testing) 
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
