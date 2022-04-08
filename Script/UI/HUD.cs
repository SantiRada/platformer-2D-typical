using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUD : MonoBehaviour {

    [Header("Variables")]
    private string scene;

    [Header("Component")]
    public Text chronommeter;
    public Text bullets;
    public Text score;
    public Text finalScore;
    public Slider healthbar;
    public GameObject screenGameOver;

    [Header("Object")]
    private Player target;
    private VolumeAndVFX sfx;

    private void Start()
    {
        sfx = FindObjectOfType<VolumeAndVFX>();
        target = FindObjectOfType<Player>();

        healthbar.maxValue = target.health;
        screenGameOver.gameObject.SetActive(false);
    }
    private void Update()
    {
        // -- Data Level -------
        if(GameManager.instance.time < 10 && GameManager.instance.time > 0)
        {
            chronommeter.text = "0" + GameManager.instance.time.ToString("f0");
        }
        else if(GameManager.instance.time > 10)
        {
            chronommeter.text = GameManager.instance.time.ToString("f0");
        }
        else
        {
            chronommeter.text = "00";
        }
        score.text = GameManager.instance.Score.ToString();
        finalScore.text = "Final Score: " + GameManager.instance.Score.ToString();

        if (target.isDead)
            screenGameOver.gameObject.SetActive(true);

        // -- Data Player ------
        bullets.text = target.cantBullet + "<size=50>  " + target.cantTotalBullet + "</size>";
        healthbar.value = target.health;
    }
    public void ChangeScene(string scene)
    {
        this.scene = scene;
        sfx.PlayButton();

        Invoke("Changer", 0.05f);
    }
    private void Changer()
    {
        SceneManager.LoadScene(scene);
    }
}
