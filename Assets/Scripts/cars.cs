using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cars : MonoBehaviour 
{
    // Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (transform.localPosition.x > 7.3f) {
            transform.localPosition = new Vector3(Mathf.Lerp(transform.localPosition.x, 7.3f, 0.025f), transform.localPosition.y, transform.localPosition.z);
        }
        if (transform.localPosition.x < -6.7f) {
            transform.localPosition = new Vector3(Mathf.Lerp(transform.localPosition.x, -6.7f, 0.025f), transform.localPosition.y, transform.localPosition.z);
        }
	}
}
