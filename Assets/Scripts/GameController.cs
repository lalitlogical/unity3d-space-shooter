﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

	public GameObject[] hazards;
	public GameObject bulletActivator;
	public GameObject shieldActivator;
	public GameObject player;
	public Vector3 spawnValue;
	public int hazardCount;
	public float spawnWait;
	public float startWait;
	public float waveWait;
	public int activateBullet = 100;

	public Text scoreText; 

	private bool gameOver;
	private int score;
	private int upgradeBullet = 0;

	public GameObject menuPanel;

	private Mover mover;
	private bool activateBulletActivator = true;
	private bool activateShieldActivator = true;

	void Start () {
		gameOver = false;
		score = 0;
		UpdateScore ();
		StartCoroutine (SpanWaves ());
		PlayerPrefs.SetInt ("BulletCount", 1);	
	}

	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape)) 
			SceneManager.LoadScene ("Home");
	}

	IEnumerator SpanWaves () {
		yield return new WaitForSeconds (startWait);
		while(true) {
			activateBulletActivator = activateShieldActivator = true;
			int bulletCount = PlayerPrefs.GetInt ("BulletCount");				 
			for (int i = 0; i < hazardCount * bulletCount; i++) {
				if (gameOver) {
					break;
				}

				if (activateBulletActivator && bulletCount < 3 && upgradeBullet == Random.Range (0, 10)) {	
					if (score > bulletCount * activateBullet) {
						ObjectActivator (bulletActivator);
						activateBulletActivator = false;
					} else {
						GenerateHazard (bulletCount);
					}
				} else if (activateShieldActivator && 5 == Random.Range (0, 10)) {
					GameObject isShieldPresent = GameObject.FindWithTag ("Shield");
					if (isShieldPresent == null) {
						ObjectActivator (shieldActivator);
						activateShieldActivator = false;
					} else {
						GenerateHazard (bulletCount);
					}
				} else {
					GenerateHazard (bulletCount);
				}
				yield return new WaitForSeconds (spawnWait / bulletCount);
			}


			upgradeBullet++;
			if (upgradeBullet >= 10) {
				upgradeBullet = 0;
			}

			yield return new WaitForSeconds (waveWait / bulletCount);

			if (gameOver) {
				break;
			}
		}
	}

	void GenerateHazard (int bulletCount) {
		GameObject hazard = hazards[Random.Range (0, hazards.Length)];
		Vector3 spawnPosition = new Vector3 (Random.Range (-spawnValue.x, spawnValue.x), spawnValue.y, spawnValue.z);
		Quaternion spawnRotation = Quaternion.identity;
		GameObject hazardObject = Instantiate (hazard, spawnPosition, spawnRotation) as GameObject;	
		if (bulletCount > 1) {
			mover = hazardObject.GetComponent <Mover> (); 
			if (mover != null) {
				mover.speed = mover.speed - bulletCount;
			}	
		}
	}

	void ObjectActivator (GameObject activator) {
		Quaternion spawnRotation1 = Quaternion.identity;
		Vector3 spawnPosition1 = new Vector3 (Random.Range (-spawnValue.x, spawnValue.x), spawnValue.y, spawnValue.z);
		Instantiate (activator, spawnPosition1, spawnRotation1);
	}

	public void AddScore (int newScoreValue) {
		score += newScoreValue;
		int bestScore = PlayerPrefs.GetInt ("BestScore");
		if (score > bestScore) {
			PlayerPrefs.SetInt ("BestScore", score);	
		}
		UpdateScore ();
	}

	void UpdateScore () {
		if (scoreText != null) {
			scoreText.text = "Score: " + score;	
		}
	}

	public void GameOver () {
		gameOver = true;
		PlayerPrefs.SetInt ("BulletCount", 1);	
		menuPanel.SetActive (true);
	}

	public void UpgradeWeapon () {
		int bulletCount = PlayerPrefs.GetInt ("BulletCount");
		bulletCount += 1;
		PlayerPrefs.SetInt ("BulletCount", bulletCount);	
	}

	public void ReStartGame () {
		SceneManager.LoadScene ("Game");
	}

	public void BackFromGame () {
		SceneManager.LoadScene ("Home");
	}
}