using UnityEngine;
using System.Collections;

public class IsometricCameraBehavior : MonoBehaviour {

	public float currentZoom = 5f;

	// the larger the orthographic size, the further OUT the zoom will be
	public float maxOrthoSize = 1f;
	public float minOrthoSize = 8f;
	public float zoomSpeed;
	public float cameraRotationSpeed;

	public GameObject focus;
	public float distanceFromFocus;

	Camera isoCam;

	// Use this for initialization
	void Start () {
		isoCam = GetComponent<Camera> ();
		isoCam.orthographicSize = currentZoom;
		isoCam.transform.position = focus.transform.position - isoCam.transform.forward * distanceFromFocus;
	}
	
	// Update is called once per frame
	void Update () {
		checkInput ();
	}

	public void zoom(bool zIn)
	{
		if (zIn) {
			isoCam.orthographicSize = Mathf.Lerp (isoCam.orthographicSize, maxOrthoSize, Time.deltaTime * zoomSpeed);
		} 
		else 
		{
			isoCam.orthographicSize = Mathf.Lerp (isoCam.orthographicSize, minOrthoSize, Time.deltaTime * zoomSpeed);
		}
	}

	public void rotate(float direction)
	{
		transform.RotateAround (focus.transform.position, Vector3.up * direction, Time.deltaTime * cameraRotationSpeed);
	}

	public void checkInput()
	{
		if (Input.GetKey (KeyCode.W)) 
		{
			zoom (true);
		} 
		else if (Input.GetKey (KeyCode.S)) 
		{
			zoom (false);
		} 
		else if (Input.GetKey (KeyCode.A)) 
		{
			rotate (-1f);
		} 
		else if (Input.GetKey (KeyCode.D)) 
		{
			rotate (1f);
		}
	}
}
