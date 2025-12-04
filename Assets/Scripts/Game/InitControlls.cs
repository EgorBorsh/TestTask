using System;
using UniRx;
using UnityEngine;
using Zenject;

public class InitControlls: IDisposable
{
    private InputControls _controlls;
    private IEventBusNotResult<Unit> _eventsBusU;
    private IEventBusNotResult<float> _eventsBusF;

    private CompositeDisposable _disposables = new CompositeDisposable();

    public void Dispose()
    {
        _disposables.Dispose();
    }

    public void Init(IEventBusNotResult<Unit> eventsBusU, IEventBusNotResult<float> eventsBusF)
    {
        _eventsBusU = eventsBusU;
        _eventsBusF = eventsBusF;

        _controlls = new InputControls();
        _controlls.Enable();

        SetupCharacterControls();
    }

    private void SetupCharacterControls()
    {
        _controlls.Character.Touch.started += MoveTouch;
        _controlls.Character.Swipe.performed += MoveSwipe;
    }

    private void MoveTouch(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        _eventsBusU.Publish(EventsName.CharacterMoveForward, Unit.Default);
    }

    private void MoveSwipe(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        Vector2 value = context.ReadValue<Vector2>();
        Debug.Log(value);

        if(Math.Abs(value.x) > Math.Abs(value.y))
            _eventsBusF.Publish(EventsName.CharacterMoveRightOrLeft, value.x);
        else
            if(value.y > 0)
                _eventsBusU.Publish(EventsName.CharacterMoveForward, Unit.Default);
    }
}
