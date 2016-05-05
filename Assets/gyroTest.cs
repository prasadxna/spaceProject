using UnityEngine;
using System.Collections;

public class gyroTest : MonoBehaviour 
{
	Vector3 testRotation;
	Quaternion testRot;
	float counter;
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		
		testRotation = new Vector3(Input.acceleration.x, Input.acceleration.y, Input.acceleration.z);

		testRot =  Cardboard.SDK.HeadPose.Orientation;

		if (testRot.z >= Mathf.Abs (0.26f)) 
		{
			print(counter++);
			print ("Zquit the scene");
		} 



		if(counter >= 25)
		{
			print("quit it");
		}


	}


}
