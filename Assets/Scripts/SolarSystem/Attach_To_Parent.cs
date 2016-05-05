using UnityEngine;
using System.Collections;

public class Attach_To_Parent : MonoBehaviour {


	[SerializeField]
	GameObject parent;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = parent.transform.position;
	
	}
}
