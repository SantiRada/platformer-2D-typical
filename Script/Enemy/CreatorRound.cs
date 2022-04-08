using System.Collections;
using UnityEngine;

public class CreatorRound : MonoBehaviour {

    [Header("Variable")]
    [SerializeField] private EnemySpawnController[] spawnEnemy;
    [SerializeField] private Enemy[] enemys;

    [Header("Object")]
    [SerializeField] private GameObject doorWin;
    [SerializeField] private GameObject notDoor;

    [Header("Rounds")]
    [SerializeField] private string[] rounds;
    [SerializeField] private int[] timeToSpawn;
    private int[] cantEnemys;
    private int index;
    private int cantEnemysInScene = 0;

    private void Start()
    {
        CreateRound();

        doorWin.gameObject.SetActive(false);
        notDoor.gameObject.SetActive(true);
    }
    private void Update()
    {
        if(cantEnemysInScene <= 0)
        {
            IncreaseIndex();
        }
    }
    // ---- Round Maker ---------------------------
    public void CreateRound()
    {
        // -- Separate string to ROUNDS --------------
        string[] creatorEnemys = rounds[index].Split(',');
        cantEnemys = new int[creatorEnemys.Length];

        for(int i = 0; i < creatorEnemys.Length; i++)
        {
            cantEnemys[i] = int.Parse(creatorEnemys[i]);
        }
        // -- Establecer cantidad de enemigos que se deben eliminar ------
        cantEnemysInScene = 0;
        for(int i = 0; i < cantEnemys.Length; i++)
        {
            cantEnemysInScene += cantEnemys[i];
        }
        Debug.Log("Cantidad de enemigos en esta ronda: " + cantEnemysInScene);
        // -- Instantiate Enemys ----------------
        EnemyMaker();
    }
    private void EnemyMaker()
    {
        int division = 0;

        for(int i = 0; i < 3; i++)
        {
            if (cantEnemys[i] % 2 == 0)
                division = 2;
            else
                division = 3;

            StartCoroutine(InstantiateEnemy(division, i));
        }
    }
    private IEnumerator InstantiateEnemy(int div, int indexEnemys)
    {
        float newLength = 0;
        if (div == 2)
        {
            newLength = cantEnemys[indexEnemys] / div;
        }
        else
        {
            newLength = cantEnemys[indexEnemys] / div;

            if (newLength < 1 || (newLength > 1 && newLength < 2))
            {
                newLength = 1;
                div = 1;
            }
        }

        if(indexEnemys > 0)
        {
            yield return new WaitForSeconds(timeToSpawn[0]);
        }

        for(int j = 0; j < div; j++)
        {
            for (int i = 0; i < newLength; i++)
            {
                Instantiate(enemys[indexEnemys], spawnEnemy[indexEnemys].transform.position, Quaternion.identity);
                yield return new WaitForSeconds(0.075f);
                cantEnemysInScene--;
            }
            yield return new WaitForSeconds(timeToSpawn[indexEnemys]);
        }
    }
    // ---- Increase Index value ------------------
    public void IncreaseIndex()
    {
        index++;

        if (rounds.Length <= index)
        {
            Debug.LogWarning("¡¡Has acabado con todas las ordas enemigas!!");
            notDoor.gameObject.SetActive(false);
            doorWin.gameObject.SetActive(true);
        }
        else
        {
            CreateRound();
        }
    }
}
