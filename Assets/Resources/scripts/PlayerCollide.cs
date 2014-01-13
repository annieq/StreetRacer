/// 
/// Autor: Tomasz Szołtysek
/// Data: 5.01.2013
/// 

using UnityEngine;
using System.Collections;

public class PlayerCollide : MonoBehaviour {
	public AudioClip oops;
	public AudioClip heart;
	public AudioClip letter;

	private int life;
    private bool[] lettersFound;
    private int lettersCount;
    private int isProtected = 0;
	private bool fadeGlow = false;
	
	private static Vector2[] heartRelPos;
	private static Vector2[] letterRelPos;

	//private float camRefY;

	// Use this for initialization
	void Start () {
		float carPosX = GameObject.FindGameObjectWithTag("Car").transform.position.x;
		GameObject[] hearts = GameObject.FindGameObjectsWithTag ("Heart");
		heartRelPos = new Vector2[hearts.Length];
		letterRelPos = new Vector2[5];
		lettersFound = new bool[letterRelPos.Length];
		
		life = heartRelPos.Length;  // ilość życia
		/*for (int i = 0; i < heartRelPos.Length; ++i) {
			heartRelPos[i].x = hearts[i].transform.position.x - carPosX;
			heartRelPos[i].y = hearts[i].transform.position.y;
		}

		GameObject[] letters = new GameObject[5];
		letters[0] = GameObject.Find("m");
		letters[1] = GameObject.Find("a");
		letters[2] = GameObject.Find("r");
		letters[3] = GameObject.Find("i");
		letters[4] = GameObject.Find("o");*/
		for (int i = 0; i < lettersFound.Length; ++i) {
			//letterRelPos[i].x = letters[i].transform.position.x - carPosX;
			//letterRelPos[i].y = letters[i].transform.position.y;
			
			lettersFound[i] = false;
		}
		
		//gameOverRelPos = GameObject.Find ("gameover").transform.position.x - carPosX;

		//camRefY = Camera.main.transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
		
		GameObject car = GameObject.FindGameObjectWithTag("Car");
		GameObject glow = GameObject.Find("glow");
		// zmienne potrzebne przy kolizjach
		float pos_c_x1 = car.transform.position.x + car.GetComponent<SpriteRenderer>().sprite.bounds.size.x / 2.0f; 	// przednia krawędź auta
		float pos_c_x2 = car.transform.position.x - car.GetComponent<SpriteRenderer>().sprite.bounds.size.x / 2.0f; 	// tylnia krawędź auta
		float pos_c_y1 = car.transform.position.y + car.GetComponent<SpriteRenderer>().sprite.bounds.size.y / 2.0f;    // górna krawędź auta
		float pos_c_y2 = car.transform.position.y - car.GetComponent<SpriteRenderer>().sprite.bounds.size.y / 2.0f;    // dolna krawędź auta
		
		Vector2 size, pos;
		
		// zbieranie literek
		GameObject[] letters_block = new GameObject[5];
		letters_block[0] = GameObject.Find("m-b");
		letters_block[1] = GameObject.Find("a-b");
		letters_block[2] = GameObject.Find("r-b");
		letters_block[3] = GameObject.Find("i-b");
		letters_block[4] = GameObject.Find("o-b");
		
		GameObject[] letters = new GameObject[5];
		letters[0] = GameObject.Find("m");
		letters[1] = GameObject.Find("a");
		letters[2] = GameObject.Find("r");
		letters[3] = GameObject.Find("i");
		letters[4] = GameObject.Find("o");
		for (int i = 0; i < letters_block.Length; ++i)
		{
			if (lettersFound[i]) continue;
			pos = letters_block[i].transform.position;
			if (letters_block[i].GetComponent<SpriteRenderer>().enabled 
			    && pos.x <= pos_c_x1 && pos.x >= pos_c_x2 && pos.y <= pos_c_y1 && pos.y >= pos_c_y2)
			{
                ++lettersCount;
				Camera.main.audio.PlayOneShot(letter);
                if (lettersCount == 5)
                {
                    isProtected = 3;
                    pos = glow.transform.position;
                    pos.y += 50;
                    glow.transform.position = pos;
					glow.transform.localScale = new Vector3(0.0f, 0.0f, 1.0f);
                }
				lettersFound[i] = true;
				pos = letters[i].transform.position;
				pos.x -= 50.0f;
				letters[i].transform.position = pos;
				letters_block[i].GetComponent<SpriteRenderer>().enabled = false;
				letters_block[i].GetComponent<ParticleSystem>().Play();
			}
		}
		// pojawianie się glow
		if ( (glow.transform.localScale.x < 1.0f || glow.transform.localScale.y < 1.0f) && !fadeGlow)
		{
			Vector2 scale = glow.transform.localScale;
			scale.x += 0.02f;
			scale.y += 0.02f;
			glow.transform.localScale = scale;
		}

		// zbieranie serduszek
		GameObject[] hearts = GameObject.FindGameObjectsWithTag("Heart");
		GameObject[] hearts_block = GameObject.FindGameObjectsWithTag("Heart-b");
		for (int i = 0; i < hearts_block.Length; ++i)
		{
			pos = hearts_block[i].transform.position;
			if (hearts_block[i].GetComponent<SpriteRenderer>().enabled 
			    && pos.x <= pos_c_x1 && pos.x >= pos_c_x2 && pos.y <= pos_c_y1 && pos.y >= pos_c_y2)
			{
				if (life < 3) {
					++life;
					pos = hearts[life-1].transform.position;
					pos.x += 100.0f;
					hearts[life-1].transform.position = pos;
				}
				Camera.main.audio.PlayOneShot(heart);
				hearts_block[i].GetComponent<SpriteRenderer>().enabled = false;
				hearts_block[i].GetComponent<ParticleSystem>().Play ();
			}
		}
		
		// sprawdzenie kolizji z przeszkodami
		GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
		for (int i = 0; i < obstacles.Length; ++i)
		{
			pos = obstacles[i].transform.position;
			size = obstacles[i].GetComponent<SpriteRenderer>().sprite.bounds.size;
			size.x *= obstacles[i].transform.localScale.x / 2.0f;
			size.y *= obstacles[i].transform.localScale.y / 2.0f;
			if(obstacles[i].GetComponent<SpriteRenderer>().enabled)
			if (((pos.x - size.x < pos_c_x1 && pos.x - size.x > pos_c_x2) || (pos.x + size.x < pos_c_x1 && pos.x + size.x > pos_c_x2))
			    && ((pos.y - size.y < pos_c_y1 && pos.y + size.y > pos_c_y1) || (pos.y - size.y < pos_c_y2 && pos.y + size.y > pos_c_y2)))
			{
				if (isProtected == 0)
				{
                    --life;
					pos = hearts[life].transform.position;
					pos.x -= 100.0f;
					hearts[life].transform.position = pos;
                    if (life == 0)
                    {
						PlayerMove.isPlaying = false;
						pos = GameObject.Find("gameover").transform.position;
						pos.x -= 50.0f;
						GameObject.Find("gameover").transform.position = pos;
					}
                }
                else
                {
                    --isProtected;
                    if (isProtected == 0)
                    {
						fadeGlow = true;
                        //pos = GameObject.Find("glow").transform.position;
                        //pos.y -= 50;
                        //GameObject.Find("glow").transform.position = pos;
                    }
				}
				Camera.main.audio.PlayOneShot(oops);
				obstacles[i].GetComponent<SpriteRenderer>().enabled = false;
				obstacles[i].GetComponent<ParticleSystem>().Play();
			}
		}
		// znikanie glow
		if (fadeGlow)
		{
			if (glow.transform.localScale.x > 0.0f || glow.transform.localScale.y > 0.0f)
			{
				Vector2 scale = glow.transform.localScale;
				scale.x -= 0.02f;
				scale.y -= 0.02f;
				glow.transform.localScale = scale;
			}
			if (glow.transform.localScale.x <= 0.0f)
			{
				fadeGlow = false;
				pos = glow.transform.position;
				pos.y -= 50;
				glow.transform.position = pos;
			}
		}
		// GUI
		/*// serca
		GameObject[] hearts = GameObject.FindGameObjectsWithTag("Heart");
		for (int i = 0; i < hearts.Length; ++i)
		{
			pos = hearts[i].transform.position;
			pos.x = car.transform.position.x + heartRelPos[i].x;
			pos.y = heartRelPos[i].y + Camera.main.transform.position.y - camRefY;
			hearts[i].transform.position = pos;
		}
		// literki
		GameObject[] letters = new GameObject[5];
		letters[0] = GameObject.Find("m");
		letters[1] = GameObject.Find("a");
		letters[2] = GameObject.Find("r");
		letters[3] = GameObject.Find("i");
		letters[4] = GameObject.Find("o");
		for (int i = 0; i < letters.Length; ++i)
		{
			pos = letters[i].transform.position;
			pos.x = car.transform.position.x + letterRelPos[i].x;
			pos.y = letterRelPos[i].y + Camera.main.transform.position.y - camRefY;
			letters[i].transform.position = pos;
        }

        // Game Over
        GameObject gameover = GameObject.Find("gameover");
        pos = gameover.transform.position;
        pos.x = car.transform.position.x + gameOverRelPos;
        gameover.transform.position = pos; */
	}
}
