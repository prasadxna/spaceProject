using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;
public class Autowalk : MonoBehaviour 
{
	private const int RIGHT_ANGLE = 90; 

	// This variable determinates if the player will move or not 
	private bool isWalking = false;

	CardboardHead head = null;

	//This is the variable for the player speed
	[Tooltip("With this speed the player will move.")]
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

	//Debug things

	[SerializeField]
	Text mfcText;
	int mfcCount;
	[SerializeField]
	Text doubleText;
	int dbClickCnt;

	Ray stickToGrndRay;
	RaycastHit groundRayHitData;
	Vector3 direction;
	Quaternion rotation ;
	[SerializeField]
	float stoppingDistance;

	[SerializeField]
	float stickToGroundValue;
	void Start () 
	{
		head = Camera.main.GetComponent<StereoController>().Head;
		onDoubleTap.AddListener (doubleClickThisFrame );
		print ("Print");
		walk ();
		stickToGround ();

	}
	public void doubleClickThisFrame()
	{
		//print ("Invoking double tap");
		hasDoubleClicked = true;
	}
	void Update () 
	{

		//Para input events
		if (Cardboard.SDK.Triggered) 
		{
			print ("Clicked");
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
		if (isWalking) {
			walk ();
		}
		if (freezeYPosition) {
			stickToGround ();
		}
			
		hasDoubleClicked = false;
	}
	void doubleClickMoniter()
	{

		if (isDoubleClickValid == false) {
			isDoubleClickValid = true;
			StartCoroutine ("multiClickInvalidator");
		} else {
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
		stickToGrndRay = new Ray (this.transform.position + new Vector3 (0, this.transform.localScale.y / 2f, 0), -this.transform.up);
		transform.Translate (rotation * direction);
		Physics.Raycast (stickToGrndRay, out groundRayHitData);
		Debug.DrawRay (stickToGrndRay.origin, stickToGrndRay.direction * groundRayHitData.distance);

		this.transform.position = new Vector3 (this.transform.position.x, this.transform.position.y, this.transform.position.z);
	}

	void stickToGround ()
	{
		avoidWalls ();
		transform.position = Vector3.Lerp(this.transform.position,new Vector3(transform.position.x,groundRayHitData.point.y+this.transform.localScale.y/stickToGroundValue, transform.position.z),0.4f);
	}

	void avoidWalls()
	{
		Ray avdWalls = new Ray (this.transform.position, direction);
		RaycastHit wallHit;
		Physics.Raycast (avdWalls, out wallHit);
		Debug.DrawRay (avdWalls.origin, avdWalls.direction * wallHit.distance);
		if (wallHit.distance <= stoppingDistance)
			isWalking = false;
	}


}