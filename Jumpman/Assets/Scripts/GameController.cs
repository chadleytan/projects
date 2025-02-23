﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	public GameObject[] blocks;
	private GameObject nextBlock;
	private GameObject prevBlock;
	private AudioSource music;

	public Text scoreText;
	public Text restartText;
	public Text titleText;
	public Text instructionText;

	private bool gameOver;
	private bool gameStart;
	private bool restart;
	private float score;

	// Position where blocks spawn
	private Vector3 spawnPosition = new Vector3(13.5f, -4.0f, 0.0f);

	void Start () {
		music = GetComponent<AudioSource>();

		gameStart = false;
		gameOver = true;
		restart = false;
		score = 0;

		titleText.text = "Jumpman";
		scoreText.text = "";
		restartText.text = "";
		instructionText.text = "Press 'Space' to Begin";

		nextBlock = GameObject.FindGameObjectWithTag("FirstBlock");
	}

	void Update ()
	{	
		if (!gameStart && Input.GetKeyDown("space")) {
			gameStart = true;
			gameOver = false;
			titleText.text = "";
			instructionText.text = "";
			music.Play();
		}

		if (restart && gameOver)
		{
			if (Input.GetKeyDown (KeyCode.R))
			{
				Application.LoadLevel(Application.loadedLevel);
			}
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		//Generate new block when previous block has passed certain point
		if (nextBlock.transform.position.x <= 8.75f) {
			nextBlock = MakeNextBlock();
			Debug.Log("Block Name: "+ nextBlock.name);
		}

		if (!gameOver) {
			AddScore ();
		}
	}


	// Generates a new block and return instantiated block
	GameObject MakeNextBlock () {
		GameObject block = ChooseNextBlock();
		Quaternion spawnRotation = Quaternion.identity;
		return Instantiate(block, spawnPosition, spawnRotation);
	}

	// Generate the next possible choices for a block based on previously generated
	GameObject [] GenerateNextChoices() {
		

		if (nextBlock.name.Contains("LowBlock")) {
			GameObject [] nextChoices = new GameObject [3];
			nextChoices[0] = blocks[0];
			nextChoices[1] = blocks[1];
			nextChoices[2] = blocks[3];

			return nextChoices;
		}
		// Special case for NoBlock because height of next block must be equal or less
		else if (nextBlock.name.Contains("NoBlock")) {
			if (prevBlock.name.Contains("LowBlock")) {
				GameObject [] nextChoices = new GameObject [1];
				nextChoices[0] = blocks[0];

				return nextChoices;
			}
			else if (prevBlock.name.Contains("MidBlock")) {
				GameObject [] nextChoices = new GameObject [2];
				nextChoices[0] = blocks[0];
				nextChoices[1] = blocks[1];

				return nextChoices;
			}
			else {
				GameObject [] nextChoices = new GameObject [3];
				nextChoices[0] = blocks[0];
				nextChoices[1] = blocks[1];
				nextChoices[2] = blocks[2];

				return nextChoices;
			}
		}
		return blocks;
	}

	// Randomly select from next possible choices and return next choice
	GameObject ChooseNextBlock() {
		GameObject nextChoice;

		GameObject [] nextChoices = GenerateNextChoices();
		nextChoice = nextChoices[Random.Range(0, nextChoices.Length)];

		// Record previous block for next iteration so player can make jumps
		if (nextChoice.name.Contains("NoBlock")) {
			prevBlock = nextBlock;
		}
		
		return nextChoice;
	
	}

	void AddScore() {
		score += Time.deltaTime;
		UpdateScore();
	}
	void UpdateScore() {
		scoreText.text = "Time: " + Mathf.RoundToInt(score) + "s";
	}

	public bool getGameStatus() {
		return gameOver;
	}

	public void GameOver() {
		gameOver = true;
		restart = true;
		titleText.text = "GameOver";
		restartText.text = "Press 'R' to Restart";
		scoreText.text = "Final " + scoreText.text; 
		music.Stop();
	}
}
