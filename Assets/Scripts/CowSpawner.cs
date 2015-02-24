using UnityEngine;
using System.Collections;

public class CowSpawner : MonoBehaviour {

	public int numCows;
	public int timeDifferential;
	public GameObject cow;
	public float amtRandomness;
	public float stepSize;

	private GameObject[] cows;

	static System.Random rand1 = new System.Random(1);
	private float seed = 1;

	private float startY = 1.0f;

	// To keep away from edge
	public float awayFromEdge;

	// Use this for initialization
	void Start () {
		float height = Camera.main.orthographicSize;
		float width = height * Screen.width / Screen.height;


		cows = new GameObject[numCows];
		float widthStep = (2.0f * (width - awayFromEdge) ) / (numCows - 1);
		for (int i = 0; i < numCows; i++) {
			float x = - (width) + awayFromEdge + i * widthStep;
			float z = Random.Range(-height + awayFromEdge, height - awayFromEdge);
			cows[i] = (GameObject) Instantiate (cow, new Vector3(x,startY,z), Quaternion.identity);
		}
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


	void Update(){
		Vector3[] prevCowLocations = new Vector3[numCows];
		for (int i = 0; i < numCows; i++) {
			GameObject currGO = (GameObject) cows[i];
			if (i == 0){

				getRandomDirection(currGO);
			} else{
				
				Vector3[] prevCowFacing = new Vector3[i];
				float[] weights = new float[i];
				float maxLength = 0;
				for (int j = 0; j < i; j++){
					prevCowFacing[j] = prevCowLocations[j] - currGO.rigidbody.transform.position;
					float length = Vector3.Magnitude(prevCowFacing[j]);
					Vector3.Normalize(prevCowFacing[j]);
					weights[j] = length;
					maxLength = Mathf.Max(maxLength,length);
				} 

				maxLength += 1;
				Vector3 newForward  = new Vector3(0.0f,0.0f,0.0f);
				for (int j = 0; j < i; j++){
					newForward += ((weights[j]) / maxLength) * prevCowFacing[j];

				} 
				cows[i].rigidbody.transform.forward = Vector3.Normalize(newForward);

			}

			Vector3 nextStepForward = Time.deltaTime * stepSize * currGO.rigidbody.transform.forward;
			nextStepForward.y = 0;
//			currGO.rigidbody.transform.Translate(nextStepForward);
//			Vector3 pos = currGO.rigidbody.transform.position;
//			pos.y = startY;
//			currGO.rigidbody.transform.position = pos;
//			prevCowLocations[i] = currGO.rigidbody.transform.position;
			currGO.rigidbody.AddForce (nextStepForward);
			Vector3 pos = currGO.rigidbody.transform.position;
			pos.y = startY;
			currGO.rigidbody.transform.position = pos;
			prevCowLocations[i] = currGO.rigidbody.transform.position;
		}

	}

	void getRandomDirection( GameObject currGO){
		Vector3 headingAngles = Quaternion.LookRotation (currGO.rigidbody.transform.forward).eulerAngles;
		
//		Debug.Log ("headingAngle" + headingAngle);
		headingAngles.y = nextGaussianRand (headingAngles.y, amtRandomness);
		headingAngles.z = 0;
		headingAngles.x = 0;
//		Debug.Log ("nextHeadingAngle" + nextHeadingAngle);


		currGO.rigidbody.transform.eulerAngles = headingAngles;

//		Vector3 eulerAngleVelocity = new Vector3 (0, nextHeadingAngle, 0);
//
//
//		float nextHeadingAngleRadians = nextHeadingAngle * Mathf.Deg2Rad;
//		float nextXForward = Mathf.Cos (nextHeadingAngleRadians);
//		float nextZForward = Mathf.Sin (nextHeadingAngleRadians);
//		Vector3 nextForward = new Vector3 (nextXForward, 0.0f, nextZForward);
//		Debug.Log ("nextForward" + nextForward);
//		return nextForward;
	}
}
