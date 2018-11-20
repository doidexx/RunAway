using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerS : MonoBehaviour {

	private Rigidbody rb;
	private GameObject seek, clone;
    public GameObject[] prefabs;
	public float speed, tspeed, gravForce, jumpForce, orH, slideT, jumpT;
	private bool movLeft, movRight, jumping, slide, onTheFloor, wallRuning, center, right, left;

	private Vector3 slideH;
	// Use this for initialization
    void Start () {
        clone = null;
		speed = 40f;
		tspeed = 5f;
		gravForce = 20f;
		jumpForce = 40f;
		seek = GameObject.Find("Seeker");
		rb = GetComponent<Rigidbody>();
		orH = transform.lossyScale.y;
		slideH = new Vector3(transform.lossyScale.x, 2.0f, transform.lossyScale.z);
	}

    // Update is called once per frame
    void Update() {//Getting Controll Key
        if (Input.GetKeyDown(KeyCode.A) && !movRight) {
            movLeft = true;
        }
        if (Input.GetKeyDown(KeyCode.D) && !movLeft) {
            movRight = true;
        }
        //jumping
        if (Input.GetKeyDown(KeyCode.Space) && onTheFloor) {
            jumping = true;
        }
        //sliding
        if (Input.GetKeyDown(KeyCode.S)) {
            slide = true;
            wallRuning = false;
        }
        //wallruning
        if (Input.GetKey(KeyCode.W) && !slide) {
            Debug.Log(wallRuning);
            wallRuning = true;
        }
        if (Input.GetKeyUp(KeyCode.W)) {
            wallRuning = false;
            Debug.Log(wallRuning);
        }
        // Detecting when there is no floor contact
        if (jumpT >= 0.4f) {
            jumpT = 0;
            jumping = false;
        }
        // Limitting jumping air time
        if (jumping) {
            jumpT += Time.deltaTime;
        }
        // Limitting the movement to the left
        if (transform.position.x <= -5) {
            transform.position = new Vector3(-5, transform.position.y, transform.position.z);
            movLeft = false;
        }
        // Limitting the movement to the right
        if (transform.position.x >= 5) {
            transform.position = new Vector3(5, transform.position.y, transform.position.z);
            movRight = false;
        }
        // Getting out of sliding
        if (transform.localScale == slideH) {
            slideT += Time.deltaTime;
        }
        if (slideT >= 0.15f) {
            slide = false;
            slideT = 0;
        }
        // Player fall off the map
        if (transform.position.y < -13) {
            SceneManager.LoadScene(2);
        }
        // Making the player stop at the center track
        if (transform.position.x > -0.5f && transform.position.x < 0.5f && !center) {
            movRight = false;
            movLeft = false;
            center = true;
        }
        if (center && !movLeft && !movRight) {
            transform.position = new Vector3(0, transform.position.y, transform.position.z);
        }
        if (transform.position.x >= 2 || transform.position.x <= -2) {
            center = false;
        }
        if (WorldScript.load > 4){
            clone.transform.position = Vector3.Lerp(clone.transform.position, new Vector3(0,0, clone.transform.position.z), 0.125f);
        }
	}

	void FixedUpdate () {
		// Seeker always following
		seek.GetComponent<Rigidbody>().MovePosition(seek.transform.position + seek.transform.forward * (Time.deltaTime * speed));
		//Constantly moving forward
		rb.MovePosition(transform.position + transform.forward * (Time.deltaTime * speed));
        //Gravity
        rb.AddForce(Vector3.down * gravForce);
		//Moving to the left;
        if (movLeft) {
            rb.AddForce(Vector3.left * tspeed, ForceMode.VelocityChange);
		}
		//Moving to the right
		if (movRight) {
			rb.AddForce(Vector3.right * tspeed, ForceMode.VelocityChange);
        }
		// jumping
		if (jumping) {
            rb.AddForce(Vector3.up * jumpForce * Time.deltaTime, ForceMode.Impulse);
        }
		// sliding
		if (slide) {
			transform.localScale = Vector3.Lerp(transform.lossyScale, slideH, 0.3f);
            wallRuning =  false;
		}
        if (!slide) {
            transform.localScale = Vector3.Lerp(transform.lossyScale, new Vector3(transform.lossyScale.x, orH, transform.lossyScale.z), 0.3f);
        }
	}

	void OnCollisionStay(Collision other) {
		if (other.gameObject.tag == "floor") {
			onTheFloor = true;
		} else {
			onTheFloor = false;
		}
	}

	void OnTriggerStay(Collider other){
        if (other.gameObject.tag == "empty" && wallRuning){
            // WallRun gravity
            gravForce = 4f;
			Debug.Log("wall runing");
		}
	}
	void OnTriggerEnter(Collider other){
        if (other.gameObject == seek) {
			// losing/worst case scenario
			SceneManager.LoadScene(2);
        }
		if (other.gameObject.CompareTag("portal")) {
            // add scene controller here
            SceneManager.LoadScene(3);
        }
        if (other.gameObject.CompareTag("loader")) {
            if (WorldScript.load < WorldScript.lenght) {
                WorldScript.load++;
                clone = Instantiate(prefabs[Random.Range(1, 11)], new Vector3(Random.Range(-20, 20), Random.Range(-20,20), WorldScript.load * 44.3f), Quaternion.identity);
            }
            if (WorldScript.load == WorldScript.lenght) {
                clone = Instantiate(prefabs[12], new Vector3(Random.Range(-20, 20), Random.Range(-20, 20), WorldScript.load * 44.3f), Quaternion.identity);
            }
        }
	}

	void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "empty" || !wallRuning) {
            // regular gravity
            gravForce = 20f;
			wallRuning = false;
            Debug.Log("falling");
        }
	}
}