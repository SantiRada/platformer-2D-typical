using UnityEngine;

public class Enemy : MonoBehaviour {

    [Header("Variable")]
    public int health = 10;
    public float speed = 2;
    public int power;
    [SerializeField] private int scorePoint;

    [Header("Object")]
    private Player player;
    private SpriteRenderer spr;
    private VolumeAndVFX sfx;

    private void Start()
    {
        sfx = FindObjectOfType<VolumeAndVFX>();
        player = FindObjectOfType<Player>();

        spr = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        if (player.isDead)
        {
            Dead();
        }
        if(health <= 0)
        {
            Dead();
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);

            if (player.transform.position.x > transform.position.x)
                spr.flipX = false;
            else
                spr.flipX = true;
        }
    }
    private void Dead()
    {
        sfx.EnemyDeath();

        Destroy(gameObject);
    }
    public void TakeDamage()
    {
        float newX = (transform.position.x - player.transform.position.x) / 4;
        float newY = (transform.position.y - player.transform.position.y) / 4;

        Vector3 distance = new Vector3(newX, newY, 0);
        transform.position += distance;

        GameManager.instance.Score += scorePoint;

        health -= 5;
    }
}
