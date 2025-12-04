using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;

public class SpawnMap : MonoBehaviour
{
    [SerializeField] private int roadCount = 3;

    private float stepPos = 2f;
    private float startPos = 12f;

    public async Task Init(DiContainer container)
    {
        AsyncOperationHandle<GameObject> handleStartGame = Addressables.InstantiateAsync("StartGame");
        GameObject startGame = await handleStartGame.Task;

        AsyncOperationHandle<GameObject> handleRoad = Addressables.LoadAssetAsync<GameObject>("Road");
        GameObject prefabRoad = await handleRoad.Task;

        for (int i = 0; i < roadCount; i++)
        {
            GameObject road = Instantiate(prefabRoad);
            road.transform.position = new Vector3(0, 0, startPos + i * stepPos);
            road.GetComponent<Road>().Init();
        }
    }
}
