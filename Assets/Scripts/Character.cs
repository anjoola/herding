using UnityEngine;

public class Character : MonoBehaviour {
	/**
	 * Face the rigid body towards the direction of travel.
	 */
	public void FaceTowardsHeading(Vector2 heading) {
		float rotation = -Mathf.Atan2(heading.x, heading.y) * Mathf.Rad2Deg;
		rigidbody2D.MoveRotation(rotation);
	}
}
