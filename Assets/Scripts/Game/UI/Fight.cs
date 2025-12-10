using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class Fight : MonoBehaviour
{
    [SerializeField]
    private Button _buttonStartFight;
    [SerializeField]
    private Slider _sliderFight;
    [SerializeField]
    private TMP_Text _text;

    private IEventBusNotResult<Unit> _eventsBusU;
    private CompositeDisposable _disposables = new CompositeDisposable();

    private bool _isFight = false;
    private float _durationLose = 20;

    [Inject]
    private void Contanier(IEventBusNotResult<Unit> eventsBusU)
    {
        _eventsBusU = eventsBusU;

        _buttonStartFight.onClick.AddListener(() =>
        {
            _eventsBusU.Publish(EventsName.EnabledFight, Unit.Default);

            _sliderFight.value = 0.5f;
            SelectDifficulty();

            TurnOnTheFight(true);

        });

        _eventsBusU.Subscribe(EventsName.CharacterKick, Observer.Create<Unit>(AddSliderValue)).AddTo(_disposables);
    }

    private void AddSliderValue(Unit unit)
    {
        if(_sliderFight != null)
            _sliderFight.value += 0.03f;
    }

    private void OnEnable()
    {
        TurnOnTheFight(false);
    }

    private void Update()
    {
        if (_isFight)
        {
            if (_sliderFight.value == 1f)
            {
                _isFight = false;
                TurnOnTheFight(false);
                _eventsBusU.Publish(EventsName.EnemyLose, Unit.Default);
                _eventsBusU.Publish(EventsName.EnebaleMove, Unit.Default);
                gameObject.SetActive(false);
            }

            if(_sliderFight.value == 0)
            {
                _eventsBusU.Publish(EventsName.DisableMove, Unit.Default);
                _eventsBusU.Publish(EventsName.CharacterDie, Unit.Default);
            }

            _sliderFight.value -= Time.deltaTime / _durationLose;
        }
    }

    private void TurnOnTheFight(bool value)
    {
        _buttonStartFight.gameObject.SetActive(!value);
        _sliderFight.gameObject.SetActive(value);
        _text.gameObject.SetActive(value);

        _isFight = value;
    }

    private void SelectDifficulty()
    {
        int rand = Random.Range(0, 100);

        if (rand < 33) _durationLose = 10f;
        if (rand >= 33  && rand < 66) _durationLose = 8f;
        if (rand >= 66) _durationLose = 4f;
    }

    private void OnDisable()
    {
        TurnOnTheFight(false);
    }

    private void OnDestroy()
    {
        _disposables.Dispose();
    }
}
