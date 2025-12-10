using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EntryPointBootstrap : MonoBehaviour
{
    [SerializeField]
    private int _indexScene = 1;

    private async void Start()
    {
        await Task.Delay(3000); // Имитация загрузки данных с сервера

        SceneManager.LoadScene(_indexScene);
    }
}
