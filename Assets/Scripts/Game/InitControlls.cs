using System;
using UniRx;
using UnityEngine;

public class InitControlls: IDisposable
{
    private InputControls _controlls;
    private IEventBusNotResult<Unit> _eventsBusU;
    private IEventBusNotResult<float> _eventsBusF;

    private CompositeDisposable _disposables = new CompositeDisposable();

    public void Dispose()
    {
        _disposables.Dispose();

        _controlls.CharacterMove.Touch.started -= MoveTouch;
        _controlls.CharacterMove.Swipe.performed -= MoveSwipe;

        _controlls.CharacterFight.Fight.performed -= KickTouch;
    }

    public void Init(IEventBusNotResult<Unit> eventsBusU, IEventBusNotResult<float> eventsBusF)
    {
        _eventsBusU = eventsBusU;
        _eventsBusF = eventsBusF;

        _controlls = new InputControls();

        SetupCharacterControls();

        _eventsBusU.Subscribe(EventsName.EnebaleMove, Observer.Create<Unit>(EnableMove)).AddTo(_disposables);
        _eventsBusU.Subscribe(EventsName.DisableMove, Observer.Create<Unit>(DisableMove)).AddTo(_disposables);
        _eventsBusU.Subscribe(EventsName.EnabledFight, Observer.Create<Unit>(EnablesFight)).AddTo(_disposables);
    }

    private void EnableMove(Unit unit)
    {
        _controlls.CharacterMove.Enable();
        _controlls.CharacterFight.Disable();
    }

    private void DisableMove(Unit unit)
    {
        _controlls.CharacterMove.Disable();
    }

    private void EnablesFight(Unit unit)
    {
        _controlls.CharacterFight.Enable();
        _controlls.CharacterMove.Disable();
    }

    private void SetupCharacterControls()
    {
        _controlls.CharacterMove.Touch.started += MoveTouch;
        _controlls.CharacterMove.Swipe.performed += MoveSwipe;

        _controlls.CharacterFight.Fight.performed += KickTouch;
    }

    private void KickTouch(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        _eventsBusU.Publish(EventsName.CharacterKick, Unit.Default);
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
