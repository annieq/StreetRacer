/// 
/// Autor: Anna Kuśnierz
/// Data: 5.01.2013
/// 

using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour {

    public float step = 0.5f;
    public float jumpStep = 0.5f;

	private static bool isJumping = false;
	public static bool isPlaying = true;
	
	private static float camRelPos;

	private float carJumpRefY = 0.0f;
	private float camRefY;

	// Use this for initialization
	void Start () {
		float carPosX = GameObject.FindGameObjectWithTag("Car").transform.position.x;
		camRelPos = Camera.main.transform.position.x - carPosX;

		camRefY = Camera.main.transform.position.y;
	}

    // Update is called once per frame
    void Update()
    {
		if (!isPlaying) {
			if(Input.GetKeyDown(KeyCode.Return)) {
				Application.LoadLevel(Application.loadedLevelName);
				isPlaying = true;
			}
			else if(Input.GetKeyDown(KeyCode.Escape))
				Application.Quit();
			return;
		}

		
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

            // ruch kamery
            Vector3 camPos = Camera.main.transform.position;
            //camPos.x += step * axis;
            camPos.x = car.transform.position.x + camRelPos;
			if(car.transform.position.y > 5.0f)
				camPos.y = camRefY + car.transform.position.y - 5.0f;
			else
				camPos.y = camRefY;
            Camera.main.transform.position = camPos;
		}

        // skakanie
        if (Input.GetKeyDown(KeyCode.UpArrow) && !(car.rigidbody2D.velocity.y < -0.2f))
        {
            isJumping = true;
			carJumpRefY = car.transform.position.y;
        }
        if (Input.GetKeyUp(KeyCode.UpArrow))
            isJumping = false;

        if (isJumping)
        {
			Vector2 pos = car.transform.position;
            pos.y += jumpStep;
            car.transform.position = pos;
            if (pos.y - carJumpRefY > 6.0)
                isJumping = false;
        }
	}
}
