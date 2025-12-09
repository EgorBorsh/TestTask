using System.Threading.Tasks;
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
    private AsyncOperationHandle<GameObject> handle;

    private IEventBusNotResult<Unit> _eventsBusU;
    private CompositeDisposable _disposables = new CompositeDisposable();
    [Inject] private DiContainer _container;

    [Inject]
    private void Contanier(IEventBusNotResult<Unit> eventsBusU, IEventBusNotResult<float> eventsBusF)
    {
        _eventsBusU = eventsBusU;
        _initControlls = new InitControlls();

        _initControlls.Init(eventsBusU, eventsBusF);

        _eventsBusU.Subscribe(EventsName.LoadFight, Observer.Create<Unit>(LoadFight)).AddTo(_disposables);
    }


    private void Awake()
    {
        StartLevel();
        //LoadFight(Unit.Default);
    }

    private async void StartLevel()
    {
        _eventsBusU.Publish(EventsName.EnabledFight, Unit.Default);
        _panelStart.StartAnim();

        await _spawnMap.SpawnStartLevel(_container);

        await CreateCharacter(false);

        _panelStart.EndAnim();
    }

    private async void LoadFight(Unit unit)
    {
        _eventsBusU.Publish(EventsName.EnabledFight, Unit.Default);

        if (handle.IsValid())
            Addressables.ReleaseInstance(handle);

        _panelStart.StartAnim();

        await _spawnMap.SpawnFight(_container);

        await CreateCharacter(true);

        _panelStart.EndAnim();
    }
    private async Task CreateCharacter(bool isFight)
    {
        handle = Addressables.InstantiateAsync("Character");
        GameObject _character = await handle.Task;

        if (isFight)
        {
            _character.AddComponent<SearchFight>();
            _character.AddComponent<SearcgChest>();
        }

        _container.InjectGameObject(_character);
        _cameraMove.Init(_character.transform);

    }

    private void OnDestroy()
    {
        _disposables.Dispose();
    }
}
