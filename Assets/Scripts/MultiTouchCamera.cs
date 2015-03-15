using UnityEngine;
using System.Collections;

public class MultiTouchCamera : MonoBehaviour {

	public static bool testing = true;

	// Mapping of all the touches.
	public static Hashtable hmap = new Hashtable();
	private float dist;
	private Vector3 v3Offset;
	private Plane plane;
	private Vector2 prevPosition;

	void Update(){
		if (Input.touchCount > 0) {
			for (int i = 0; i < Input.touchCount; i++) {
				Touch t = Input.GetTouch(i);
				Ray ray = Camera.main.ScreenPointToRay(t.position);

				if (t.phase == TouchPhase.Began) {
					RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(t.position), Vector2.zero);

					// No hit.
					if (hit.collider == null || !hit.collider.gameObject.tag.Equals("Draggable")) continue;

					// Hit this particular game object.
					hmap.Add(t.fingerId, hit.collider);
					OnInputDown(hit.collider, ray);
				} else if (t.phase == TouchPhase.Moved || t.phase == TouchPhase.Stationary) {
					OnInputDrag((Collider2D)hmap[t.fingerId], ray);
				} else {
					OnInputUp((Collider2D)hmap[t.fingerId], ray);
					hmap.Remove(t.fingerId);
				}
			}
		} else {
			if (Input.GetMouseButtonDown(0)){
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
				
				// No hit.
				if (hit.collider == null) return;

				hmap.Add(1, hit.collider);
				OnInputDown(hit.collider, ray);

			} else if (Input.GetMouseButton(0) || Input.GetMouseButtonUp(0)) {
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				if (Input.GetMouseButton(0)) {
					OnInputDrag((Collider2D)hmap[1], ray);
				} else if (Input.GetMouseButtonUp(0)) {
					OnInputUp((Collider2D)hmap[1], ray);
					hmap.Remove(1);
				}
			}
		}
	}

	void OnInputDown(Collider2D col, Ray ray) {
		if (col == null || col.rigidbody2D == null || !col.gameObject.tag.Equals("Draggable")) return;

		v3Offset = col.transform.position - ray.GetPoint (10.0f);
		v3Offset.z = 0.0f;
	}

	void OnInputUp(Collider2D col, Ray ray) { }

	void OnInputDrag(Collider2D col, Ray ray) {
		if (col == null || col.rigidbody2D == null || !col.gameObject.tag.Equals("Draggable")) return;

		Vector3 v3Pos = ray.GetPoint(10.0f);
		col.transform.position = v3Pos + v3Offset;

		Vector2 predictedDir = col.rigidbody2D.position - prevPosition;
		if (predictedDir.magnitude > 2) {
			prevPosition = col.rigidbody2D.position;
			Character character = col.gameObject.GetComponent<Character>();
			character.FaceTowardsHeading(predictedDir);
		}
	}
}
