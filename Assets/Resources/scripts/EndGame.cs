/// 
/// Autor: Tomasz Szołtysek
/// Data: 9.01.2013
///

using UnityEngine;
using System.Collections;

public class EndGame : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		if (Input.anyKeyDown)
			Application.Quit();
	}
}
