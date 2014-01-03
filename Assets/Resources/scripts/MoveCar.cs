using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoveCar : MonoBehaviour {

    public float step = 0.2f;
	public List<GameObject> bg;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        float axis = Input.GetAxis("Horizontal");

		if (axis != 0.0) {

			// przesuwanie tła
			Vector2 pos;
			foreach( GameObject bground in bg)
			{
				pos = bground.transform.position;
				pos.x -= step * axis;

				if (pos.x < -37.0)
					pos.x += 37.0f + (bg.Count - 1) * 29.65f;

				bground.transform.position = pos;
			}

			// obrót auta
			Vector2 scale = transform.localScale;
			if (axis < 0.0) {
					scale.x = -1.0f;
					transform.localScale = scale;

			} else if (axis > 0.0) {
					scale.x = 1.0f;
					transform.localScale = scale;
			}
		}

	}
}
