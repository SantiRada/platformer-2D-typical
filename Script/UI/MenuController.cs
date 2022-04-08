using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {

    [Header("Audio")]
    [SerializeField] private AudioSource sfx;
    [SerializeField] private AudioClip button;

    public void LoadButton()
    {
        sfx.PlayOneShot(button);

        Invoke("LoadLevel", 0.075f);
    }
    private void LoadLevel()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
