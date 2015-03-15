using UnityEngine;
using System.Collections;

public class MultiTouchController : MonoBehaviour {

	public static bool testing = true;
	private float dist;
	private Vector3 v3Offset;
	private Plane plane;
	protected bool isMouseDown;
	private Vector2 prevPosition;
	
	void Start () {
		isMouseDown = false;
	}

	void Update(){
		//Debug.Log ("Multitouch enabled?" + Input.multiTouchEnabled);
		if (Input.touchCount > 0) {
			Debug.Log ("Num touches: " + Input.touchCount);
			for (int i = 0; i < Input.touchCount; i++) {
				
				Touch t = Input.GetTouch (i);
				Debug.Log ("Checking touch: " + i + "at position: " + t.position);
				
				Ray ray = Camera.main.ScreenPointToRay (t.position);
				RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint(t.position), Vector2.zero);
				
				// No hit
				if (hit.collider == null) continue;
				if (hit.collider.gameObject == collider2D.gameObject){
					// hit this particular game object
					
					Debug.Log (hit.collider.gameObject);
					if (t.phase == TouchPhase.Began) {
						Debug.Log ("Began");
						OnInputDown (hit.collider, ray);
					} else if (t.phase == TouchPhase.Moved || t.phase == TouchPhase.Stationary){
						Debug.Log ("Drag");
						OnInputDrag(hit.collider, ray);
					} else {
						Debug.Log ("Up");
						OnInputUp(hit.collider, ray);
					}
				}
			}
		} else {
			if (Input.GetMouseButtonDown (0) || Input.GetMouseButton (0) || Input.GetMouseButtonUp (0)){
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

				// No hit
				if (hit.collider == null) return;
				if (hit.collider.gameObject == collider2D.gameObject){
					
					Debug.Log ("Mouse click");
					// hit this particular game object
					
					Debug.Log (hit.collider.gameObject);
					if (Input.GetMouseButtonDown (0)) {
						Debug.Log ("Down");
						OnInputDown (hit.collider, ray);
					} else if (Input.GetMouseButton (0)) {
						Debug.Log ("Still Down");
						OnInputDrag (hit.collider, ray);
					} else if (Input.GetMouseButtonUp (0)) {
						Debug.Log ("Up");
						OnInputUp (hit.collider, ray);
					}
				}
				
			}
		}
	}
	
	void OnInputDown(Collider2D col, Ray ray)
	{
		//Debug.Log (col.gameObject);
		v3Offset = col.transform.position - ray.GetPoint (10.0f);
		v3Offset.z = 0.0f;
		isMouseDown = true;
	}
	
	void OnInputUp(Collider2D col, Ray ray)
	{
		isMouseDown = false; 
	}
	
	void OnInputDrag(Collider2D col, Ray ray)
	{
		if (!isMouseDown) return;
		
		Vector3 v3Pos = ray.GetPoint(10.0f);
		transform.position = v3Pos + v3Offset;

		Vector2 predictedDir = rigidbody2D.position - prevPosition;
		if (predictedDir.magnitude > 2) 
		{
			prevPosition = rigidbody2D.position;
			FaceTowardsHeading (predictedDir);
		}
	}

	void FaceTowardsHeading(Vector2 heading) {
		float rotation = -Mathf.Atan2(heading.x, heading.y) * Mathf.Rad2Deg;
		rigidbody2D.MoveRotation(rotation);
	}
}
