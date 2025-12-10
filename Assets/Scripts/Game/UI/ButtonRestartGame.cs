using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonRestartGame : MonoBehaviour
{
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        });
    }
}