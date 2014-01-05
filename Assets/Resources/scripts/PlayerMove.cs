/// 
/// Autor: Anna Kuśnierz
/// Data: 5.01.2013
/// 

using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour {

    public float step = 0.5f;
	//public List<GameObject> bg;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        float axis = Input.GetAxis("Horizontal");
        GameObject car = GameObject.FindGameObjectWithTag("Car");

		if (axis != 0.0) {

            //// przesuwanie tła
            //Vector2 pos;
            //foreach( GameObject bground in bg)
            //{
            //    pos = bground.transform.position;
            //    pos.x -= step * axis;

            //    if (pos.x < -37.0)
            //        pos.x += 37.0f + (bg.Count - 1) * 29.65f;

            //    bground.transform.position = pos;
            //}
            
            // ruch autka
            Vector2 pos = car.transform.position;
            pos.x += step * axis;
            car.transform.position = pos;

            // obrót auta
            Vector2 scale = car.transform.localScale;
            if (axis < 0.0)
                scale.x = -1.0f;
            else if (axis > 0.0)
                scale.x = 1.0f;
            car.transform.localScale = scale;

            // GUI
            // serca
            GameObject[] hearts = GameObject.FindGameObjectsWithTag("Heart");
            if (hearts.Length > 0)
                foreach (GameObject h in hearts)
                {
                    pos = h.transform.position;
                    pos.x += step * axis;
                    h.transform.position = pos;
                }
            // literki
            GameObject[] letters = GameObject.FindGameObjectsWithTag("Letter");
            if (letters.Length > 0)
                foreach (GameObject l in letters)
                {
                    pos = l.transform.position;
                    pos.x += step * axis;
                    l.transform.position = pos;
                }            

            // ruch kamery
            Vector3 camPos = Camera.main.transform.position;
            camPos.x += step * axis;
            Camera.main.transform.position = camPos;
		}

        // skakanie
        if (Input.GetAxis("Vertical") > 0.0)
        {
            Vector2 pos = car.transform.position;
            pos.y += step;
            car.transform.position = pos;
        }

	}
}
