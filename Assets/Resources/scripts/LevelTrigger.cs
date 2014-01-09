/// 
/// Autor: Anna Kuśnierz
/// Data: 8.01.2013
/// 

using UnityEngine;
using System.Collections;

public class LevelTrigger : MonoBehaviour {

	public int nextLevel = 0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D (Collision2D col)
	{
		Application.LoadLevel(nextLevel);
	}
}
