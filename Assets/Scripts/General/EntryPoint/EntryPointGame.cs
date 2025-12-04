using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;

public class EntryPointGame : MonoBehaviour
{
    [SerializeField]
    private CameraMove _cameraMove;
    [SerializeField]
    private MoveTextStart _panelStart;
    [SerializeField]
    private SpawnMap _spawnMap;

    private InitControlls _initControlls;

    private IEventBusNotResult<Unit> _eventsBusU;
    [Inject] private DiContainer _container;

    [Inject]
    private void Contanier(IEventBusNotResult<Unit> eventsBusU, IEventBusNotResult<float> eventsBusF)
    {
        _eventsBusU = eventsBusU;
        _initControlls = new InitControlls();

        _initControlls.Init(eventsBusU, eventsBusF);
    }


    private async void Awake()
    {
        _panelStart.Init();

        await _spawnMap.Init(_container);

        AsyncOperationHandle<GameObject> handle = Addressables.InstantiateAsync("Character");
        GameObject _character = await handle.Task;

        _container.InjectGameObject(_character);

        _cameraMove.Init(_character.transform);
    }
}
