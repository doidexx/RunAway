using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerS2 : MonoBehaviour {

	private Rigidbody playerRB;
	public GameObject[] prefabs;
	private GameObject clone, car;
    public GameObject liveF, myCanvas, seeker;
    private GameObject[] live, updaters;
	private Animator playerAni;

    private Vector3 initial, initialF;
	private float seekerS, speed, jumpSpeed, timer, jumpT, grav, l, c, r, bumpTime, seekerTime, dist, initialD;
	public int jumpNum, lives;
	private bool sliding, jumping, falling, movingR, movingL, onFloor, land, leftT, rightT, centerT, canMove, wallR, deciding, bumpT, chosen;
    // Use this for initialization
	void Start () {
		clone =  null;
        car = null;
		playerRB =  GetComponent<Rigidbody>();
		playerAni = GetComponent<Animator>();
        initialD = Vector3.Distance(playerRB.transform.position, seeker.transform.position);
		speed = 30f;
        seekerS = 30f;
		jumpSpeed = 10f;
		grav = 16f;
		l = -5f;
		c = 0f;
		r = 5f;
        lives = 3;
		centerT = true;
        chosen = false;
        deciding = false;
        initial = playerRB.transform.position;
        initialF = seeker.transform.position;
        //////////////////////////////////////////////////Set tracker triggers////////////////////////////////////
        updaters = new GameObject[WorldScript.lenght + 2];
        //////////////////////////////////////////////////Display lives///////////////////////////////////////////
        live = new GameObject[lives];
        for (int i = 0; i < lives; i++) {
            if (i > 0) {
                live[i] = Instantiate(liveF, new Vector3(live[i - 1].transform.position.x, live[i-1].transform.position.y - 60, liveF.transform.position.z), Quaternion.identity);
            } else {
                live[i] = Instantiate(liveF, new Vector3(liveF.transform.position.x + 62, liveF.transform.position.y, liveF.transform.position.z), Quaternion.identity); ;
            }
            live[i].transform.SetParent(myCanvas.transform);
        }
	}
	
	// Update is called once per frame
	void Update () {
        ////////////////////////////////////Get Space to jump/////////////////////////////////////////////////
        if (Input.GetKeyDown(KeyCode.Space) && onFloor && !jumping) {
			jumping = true;
        }
        ////////////////////////////////////Deciding which jump will be donw when/////////////////////////////
        if (deciding) {
            jumpNum = 10;
            speed = 5f;
            grav = 2f;
            seeker.SetActive(false);
            if (jumping) {
                deciding = false;
                chosen = true;
            }
        } else {
            if (!wallR) {
                jumpNum = 2;
                grav = 16f;
            } else {
                grav = 9.8f;
                if (transform.position.x < -2.5f) {
                    jumpNum = 6;
                }
                if (transform.position.x > 2.5f) {
                    jumpNum = 7;
                }
            }
		}
        ////////////////////////////////////Slow down by bumping in a table///////////////////////////////////
        if (bumpT) {
            playerAni.SetInteger("state", 8);
            speed = 20f;
            bumpTime+= Time.deltaTime;
            seekerTime = 0;
            seekerS = 30f;
        }
        if (bumpTime > 0.6f) {
            bumpT = false;
            bumpTime = 0;
            speed = 30f;
        }
        /////////////////////////////////////////Get seeker back to position//////////////////////////////////
        dist = Vector3.Distance(playerRB.transform.position, seeker.transform.position);
        if (!bumpT && dist < initialD) {
            seekerTime += Time.deltaTime;
        } 
        if (seekerTime > 5f) {
            if (dist < initialD) {
                seekerS = 20f;
            } else {
                seekerS = 30f;
                seekerTime = 0;
            }
        }
        
        ///////////////////////////////////Jumping////////////////////////////////////////////////////////////
        if (jumping) {
            jumpT += Time.deltaTime;
            speed = 30f;
            playerAni.SetInteger("state", jumpNum);
        }
        ///////////////////////////////////Get S to slide/////////////////////////////////////////////////////
        if (Input.GetKeyDown(KeyCode.S) && onFloor) {
			sliding = true;
        }
		if (sliding) {
            timer += Time.deltaTime;
            playerAni.SetInteger("state", 3);
		}
		////////////////////////////////////Stopping Sliding and Jumping////////////////////////////////////
		if (timer >= 0.02f) {
            sliding = false;
            timer = 0;
        }
		if (jumpT >= 0.35f && playerRB.transform.position.y > 3.5f) {
            jumping = false;
            jumpT = 0;
		}
		/////////////////////////////////////Run if on the floor////////////////////////////////////////////
		if (!sliding && !jumping && onFloor) {
            playerAni.SetInteger("state", 0);
		}
		///////////////////////////////////Changing tracks/////////////////////////////////////////////////////
        if (Input.GetKeyDown(KeyCode.D) && !movingL && !chosen) {
			movingR = true;
		}
        if (Input.GetKeyDown(KeyCode.A) && !movingR && !chosen) {
            movingL = true;
        }
		////////////////////////////////////Stopping on the Right Track///////////////////////////////////////
		if (transform.position.x >= r - 0.2f) {
			rightT = true;
			centerT = false;
            movingR = false;
		}
        ////////////////////////////////////Stopping on the Center Track///////////////////////////////////////
        if (transform.position.x >= c - 0.2f && transform.position.x <= c + 0.2f && !centerT && !deciding) {
			centerT = true;
			rightT = false;
			leftT = false;
			movingL = false;
			movingR = false;
		}
		////////////////////////////////////Stopping on the Left Track//////////////////////////////////////////
        if (transform.position.x <= l + 0.2f) {
            movingL = false;
			centerT = false;
			leftT = true;
        }
		///////////////////////////////////Landing After Jumping///////////////////////////////////////////////
		if (land) {
            playerAni.SetInteger("state", 4);
			land = false;
		}
		////////////////////////////////////Mid air animation if falling of the map////////////////////////////
		if (falling) {
            playerAni.SetInteger("state", 5);
		}
        ////////////////////////////////////Map animation loader/////////////////////////////////////////////////
		if (WorldScript.load > 4) {
        	clone.transform.position = Vector3.Lerp(clone.transform.position, new Vector3(0, 0, clone.transform.position.z), 0.125f);
		}
        /////////////////////////////////////////////////////////////////////////////////////////////////////////
        Debug.Log(playerAni.GetInteger("state"));
        //////////////////////////////////////Falling off////////////////////////////////////////////////////////
        if (playerRB.transform.position.y <= -10f) {
            playerReseter();
        }
        /////////////////////////////////////puting lives on screen/////////////////////////////////////////////
        if (lives > 0) {
            live[lives - 1].gameObject.SetActive(false);
        }
        /////////////////////////////////////Losing by falling to much//////////////////////////////////////////
        if (lives == 0) {
            SceneManager.LoadScene(2);
        }
    }

    void OnCollisionEnter(Collision other){
        if ((other.gameObject.tag == "table" || other.gameObject.tag == "locker") && !bumpT) {
            bumpT = true;
        }
        if (other.gameObject.tag == "garbage" &&  transform.position.z < other.transform.position.z) {
            bumpT = true;
            other.rigidbody.AddForce(Vector3.forward * 10, ForceMode.Impulse);
        }
        if (other.gameObject.tag == "car" && transform.position.z < other.transform.position.z - 4) {
            playerAni.SetInteger("state", 9);
            Debug.Log("state " + playerAni.GetInteger("state"));
            speed = -1;
            seekerS = 2f;
            Invoke("playerReseter", 0.5f);
        }
    }

	void OnTriggerEnter(Collider other){
    	if (other.gameObject.tag == "floor") {
			land = true;
			falling = false;
        }
        if (other.gameObject == seeker) {
            Invoke("seekerCatch", 1);
        }
        if (other.gameObject.tag == "portal") {
            if (SceneManager.GetActiveScene().buildIndex == 5) {
                Invoke("Win", 1);
            } else {
            Invoke("Portal", 0.1f);
            }
        }
        if (other.gameObject.tag == "portalR") {
            if (SceneManager.GetActiveScene().buildIndex == 5) {
                Invoke("Win", 1);
            } else {
            Invoke("PortalR", 0.1f);
            }
        }
        if (other.gameObject.tag == "updater") {
            updaters[WorldScript.meter] = other.gameObject;
            updaters[WorldScript.meter].SetActive(false);
            WorldScript.meter++;
        }
        if (other.gameObject.CompareTag("loader")) {
            WorldScript.load++;
            if (WorldScript.load <= WorldScript.lenght){
                if (WorldScript.load < WorldScript.lenght) {
                    other.gameObject.SetActive(false);
                    if (SceneManager.GetActiveScene().buildIndex == 1) {
                    clone = Instantiate(prefabs[Random.Range(1, 11)], new Vector3(Random.Range(-20, 20), Random.Range(-20, 20), WorldScript.load * 44.3f), Quaternion.identity);
                    }
                    if (SceneManager.GetActiveScene().buildIndex == 5) {
                        clone = Instantiate(prefabs[Random.Range(1, 11)], new Vector3(0, -40, WorldScript.load * 44.3f), Quaternion.identity);
                        if (clone.name == "red car(Clone)" || clone.name == "blue car(Clone)") {
                            car = clone.transform.GetChild(0).gameObject;
                            Debug.Log(car.name);
                        }
                    }
                    Destroy(other.gameObject);   
                }
                if (WorldScript.load == WorldScript.lenght) {
                    other.gameObject.SetActive(false);
                    clone = Instantiate(prefabs[12], new Vector3(Random.Range(-20, 20), Random.Range(-20, 20), WorldScript.load * 44.3f), Quaternion.identity);
                }
                other.gameObject.SetActive(false);
            }
        }
        if (other.gameObject.tag == "deside") {
            deciding = true;
        }
    }

	void OnTriggerExit(Collider other){
        if (other.gameObject.tag == "floor") {
            onFloor = false;
            falling = true;
        }
        if (other.gameObject.tag == "empty") {
			wallR = false;
        }
        if (other.gameObject.tag == "deside") {
            deciding = false;
            seeker.gameObject.SetActive(false);
        }
	}

	void OnTriggerStay(Collider other) {
        if (other.gameObject.tag == "floor"){
            onFloor = true;
			falling = false;
        }
		if (other.gameObject.tag == "empty") {
			if (Input.GetKey(KeyCode.W)) {
                wallR = true;
			}
		}
	}
	
	void FixedUpdate() {
		//////////////////////////////////Moving Right/////////////////////////////////////////////////
        if (centerT && movingR && !deciding) {
            playerRB.transform.position = new Vector3(Mathf.Lerp(transform.position.x, r, 0.125f), transform.position.y, transform.position.z);
		}
		if (rightT && movingL && !deciding) {
            playerRB.transform.position = new Vector3(Mathf.Lerp(transform.position.x, c, 0.125f), transform.position.y, transform.position.z);
        }
		//////////////////////////////////Moving Left/////////////////////////////////////////////////
        if (!deciding && centerT & movingL) {
            playerRB.transform.position = new Vector3(Mathf.Lerp(transform.position.x, l, 0.125f), transform.position.y, transform.position.z);
        }
		if (!deciding && leftT & movingR) {
            playerRB.transform.position = new Vector3(Mathf.Lerp(transform.position.x, c, 0.125f), transform.position.y, transform.position.z);
        }
        if (deciding && (leftT || centerT) && movingR) {
            playerRB.transform.position = new Vector3(Mathf.Lerp(transform.position.x, r, 0.125f), transform.position.y, transform.position.z);
        }
        if (deciding && (rightT || centerT) && movingL) {
            playerRB.transform.position = new Vector3(Mathf.Lerp(transform.position.x, l, 0.125f), transform.position.y, transform.position.z);
        }
        ///////////////////////////////Constant Movement Forward///////////////////////////////////////
        playerRB.MovePosition(playerRB.transform.position += Vector3.forward * speed * Time.deltaTime);
        seeker.GetComponent<Rigidbody>().MovePosition(seeker.transform.position += Vector3.forward * seekerS * Time.deltaTime);
        /////////////////////////////////////Gravity///////////////////////////////////////////////////
        playerRB.AddForce(Vector3.down * grav, ForceMode.Acceleration);
		if (jumping) {
            //playerRB.velocity += Vector3.up * jumpSpeed * Time.deltaTime;
			playerRB.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
		}
        //////////////////////////////////////Moving cars arround/////////////////////////////////////////
        if (car != null) {
            if (car.transform.localPosition.x < -6.7f) {
                car.transform.localPosition = new Vector3(Mathf.Lerp(car.transform.localPosition.x, -6.7f, 0.055f), car.transform.localPosition.y, car.transform.localPosition.z);
            }
            if (car.transform.localPosition.x > 7.3f) {
                car.transform.localPosition = new Vector3(Mathf.Lerp(car.transform.localPosition.x, 7.3f, 0.055f), car.transform.localPosition.y, car.transform.localPosition.z);
            }
        }
	}

    void playerReseter() {
        playerRB.transform.position = initial;
        seeker.transform.position = initialF;
        lives--;
        for (int i = 0; i < WorldScript.meter; i++) {
            updaters[i].SetActive(true);
        }
        playerAni.SetInteger("state", 0);
        speed = 30f;
        seekerS = 30f;
        WorldScript.meter = 0;
    }

    void seekerCatch() {
        SceneManager.LoadScene(2);
    }

    void Win  () {
        SceneManager.LoadScene(8);
    }
    void Portal() {
        SceneManager.LoadScene(7);
    }
    void PortalR() {
        SceneManager.LoadScene(7);
    }
}
