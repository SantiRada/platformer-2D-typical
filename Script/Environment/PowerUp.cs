using System.Collections;
using UnityEngine;

public class PowerUp : MonoBehaviour {

    public enum TypePowerUp
    {
        fireRateIncresase,
        IncreaseBullet,
        healthMaximize,
        modeGhost
    }

    [Header("Variables")]
    public float offsetToPowerUp;

    [Header("Object")]
    public TypePowerUp typePU;
    private Player player;
    private VolumeAndVFX sfx;

    private void Start()
    {
        sfx = FindObjectOfType<VolumeAndVFX>();
        player = FindObjectOfType<Player>();
        Invoke("DestroyItem", 3.5f);
    }
    private void DestroyItem()
    {
        Destroy(gameObject, 0.075f);
    }
    private void ActionPowerUp()
    {
        switch (typePU)
        {
            case TypePowerUp.fireRateIncresase:
                player.ChangeRateOfFire(offsetToPowerUp);
                break;

            case TypePowerUp.IncreaseBullet:
                player.cantTotalBullet += 30;
                break;

            case TypePowerUp.healthMaximize:
                player.health = player.maxHealth;
                break;

            case TypePowerUp.modeGhost:
                player.ActivateModeGhost(offsetToPowerUp);
                break;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            sfx.AddItem();
            ActionPowerUp();
            Destroy(gameObject, 0.075f);
        }
    }
}
