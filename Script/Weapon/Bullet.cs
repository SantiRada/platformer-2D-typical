using UnityEngine;

public class Bullet : MonoBehaviour {

    [Header("Variables")]
    [SerializeField] private float speedBullet;
    [SerializeField] private float rangeBullet;
    private float range;

    private void Update()
    {
        // Movimiento de la bala
        transform.position += transform.right * speedBullet * Time.deltaTime;

        // Desaparecer la bala a los 8 metros
        range = rangeBullet / (0.02f * speedBullet) * Time.deltaTime;
        Invoke("DestroyObject", range);
    }
    private void DestroyObject()
    {
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Si la colisión es contra un enemigo se activa la función "TakeDamage()"
        if(collision.gameObject.tag == "Enemy")
        {
            collision.GetComponent<Enemy>().TakeDamage();
            DestroyObject();
        }
    }
}
