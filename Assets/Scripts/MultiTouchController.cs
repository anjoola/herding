using UnityEngine;
using System.Collections;

public class MultiTouchController : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
//
//#if UNITY_EDITOR
//		if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(0) ||Input.GetMouseButtonUp(0) ) 
//		{
//			Debug.Log ("getting input");
//			Ray ray = camera.ScreenPointToRay(Input.mousePosition);
//			RaycastHit hit;
//			Debug.Log (ray);
//			Debug.DrawRay(ray.origin, ray.direction * 100, Color.yellow);
//			if (Physics2D.Raycast(ray, out hit))
//			{
//				Debug.Log ("ray hit!!!!");
//				GameObject recipient = hit.transform.gameObject;
//				
//				if ( Input.GetMouseButtonDown(0))
//				{
//					recipient.SendMessage("RecipOnMouseDown",hit.point, SendMessageOptions.RequireReceiver);
//					
//				}
//				if (Input.GetMouseButtonUp(0))
//				{
//					recipient.SendMessage("RecipOnMouseUp",hit.point, SendMessageOptions.RequireReceiver);
//					
//				}
//				if (Input.GetMouseButton(0))
//				{
//					recipient.SendMessage("RecipOnMouseDrag",hit.point, SendMessageOptions.RequireReceiver);
//					
//				}
//				
//				
//			}
//			
//		}
//
//#endif 
//		Debug.Log ("Number of touches" + Input.touchCount);
//		if (Input.touchCount > 0) 
//		{
//			Debug.Log ("touch greater than 0");
//			foreach (Touch touch in Input.touches)
//			{
//				Debug.Log ("B");
//				Ray ray = camera.ScreenPointToRay(touch.position);
//				RaycastHit hit;
//				
//				if (Physics.Raycast(ray, out hit))
//				{
//					Debug.Log ("C");
//					GameObject recipient = hit.transform.gameObject;
//					
//					if (touch.phase == TouchPhase.Began)
//					{
//						recipient.SendMessage("RecipOnMouseDown",hit.point, SendMessageOptions.RequireReceiver);
//						
//					}
//					if (touch.phase == TouchPhase.Ended)
//					{
//						recipient.SendMessage("RecipOnMouseUp",hit.point, SendMessageOptions.RequireReceiver);
//						
//					}
//					if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
//					{
//						recipient.SendMessage("RecipOnMouseDrag",hit.point, SendMessageOptions.RequireReceiver);
//						
//					}
//					if (touch.phase == TouchPhase.Canceled)
//					{
//						recipient.SendMessage("RecipOnMouseUp",hit.point, SendMessageOptions.RequireReceiver);
//					}
//					
//					
//				}
//				
//			}
//			
//		}
	}
}
