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
		Vector2 pos = GameObject.Find("victory").transform.position;
		pos.y -= 50.0f;
		GameObject.Find ("victory").transform.position = pos;
		PlayerMove.levelID = nextLevel;
		PlayerMove.isPlaying = false;
	}
}
