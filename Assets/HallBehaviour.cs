using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HallBehaviour : MonoBehaviour
{
    [SerializeField] float speed;

    private MapHandler handler;

    private void Awake()
    {
        handler = FindObjectOfType<MapHandler>();
    }

    private void Update()
    {
        transform.Translate(Vector3.back * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player") return;
        handler.SpawnNewHall();
        StartCoroutine(Deactivate());
    }

    private IEnumerator Deactivate()
    {
        yield return new WaitForSeconds(3);
        gameObject.SetActive(false);
    }
}
