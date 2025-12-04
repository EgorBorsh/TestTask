using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;

public class EntryPointGame : MonoBehaviour
{
    [SerializeField]
    private CameraMove _cameraMove;

    private InitControlls _initControlls;

    private IEventBusNotResult<Unit> _eventsBusU;
    [Inject] private DiContainer _container;

    [Inject]
    private void Contanier(IEventBusNotResult<Unit> eventsBusF)
    {
        _eventsBusU = eventsBusF;
        _initControlls = new InitControlls();

        _initControlls.Init(eventsBusF);
    }


    private async void Awake()
    {
        AsyncOperationHandle<GameObject> handle = Addressables.InstantiateAsync("Character");
        GameObject _character = await handle.Task;

        _container.InjectGameObject(_character);

        _cameraMove.Init(_character.transform);
    }
}
