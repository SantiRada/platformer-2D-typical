using UnityEngine;

public class Checkpoint : MonoBehaviour {

    [SerializeField] private int addedTime = 10;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameManager.instance.time += addedTime;
            Destroy(this.gameObject, 0.075f);
        }
    }
}
