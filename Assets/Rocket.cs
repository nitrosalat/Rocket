using UnityEngine;
using System.Collections;

public class Rocket : MonoBehaviour {

	// Use this for initialization

	public Transform pointApplyForce;

	public float Fuel = 100.0f;
	public float ThrottlePercentage = 0.0f;

	public float EnginePower = 5.0f;


	private ParticleSystem flame;

	public GameObject explosion;

	private Planet planet;

	void Start () {
		flame = (ParticleSystem)GetComponentInChildren(typeof(ParticleSystem));
		planet = (Planet)GameObject.Find("Planet").GetComponent(typeof(Planet));

	}

	void OnCollisionEnter2D(Collision2D coll)
	{
		if(coll.relativeVelocity.magnitude >= 20)
		{
			Instantiate(explosion,this.transform.position,Quaternion.identity);
			Destroy(this.gameObject);
		}
	}
	void FixedUpdate()
	{
		RaycastHit2D hit = Physics2D.Raycast(transform.position,planet.GetGravityVectorForPoint(transform.position));
		if (hit.collider != null) {
			float distance = Vector2.Distance(hit.point, transform.position);
	
		}
	}
	// Update is called once per frame
	void Update () {

		ThrottlePercentage+=Input.GetAxis("Vertical");
		if(ThrottlePercentage <= 0) ThrottlePercentage = 0.0f;
		else if(ThrottlePercentage >= 100) ThrottlePercentage = 100.0f;

		//Fuel -= (ThrottlePercentage/100) *  Time.deltaTime;
		//if(Fuel<=0)
		//{
		//	Fuel = 0;
		//	return;
		//}
		Vector2 force = pointApplyForce.up * EnginePower;
		if(Input.GetKey(KeyCode.UpArrow))
		{
			if(flame.isStopped) flame.Play();
			
			gameObject.rigidbody2D.AddForceAtPosition(force,pointApplyForce.position);
		}
		else
		{
			flame.Stop();
		}


		if(Input.GetKey(KeyCode.LeftArrow))
		{
			gameObject.rigidbody2D.AddTorque(2);
		}
		else if(Input.GetKey(KeyCode.RightArrow))
		{
			gameObject.rigidbody2D.AddTorque(-2);
		}

	}
}
