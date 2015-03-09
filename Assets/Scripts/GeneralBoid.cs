using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Boid created by the BoidController class
public class GeneralBoid : MonoBehaviour 
{
	public static List<Rigidbody2D> _boids; // A list of all the boids rigidbodies in the scene
	private BoidController _boid_controller; // The boid controller
	
	private float _left, _right, _top, _bottom, _width, _height; // Screen positions in world space, used for wrapping the boids at the edge of the screen


	protected float forceMag;
	
	private static List<Vector2> pausedVel;
	
	private bool paused;
	private bool isMouseDown = false;
	private bool isOnSeat;

	private static Vector2 gravity;


	protected bool inCollision;

	public static bool testing = false;

	public void Awake(){
		_boids = new List<Rigidbody2D>();
		pausedVel = new List<Vector2>();

		Input.multiTouchEnabled = true;
	}
	
	public static void PauseBoids()
	{
		gravity = Physics2D.gravity;
		Physics2D.gravity = new Vector2 (0, 0);
		for (int i = 0; i < _boids.Count; i++)
		{
			pausedVel.Add (_boids[i].velocity);
			_boids[i].velocity = new Vector2(0,0);
		}
	}
	
	public static void UnPauseBoids()
	{
		Physics2D.gravity = gravity;
		if (_boids == null || pausedVel == null) return;
		if (pausedVel.Count < _boids.Count || pausedVel.Count == 0) return;
		for (int i = 0; i < _boids.Count; i++)
		{
			_boids[i].velocity = pausedVel[i];
		}

		pausedVel = new List<Vector2>();
	}

