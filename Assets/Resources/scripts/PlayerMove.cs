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
	private static bool isCamMoving = false;
	public static bool isPlaying = true;
	public static int levelID = 0;
	
	private static Vector2 camRelPos;

	private float carJumpRefY = 0.0f;
	private float camRefY;

	// Use this for initialization
	void Start () {
		Vector2 carPos = GameObject.FindGameObjectWithTag("Car").transform.position;
		camRelPos = new Vector2(Camera.main.transform.position.x - carPos.x,Camera.main.transform.position.y - carPos.y);
		levelID = Application.loadedLevel;
	}

    // Update is called once per frame
    void Update()
	{
		if(Input.GetKeyDown(KeyCode.Escape))
			Application.Quit();

		if (!isPlaying) {
			if(Input.GetKeyDown(KeyCode.Return)) {
				Application.LoadLevel(levelID);
				isPlaying = true;
			}
			return;
		}
		
		float axis = Input.GetAxis("Horizontal");
        GameObject car = GameObject.FindGameObjectWithTag("Car");
		Quaternion carRot = car.transform.rotation;
		float carRotZ = carRot.eulerAngles.z;
		if (carRotZ > 30.0f && carRotZ < 180.0f)
			carRotZ = 30.0f;
		else if (carRotZ > 180.0f && carRotZ < 330.0f)
			carRotZ = 330.0f;
		else if (carRotZ > -180.0f && carRotZ < -30.0f)
			carRotZ = -30.0f;
		carRot.eulerAngles = new Vector3(
			carRot.eulerAngles.x,
			carRot.eulerAngles.y,
			carRotZ
		);
		car.transform.rotation = carRot;
		GameObject[] wheels = GameObject.FindGameObjectsWithTag ("wheel");
		if (axis > 0.0) {
            
            // ruch autka
			Vector2 pos = car.transform.position;
            pos.x += step * axis;
            car.transform.position = pos;

			float radius = (step*axis*360.0f)/(2.04f*Mathf.PI);

			foreach(GameObject w in wheels)
			{
				Quaternion wRot = w.transform.rotation;
				wRot.eulerAngles = new Vector3(
					wRot.eulerAngles.x, wRot.eulerAngles.y,
					wRot.eulerAngles.z - radius
					);
				w.transform.rotation = wRot;
			}
            // obrót auta
            /*Vector2 scale = car.transform.localScale;
            if (axis < 0.0)
                scale.x = -1.0f;
            else if (axis > 0.0)
                scale.x = 1.0f;
            car.transform.localScale = scale;*/

		}
		// ruch kamery
		Vector3 camPos = Camera.main.transform.position;
		//camPos.x += step * axis;
		camPos.x = Mathf.Lerp(camPos.x,car.transform.position.x + camRelPos.x,0.2f);
		if ((car.transform.position.y - camPos.y > 6.0f) || (car.transform.position.y - camPos.y < -3.0f))
			isCamMoving = true;
		
		if (isCamMoving) {
			camPos.y = Mathf.Lerp (camPos.y, car.transform.position.y + camRelPos.y, 0.05f);
			if ((car.transform.position.y - camPos.y < 2.5f) && (car.transform.position.y - camPos.y > -1.5f))
				isCamMoving = false;
		}
		Camera.main.transform.position = camPos;

        // skakanie
        if (Input.GetKeyDown(KeyCode.UpArrow) && !(car.rigidbody2D.velocity.y < -0.5f))
        {
            isJumping = true;
			carJumpRefY = car.transform.position.y;
        }
        if (Input.GetKeyUp(KeyCode.UpArrow))
            isJumping = false;

        if (isJumping)
		{
			foreach (GameObject fire in GameObject.FindGameObjectsWithTag("Fire"))
			{
				fire.GetComponent<SpriteRenderer>().enabled = true;
			}
			Vector2 pos = car.transform.position;
            pos.y += jumpStep;
            car.transform.position = pos;
            if (pos.y - carJumpRefY > 8.0f)	// poprawka tolerancji kamery
                isJumping = false;
        }
		if (!isJumping)
		{
			foreach (GameObject fire in GameObject.FindGameObjectsWithTag("Fire"))
			{
				fire.GetComponent<SpriteRenderer>().enabled = false;
			}
		}
	}
	void LateUpdate()
	{
	}
}
