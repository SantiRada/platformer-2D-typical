using System.Collections;
using UnityEngine;

public class FollowCamera : MonoBehaviour {

    [Header("Variables")]
    public float limitX;
    public float limitY;

    [Header("Shake Camera")]
    [SerializeField] private float intensity;
    [SerializeField] private int repeats;
    [SerializeField] private float offset;
    private float timer;

    [Header("Object")]
    private Player target;

    private void Start()
    {
        target = FindObjectOfType<Player>();
    }
    private void Update()
    {
        transform.position = new Vector3(target.transform.position.x, target.transform.position.y, -10);

        float newX = Mathf.Clamp(transform.position.x, -limitX, limitX);
        float newY = Mathf.Clamp(transform.position.y, -limitY, limitY);

        transform.position = new Vector3(newX, newY, -10);

        if(timer > 0)
        {
            timer -= Time.deltaTime;
        }
    }
    public void Shake()
    {
        if(timer <= 0)
        {
            StopAllCoroutines();
            StartCoroutine(ShakeToTakeDamagePlayer());
        }
    }
    private IEnumerator ShakeToTakeDamagePlayer()
    {
        timer = 1.5f;
        for (int i = 0; i < repeats; i++)
        {
            transform.position = new Vector3(transform.position.x - intensity, transform.position.y, transform.position.z);
            yield return new WaitForSeconds(offset);
            transform.position = new Vector3(transform.position.x + (intensity * 2), transform.position.y, transform.position.z);
            yield return new WaitForSeconds(offset);
        }
    }
}
