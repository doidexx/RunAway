using UnityEngine;

public class MapHandler : MonoBehaviour
{
    [SerializeField] int startingHalls;
    [SerializeField] GameObject[] hallsPrefabs;
    [SerializeField] float hallDepth;
    [SerializeField] float movingHallDepth;

    private GameObject[] halls;

    private void Awake()
    {
        halls = new GameObject[hallsPrefabs.Length + startingHalls];
        for (int i = 0; i < halls.Length; i++)
        {
            if (i < startingHalls)
                halls[i] = Instantiate(hallsPrefabs[0], transform);
            else
                halls[i] = Instantiate(hallsPrefabs[i - startingHalls], transform);

            halls[i].SetActive(false);
        }
    }

    private void Start()
    {
        for (int i = 0; i < startingHalls; i ++)
        {
            halls[i].transform.position = Vector3.forward * i * hallDepth;
            halls[i].SetActive(true);
        }
    }

    public void SpawnNewHall()
    {
        var index = Random.Range(startingHalls, halls.Length);
        if (halls[index].activeSelf)
        {
            SpawnNewHall();
            return;
        }
        halls[index].transform.position = Vector3.forward * movingHallDepth;
        halls[index].SetActive(true);
    }
}
