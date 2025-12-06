using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;

public class CharacterColFinish : MonoBehaviour
{


    private IEventBusNotResult<Unit> _eventsBusU;

    [Inject]
    private void Contanier(IEventBusNotResult<Unit> eventsBusU, IEventBusNotResult<float> eventsBusF)
    {
        _eventsBusU = eventsBusU;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Finish>()) _eventsBusU.Publish(EventsName.LoadFight, Unit.Default);
    }
}
