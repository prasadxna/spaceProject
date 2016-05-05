using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement; // Required to load a different scene  
using UnityEngine.UI;


//Enumeration to move a planet closer to the player   --> bringPlanetsCloser() is making use of this enumeration
public enum ViewState
{	
	Orbiting,
	rotating,
	arrange
};

public class livingRoomManager : MonoBehaviour 
{
	[SerializeField]
	GameObject placeHolder; // objects out of its orbit comes to this position.

	[SerializeField]
	string currentPlanet; // current planet that is out of its orbit.

	[SerializeField]
	Vector3 initialScale; // Holds inital scale of the object which is pulled out of its orbit. 


	GameObject planetOutOfOrbit; // will hold the refrernce of the game object which will be passed out as an argument 
								//	during the first click of the mouse and using this game object the second click --> rotates the object
								//	retains the game object collected during the first click and sends it back to orbit (3rd click)					
		
	[SerializeField]
	Camera camera;


	//user defined datatypes
	[SerializeField]
	ViewState currentState;


	[SerializeField]
	GameObject exploreCanvas;

	[SerializeField]
	GameObject m_camera;

	[SerializeField]
	Text terrain_name_display;

	[SerializeField]
	GameObject planet_placeHolder;


	void Start()
	{
		currentState = ViewState.Orbiting;
		exploreCanvas.SetActive (false);
	}


	void Update () 
	{
		if (exploreCanvas.activeSelf) 
		{
			
			exploreCanvas.transform.rotation = new Quaternion (0, exploreCanvas.transform.localEulerAngles.y, 0, 0);
			exploreCanvas.transform.LookAt (m_camera.transform, exploreCanvas.transform.up);

		}
	}

	//Method Invoked by CardBoardReticle script while Trigger is Down
	public void bringPlanetsCloser(GameObject GO)
	{

		if (currentState == ViewState.Orbiting && GO.tag == "SolarSystemObjects") 
		{
			currentPlanet = GO.name;
			if (currentPlanet == "Mars" || currentPlanet == "Jupiter" || currentPlanet == "Earth") 
			{
				
				exploreCanvas.SetActive (true);
				exploreCanvas.transform.position = new Vector3 (placeHolder.transform.position.x, placeHolder.transform.position.y + 0.3f, placeHolder.transform.position.z);
				//exploreCanvas.transform.position = new Vector3 (GO.transform.position.x, GO.transform.position.y + 0.3f, GO.transform.position.z);
				switch (currentPlanet) 
				{
					case "Earth":
						terrain_name_display.text = "Explore Moon";
						break;

					case "Mars":
						terrain_name_display.text = "Explore Mars";
						break;

					case "Jupiter":
						terrain_name_display.text = "Explore Europa";
						break;
				}

			}

			GO.transform.GetComponent<SgtSimpleOrbit> ().enabled = false;
			initialScale = GO.transform.localScale;
			GO.transform.localScale = new Vector3 (3, 3, 3);
			GO.transform.position = placeHolder.transform.position;
			planetOutOfOrbit = GO;
			currentState = ViewState.arrange;
			planetOutOfOrbit.transform.GetComponent<Planet_Y_Rot> ().enabled = true;

		}
			
		else if (currentState == ViewState.arrange && planetOutOfOrbit != null) 
		{
			//currentPlanet = null;

			planetOutOfOrbit.transform.localScale = initialScale;
			planetOutOfOrbit.transform.GetComponent<Planet_Y_Rot> ().enabled = false;
			planetOutOfOrbit.transform.GetComponent<SgtSimpleOrbit> ().enabled = true;
			currentState = ViewState.Orbiting;

			exploreCanvas.SetActive (false);
		}
		
	}


	//Mehod Invoked by Canvas button elements 
	public void exploreTerrain(GameObject m_terrain)
	{

		string terrain_name = currentPlanet;
		switch(terrain_name)
		{

		case "Mars":
			SceneManager.LoadScene ("Mars");
			break;

		case "Earth":
			SceneManager.LoadScene ("Moon");
			break;

		case "Jupiter":
			SceneManager.LoadScene ("Europa");
			break;

		}
	}
		
}
