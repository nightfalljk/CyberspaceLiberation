using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UnityEngine.InputSystem.Controls;

public class PlayerCharacterControllerInput : MonoBehaviour
{

    private  PlayerInputActions _input;

    private IObservable<Vector2> _move;
    private IObservable<Vector2> _aim;
    private Subject<Unit> _shoot;
    private Subject<Unit> _slowField;
    private Subject<Unit> _hack;
    private ReadOnlyReactiveProperty<bool> _teleportEnable;
    private Subject<Unit> _weaponBoost;
    private ReadOnlyReactiveProperty<bool> _dash;

    private Subject<Unit> _mobility;
    private Subject<Unit> _offense;
    private Subject<Unit> _passive;
    
    private IObservable<Vector2> _teleportLocation;
    private IObservable<Vector2> _teleportMove;
    private Subject<Unit> _teleportConfirmLocation;
    private Subject<Unit> _cancelTeleport;


    private void Awake()
    {
        
        
        _input = new PlayerInputActions();

        
        _shoot = new Subject<Unit>();
        _slowField = new Subject<Unit>();
        _hack = new Subject<Unit>();
        _weaponBoost = new Subject<Unit>();
        
        _mobility = new Subject<Unit>();
        _offense = new Subject<Unit>();
        _passive = new Subject<Unit>();
        
        _teleportConfirmLocation = new Subject<Unit>();
        _cancelTeleport = new Subject<Unit>();

        _move = this.UpdateAsObservable()
            .Select(_ =>
            {
                return _input.Character.Move.ReadValue<Vector2>();
            });

        _aim = this.UpdateAsObservable()
            .Select(_ =>
            {
                return _input.Character.Aim.ReadValue<Vector2>();
            });

        _input.Character.Shoot.performed += context =>
        {
            _shoot.OnNext(Unit.Default);
        };

        _input.Character.SlowField.performed += context =>
        {
            _slowField.OnNext(Unit.Default);
        };

        _input.Character.WeaponPower.performed += context =>
        {
            _weaponBoost.OnNext(Unit.Default);
        };

        _input.Character.Hack.performed += context =>
        {
            _hack.OnNext(Unit.Default);
        };

        _teleportEnable = this.UpdateAsObservable()
            .Select(_ => _input.Character.TeleportEnable.ReadValueAsObject() != null)
            .ToReadOnlyReactiveProperty();

        _dash = this.UpdateAsObservable()
            .Select(_ => _input.Character.Dash.ReadValueAsObject() != null)
            .ToReadOnlyReactiveProperty();
        
        
        _teleportMove = this.UpdateAsObservable()
            .Select(_ =>
            {
                return _input.Teleport.Move.ReadValue<Vector2>();
            });

        _teleportLocation = this.UpdateAsObservable()
            .Select(_ =>
            {
                return _input.Teleport.SelectLocation.ReadValue<Vector2>();
            });

        _input.Teleport.ConfirmLocation.performed += context =>
        {
            _teleportConfirmLocation.OnNext(Unit.Default);
        };
        
        _input.Teleport.Cancel.performed += context =>
        {
            _cancelTeleport.OnNext(Unit.Default);
        };
        
        
        _input.Character.SwitchMobility.performed += context =>
        {
            _mobility.OnNext(Unit.Default);
        };

        _input.Character.SwitchOffense.performed += context =>
        {
            _offense.OnNext(Unit.Default);
        };

        _input.Character.SwitchDefense.performed += context =>
        {
            _passive.OnNext(Unit.Default);
        };
        
        _input.Teleport.Disable();

    }

    private void OnEnable()
    {
        _input.Enable();
    }

    private void OnDisable()
    {
        _input.Disable();
    }

    public void ToggleTeleportInput(bool enable)
    {
        if (enable)
        {
            _input.Character.Disable();
            _input.Teleport.Enable();
        }
        else
        {
            _input.Character.Enable();
            _input.Teleport.Disable();
        }
    }

    public void Reset()
    {
        _input.Character.Enable();
        _input.Teleport.Disable();
    }

    public IObservable<Vector2> Move => _move;

    public IObservable<Vector2> Aim => _aim;

    public IObservable<Unit> Shoot => _shoot;

    public Subject<Unit> SlowField => _slowField;

    public Subject<Unit> WeaponBoost => _weaponBoost;

    public Subject<Unit> Hack => _hack;

    public ReadOnlyReactiveProperty<bool> TeleportEnable => _teleportEnable;

    public IObservable<Vector2> TeleportLocation => _teleportLocation;

    public IObservable<Vector2> TeleportMove => _teleportMove;

    public Subject<Unit> TeleportConfirmLocation => _teleportConfirmLocation;

    public Subject<Unit> CancelTeleport => _cancelTeleport;

    public ReadOnlyReactiveProperty<bool> Dash => _dash;

    public Subject<Unit> Mobility => _mobility;

    public Subject<Unit> Offense => _offense;

    public Subject<Unit> Passive => _passive;
}
