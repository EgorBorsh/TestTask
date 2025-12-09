using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterColObstacles : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Obstacle>())
        {
            GetComponent<CharacterMove>().Dispose();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
