using UnityEngine;
using System.Collections;

public class MultiTouchCamera : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}


	
	void MouseCheck()
	{
		int layerMask = 1 << 8;
		if (Input.GetMouseButton(0) || Input.GetMouseButtonUp(0)|| Input.GetMouseButtonDown(0) ){
			Ray ray = camera.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;

			if ( Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask) )
			{

				GameObject hitObject = hit.transform.gameObject;
				Debug.Log (hitObject);
				if (Input.GetMouseButtonDown(0))
				{
					hitObject.SendMessage ("OnInputDown",Input.mousePosition);
				} else if (Input.GetMouseButton(0))
				{
					hitObject.SendMessage ("OnInputUp",Input.mousePosition);
				} else if (Input.GetMouseButtonUp(0))
				{
					hitObject.SendMessage ("OnInputDrag", Input.mousePosition);
				}
			}
		}
		
	}
	
	void TapCheck()
	{
		int layerMask = 1 << 8;
		foreach(Touch touch in Input.touches){
			Ray ray = camera.ScreenPointToRay(touch.position);
			Debug.Log (ray);
			RaycastHit hit;
			if ( Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask) )
			{
				GameObject hitObject = hit.transform.gameObject;
				if (touch.phase == TouchPhase.Began)
				{
					hitObject.SendMessage ("OnInputDown",touch.position);
				} else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
				{
					hitObject.SendMessage ("OnInputUp",touch.position);
				} else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
				{
					hitObject.SendMessage ("OnInputDrag", touch.position);
				}
			}
		}
	}
	
	
	
	void Update(){
		MouseCheck ();
		TapCheck ();
	}

}
