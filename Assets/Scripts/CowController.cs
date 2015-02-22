using UnityEngine;
using System.Collections;
using System;

public class CowController : MonoBehaviour {

	
	static System.Random rand1 = new System.Random(1);
	private float seed = 1;
	public float amtRandomness;
	public float stepSize;

	private Vector3 screenPoint;
	private Vector3 offset;
	private Vector3 screenSpaceOffset;

	private float startY;

	private bool isMouseDown;

	// Use this for initialization
	void Start () {
		startY = rigidbody.transform.position.y;
		Debug.Log (rigidbody.transform.forward);
	}

	private float dist;
	private Vector3 v3Offset;
	private Plane plane;
	
	void OnMouseDown() {
		plane.SetNormalAndPosition(Camera.main.transform.forward, transform.position);
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		float dist;
		plane.Raycast (ray, out dist);
		v3Offset = transform.position - ray.GetPoint (dist);         
	}
	
	void OnMouseDrag() {
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		float dist;
		plane.Raycast (ray, out dist);
		Vector3 v3Pos = ray.GetPoint (dist);
		transform.position = v3Pos + v3Offset;    
	}

//	void OnMouseDown()
//	{
//		isMouseDown = true;
//		Debug.Log ("Down");
//		Vector3 curWorldPosition = gameObject.transform.position;
//		curWorldPosition.
//		screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
//		screenSpaceOffset = Input.mousePosition - screenPoint;
//		screenSpaceOffset.y = 0;
//		//################********************************TODO
//
//		offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,Input.mousePosition.z));
//		offset.y = 0;
//	}

	void OnMouseUp()
	{
		isMouseDown = false;
	}

	private float nextGaussianRand(float mean, float stdDev){
		for (int i = 0; i < seed; i++) rand1.NextDouble();
		float u1 = (float)rand1.NextDouble(); //these are uniform(0,1) random doubles
		float u2 = (float)rand1.NextDouble();
		float value = -2 * Mathf.Log10 (u1);
		float randStdNormal = Mathf.Sqrt(value) *
			Mathf.Sin(2 * Mathf.PI * u2); //random normal(0,1)
		float randNormal =
			mean + stdDev * randStdNormal;
		return randNormal;
	}

//	void OnMouseDrag() { 
//		Debug.Log ("Drag");
//		Vector3 curScreenPoint = Input.mousePosition;
//		Vector3 curWorldPosition = Camera.main.ScreenToWorldPoint(curScreenPoint + screenSpaceOffset);
//		curWorldPosition.z = curWorldPosition.y;
//		curWorldPosition.y = startY;
//		rigidbody.transform.position = curWorldPosition;
//	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (Time.frameCount % 10 == 0) {
			float headingAngle = Quaternion.LookRotation (rigidbody.transform.forward).eulerAngles.y;
			float nextHeadingAngle = nextGaussianRand (headingAngle, amtRandomness);
			Vector3 eulerAngleVelocity = new Vector3 (0, nextHeadingAngle, 0);

			Debug.Log (nextHeadingAngle);

			
			float nextXForward = stepSize * ((float)Math.Cos (nextHeadingAngle));
			float nextZForward = stepSize * ((float)Math.Sin (nextHeadingAngle));
			Vector3 nextForward = new Vector3 (nextXForward, 0.0f, nextZForward);
			rigidbody.transform.forward = nextForward;

			if (!isMouseDown){
				rigidbody.transform.position += stepSize * rigidbody.transform.forward;
			}
		}

	}
}
