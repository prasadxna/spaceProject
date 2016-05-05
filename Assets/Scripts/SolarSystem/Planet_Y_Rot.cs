using UnityEngine;
using System.Collections;

public class Planet_Y_Rot : MonoBehaviour {


	float speed;
	// Use this for initialization
	void Start () 
	{
		speed = 30;
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.Rotate (0, 1 * Time.deltaTime * speed, 0);
	}
}