	public void Start ()
	{
		forceMag = 1.0f;
		paused = true;
		isOnSeat = false;
		inCollision = false;

		// Get the boid controller from the parent
		_boid_controller = GetComponentInParent<BoidController>();
		
		// Add the boids rigidbody2D to the boids list
		_boids.Add(rigidbody2D);
		
		// Give the boid a random starting velocity
		Vector2 vel = Random.insideUnitCircle;
		vel *= Random.Range(20, 40);
		rigidbody2D.velocity = vel;
		
		// Get some camera coordinates in world coordinates
		_left = Camera.main.ScreenToWorldPoint(Vector2.zero).x;
		_bottom = Camera.main.ScreenToWorldPoint(Vector2.zero).y;
		_top = Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)).y;
		_right = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x;
		_width = _right - _left;
		_height = _top - _bottom;
		
		
		StartWrap ();
	}
	
	private float dist;
	private Vector3 v3Offset;
	private Plane plane;



	
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


	protected void OnMouseDown()
	{
		OnInputDown (Input.mousePosition);
	}

	protected void OnMouseUp()
	{
		OnInputUp (Input.mousePosition);
	}

	protected void OnMouseDrag()
	{
		OnInputDrag (Input.mousePosition);
	}

	
	// Fixed update used when dealing with rigid bodies
	protected void FixedUpdate () 
	{
		if (isMouseDown) return;
		if (inCollision) return;
		if (GlobalStateController.isPaused && !testing) 
		{
			PauseBoids ();
			return;
		}
		// Get the cohesion, alignment, and separation components of the flocking
		Vector2 acceleration = Cohesion() * _boid_controller._cohesion_weight;
		acceleration += Alignment() * _boid_controller._alignment_weight;
		acceleration += Separation() * _boid_controller._separation_weight;
		// Clamp the acceleration to a maximum value
		acceleration = Vector2.ClampMagnitude(acceleration, _boid_controller._max_acceleration);
		
		// Add the force to the rigid body and face the direction of movement
		rigidbody2D.AddForce(acceleration * forceMag *Time.fixedDeltaTime);
		FaceTowardsHeading();

		// When going off screen, wrap to the opposite screen edge
		
		//		DestroyNotWrap ();
		Wrap();
	}

	public void Destroy()
	{
		_boids.Remove (gameObject.rigidbody2D);
		Destroy (gameObject);
		if (_boids.Count == 0) 
		{
			if (!testing) GlobalStateController.finishLevel();
		}
	}

	public void OutOfBoundsDestroy(bool rightSide)
	{
		_boids.Remove (gameObject.rigidbody2D);
		Destroy (gameObject);
		if (rightSide) {
			// made it to the other side!
			// TODO this breaks the cow palace level
			// this should be moved to the subclasses
			//GlobalStateController.addScore(40);
        }
		if (_boids.Count == 0) 
		{
			if (!testing) GlobalStateController.finishLevel();
        }
    }
    

	public void RemovePhysicsNoDestroy()
	{
		_boids.Remove (gameObject.rigidbody2D);
		if (_boids.Count == 0) 
		{
			if (!testing) GlobalStateController.finishLevel();
		}
	}
	
	// Face the rigid body towards the direction of travel
	void FaceTowardsHeading()
	{
		Vector2 heading = rigidbody2D.velocity.normalized;
		float rotation = -Mathf.Atan2(heading.x, heading.y)*Mathf.Rad2Deg;
		rigidbody2D.MoveRotation(rotation);
	}
	
	// Wrap the edges of the screen to keep the boids from going off screen
	void StartWrap ()
	{
		float centerX = _left + 0.5f * _width;
		float centerY = _bottom + 0.5f * _height;
		if (rigidbody2D.position.x < _left)
			rigidbody2D.position = new Vector2(centerX , rigidbody2D.position.y);
		else if (rigidbody2D.position.x > _right)
			rigidbody2D.position = new Vector2(centerX , rigidbody2D.position.y);
		if (rigidbody2D.position.y < _bottom)
			rigidbody2D.position = new Vector2(rigidbody2D.position.x, centerY);
		else if (rigidbody2D.position.y > _top)
			rigidbody2D.position = new Vector2(rigidbody2D.position.x, centerY);
	}
	
	void Wrap ()
	{
		bool rightSide = false;
		bool changed = false;
		if (rigidbody2D.position.x < _left) 
		{
			changed = true;
			rigidbody2D.position = rigidbody2D.position + new Vector2 (_width, 0);
		} else if (rigidbody2D.position.x > _right) {
			changed = true;
			rightSide = true;
			rigidbody2D.position = rigidbody2D.position - new Vector2 (_width, 0);
		}
		if (rigidbody2D.position.y < _bottom) 
		{
			changed = true;
			rigidbody2D.position = rigidbody2D.position + new Vector2 (0, _height);
		} else if (rigidbody2D.position.y > _top) {
			changed = true;
			rigidbody2D.position = rigidbody2D.position - new Vector2 (0, _height);
		}
		
		if (changed) 
		{
			OutOfBoundsDestroy (rightSide);
		}
	}
	
	
	// Calculate the cohesive component of the flocking algorithm
	Vector2 Cohesion ()
	{
		Vector2 sum_vector = new Vector2();
		int count = 0;
		
		// For each boid, check the distance from this boid, and if withing a neighbourhood, add to the sum_vector
		for (int i=0; i<_boids.Count; i++)
		{
			float dist = Vector2.Distance(rigidbody2D.position, _boids[i].position);
			
			if (dist < _boid_controller._cohesion_radius && dist > 0) // dist > 0 prevents including this boid
			{
				sum_vector += _boids[i].position;
				count++;
			}
		}
		
		// Average the sum_vector and return value
		if (count > 0)
		{
			sum_vector /= count;
			return  sum_vector - rigidbody2D.position;
		}
		
		return sum_vector; // Sum vector is empty here
	}
	
	// Calculate the alignment component of the flocking algorithm
	Vector2 Alignment ()
	{
		Vector2 sum_vector = new Vector2();
		int count = 0;
		
		// For each boid, check the distance from this boid, and if withing a neighbourhood, add to the sum_vector
		for (int i=0; i<_boids.Count; i++)
		{
			float dist = Vector2.Distance(rigidbody2D.position, _boids[i].position);
			
			if (dist < _boid_controller._cohesion_radius && dist > 0) // dist > 0 prevents including this boid
			{
				sum_vector += _boids[i].velocity;
				count++;
			}
		}
		
        // Average the sum_vector and clamp magnitude
        if (count > 0)
        {
            sum_vector /= count;
            sum_vector = Vector2.ClampMagnitude(sum_vector, 1);
        }
        
        return sum_vector;
    }
    
    // Calculate the separation component of the flocking algorithm
    Vector2 Separation ()
    {
        Vector2 sum_vector = new Vector2();
        int count = 0;
        
        // For each boid, check the distance from this boid, and if withing a neighbourhood, add to the sum_vector
        for (int i=0; i<_boids.Count; i++)
        {
            float dist = Vector2.Distance(rigidbody2D.position, _boids[i].position);
            
            if (dist < _boid_controller._separation_radius && dist > 0) // dist > 0 prevents including this boid
            {
                sum_vector += (rigidbody2D.position - _boids[i].position).normalized / dist;
                count++;
            }
        }
        
        // Average the sum_vector
        if (count > 0)
        {
            sum_vector /= count;
        }
        return  sum_vector;
    }
    
    // Draw the radius of the cohesion neighbourhood in green, and the radius of the separation neighbourhood in red, in the scene view
    void OnDrawGizmosSelected ()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _boid_controller._cohesion_radius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _boid_controller._separation_radius);
    }
}
