/// 
/// Autor: Tomasz Szołtysek
/// Data: 9.01.2013
///

using UnityEngine;

public class EndGame : MonoBehaviour {

	void Update () {
	
		if (Input.anyKeyDown)
			Application.Quit();
	}
}
