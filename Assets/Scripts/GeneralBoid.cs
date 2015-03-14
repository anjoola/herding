using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GeneralBoid : MonoBehaviour {
	// List of all the boids.
	public static List<GeneralBoid> boidsList;
	public static List<Rigidbody2D> boidRigidbodies;

	// Stores the velocity and gravity of the boid before pausing;
	private Vector2 pauseVel;
	private Vector2 gravity;

	// Boid controller.
	private BoidController boidController;
	protected float forceMag;
	protected bool inCollision;
	private bool isMouseDown;

	// Screen positions in world space, used for wrapping the boids at the edge of the screen.
	private float _left, _right, _top, _bottom, _width, _height;

	// TODO remove
	public static bool testing = false;

	public void Awake() {
		isMouseDown = false;
		if (boidsList == null)	{
			boidsList = new List<GeneralBoid>();
			boidRigidbodies = new List<Rigidbody2D>();
		}
		Input.multiTouchEnabled = true;
	}
	public void Pause() {
		gravity = Physics2D.gravity;
		Physics2D.gravity = new Vector2(0, 0);
		pauseVel = rigidbody2D.velocity;
		rigidbody2D.velocity = new Vector2(0,0);
	}
	public void Unpause() {
		Physics2D.gravity = gravity;
		rigidbody2D.velocity = pauseVel;
	}

	public static void PauseBoids() {
		for (int i = 0; i < boidsList.Count; i++) {
			boidsList[i].Pause();
		}
	}

	public static void UnpauseBoids() {
		if (boidsList == null) return;

		for (int i = 0; i < boidsList.Count; i++) {
			boidsList[i].Unpause();
		}
	}

	public void Start () {
		forceMag = 1.0f;
		inCollision = false;

		// Get the boid controller from the parent.
		boidController = GetComponentInParent<BoidController>();
		
		// Add the boid to the boids list.
		boidsList.Add(this);
		boidRigidbodies.Add(rigidbody2D);

		// Give it a random starting velocity.
		Vector2 vel = Random.insideUnitCircle;
		vel *= Random.Range(20, 40);
		rigidbody2D.velocity = vel;
		
		// Get some camera coordinates in world coordinates.
		_left = Camera.main.ScreenToWorldPoint(Vector2.zero).x;
		_bottom = Camera.main.ScreenToWorldPoint(Vector2.zero).y;
		_top = Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)).y;
		_right = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x;
		_width = _right - _left;
		_height = _top - _bottom;
		
		StartWrap();
	}
	void OnDestroy() {
		boidRigidbodies.Remove(gameObject.rigidbody2D);
		boidsList.Remove(this);
	}
	
	private Vector3 v3Offset;
	private Plane plane;

	void OnInputDown(Vector2 mousePosition) {
		isMouseDown = true;
		if (GlobalStateController.shouldPause() && !testing) return;
		plane.SetNormalAndPosition(Camera.main.transform.forward, transform.position);
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		float dist;
		plane.Raycast(ray, out dist);
		v3Offset = transform.position - ray.GetPoint(dist);
	}
	
	void OnInputUp(Vector2 mousePosition) {
		isMouseDown = false; 
	}
	
	void OnInputDrag(Vector2 mousePosition) {
		if (GlobalStateController.shouldPause() && !testing) return;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		float dist;
		plane.Raycast(ray, out dist);
		Vector3 v3Pos = ray.GetPoint(dist);
		transform.position = v3Pos + v3Offset; 
	}

	protected void OnMouseDown() {
		OnInputDown(Input.mousePosition);
	}
	protected void OnMouseUp() {
		OnInputUp(Input.mousePosition);
	}
	protected void OnMouseDrag() {
		OnInputDrag(Input.mousePosition);
	}

	// Fixed update used when dealing with rigid bodies
	protected void FixedUpdate () {
		// Don't update position if being dragged or colliding.
		if (isMouseDown || inCollision) return;

		if (GlobalStateController.shouldPause() && !testing) {
			Pause();
			return;
		}

		// Get the cohesion, alignment, and separation components of the flocking.
		Vector2 acceleration = Cohesion() * boidController._cohesion_weight;
		acceleration += Alignment() * boidController._alignment_weight;
		acceleration += Separation() * boidController._separation_weight;
		// Clamp the acceleration to a maximum value.
		acceleration = Vector2.ClampMagnitude(acceleration, boidController._max_acceleration);
		
		// Add the force to the rigid body and face the direction of movement.
		rigidbody2D.AddForce(acceleration * forceMag * Time.fixedDeltaTime);
		FaceTowardsHeading();

		// Check if boids go off the screen.
		Wrap();
	}

	public void Destroy() {
		Destroy(gameObject);
		// All boids have hit the target.
		Debug.Log (boidRigidbodies.Count);
		if (boidRigidbodies.Count == 1) {
			if (!testing) GlobalStateController.finishLevel(GlobalStateController.CompletionType.LevelComplete);
		}
	}

	public void OutOfBoundsDestroy(bool rightSide) {
		Destroy(gameObject);

		// All the boids have left, end the level.
		if (boidRigidbodies.Count == 0) {
			if (!testing) GlobalStateController.finishLevel(GlobalStateController.CompletionType.GameOver);
        }
    }
    
	// TODO remove
	public void RemovePhysicsNoDestroy() {
		boidRigidbodies.Remove(rigidbody2D);
		boidsList.Remove(this);
		//Destroy(gameObject); TODO
		// All the boids have been saved.
		if (boidRigidbodies.Count == 0) {
			if (!testing) GlobalStateController.finishLevel();
		}
	}

	/**
	 * Face the rigid body towards the direction of travel.
	 */
	void FaceTowardsHeading() {
		Vector2 heading = rigidbody2D.velocity.normalized;
		float rotation = -Mathf.Atan2(heading.x, heading.y) * Mathf.Rad2Deg;
		rigidbody2D.MoveRotation(rotation);
	}

	/**
	 * Wrap edges of the screen to keep boids from going off screen.
	 */
	void StartWrap () {
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

	/**
	 * Handle if boids go off screen.
	 */
	void Wrap() {
		bool rightSide = false;
		bool changed = false;
		if (rigidbody2D.position.x < _left) {
			changed = true;
			rigidbody2D.position = rigidbody2D.position + new Vector2 (_width, 0);
		} else if (rigidbody2D.position.x > _right) {
			changed = true;
			rightSide = true;
			rigidbody2D.position = rigidbody2D.position - new Vector2 (_width, 0);
		}
		if (rigidbody2D.position.y < _bottom) {
			changed = true;
			rigidbody2D.position = rigidbody2D.position + new Vector2 (0, _height);
		} else if (rigidbody2D.position.y > _top) {
			changed = true;
			rigidbody2D.position = rigidbody2D.position - new Vector2 (0, _height);
		}

		if (changed) {
			OutOfBoundsDestroy(rightSide);
		}
	}
	
	/**
	 * Calculate the cohesive component of the flocking algorithm.
	 */
	Vector2 Cohesion() {
		Vector2 sumVector = new Vector2();
		int count = 0;

		// For each boid, check the distance from this boid, and if within a neighbourhood, add to the sumVector.
		for (int i = 0; i < boidRigidbodies.Count; i++) {
			float dist = Vector2.Distance(rigidbody2D.position, boidRigidbodies[i].position);

			// dist > 0 prevents including this boid.
			if (dist < boidController._cohesion_radius && dist > 0) {
				sumVector += boidRigidbodies[i].position;
				count++;
			}
		}
		
		// Average the sum_vector and return value
		if (count > 0) {
			sumVector /= count;
			return sumVector - rigidbody2D.position;
		}
		// Sum vector is empty here.
		return sumVector;
	}
	
	/**
	 * Calculate the alignment component of the flocking algorithm.
	 */
	Vector2 Alignment() {
		Vector2 sumVector = new Vector2();
		int count = 0;
		
		// For each boid, check the distance from this boid, and if withing a neighbourhood, add to the sumVector.
		for (int i = 0; i < boidRigidbodies.Count; i++) {
			float dist = Vector2.Distance(rigidbody2D.position, boidRigidbodies[i].position);
			
			if (dist < boidController._cohesion_radius && dist > 0) {
				sumVector += boidRigidbodies[i].velocity;
				count++;
			}
		}

        // Average the sum_vector and clamp magnitude.
        if (count > 0) {
			sumVector /= count;
			sumVector = Vector2.ClampMagnitude(sumVector, 1);
        }
		return sumVector;
    }
    
    /**
     * Calculate the separation component of the flocking algorithm.
     */
    Vector2 Separation() {
		Vector2 sumVector = new Vector2();
        int count = 0;
        
		// For each boid, check the distance from this boid, and if within a neighbourhood, add to the sumVector
        for (int i = 0; i < boidRigidbodies.Count; i++) {
			float dist = Vector2.Distance(rigidbody2D.position, boidRigidbodies[i].position);

            if (dist < boidController._separation_radius && dist > 0) {
				sumVector += (rigidbody2D.position - boidRigidbodies[i].position).normalized / dist;
                count++;
            }
        }
        
        // Average the sumVector.
        if (count > 0) {
			sumVector /= count;
        }
		return sumVector;
    }
    
    /**
     * Draw the radius of the cohesion neighbourhood in green, and the radius of the separation
     * neighbourhood in red, in the scene view.
     */
    void OnDrawGizmosSelected () {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, boidController._cohesion_radius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, boidController._separation_radius);
    }
}
