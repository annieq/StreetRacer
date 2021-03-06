﻿/// 
/// Autor: Anna Kuśnierz
/// Data: 7.01.2013
/// 

using UnityEngine;
using System.Collections;

public class WrapBackground : MonoBehaviour {

    public int numberOfBgrounds = 3;
    public GameObject player;

	void Update () {
        if (player == null)
            return;

        Vector2 pos = transform.position;   // pozycja tła
		float bgroundWidth = GetComponent<SpriteRenderer>().sprite.bounds.size.x * transform.localScale.x;	// szerokość tła

		if (player.transform.position.x > pos.x + 1.5f * bgroundWidth)
        {
			pos.x += numberOfBgrounds * bgroundWidth - 0.1f;
            transform.position = pos;
        }
	}
}
