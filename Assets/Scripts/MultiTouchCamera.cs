using UnityEngine;
using System.Collections;

public class MultiTouchCamera : MonoBehaviour {

	
	public static bool testing = true;
	public static Hashtable hmap = new Hashtable();
	private float dist;
	private Vector3 v3Offset;
	private Plane plane;
	private Vector2 prevPosition;
	
	void Start () {

	}
	
	void Update(){
		//Debug.Log ("Multitouch enabled?" + Input.multiTouchEnabled);
		if (Input.touchCount > 0) {
			Debug.Log ("Num touches: " + Input.touchCount);
			for (int i = 0; i < Input.touchCount; i++) {
				
				Touch t = Input.GetTouch (i);
				Ray ray = Camera.main.ScreenPointToRay (t.position);

				if (t.phase == TouchPhase.Began) {
					RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint(t.position), Vector2.zero);
					
					// No hit
					if (hit.collider == null) continue;
					// hit this particular game object
					
					Debug.Log (hit.collider.gameObject);

					Debug.Log ("Began");
					hmap.Add (t.fingerId,hit.collider);
					OnInputDown (hit.collider, ray);
				} else if (t.phase == TouchPhase.Moved || t.phase == TouchPhase.Stationary){
					Debug.Log ("Drag");
					//						OnInputDrag(hit.collider, ray);
					OnInputDrag ((Collider2D) hmap[t.fingerId],ray);
				} else {
					Debug.Log ("Up");
					OnInputUp((Collider2D) hmap[t.fingerId], ray);
					
					hmap.Remove(t.fingerId);
				}
			}
		} else {
			if (Input.GetMouseButtonDown (0)){
				
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
				
				// No hit

				
				Debug.Log ("Mouse click");
				if (hit.collider == null) return;
				
				Debug.Log ("Hit something");
				
				Debug.Log (hit.collider.gameObject);
				if (Input.GetMouseButtonDown (0)) {
					Debug.Log ("Began");
					hmap.Add (1,hit.collider);
					OnInputDown (hit.collider, ray);
				}
				
				
			} else if(Input.GetMouseButton (0) || Input.GetMouseButtonUp (0)){
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				if (Input.GetMouseButton (0)) {
					Debug.Log ("Still Down");
					OnInputDrag ((Collider2D) hmap[1],ray);
				} else if (Input.GetMouseButtonUp (0)) {
					Debug.Log ("Up");
					OnInputUp((Collider2D) hmap[1], ray);
					hmap.Remove(1);
				}

			}
			
		}
	}
	
	void OnInputDown(Collider2D col, Ray ray)
	{
		//Debug.Log (col.gameObject);
		v3Offset = col.transform.position - ray.GetPoint (10.0f);
		v3Offset.z = 0.0f;
//		isMouseDown = true;
	}
	
	void OnInputUp(Collider2D col, Ray ray)
	{
//		isMouseDown = false; 
	}
	
	void OnInputDrag(Collider2D col, Ray ray)
	{
//		if (!isMouseDown) return;
		
		Vector3 v3Pos = ray.GetPoint(10.0f);
		col.transform.position = v3Pos + v3Offset;
		
		Vector2 predictedDir = col.rigidbody2D.position - prevPosition;
		if (predictedDir.magnitude > 2) 
		{
			prevPosition = col.rigidbody2D.position;
			MultiTouchController mtc = col.gameObject.GetComponent<MultiTouchController>();;
			mtc.FaceTowardsHeading (predictedDir);
		}
	}
}
