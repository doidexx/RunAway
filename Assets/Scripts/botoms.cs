﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class botoms : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

	public void PlayAgain(){
		SceneManager.LoadScene(1);
	}

	public void ExitTheGame(){
		Application.Quit();
	}
}
