/// 
/// Autor: Tomasz Szołtysek
/// Data: 5.01.2013
/// 

using UnityEngine;

public class PlayerCollide : MonoBehaviour {
	public AudioClip oops;
	public AudioClip heart;
	public AudioClip letter;

	private int life;
    private bool[] lettersFound;
    private int lettersCount;
    private int isProtected = 0;
	private bool fadeGlow = false;
	private bool fadeHeart = false;

	private Vector2 curLetterPos;
	private Vector2 destLetterPos;
    private Vector2 step;
    private int curLetter;
    private Vector2 curHeartPos;
    private Vector2 destHeartPos;
    private Vector2 hstep;
    private int curHeart;
	
	private static Vector2[] heartRelPos;
	private static Vector2[] letterRelPos;

	void Start () {
		GameObject[] hearts = GameObject.FindGameObjectsWithTag ("Heart");
		heartRelPos = new Vector2[hearts.Length];
		letterRelPos = new Vector2[5];
		lettersFound = new bool[letterRelPos.Length];
		
		life = heartRelPos.Length;  // ilość życia

		for (int i = 0; i < lettersFound.Length; ++i) {
			lettersFound[i] = false;
		}

        curLetter = -1;
        curHeart = -1;
	}

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
			if (lettersFound[i]) continue;	// ta literka jest juz zebrana
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
				// przesuniecie literki na swoje miejsce
				curLetter = i;
                curLetterPos = letters_block[i].transform.position;
                curLetterPos.x -= Camera.main.transform.position.x;
                curLetterPos.y -= Camera.main.transform.position.y;
				destLetterPos = letters[i].transform.localPosition;
				destLetterPos.x -= 50.0f;
                letters[i].transform.localPosition = curLetterPos;
                Vector2 diff = destLetterPos - curLetterPos;
                step = diff / 25;

				letters_block[i].GetComponent<SpriteRenderer>().enabled = false;
				letters_block[i].GetComponent<ParticleSystem>().Play();
			}
		}
        if (curLetter >= 0)
        {
            // przesuniecie literki na swoje miejsce
            Vector2 diff = destLetterPos - curLetterPos;
            if (diff.x > 0.0f && diff.y > 0.0f)
            {
                curLetterPos += step;
                letters[curLetter].transform.position = curLetterPos + new Vector2(Camera.main.transform.position.x,Camera.main.transform.position.y);
            }
            else
            {
                letters[curLetter].transform.position = destLetterPos + new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y);
                curLetter = -1;
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
                    curHeart = life;
                    curHeartPos = hearts_block[i].transform.position;
                    curHeartPos.x -= Camera.main.transform.position.x;
                    curHeartPos.y -= Camera.main.transform.position.y;
                    destHeartPos = hearts[life].transform.localPosition;
                    destHeartPos.x += 100.0f;
                    hearts[life].transform.localPosition = curHeartPos;
                    Vector2 diff = destHeartPos - curHeartPos;
                    hstep = diff / 25;

					++life;
				}
				Camera.main.audio.PlayOneShot(heart);
				hearts_block[i].GetComponent<SpriteRenderer>().enabled = false;
				hearts_block[i].GetComponent<ParticleSystem>().Play ();
			}
        }
        if (curHeart >= 0)
        {
            // przesuniecie serca na swoje miejsce
            Vector2 diff = destHeartPos - curHeartPos;
            if (diff.x < 0.0f && diff.y > 0.0f)
            {
                curHeartPos += hstep;
                hearts[curHeart].transform.position = curHeartPos + new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y);
            }
            else
            {
                hearts[curHeart].transform.position = destHeartPos + new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y);
                curHeart = -1;
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
					fadeHeart = true;

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
		// znikanie serduszka
		if (fadeHeart)
		{
			if (hearts[life].transform.localScale.x > 0.0f || hearts[life].transform.localScale.y > 0.0f)
			{
				Vector2 scale = hearts[life].transform.localScale;
				scale.x -= 0.02f;
				scale.y -= 0.02f;
				hearts[life].transform.localScale = scale;
			}
			if (hearts[life].transform.localScale.x <= 0.0f)
			{
				fadeHeart = false;
				pos = hearts[life].transform.position;
				pos.x -= 100;
				hearts[life].transform.position = pos;
				Vector2 scale = hearts[life].transform.localScale;
				scale.x = 0.3f;
				scale.y = 0.3f;
				hearts[life].transform.localScale = scale;
			}
		}
	}
}
