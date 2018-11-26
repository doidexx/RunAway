using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldScript : MonoBehaviour {
    public GameObject[] prefabs;
    private GameObject clone;
    public GameObject dak, dad;
    private Vector3[] dakpos;
    private float dakMU;

	public int worldLenght;
    public static int load, lenght, meter;
    private int lvl1, lvl2, lvl3, lvl4, lvl5;
    public float distance;
	// Use this for initialization
	void Awake () {
        meter = 0;
        lvl1 = 35;
        lvl2 = lvl1 * 2;
        lvl3 = lvl1 * 3;
        lvl4 = lvl1 * 5;
        lvl5 = lvl1 * 7;
        clone = null;
        distance = 44.3f;
        load = 4;
        for (int i = 0; i < 5; i++) {
            if (i < 3) {
                Instantiate(prefabs[0], new Vector3(0, 0, i * distance), Quaternion.identity);
            }
            if (i == 3) {
                Instantiate(prefabs[11], new Vector3(0, 0, i * distance), Quaternion.identity);
            }
            if (i == 4) {
                Instantiate(prefabs[Random.Range(1,11)], new Vector3(0, 0, i * distance), Quaternion.identity);
            }
        }
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.buildIndex == 1)
        {
            lenght = lvl1;
        }
        if (currentScene.buildIndex == 5)
        {
            lenght = lvl2;
        }
        dakpos = new Vector3[lenght + 2];
        dakMU = 745 / lenght - 2;
        for (int i = 0; i < lenght + 2; i++) {
            dakpos[i] = new Vector3(dak.transform.position.x + (dakMU * i), dak.transform.position.y, dak.transform.position.z);
        }
	}
	
	// Update is called once per frame
    void Update () {
        //////////////////////////////////////////////////Seeting up World lenght for each lvl////////////////////////////////////    
        dak.transform.position = Vector3.Lerp(dak.transform.position, dakpos[meter], 0.025f);
        dak.transform.SetParent(dad.transform);
        Debug.Log("meter" + meter);
        Debug.Log("load" + load);
    }
}
