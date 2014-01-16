/// 
/// Autor: Anna Kuśnierz
/// Data: 8.01.2013
/// 

using UnityEngine;

public class LevelTrigger : MonoBehaviour {

	public int nextLevel = 0;

	void OnCollisionEnter2D (Collision2D col)
	{
		Vector2 pos = GameObject.Find("victory").transform.position;
		pos.y += 50.0f;
		GameObject.Find ("victory").transform.position = pos;
		GameObject.Find ("victory").GetComponent<ParticleSystem>().Play();
		GameObject.Find ("victory").GetComponent<AudioSource>().Play();
		PlayerMove.levelID = nextLevel;
		PlayerMove.isPlaying = false;
	}
}
