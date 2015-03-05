using UnityEngine;
using System.Collections;

// Creates a flock of boids and defines values for the boids to use
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

	public int start_format; // To start in Grid, Random, etc
	enum START_FORMATS {RANDOM=1, GRID};
	


	void Start () 
	{

		switch(start_format){
		case (int) START_FORMATS.GRID:
			GridStart ();
			break;
		case (int) START_FORMATS.RANDOM:
			RandomStart ();
			break;
		default:
			break;
		}
	}

	void GridStart()
	{
		
		float _left = Camera.main.ScreenToWorldPoint(Vector2.zero).x;
		float _bottom = Camera.main.ScreenToWorldPoint(Vector2.zero).y;
		float _top = Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)).y;
		float _right = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x;
		float _width = _right - _left;
		float _height = _top - _bottom;
		
		float centerX = _left + 0.5f * _width;
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


	void RandomStart()
	{
		
		float _left = Camera.main.ScreenToWorldPoint(Vector2.zero).x;
		float _bottom = Camera.main.ScreenToWorldPoint(Vector2.zero).y;
		float _top = Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)).y;
		float _right = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x;
		float _width = _right - _left;
		float _height = _top - _bottom;
		
		float centerX = _left + 0.5f * _width;
		float centerY = _bottom + 0.5f * _height;
		
		// Create all the boids and add them as a child of the controller
		for (int i=0; i<_number_of_boids; i++)
		{
			
			float nucleusX;
			float nucleusY;

			nucleusX = Random.Range(_left, _right);
			nucleusY = Random.Range(_top, _bottom);

			
			GameObject go = (GameObject)Instantiate(_boid_prefab, new Vector3(nucleusX,nucleusY,0), Quaternion.Euler(90, -20, 180));
			go.transform.parent = transform;
		}
	}


}
