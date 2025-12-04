using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Road : MonoBehaviour
{
    private int _isRight;
    private int _countObstcales = 3;
    private int _speedSpawn = 2;
    private int _speedObstacle = 2;
    Obstacle prefabObstcales;

    public async void Init()
    {
        int rand = Random.Range(1, _countObstcales+1);
        string nameObstacle = $"Obstacle{rand}";

        AsyncOperationHandle<GameObject> handleObstacle = Addressables.LoadAssetAsync<GameObject>(nameObstacle);

        GameObject prefabObstcalesGameObject = await handleObstacle.Task;
        prefabObstcales = prefabObstcalesGameObject.GetComponent<Obstacle>();

        AddObjectParticlePool(prefabObstcales, 1, 10);

        _isRight = Random.Range(0, 2);
        _speedSpawn = Random.Range(1, 6);
        _speedObstacle = Random.Range(3, 9);

        Invoke("SpawnObstacle", _speedSpawn);
    }

    private void SpawnObstacle()
    {
        Transform child = transform.GetChild(_isRight);
        Obstacle obj = ManagerPool<Obstacle>.Spawn(prefabObstcales.GetComponent<Obstacle>(), child.position, child.localRotation);

        obj.SpeedMove = _speedObstacle;
        obj.SetDirection(_isRight == 0 ? Vector3.left : Vector3.right);
        obj.Move();

        Invoke("SpawnObstacle", _speedSpawn);
    }

    private void AddObjectParticlePool(Obstacle objectPoolAdd, int lenghtObject, int quantity)
    {
        for (int i = 0; i < lenghtObject; i++)
            ManagerPool<Obstacle>.AddPool(objectPoolAdd).PopulatePool(objectPoolAdd, quantity, new Vector3(-100, 0, -100));
    }

}
