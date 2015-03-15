using UnityEngine;
using System.Collections;

public class MultiTouchController : MonoBehaviour {


	public void FaceTowardsHeading(Vector2 heading) {
		float rotation = -Mathf.Atan2(heading.x, heading.y) * Mathf.Rad2Deg;
		rigidbody2D.MoveRotation(rotation);
	}
}
