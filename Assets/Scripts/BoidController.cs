using UnityEngine;
using System.Collections;

// Creates a flock of boids and defines values for the boids to use.
public class BoidController : MonoBehaviour 
{
	public GameObject _boid_prefab; // The prefab boid object
	public int _number_of_boids = 0; // The number of boids to create upon startup

	public float _cohesion_radius = 30; // The radius of the neighbourhood used for cohesion
	public float _cohesion_weight = 30; // The affect cohesion has on the acceleration
	public float _alignment_weight = 1000; // The affect alignment has on the acceleration
	public float _separation_radius = 20; // The radius of the neighbourhood user for the separation
	public float _separation_weight = 5000; // The affect separation has on the acceleration
	public float _max_acceleration = 650; // The maximum acceleration allowed
	public GameObject[] spawnLocations;

	public int start_format; // To start in Grid, Random, etc
	enum START_FORMATS {RANDOM=1, GRID, FISH, CHILD};

	void Start () 
	{
		switch(start_format) {
		case (int) START_FORMATS.GRID:
			PointStart ();
			break;
		case (int) START_FORMATS.RANDOM:
			RandomStart ();
			break;
		case (int) START_FORMATS.FISH:
			FishStart ();
			break;
		case (int) START_FORMATS.CHILD:
			ChildStart();
			break;
		default:
			break;
		}
	}

	void PointStart()
	{
		/*
		float _left = Camera.main.ScreenToWorldPoint(Vector2.zero).x;
		float _bottom = Camera.main.ScreenToWorldPoint(Vector2.zero).y;
		float _top = Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)).y;
		float _right = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x;
		*/

		float nucleusY = 20;
		float nucleusX = -25;

		// Create all the boids and add them as a child of the controller
		for (int i=0; i<_number_of_boids; i++)
		{
			GameObject go = (GameObject)Instantiate(_boid_prefab, new Vector3(nucleusX,nucleusY,0), Quaternion.Euler(90, -20, 180));
			go.transform.parent = transform;
		}
	}

	void ChildStart()
	{
		/*
		float _left = Camera.main.ScreenToWorldPoint(Vector2.zero).x;
		float _bottom = Camera.main.ScreenToWorldPoint(Vector2.zero).y;
		float _top = Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)).y;
		float _right = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x;
		*/

		float nucleusY = 20;
		float nucleusX = -25;
		
		// Create all the boids and add them as a child of the controller
		for (int i=0; i<_number_of_boids; i++)
		{
			GameObject go = (GameObject)Instantiate(_boid_prefab, new Vector3(nucleusX,nucleusY,0), Quaternion.Euler(0, 0, 0));
			go.transform.parent = transform;
		}
	}


	void GridStart()
	{
		float _left = Camera.main.ScreenToWorldPoint(Vector2.zero).x;
		float _bottom = Camera.main.ScreenToWorldPoint(Vector2.zero).y;
		float _top = Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)).y;
		//float _right = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x;
		float _height = _top - _bottom;
		
		float centerY = _bottom + 0.5f * _height;

		float leftOffset = 20;
		float stepX = 30;
		float stepY = 20;
		
		// Create all the boids and add them as a child of the controller
		for (int i=0; i<_number_of_boids; i++)
		{
			
			float nucleusX;
			float nucleusY;

			nucleusX = _left + leftOffset + stepX * i;
			for (int j=-1; j<=1; j++){
				nucleusY = centerY + stepY * j;

				GameObject go = (GameObject)Instantiate(_boid_prefab, new Vector3(nucleusX,nucleusY,0), Quaternion.Euler(90, -20, 180));
				go.transform.parent = transform;
			}
		}
	}

	/**
	 * Randomly generates a game object at particular spawn points.
	 */
	void RandomStart() {
		// Compute probabilistic weights to select each spawn area.
		float[] weights = new float[spawnLocations.Length];
		for (int i = 0; i < spawnLocations.Length; i++) {
			GameObject spawnLoc = spawnLocations[i];
			float spawnWidth = spawnLoc.transform.localScale.x;
			float spawnHeight = spawnLoc.transform.localScale.y;
			float area = spawnWidth * spawnHeight;

			if (i > 0) {
				weights[i] = weights[i-1] + area;
			} else {
				weights[i] = area;
			}
		}

		// Create all the boids and add them as a child of the controller
		for (int i = 0; i < _number_of_boids; i++) {
			// Randomly select a spawning location with weighted probability equal to the area.
			float weight = Random.Range(0, weights[weights.Length-1]);
			int randomIndex = 0;
			while (randomIndex < weights.Length) {
				if (weights[randomIndex] >= weight) break;
				randomIndex++;
			}
	
			GameObject spawnLoc = spawnLocations[randomIndex];
			float spawnX = spawnLoc.transform.position.x;
			float spawnY = spawnLoc.transform.position.y;
			float spawnWidth = spawnLoc.transform.localScale.x / 2;
			float spawnHeight = spawnLoc.transform.localScale.y / 2;

			// Set random location within spawn box.
			float nucleusX = Random.Range(spawnX - spawnWidth, spawnX + spawnWidth);
			float nucleusY = Random.Range(spawnY - spawnHeight, spawnY + spawnHeight);
			GameObject newObj = (GameObject)Instantiate(_boid_prefab,
			                                            new Vector3(nucleusX, nucleusY, 0),
			                                            Quaternion.Euler(90, -20, 180));
			newObj.transform.parent = transform;
		}
	}

	void FishStart()
	{
		// TODO remove
		/*
		float _left = Camera.main.ScreenToWorldPoint(Vector2.zero).x;
		float _bottom = Camera.main.ScreenToWorldPoint(Vector2.zero).y;
		float _top = Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)).y;
		float _right = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x;
		*/

		float nucleusY = 30;
		float nucleusX = -5;
		
		// Create all the boids and add them as a child of the controller
		for (int i=0; i<_number_of_boids; i++)
		{
			GameObject go = (GameObject)Instantiate(_boid_prefab, new Vector3(nucleusX,nucleusY,0), Quaternion.Euler(90, -20, 180));
            go.transform.parent = transform;
		}
	}
}
