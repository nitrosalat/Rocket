/*
 
  Anthony D'incau - 2013
  2D Fractal Tree
  
  This code has been ported from Javascript to C#
  The original code can be found at: http://rosettacode.org/wiki/Fractal_tree
  Feel free to use this code for any commercial or non-commercial use.
  
  Simply attach this script to an empty gameOjbect in the Unity Game engine in order
  to draw a 2D fractal tree. Don't forget to uncomment the buildNewTree() call in the
  start function if you're not using the MainController class to control the scene.
  
*/

using UnityEngine;
using System.Collections;

public class Tree2D : MonoBehaviour {
	
	public int depth = 9;
	public float angleSpread = 30.0f;
	public Material branchMat;
	public Material LeavesMat;

	public GameObject SeedPrefab;
	public GameObject LeafPrefab;
	public GameObject BranchPrefab;

	private float deg_to_rad = 3.14159265f / 180.0f;
	private float scale = 0.25f;
	private LineRenderer line;
	private GameObject fractalTree;
	private GameObject branch;

	private GameObject lastBranch;

	public int MinDepth = 2;
	public int MaxDepth = 10;

	public float MinAngleSpread = 10f;
	public float MaxAngleSpread = 40f;

	public float MinScale = 0f;
	public float MAxScale = 1f;

	private Material intermediateMat;


	void Start () {
		depth = Random.Range(MinDepth,MaxDepth);
		angleSpread = Random.Range(MinAngleSpread,MaxAngleSpread);

		intermediateMat = branchMat;
		//un-comment the line below to build the tree on start if not using MainController
		drawTree(transform.position.x,transform.position.y,90f,depth);
	}
		
	//A recursive function used to draw the fractal tree
	void drawTree(float x1 ,float y1, float angle, int depth){

		if (depth != 0){
			scale = Random.Range(MinScale,MAxScale);
			//Set the x2 point
			float x2 = x1 + (Mathf.Cos(angle * deg_to_rad) * depth * scale);
			float y2 = y1 + (Mathf.Sin(angle * deg_to_rad) * depth * scale);

			angleSpread = Random.Range(MinAngleSpread,MaxAngleSpread);
			//Call the drawLine function to draw this single line
			drawLine(x1, y1, x2, y2, depth);
			//Iterate the function recursively, change the rotation of the branches
			drawTree(x2, y2, angle - angleSpread, depth - 1);
	
			drawTree(x2, y2, angle + angleSpread, depth - 1);
		}
	}
	
	//Draws a single branch of the tree
	void drawLine(float x1, float y1, float x2, float y2, int color){
		//Create a gameObject for the branch
		branch = new GameObject("Branch");

		//Make the branch child of the main gameobject
		branch.transform.parent = transform;

		//Add a line renderer to the branch gameobject
		line = branch.AddComponent("LineRenderer") as LineRenderer;



		line.material.color = Color.Lerp(intermediateMat.color, Color.green, 1/color);


		if(color < 6)
		{
			GameObject leaf = (GameObject)Instantiate(LeafPrefab, new Vector2(x2,y2), Quaternion.identity);
			leaf.transform.Rotate(0,0,Mathf.Atan2(y2 - y1,x2-x1) * Mathf.Rad2Deg - 90f);
			leaf.transform.parent = transform;
			//Instantiate(SeedPrefab, new Vector2(x2,y2), Quaternion.identity);
		}
		//Change the material of the LineRenderer


		//Thin the branches through each iteration
		line.SetWidth(color*0.04f, color*0.03f);

		//Draw the line.
		line.SetPosition(0, new Vector3(x1,y1,0));
		line.SetPosition(1, new Vector3(x2,y2,0));
	}
}