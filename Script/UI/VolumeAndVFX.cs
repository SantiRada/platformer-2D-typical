using UnityEngine;

public class VolumeAndVFX : MonoBehaviour {

    [Header("Variables")]
    private float timerImpact;

    [Header("Audio Clip")]
    public AudioClip bullet;
    public AudioClip button;
    public AudioClip enemyDeath;
    public AudioClip loop;
    public AudioClip gameOver;
    public AudioClip impact;
    public AudioClip item;

    [Header("Source")]
    public AudioSource music;
    public AudioSource sfx;
    public AudioSource enemys;

    [Header("Object")]
    private Player player;

    private void Start()
    {
        player = FindObjectOfType<Player>();

        MusicSelector();
    }
    private void Update()
    {
        if(timerImpact > 0)
            timerImpact -= Time.deltaTime;

        if (player.isDead)
        {
            music.loop = false;
            music.clip = gameOver;
            enemys.enabled = false;
        }
    }
    public void PlayButton()
    {
        sfx.PlayOneShot(button);
    }
    public void EnemyDeath()
    {
        enemys.PlayOneShot(enemyDeath);
    }
    public void Shot()
    {
        sfx.PlayOneShot(bullet);
    }
    public void TakeDamagePlayer()
    {
        if(timerImpact <= 0)
        {
            sfx.PlayOneShot(impact);
            timerImpact = 2;
        }
    }
    public void AddItem()
    {
        sfx.PlayOneShot(item);
    }
    public void MusicSelector()
    {
        // Selector Music
        if (player.isDead)
        {
            music.loop = false;
            music.clip = gameOver;
            enemys.enabled = false;
        }
        else
        {
            music.loop = true;
            music.clip = loop;
            enemys.enabled = true;
        }

        // Play Music
        music.Play();
    }
}
