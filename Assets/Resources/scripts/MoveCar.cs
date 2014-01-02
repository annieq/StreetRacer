using UnityEngine;
using System.Collections;

public class MoveCar : MonoBehaviour {

    public float step = 0.2f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        float axis = Input.GetAxis("Horizontal");

        Vector2 scale = transform.localScale;
        Vector2 pos = transform.position;

        if (axis < 0.0)
        {
            scale.x = -1.0f;
            transform.localScale = scale;

            pos.x += step * axis;
            transform.position = pos;
        }
        else if (axis > 0.0)
        {
            scale.x = 1.0f;
            transform.localScale = scale;

            pos.x += step * axis;
            transform.position = pos;
        }

	}
}
