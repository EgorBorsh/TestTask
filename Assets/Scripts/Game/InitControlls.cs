using System;
using UniRx;
using UnityEngine;
using Zenject;

public class InitControlls: IDisposable
{
    InputControls _controlls;
    IEventBusNotResult<Unit> _eventsBusF;
    IEventBusResult<Unit, float> _eventsBusFR;
    IEventBusResult<Unit, Vector2> _eventsBusFV2;
    private CompositeDisposable _disposables = new CompositeDisposable();

    public void Dispose()
    {
        _disposables.Dispose();
    }

    public void Init(IEventBusNotResult<Unit> eventsBusF)
    {
        _eventsBusF = eventsBusF;

        _controlls = new InputControls();
        _controlls.Enable();

        SetupCharacterControls();
    }

    private void SetupCharacterControls()
    {
        _controlls.Character.Touch.started += Move;
    }

    private void Move(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        _eventsBusF.Publish(EventsName.CharacterMoveForward, Unit.Default);
    }
}
