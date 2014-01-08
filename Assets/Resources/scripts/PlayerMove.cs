/// 
/// Autor: Anna Kuśnierz
/// Data: 5.01.2013
/// 

using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour {

    public float step = 0.5f;
    public float jumpStep = 0.5f;
	public int life = 3;

    private static bool isJumping = false;

    private static float camRelPos;
    private static float[] heartRelPos;
    private static float[] letterRelPos;

	// Use this for initialization
	void Start () {
        camRelPos = Camera.main.transform.position.x
                    - GameObject.FindGameObjectWithTag("Car").transform.position.x;
        heartRelPos = new float[GameObject.FindGameObjectsWithTag("Heart").Length];
        letterRelPos = new float[GameObject.FindGameObjectsWithTag("Letter").Length];

        for (int i = 0; i < heartRelPos.Length; ++i)
            heartRelPos[i] = GameObject.FindGameObjectsWithTag("Heart")[i].transform.position.x 
                                - GameObject.FindGameObjectWithTag("Car").transform.position.x;
        
        for (int i = 0; i < letterRelPos.Length; ++i)
            letterRelPos[i] = GameObject.FindGameObjectsWithTag("Letter")[i].transform.position.x
                                - GameObject.FindGameObjectWithTag("Car").transform.position.x;
	}
	
	// Update is called once per frame
	void Update () {
        float axis = Input.GetAxis("Horizontal");
        GameObject car = GameObject.FindGameObjectWithTag("Car");

		if (axis > 0.0) {
            
            // ruch autka
			Vector2 pos = car.transform.position;
            pos.x += step * axis;
            car.transform.position = pos;

            // obrót auta
            /*Vector2 scale = car.transform.localScale;
            if (axis < 0.0)
                scale.x = -1.0f;
            else if (axis > 0.0)
                scale.x = 1.0f;
            car.transform.localScale = scale;*/

            // GUI
            // serca
            GameObject[] hearts = GameObject.FindGameObjectsWithTag("Heart");
            for (int i = 0; i < hearts.Length; ++i)
            {
                pos = hearts[i].transform.position;
                pos.x = car.transform.position.x + heartRelPos[i];
                hearts[i].transform.position = pos;
            }
            // literki
            GameObject[] letters = GameObject.FindGameObjectsWithTag("Letter");
            for (int i = 0; i < letters.Length; ++i)
            {
                pos = letters[i].transform.position;
                pos.x = car.transform.position.x + letterRelPos[i];
                letters[i].transform.position = pos;
            }            

			// sprawdzenie kolizji z przeszkodami
			float pos_c_x = car.transform.position.x + car.GetComponent<BoxCollider2D>().size.x/2.0f;
			float pos_c_y2 = car.transform.position.y - car.GetComponent<BoxCollider2D>().size.y/2.0f;
			Vector2 size;
			GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
			for (int i = 0; i < obstacles.Length; ++i)
			{
                pos = obstacles[i].transform.position;
                size = obstacles[i].GetComponent<SpriteRenderer>().sprite.bounds.size;
				size.x *= obstacles[i].transform.localScale.x / 2.0f;
                size.y *= obstacles[i].transform.localScale.y / 2.0f;
                if (pos.x - size.x <= pos_c_x && pos.x + size.x >= pos_c_x && pos.y + size.y >= pos_c_y2)
				{
					--life;
					pos.x += Random.Range(100,150);
					obstacles[i].transform.position = pos;
                    heartRelPos[life] -= 100;
				}
                else if (pos.x < pos_c_x - 50)
                {
                    pos.x += Random.Range(150, 200);
                    obstacles[i].transform.position = pos;
                }
			}  



            // ruch kamery
            Vector3 camPos = Camera.main.transform.position;
            //camPos.x += step * axis;
            camPos.x = car.transform.position.x + camRelPos;
            Camera.main.transform.position = camPos;
		}

        // skakanie
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            isJumping = true;
        }
        if (Input.GetKeyUp(KeyCode.UpArrow))
            isJumping = false;

        if (isJumping)
        {
            Vector2 pos = car.transform.position;
            pos.y += jumpStep;
            car.transform.position = pos;
            if (pos.y > 5.0)
                isJumping = false;
        }
	}
}
