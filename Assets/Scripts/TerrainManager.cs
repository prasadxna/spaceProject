using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TerrainManager : MonoBehaviour 
{
	[SerializeField]
	Slider slider_Indicating_Players_Oxygen_Level;

	bool load_next_level;

	void Start () 
	{
		slider_Indicating_Players_Oxygen_Level.value = 100;
		//load_next_level = false;
		//StartCoroutine("Start_Loading");
	}

	void FixedUpdate()
	{
		slider_Indicating_Players_Oxygen_Level.value -= Time.fixedDeltaTime;
		if(slider_Indicating_Players_Oxygen_Level.value <= 0)
		{
			//SceneManager.LoadScene ("LivingRoom");
			load_next_level = true;

			if(load_next_level)
			{
				//StartCoroutine ("Start_Loading");
				SceneManager.LoadScene ("LR");
			}
		}



	}



	IEnumerator Start_Loading() 
	{
		load_next_level = false;
		AsyncOperation async = Application.LoadLevelAdditiveAsync("LivRoom1");
		//yield return async;
		async.allowSceneActivation = false;
		yield return null;
		Debug.Log("Loading complete");
	}
}
