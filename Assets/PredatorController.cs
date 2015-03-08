using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PredatorController : MonoBehaviour {
    private List<Rigidbody2D> _prey;

	public float REPULSION_DISTANCE = 10;
	public float TO_APPLY = 1;
	
	public static bool testing = true;

	
	
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
		if (GlobalStateController.isPaused && !testing) return;
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
		if (GlobalStateController.isPaused && !testing) return;
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		float dist;
		plane.Raycast (ray, out dist);
		Vector3 v3Pos = ray.GetPoint (dist);
		transform.position = v3Pos + v3Offset; 
	}

	void OnMouseDown()
	{
		Debug.Log ("Down");
		OnInputDown (Input.mousePosition);
	}
	
	void OnMouseUp()
	{
		OnInputUp (Input.mousePosition);
	}
	
	void OnMouseDrag()
	{
		OnInputDrag (Input.mousePosition);
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
