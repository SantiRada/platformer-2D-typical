using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour {

    [Header("Object")]
    public static GameManager instance;

    [Header("Variable")]
    public int time = 30;
    private int score;
    private int difficulty;

    public int Score
    {
        get => score;
        set
        {
            score = value;
            if(score % 1000 == 0)
                difficulty++;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    private void Start()
    {
        StartCoroutine(CountdownRoutine());
    }
    private IEnumerator CountdownRoutine()
    {
        while(time > 0)
        {
            yield return new WaitForSeconds(1);
            time--;
        }

        Debug.Log("Te has quedado sin tiempo, el juego terminó");
    }
}
