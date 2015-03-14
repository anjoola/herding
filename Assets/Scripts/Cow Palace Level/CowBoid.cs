using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CowBoid : GeneralBoid {
	public static int POINT_PER_BOID = 40;

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "DetectionTag") {
			if (!testing) {
				GlobalStateController.addScore(POINT_PER_BOID);
				AudioController.playSFX("SingleCow");
            }
			base.Destroy();
        }
	}

//	
//	private Vector3 v3Offset;
//	private Plane plane;
//	
//	void OnInputDown(Vector2 mousePosition) {
//		base.isMouseDown = true;
//		if (GlobalStateController.shouldPause() && !testing) return;
//		plane.SetNormalAndPosition(Camera.main.transform.forward, transform.position);
//		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//		float dist;
//		plane.Raycast(ray, out dist);
//		v3Offset = transform.position - ray.GetPoint(dist);
//	}
//	
//	void OnInputUp(Vector2 mousePosition) {
//		base.isMouseDown = false; 
//	}
//	
//	void OnInputDrag(Vector2 mousePosition) {
//		if (GlobalStateController.shouldPause() && !testing) return;
//		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//		float dist;
//		plane.Raycast(ray, out dist);
//		Vector3 v3Pos = ray.GetPoint(dist);
//		transform.position = v3Pos + v3Offset; 
//	}
//
//	protected void OnMouseDown() {
//		OnInputDown(Input.mousePosition);
//	}
//	protected void OnMouseUp() {
//		OnInputUp(Input.mousePosition);
//	}
//	protected void OnMouseDrag() {
//		OnInputDrag(Input.mousePosition);
//	}


}
