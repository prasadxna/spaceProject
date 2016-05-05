using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;
public class RigidbodyAutowalk : MonoBehaviour 
{
	private const int RIGHT_ANGLE = 90; 

	// This variable determinates if the player will move or not 
	public bool isWalking = false;

	CardboardHead head = null;

	//This is the variable for the player speed
	//[Tooltip("With this speed the player will move.")]
	public float speed;

	[Tooltip("Activate this checkbox if the player shall move when the Cardboard trigger is pulled.")]
	public bool walkWhenTriggered;

	[Tooltip("Activate this checkbox if the player shall move when he looks below the threshold.")]
	public bool walkWhenLookDown;

	[Tooltip("This has to be an angle from 0° to 90°")]
	public double thresholdAngle;

	[Tooltip("Activate this Checkbox if you want to freeze the y-coordiante for the player. " +
		"For example in the case of you have no collider attached to your CardboardMain-GameObject" +
		"and you want to stay in a fixed level.")]
	public bool freezeYPosition; 

	[Tooltip("This is the fixed y-coordinate.")]
	public float yOffset;

	bool isDoubleClickValid;
	[SerializeField]
	float doubleClickDelay;
	bool hasDoubleClicked;
	[SerializeField]
	UnityEvent onDoubleTap;


	Rigidbody rgdBody;


	//Debug things

//	[SerializeField]
//	Text mfcText;
	int mfcCount;
//	[SerializeField]
//	Text doubleText;
	int dbClickCnt;

	Ray stickToGrndRay;
	RaycastHit groundRayHitData;
	Vector3 direction;
	Quaternion rotation ;
	[SerializeField]
	float stoppingDistance;


	void Start () 
	{
		head = this.gameObject.GetComponentInChildren<StereoController>().Head;
		onDoubleTap.AddListener (doubleClickThisFrame );
		rgdBody = this.GetComponent<Rigidbody> ();
		head.transform.forward = this.transform.forward;
	

	}
	public void doubleClickThisFrame()
	{
		print ("Invoking double tap");

		hasDoubleClicked = true;
	}
	void Update () 
	{

		//Para input events
		if (Cardboard.SDK.Triggered) 
		{
			doubleClickMoniter ();
		}



		// Walk when the Cardboard Trigger is used 
		if (walkWhenTriggered && !walkWhenLookDown && !isWalking && hasDoubleClicked) 
		{
			isWalking = true;
		} 
		else if (walkWhenTriggered && !walkWhenLookDown && isWalking && hasDoubleClicked) 
		{
			isWalking = false;
		}

		// Walk when player looks below the threshold angle 
		if (walkWhenLookDown && !walkWhenTriggered && !isWalking &&  
			head.transform.eulerAngles.x >= thresholdAngle && 
			head.transform.eulerAngles.x <= RIGHT_ANGLE) 
		{
			isWalking = true;
		} 
		else if (walkWhenLookDown && !walkWhenTriggered && isWalking && 
			(head.transform.eulerAngles.x <= thresholdAngle ||
				head.transform.eulerAngles.x >= RIGHT_ANGLE)) 
		{
			isWalking = false;
		}

		// Walk when the Cardboard trigger is used and the player looks down below the threshold angle
		if (walkWhenLookDown && walkWhenTriggered && !isWalking &&  
			head.transform.eulerAngles.x >= thresholdAngle && 
			Cardboard.SDK.Triggered &&
			head.transform.eulerAngles.x <= RIGHT_ANGLE) 
		{
			isWalking = true;
		} 
		else if (walkWhenLookDown && walkWhenTriggered && isWalking && 
			head.transform.eulerAngles.x >= thresholdAngle &&
			(Cardboard.SDK.Triggered || 
				head.transform.eulerAngles.x >= RIGHT_ANGLE)) 
		{
			isWalking = false;
		}
		if (isWalking) 
		{
			
			walk ();
		}
	
		hasDoubleClicked = false;
	}
	void doubleClickMoniter()
	{

		if (isDoubleClickValid == false) 
		{
			isDoubleClickValid = true;
			StartCoroutine ("multiClickInvalidator");
		} 
		else 
		{
			isDoubleClickValid = false;
			onDoubleTap.Invoke ();
		}
	}
	IEnumerator multiClickInvalidator()
	{
		yield return new WaitForSeconds (doubleClickDelay);
		isDoubleClickValid = false;

	}
	void walk ()
	{
		
		 direction = new Vector3 (head.transform.forward.x, 0, head.transform.forward.z).normalized * speed * Time.deltaTime;
		 rotation = Quaternion.Euler (new Vector3 (0, -transform.rotation.eulerAngles.y, 0));


		rgdBody.MovePosition(this.transform.position + (rotation * direction));

	}






}