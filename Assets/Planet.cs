using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Planet : MonoBehaviour {

	public float Radius = 1000.0f;
	public float BHLenght = 1.0f;
	public float gravityCoeff = 10;
	public float RotationCoeff = 0.0f;

	public GameObject seedPrefab;

	private PolygonCollider2D coll;
	private LineRenderer rend;
	private LineRenderer BaseHorizontRend;



	//Vars of surface generation
	List<Biome> BiomesList;
	Biome currentBiome;
	Biome lastBiome;


	void Start () {
		coll = (PolygonCollider2D)collider2D;
		rend = (LineRenderer)GetComponent(typeof(LineRenderer));
		BaseHorizontRend = (LineRenderer)gameObject.AddComponent(typeof(LineRenderer));

		GenerateSurface();

		this.rigidbody2D.angularVelocity = RotationCoeff;
	}

	void GenerateSurface()
	{
		InstallBiomes();

		float dA = BHLenght/Radius;//угол кусочка поверхности длиной в метр(по горизонтальному проложению)
		int step = 0;

		int seedsCountToGenerate = 5;

		int capacity = Mathf.CeilToInt( 2* Mathf.PI /dA);
		Vector2[] path = new Vector2[capacity+1];
		rend.SetVertexCount(capacity+1);
		
		//Generation
		PerlinNoise noise = new PerlinNoise(Random.Range(-999999,999999));

		if(currentBiome == null)
		{
			currentBiome = BiomesList[0];
		}
		if(lastBiome == null)
		{
			lastBiome = currentBiome;
		}
		int len = currentBiome.lenght;
		int transitionLenght = 0;

		float currentLenght = 0;
		Camera.main.transform.position = new Vector3(0, Radius,Camera.main.transform.position.z);

		int transHeight = 0;

		for(float angle = 0; angle <= Mathf.PI * 2 + dA; angle += dA)
		{
			Vector2 dirVector = new Vector2(Mathf.Cos(angle),Mathf.Sin(angle));
			Vector2 point;

			float delta;
			delta = noise.FractalNoise1D(step,currentBiome.octaves,currentBiome.frequency,currentBiome.height);
			currentLenght += delta;


			point = dirVector* (Radius + currentLenght);

			if(seedsCountToGenerate > 0)
			{
				Instantiate(seedPrefab, dirVector *(Radius+currentLenght+1),Quaternion.identity);
				seedsCountToGenerate--;
			}
	
			if(len <= 0)
			{
				lastBiome = currentBiome;
				currentBiome = BiomesList[Random.Range(0,BiomesList.Count)];
				len = currentBiome.lenght;
				seedsCountToGenerate = Random.Range(1,10);
			}
			path[step] = point;
			rend.SetPosition(step, point);
			//////////////////////////////

			len--;
			step++;
			Debug.Log(angle);
		}
		coll.SetPath(0,path);
	}
	void InstallBiomes()
	{
		BiomesList = new List<Biome>();

		//NAME, LENGHT, FREQUENCY, OCTAVES, HEIGHT , BASE HEIGHT
		BiomesList.Add(new Biome("Plains"  ,40,5,1,1,4));
		//BiomesList.Add(new Biome("Mountains",100,10 ,2 ,10 ,30));
		//BiomesList.Add(new Biome("Ocean", 300, -1,1,0,10));
	}
	// Update is called once per frame
	void Update () {

	}



	public Vector2 GetGravityVectorForPoint(Vector3 pos)
	{
		return (this.gameObject.transform.position - pos).normalized;
	}
	void FixedUpdate()
	{

		foreach(GameObject obj in GameObject.FindGameObjectsWithTag("Physics"))
		{

			Vector2 gravityVec = (this.gameObject.transform.position - obj.transform.position).normalized;
			float distance = Vector2.Distance(obj.transform.position, transform.position);
			gravityVec *= (gravityCoeff * rigidbody2D.mass * obj.rigidbody2D.mass)/(distance*distance);


			obj.rigidbody2D.AddForce(gravityVec);
		}
	}

	private class Biome
	{
		public string name = "";
		public int lenght;
		public float frequency;
		public float height;
		public int octaves;
		public int BaseHorizont;
		public float smoothing;
		public Biome(string name, int lenght, float frequency, int octaves,float height, int BaseH)
		{
			this.name = name;
			this.lenght = lenght;
			this.frequency = frequency;
			this.octaves = octaves;
			this.height = height;
			this.BaseHorizont = BaseH;
		}
	}
}
