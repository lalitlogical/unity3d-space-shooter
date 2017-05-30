using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

	public GameObject[] hazards;
	public GameObject bulletActivator;
	public GameObject shieldActivator;
	public GameObject cleanerActivator;
	public GameObject cleanerButton;

	public GameObject player;
	public GameObject cleaner;
	public Vector3 spawnValue;

	public int hazardCount;
	public float spawnWait;
	public float startWait;
	public float waveWait;
	public int activateBullet = 100;
	public Text scoreText; 
	public Text yourScoreText; 
	public Text highestScoreText;

	private bool gameOver;
	private int score;
	private int upgradeBullet = 0;

	public GameObject gameOverPanel;
	public GameObject gamePausePanel;
	public GameObject pauseButton;

	private Mover mover;
	private int  bulletActiveIndex = 0;
	private bool activateBulletActivator = true;
	private int  shieldActiveIndex = 0;
	private bool activateShieldActivator = true;
	private int  cleanerActiveIndex = 0;
	private bool activateCleanerActivator = true;

	private bool isPaused = false;
	private string GameMode;

	public float barDisplay; //current progress
	public Vector2 pos = new Vector2(20,40);
	public Vector2 size = new Vector2(60,20);
	public Texture2D emptyTex;
	public Texture2D fullTex;

	public float speedIncreaser = -0.0001f;
	private float updateSpeedBy = 0.0f;

	void Start () {
		score = 0;
		gameOver = false;
		UpdateScore ();
		StartCoroutine (SpanWaves ());
		PlayerPrefs.SetInt ("BulletCount", 1);	
		GameMode = PlayerPrefs.GetString ("GameMode");
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			PauseTheGame ();
		}
		if (!gameOver) {
			if (GameMode == "NormalMode") {
				updateSpeedBy += speedIncreaser;
			} else if (GameMode == "ExpertMode") {
				updateSpeedBy += speedIncreaser * 10;
			}	
		}
	}

	void OnGUI() {
		
	}

//	public void DrawProgressBar () {
//		//draw the background:
//		GUI.BeginGroup(new Rect(pos.x, pos.y, size.x, size.y));
//		GUI.Box(new Rect(0,0, size.x, size.y), emptyTex);
//
//		//draw the filled-in part:
//		GUI.BeginGroup(new Rect(0,0, size.x * barDisplay, size.y));
//		GUI.Box(new Rect(0,0, size.x, size.y), fullTex);
//		GUI.EndGroup();
//		GUI.EndGroup();
//	}

	IEnumerator SpanWaves () {
		yield return new WaitForSeconds (startWait);
		while(true) {
			int bulletCount  = PlayerPrefs.GetInt ("BulletCount");		
			int totalCount = hazardCount * bulletCount;
			if (!activateBulletActivator && shieldActiveIndex >= totalCount)
				activateBulletActivator = true;
			
			if (!activateShieldActivator && shieldActiveIndex >= totalCount)
				activateShieldActivator = true;

			if (!activateCleanerActivator && cleanerActiveIndex >= totalCount)
				activateCleanerActivator = true;

			for (int i = 0; i < totalCount; i++) {
				if (gameOver) break;

				if (activateBulletActivator && bulletCount < 3 && upgradeBullet == Random.Range (0, 10)) {	
					if (score > bulletCount * activateBullet) {
						ObjectActivator (bulletActivator);
						activateBulletActivator = false;
						bulletActiveIndex = 0;
					} else {
						GenerateHazard (bulletCount);
					}
				} else if (activateShieldActivator && Random.Range (0, 20) == Random.Range (0, 20)) {
					GameObject isShieldPresent = GameObject.FindWithTag ("Shield");
					if (isShieldPresent == null) {
						ObjectActivator (shieldActivator);
						activateShieldActivator = false;
						shieldActiveIndex = 0;
					} else {
						GenerateHazard (bulletCount);
					}
				} else if (activateCleanerActivator && Random.Range (0, 20) == Random.Range (0, 20)) {
					GameObject isCleanerPresent = GameObject.FindWithTag ("Cleaner");
					if (isCleanerPresent == null) {
						ObjectActivator (cleanerActivator);
						activateCleanerActivator = false;
						cleanerActiveIndex = 0;
					} else {
						GenerateHazard (bulletCount);
					}
				} else {
					GenerateHazard (bulletCount);
				}

				if (!activateBulletActivator)
					bulletActiveIndex++;
				
				if (!activateShieldActivator)
					shieldActiveIndex++;

				if (!activateCleanerActivator)
					cleanerActiveIndex++;
				
				yield return new WaitForSeconds (spawnWait / bulletCount);
			}

			upgradeBullet++;
			if (upgradeBullet >= 10) upgradeBullet = 0;

			yield return new WaitForSeconds (waveWait / bulletCount);
			if (gameOver) break;
		}
	}

	void GenerateHazard (int bulletCount) {
		GameObject hazard = hazards[Random.Range (0, hazards.Length)];
		Vector3 spawnPosition = new Vector3 (Random.Range (-spawnValue.x, spawnValue.x), spawnValue.y, spawnValue.z);
		Quaternion spawnRotation = Quaternion.identity;
		GameObject hazardObject = Instantiate (hazard, spawnPosition, spawnRotation) as GameObject;	
		if (hazardObject != null) {
			mover = hazardObject.GetComponent <Mover> (); 
			if (mover != null) mover.speed = mover.speed + updateSpeedBy;
		}
	}

	void ObjectActivator (GameObject activator) {
		Quaternion spawnRotation1 = Quaternion.identity;
		Vector3 spawnPosition1 = new Vector3 (Random.Range (-spawnValue.x, spawnValue.x), spawnValue.y, spawnValue.z);
		Instantiate (activator, spawnPosition1, spawnRotation1);
	}

	public void AddScore (int newScoreValue) {
		score += newScoreValue;
		UpdateScore ();
	}

	void UpdateScore () {
		if (scoreText != null) scoreText.text = "Score: " + score;
	}

	public void GameOver () {
		gameOver = true;

		pauseButton.SetActive (false);
		gameOverPanel.SetActive (true);
		gamePausePanel.SetActive (false);
		PlayerPrefs.SetInt ("BulletCount", 1);

		int bestScore = PlayerPrefs.GetInt ("BestScore");
		if (score > bestScore) {
			bestScore = score;
			if (GameMode != "ChildMode") PlayerPrefs.SetInt ("BestScore", score);	
		}

		scoreText.text = "";
		yourScoreText.text = "Your Score - " + score;
		highestScoreText.text = "Best score - " + bestScore;
	}

	public void UpgradeWeapon () {
		int bulletCount = PlayerPrefs.GetInt ("BulletCount");
		bulletCount += 1;
		PlayerPrefs.SetInt ("BulletCount", bulletCount);	
	}

	public void ActivateCleaner () {
		cleanerButton.SetActive (true);
	}

	public void ReStartGame () {
		if (isPaused) {
			Time.timeScale = 1;
		}
		updateSpeedBy = 0.0f;
		SceneManager.LoadScene ("Game");
	}

	public void BackFromGame () {
		Time.timeScale = 1;
		SceneManager.LoadScene ("Home");
	}

	public void PauseTheGame () {
		pauseButton.SetActive (isPaused);
		isPaused = !isPaused;
		if (isPaused) {
			Time.timeScale = 0;
		} else {
			Time.timeScale = 1;
		}
		gamePausePanel.SetActive (isPaused);
	}
}
