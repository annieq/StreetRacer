/// 
/// Autor: Tomasz Szołtysek
/// Data: 5.01.2013
/// 

using UnityEngine;
using System.Collections;

public class PlayerCollide : MonoBehaviour {
	private int life;
    private bool[] lettersFound;
    private int lettersCount;
    private int isProtected = 0;
	
	private static float[] heartRelPos;
	private static float[] letterRelPos;
	private static float gameOverRelPos;

	// Use this for initialization
	void Start () {
		float carPosX = GameObject.FindGameObjectWithTag("Car").transform.position.x;
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
	void Update () {
		
		GameObject car = GameObject.FindGameObjectWithTag("Car");
		// zmienne potrzebne przy kolizjach
		float pos_c_x1 = car.transform.position.x + car.GetComponent<BoxCollider2D>().size.x / 2.0f; 	// przednia krawędź auta
		float pos_c_x2 = car.transform.position.x - car.GetComponent<BoxCollider2D>().size.x / 2.0f; 	// tylnia krawędź auta
		float pos_c_y1 = car.transform.position.y + car.GetComponent<BoxCollider2D>().size.y / 2.0f;    // górna krawędź auta
		float pos_c_y2 = car.transform.position.y - car.GetComponent<BoxCollider2D>().size.y / 2.0f;    // dolna krawędź auta
		
		Vector2 size, pos;
		
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
                ++lettersCount;
                if (lettersCount == 5)
                {
                    isProtected = 3;
                    pos = GameObject.Find("glow").transform.position;
                    pos.y += 50;
                    GameObject.Find("glow").transform.position = pos;
                }
				lettersFound[i] = true;
				letterRelPos[i] -= 50;
				Destroy(letters_block[i]);
			}
		}
		// zbieranie serduszek
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
                if (isProtected == 0)
                {
                    --life;
                    heartRelPos[life] -= 100;
                    if (life == 0)
                    {
                        PlayerMove.isPlaying = false;
                        gameOverRelPos -= 50;
                    }
                }
                else
                {
                    --isProtected;
                    if (isProtected == 0)
                    {
                        pos = GameObject.Find("glow").transform.position;
                        pos.y -= 50;
                        GameObject.Find("glow").transform.position = pos;
                    }
                }
                Destroy(obstacles[i]);
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
	}
}
