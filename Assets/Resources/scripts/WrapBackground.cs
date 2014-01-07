/// 
/// Autor: Anna Kuśnierz
/// Data: 7.01.2013
/// 

using UnityEngine;
using System.Collections;

public class WrapBackground : MonoBehaviour {

    public int numberOfBgrounds = 3;
    public GameObject player;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (player == null)
            return;

        Vector2 pos = transform.position;   // pozycja tła
        if (player.transform.position.x > pos.x + 37.0)
        {
            pos.x += numberOfBgrounds * 32.116f;
            transform.position = pos;
        }
	}
}
