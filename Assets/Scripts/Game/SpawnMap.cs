using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.TextCore.Text;
using Zenject;

public class SpawnMap : MonoBehaviour
{
    [SerializeField] private int roadCount = 3;

    private float stepPos = 2f;
    private float startPos = 12f;

    private List<GameObject> roads = new List<GameObject>();
    private List<GameObject> enemies = new List<GameObject>();

    private AsyncOperationHandle<GameObject> handleStartGame;
    private AsyncOperationHandle<GameObject> handleRoad;
    private AsyncOperationHandle<GameObject> handleFinishMap;
    private AsyncOperationHandle<GameObject> handleEnemy;
    private AsyncOperationHandle<GameObject> streetFight;
    private AsyncOperationHandle<GameObject> handleChest;

    public async Task SpawnStartLevel(DiContainer container)
    {
        if(streetFight.IsValid())
            Addressables.ReleaseInstance(streetFight);
        if(handleChest.IsValid())
            Addressables.ReleaseInstance(handleChest);

        foreach (GameObject enemy in enemies) Destroy(enemy);
        
        handleStartGame = Addressables.InstantiateAsync("StartGame");
        GameObject startGame = await handleStartGame.Task;

        handleRoad = Addressables.LoadAssetAsync<GameObject>("Road");
        GameObject prefabRoad = await handleRoad.Task;

        for (int i = 0; i < roadCount; i++)
        {
            GameObject road = Instantiate(prefabRoad);
            road.transform.position = new Vector3(0, 0, startPos + i * stepPos);
            road.GetComponent<Road>().Init();
            roads.Add(road);
        }

        handleFinishMap = Addressables.InstantiateAsync("FinishMap");
        GameObject finishMap = await handleFinishMap.Task;

        finishMap.transform.position = new Vector3(0, 0, startPos + roadCount * stepPos);
    }

    public async Task SpawnFight(DiContainer container)
    {
        if (handleStartGame.IsValid())
            Addressables.ReleaseInstance(handleStartGame);
        if (handleFinishMap.IsValid())
            Addressables.ReleaseInstance(handleFinishMap);

        foreach (GameObject road in roads) Destroy(road);

        streetFight = Addressables.InstantiateAsync("FightStreet");

        GameObject streetFightGameObject = await streetFight.Task;

        streetFightGameObject.transform.position = Vector3.zero;

        handleEnemy = Addressables.LoadAssetAsync<GameObject>("EnemyIdle");
        GameObject prefabEnemy = await handleEnemy.Task;

        foreach (PointSpawnEnemy point in streetFightGameObject.GetComponentsInChildren<PointSpawnEnemy>())
        {
            GameObject enemy = Instantiate(prefabEnemy);
            enemy.transform.position = point.transform.position;
            container.InjectGameObject(enemy);
            enemies.Add(enemy);
        }

        handleChest = Addressables.InstantiateAsync("Gold chest");
        GameObject prefabChest= await handleChest.Task;

        prefabChest.transform.position = streetFightGameObject.GetComponentInChildren<PointSpawnChest>().transform.position;
    }
}
