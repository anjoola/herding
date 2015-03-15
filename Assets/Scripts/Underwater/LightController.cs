using UnityEngine;
using System.Collections;

public class LightController : MonoBehaviour {
	public float RADIUS;
	public float DIF;
	public float SPEED;

	// Update is called once per frame
	void Update () {
		float radius = RADIUS + DIF * Mathf.Sin(Mathf.Deg2Rad * Time.frameCount * SPEED);
		light.spotAngle = radius;
	}
}
