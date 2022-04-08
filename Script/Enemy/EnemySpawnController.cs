using System.Collections;
using UnityEngine;

public class EnemySpawnController : MonoBehaviour {

    [Range(1f, 15f)]
    public int timeToSpawn;
    [SerializeField] private Enemy enemyPrefab;

    private Player player;

    private void Start()
    {
        player = FindObjectOfType<Player>();
        //StartCoroutine(SpawnNewEnemy());
    }
    IEnumerator SpawnNewEnemy()
    {
        while (!player.isDead)
        {
            yield return new WaitForSeconds(timeToSpawn);
            Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        }
    }
}
