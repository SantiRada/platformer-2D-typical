using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatorToCheck : MonoBehaviour {

    [Header("Variable")]
    [SerializeField] private float timeToSpawnItem;
    [SerializeField] private float timeToSpawnTimer;

    [Header("Object")]
    public GameObject[] item;
    private Player player;

    private List<GameObject> itemsInScene = new List<GameObject>();

    [Header("Positions")]
    [SerializeField] private Vector3[] availablePos;

    private void Start()
    {
        player = FindObjectOfType<Player>();

        // -- Coroutines ------
        StartCoroutine(SpawnCheckTime());
        StartCoroutine(SpawnNewObject());
    }
    private IEnumerator SpawnCheckTime()
    {
        while (!player.isDead)
        {
            yield return new WaitForSeconds(timeToSpawnTimer);

            int rnd = Random.Range(0, availablePos.Length);

            itemsInScene.Add(Instantiate(item[0], availablePos[rnd], Quaternion.identity));
        }
    }
    private IEnumerator SpawnNewObject()
    {
        while (!player.isDead)
        {
            yield return new WaitForSeconds(timeToSpawnItem);

            int rnd = Random.Range(0, availablePos.Length);
            int rndI = Random.Range(1, item.Length);

            itemsInScene.Add(Instantiate(item[rndI], availablePos[rnd], Quaternion.identity));
        }
    }
}
