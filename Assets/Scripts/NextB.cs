using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NextB : MonoBehaviour {

	public GameObject[] showThis;
	public Text myBText;
	// Use this for initialization

	private float slide;
	void Start () {
		slide = 0;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (slide == 1) {
			showThis[1].SetActive(true);
            showThis[2].SetActive(false);
            showThis[3].SetActive(false);
		}
        if (slide == 2) {
            showThis[1].SetActive(false);
            showThis[2].SetActive(true);
            showThis[3].SetActive(false);
        }
		if (slide == 3) {
            showThis[1].SetActive(false);
            showThis[2].SetActive(false);
            showThis[3].SetActive(true);
			myBText.text = "R U N";
        } else {
            myBText.text = "Next";
        }
		if (slide == 4) {
            if (SceneManager.GetActiveScene().buildIndex == 6) {
				SceneManager.LoadScene(1);
			}
			if (SceneManager.GetActiveScene().buildIndex == 7) {
                SceneManager.LoadScene(5);
            }
            if (SceneManager.GetActiveScene().buildIndex == 8) {
                SceneManager.LoadScene(4);
            }
		}
	}

	public void Next () {
		slide++;
	}
}
