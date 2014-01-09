/// 
/// Autor: Anna Kuśnierz
/// Data: 5.01.2013
/// 

using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour {

    public float step = 0.5f;
    public float jumpStep = 0.5f;
	public int life;
    public bool[] lettersFound;

	private static bool isJumping = false;
	private static bool isPlaying = true;
	
	private static float camRelPos;
    private static float[] heartRelPos;
	private static float[] letterRelPos;
	private static float gameOverRelPos;

	// Use this for initialization
	void Start () {
		float carPosX = GameObject.FindGameObjectWithTag("Car").transform.position.x;
		camRelPos = Camera.main.transform.position.x - carPosX;
        heartRelPos = new float[GameObject.FindGameObjectsWithTag("Heart").Length];
        letterRelPos = new float[GameObject.FindGameObjectsWithTag("Letter").Length];
        lettersFound = new bool[letterRelPos.Length];

        life = heartRelPos.Length;  // ilość życia

        for (int i = 0; i < heartRelPos.Length; ++i)
			heartRelPos[i] = GameObject.FindGameObjectsWithTag("Heart")[i].transform.position.x - carPosX;

        GameObject[] letters = new GameObject[5];
        letters[0] = GameObject.Find("m");
        letters[1] = GameObject.Find("a");
        letters[2] = GameObject.Find("r");
        letters[3] = GameObject.Find("i");
        letters[4] = GameObject.Find("o");
        for (int i = 0; i < letterRelPos.Length; ++i) {
			letterRelPos[i] = letters[i].transform.position.x - carPosX;

            lettersFound[i] = false;
        }
		gameOverRelPos = GameObject.Find ("gameover").transform.position.x - carPosX;
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

            /* Kolizje z przeszkodami, życie i literki
             * autor: Tomasz Szołysek
             */
            // zmienne potrzebne przy kolizjach
            float pos_c_x1 = car.transform.position.x + car.GetComponent<BoxCollider2D>().size.x / 2.0f; // przednia krawędź auta
            float pos_c_x2 = car.transform.position.x - car.GetComponent<BoxCollider2D>().size.x / 2.0f; // tylnia krawędź auta
            float pos_c_y1 = car.transform.position.y + car.GetComponent<BoxCollider2D>().size.y / 2.0f;    // górna krawędź auta
            float pos_c_y2 = car.transform.position.y - car.GetComponent<BoxCollider2D>().size.y / 2.0f;    // dolna krawędź auta

            Vector2 size;

            // zbieranie literek
            GameObject[] letters_block = new GameObject[5];
            letters_block[0] = GameObject.Find("m-b");
            letters_block[1] = GameObject.Find("a-b");
            letters_block[2] = GameObject.Find("r-b");
            letters_block[3] = GameObject.Find("i-b");
            letters_block[4] = GameObject.Find("o-b");
            for (int i = 0; i < letters_block.Length; ++i)
            {
                if (lettersFound[i]) continue;
                pos = letters_block[i].transform.position;
                if (pos.x <= pos_c_x1 && pos.x >= pos_c_x2 && pos.y <= pos_c_y1 && pos.y >= pos_c_y2)
                {
                    lettersFound[i] = true;
                    letterRelPos[i] -= 50;
                }
            }

            GameObject[] hearts_block = GameObject.FindGameObjectsWithTag("Heart-b");
            for (int i = 0; i < hearts_block.Length; ++i)
            {
                pos = hearts_block[i].transform.position;
                if (pos.x <= pos_c_x1 && pos.x >= pos_c_x2 && pos.y <= pos_c_y1 && pos.y >= pos_c_y2)
                {
                    if (life < 3) {
						++life;
	                    heartRelPos[life-1] += 100;
					}
                    Destroy(hearts_block[i]);
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
                if (((pos.x - size.x <= pos_c_x1 && pos.x + size.x >= pos_c_x1) || (pos.x - size.x <= pos_c_x2 && pos.x + size.x >= pos_c_x2))
                    && ((pos.y - size.y <= pos_c_y1 && pos.y + size.y >= pos_c_y1) || (pos.y - size.y <= pos_c_y2 && pos.y + size.y >= pos_c_y2)))
                {
                    --life;
					Destroy(obstacles[i]);
                    heartRelPos[life] -= 100;
					if(life == 0) {
						isPlaying = false;
						gameOverRelPos -= 50;
					}
                }
            }  

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

            GameObject[] letters = new GameObject[5];
            letters[0] = GameObject.Find("m");
            letters[1] = GameObject.Find("a");
            letters[2] = GameObject.Find("r");
            letters[3] = GameObject.Find("i");
            letters[4] = GameObject.Find("o");
            for (int i = 0; i < letters.Length; ++i)
            {
                pos = letters[i].transform.position;
                pos.x = car.transform.position.x + letterRelPos[i];
                letters[i].transform.position = pos;
            }            


			// Game Over

			GameObject gameover = GameObject.Find("gameover");
			pos = gameover.transform.position;
			pos.x = car.transform.position.x + gameOverRelPos;
			gameover.transform.position = pos;


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
