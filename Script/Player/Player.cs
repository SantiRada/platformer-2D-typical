using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour {

    [Header("Variables")]
    public int health;
    public float speed;
    private float maxSpeed;
    [HideInInspector] public bool isDead;
    [HideInInspector] public int maxHealth;
    
    [Header("Principal Move")]
    private float horizontal;
    private float vertical;
    private Vector3 moveDirection;
    private Vector2 facingDirection;
    
    [Header("Weapon & Guns")]
    public float rateOfFire;
    [HideInInspector] public float rateGlobalFire;
    [SerializeField] private int cantBulletToCartridge;
    [SerializeField] private float timeToReload;
    [SerializeField] private float blinkRate;
    [HideInInspector] public int cantTotalBullet;
    [HideInInspector] public int cantBullet;
    private bool reloading = false;
    private float rate;
    private float timeToDamage;

    [Header("Components")]
    [SerializeField] private Transform aim;
    [SerializeField] private GameObject bullet;
    [SerializeField] private Animator anim;
    private BoxCollider2D col;
    private SpriteRenderer spr;

    [Header("Objects")]
    private Camera cam;
    private VolumeAndVFX sfx;

    private void Start()
    {
        cam = FindObjectOfType<Camera>();
        col = GetComponent<BoxCollider2D>();
        spr = GetComponentInChildren<SpriteRenderer>();
        sfx = FindObjectOfType<VolumeAndVFX>();

        // -- Initial Values ------
        cantTotalBullet = cantBulletToCartridge * 4;
        cantBullet = cantBulletToCartridge;
        maxHealth = health;
        rateGlobalFire = rateOfFire;
        maxSpeed = speed / 2;
    }
    // ---- Globals and Movement ------------------
    private void Update()
    {
        if (!isDead)
        {
            // Direction
            #region Direction SPR
            if (horizontal < 0)
                spr.flipX = false;
            if (horizontal > 0)
                spr.flipX = true;
            #endregion

            // Animation - Animator
            #region Animator
            anim.SetBool("Move", ((horizontal != 0) || (vertical != 0)));
            #endregion

            // Move Player
            #region MovePlayer
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");
            #endregion

            // Move AIM
            #region MoveAIM
            facingDirection = cam.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            aim.position = (Vector3)facingDirection.normalized + transform.position;
            #endregion

            // Gun / / Weapon
            #region GunWeapon
            if (reloading)
                Debug.Log("Reloading");
            else
            {
                if (Input.GetButtonDown("Reload"))
                {
                    Invoke("ReloadWeapon", timeToReload);
                    reloading = true;
                }
            }
            if (rate >= 0)
                rate -= Time.deltaTime;

            if (Input.GetButton("Shot") && !reloading)
            {
                if (rate <= 0)
                    Shot();
            }
            if (Input.GetButtonUp("Shot"))
            {
                speed = maxSpeed * 2;
            }
            #endregion

            // Timer to Damage
            #region TimerToDamage
            if (timeToDamage > 0)
            {
                timeToDamage -= Time.deltaTime;
            }
            #endregion
        }
    }
    private void FixedUpdate()
    {
        if (!isDead)
        {
            // Move Player
            Move();
        }
    }
    private void Move()
    {
        moveDirection = new Vector3(horizontal, vertical, 0) * speed * Time.fixedDeltaTime;

        transform.position += moveDirection;
    }
    // ---- Function for weapon -------------------
    private void Shot()
    {
        if(cantBullet > 0)
        {
            rate = rateOfFire;
            cantBullet--;
            ShotGun();
        }
        else
        {
            if(cantTotalBullet > 0 && cantBullet <= 0)
            {
                reloading = true;
                Invoke("ReloadWeapon", timeToReload);
            }
            else if(cantTotalBullet <= 0)
            {
                Debug.Log("Sin Munición");
            }
        }
    }
    private void ReloadWeapon()
    {
        if (cantTotalBullet >= (cantBulletToCartridge - cantBullet))
        {
            cantTotalBullet -= cantBulletToCartridge - cantBullet;
            cantBullet = cantBulletToCartridge;
        }
        else
        {
            cantBullet += cantTotalBullet;
            cantTotalBullet = 0;
        }

        reloading = false;
        Debug.Log("Pistola recargada");
    }
    private void ShotGun()
    {
        sfx.Shot();

        speed = maxSpeed;
        float angle = Mathf.Atan2(facingDirection.y, facingDirection.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);

        Instantiate(bullet, transform.position, targetRotation);
    }
    // ---- Function for Take Damage --------------
    private void TakeDamage(int damage)
    {
        // Efectos de Daño visuales
        StartCoroutine(BlinkRountine());
        FollowCamera camera = FindObjectOfType<FollowCamera>();
        camera.Shake();
        // Sonido de impacto
        sfx.TakeDamagePlayer();

        // Take Damage Data
        health -= damage;

        if (health <= 0)
        {
            isDead = true;
            Debug.Log("Has muerto!");
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            timeToDamage = 0;
            TakeDamage(collision.gameObject.GetComponent<Enemy>().power);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy" && timeToDamage <= 0)
        {
            TakeDamage(collision.gameObject.GetComponent<Enemy>().power);
            timeToDamage = 1.5f;
        }
        if (collision.gameObject.tag == "Door")
        {
            HUD hud = FindObjectOfType<HUD>();
            hud.ChangeScene("SampleScene");
        }
    }
    // ---- Function VFX --------------------------
    private IEnumerator BlinkRountine()
    {
        for(int i = 0; i < 3; i++)
        {
            spr.enabled = false;
            yield return new WaitForSeconds(8 * blinkRate);
            spr.enabled = true;
            yield return new WaitForSeconds(8 * blinkRate);
        }
    }
    // ---- Function to Power Up ------------------
    // -- RATE OF FIRE ----------------
    public void ChangeRateOfFire(float offset)
    {
        StartCoroutine(ChangeRate(offset));
    }
    private IEnumerator ChangeRate(float offset)
    {
        rateOfFire = rateOfFire * 0.5f;
        yield return new WaitForSeconds(offset);
        rateOfFire = rateGlobalFire;
    }
    // -- MODE GHOST ------------------
    public void ActivateModeGhost(float offset)
    {
        StartCoroutine(ModeGhost(offset));
    }
    private IEnumerator ModeGhost(float offset)
    {
        // Activate ---------------
        spr.color = new Color(1, 1, 1, 0.6f);
        col.isTrigger = true;
        yield return new WaitForSeconds(offset);
        // Desactivate ------------
        spr.color = new Color(1, 1, 1, 1);
        col.isTrigger = false;
    }
    // --------------------------------------------
}
