using UnityEngine;
using System.Collections;

public class SmoothCamera2D : MonoBehaviour {
	
	public float dampTime = 0.15f;
	private Vector3 velocity = Vector3.zero;
	public Transform target;

	public float zoomSensitivity= 15.0f;
	public float zoomSpeed= 5.0f;
	public float zoomMin= 5.0f;
	public float zoomMax= 80.0f;

	private float zoom;


	private bool mouseClicked = false;
	private Vector3 startPoint;
	// Update is called once per frame
	void Start()
	{
		zoom = camera.orthographicSize;
	}
	void Update () 
	{
		if (target)
		{
			Vector3 point = camera.WorldToViewportPoint(target.position);
			Vector3 delta = target.position - camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
			Vector3 destination = transform.position + delta;
			transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
		}

		RaycastHit2D hit = Physics2D.Raycast(transform.position,-Vector2.up);
		if(hit.collider)
		{
			float dist = Vector2.Distance(hit.point,transform.position);
			zoom = velocity.magnitude;
		}
		//zoom = velocity.magnitude;

		//zoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSensitivity;
		zoom = Mathf.Clamp(zoom, zoomMin, zoomMax);



	}

	void LateUpdate() {
		camera.orthographicSize = Mathf.Lerp (camera.orthographicSize, zoom, Time.deltaTime * zoomSpeed);
	}
}