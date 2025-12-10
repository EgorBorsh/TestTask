using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class StartUnlockedChest : MonoBehaviour
{
    [SerializeField]
    private GameObject _panelLockAndKey;
    [SerializeField]
    private Button _buttonChestOpen;

    [Space(20)]
    [Header("Array color")]
    [SerializeField]
    private Color[] _colors;

    private Color _currentColorLock;
    private int _currentIndexColor;


    private IEventBusNotResult<Unit> _eventsBusU;

    [Inject]
    private void Contanier(IEventBusNotResult<Unit> eventsBusU)
    {
        _eventsBusU = eventsBusU;
    }

    private void Start()
    {

        _buttonChestOpen.onClick.AddListener(() =>
        {
            _panelLockAndKey.SetActive(true);
            _buttonChestOpen.gameObject.SetActive(false);

            _currentIndexColor = Random.Range(0, _colors.Length);
            _currentColorLock = _colors[_currentIndexColor];

            GetComponentInChildren<UILock>().GetComponent<Image>().color = _currentColorLock;
            GetComponentInChildren<UILock>().GetComponent<IKeyOrLock>().IndexColor = _currentIndexColor;

            List<UIKey> keys = new List<UIKey>(GetComponentsInChildren<UIKey>());

            for (int i = 0; i < 3; i++)
            {
                int rand = Random.Range(0, keys.Count);

                keys[rand].GetComponent<Image>().color = _currentColorLock;
                keys[rand].GetComponent<IKeyOrLock>().IndexColor = _currentIndexColor;
                keys.Remove(keys[rand]);
            }

            foreach (UIKey key in keys)
            {
                int index = Random.Range(0, _colors.Length);
                key.GetComponent<Image>().color = _colors[index];
                key.GetComponent<IKeyOrLock>().IndexColor = index;
            }
            _eventsBusU.Publish(EventsName.DisableMove, Unit.Default);
        });
    }
}
