using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Cinemachine;
using UnityEngine;
using UniRx;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class Teleport : MonoBehaviour
{

    [SerializeField] private PlayerCharacterControllerInput controllerInput;
    private TeleportConfig _teleportConfig;
    
    [FormerlySerializedAs("player")] [SerializeField] private GameObject playerGameObject;
    [SerializeField] private Transform vcamTarget;

    [SerializeField] private GameObject teleportIndicator;

    [SerializeField] private AudioSource teleportCanceledSound;
    [SerializeField] private AudioSource teleportConfirmedSound;

    private Vector3 _currentTeleportPosition;
    private List<Material> _indicatorMaterials;
    private bool _validTeleport;
    private bool _selected;
    private ReactiveProperty<bool> _teleportEnabled;
    private ReactiveProperty<bool> _teleportAvailable;
    private ReactiveProperty<float> _teleportCooldownPercentage;
    private ReactiveProperty<float> _teleportDurationPercentage;
    private bool _teleportActive;
    private PlayerCharacterController _player;

    private Camera _cam;
    private CinemachineVirtualCamera _vcam;
    private LayerMask _layerMask;

    private void Awake()
    {
        _teleportEnabled = new ReactiveProperty<bool>();
        _teleportAvailable = new ReactiveProperty<bool>();
        _teleportCooldownPercentage = new ReactiveProperty<float>();
        _teleportDurationPercentage = new ReactiveProperty<float>();
        _teleportEnabled.Value = false;
        _teleportAvailable.Value = true;
        _teleportCooldownPercentage.Value = 1;
        _teleportDurationPercentage.Value = 1;
        _teleportActive = false;
        _selected = false;
        _cam = Camera.main;
        _player = playerGameObject.GetComponent<PlayerCharacterController>();
        _vcam = GetComponentInChildren<CinemachineVirtualCamera>();
        _currentTeleportPosition = playerGameObject.transform.position;
        _indicatorMaterials = teleportIndicator.GetComponent<Renderer>().materials.ToList();
        _layerMask = (1 << 13) + (1<<12)  + 1;
    }

    private void Start()
    {
        controllerInput.TeleportMove
            .Where(v => v != Vector2.zero)
            .Subscribe(input =>
            {
                if (_teleportActive)
                {
                    var inputVelocity = input.normalized;
                    var camVelocity = ApplyCameraRotationToVector(new Vector3(inputVelocity.x, 0, inputVelocity.y).normalized);
                    var translation = camVelocity * _teleportConfig.moveSpeed * (Time.deltaTime/_teleportConfig.timescaleFactor);
                    transform.Translate(translation);

                    //TODO: Limit movement by map size somehow
                    //TODO: Adapt rotation
                }

            }).AddTo(this);

        controllerInput.TeleportLocation
            .Where(v => v != Vector2.zero)
            .Subscribe(input =>
            {
                if (_teleportActive)
                {
                    Ray ray = _cam.ScreenPointToRay(input);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit, 1000f, _layerMask))
                    {
                        var pos = new Vector3(hit.point.x, playerGameObject.transform.position.y, hit.point.z);
                        
                        NavMeshHit _teleportTargetPos = new NavMeshHit();
                        
                        _validTeleport = NavMesh.SamplePosition(pos, out _teleportTargetPos, 1.0f, NavMesh.AllAreas);
                        teleportIndicator.transform.position = pos;

                        if (_validTeleport && !_selected)
                        {
                            for(int i = 0; i < _indicatorMaterials.Count; i++)
                            {
                                _indicatorMaterials[i].color = new Color(0, 0, 1, 0.5f);
                            }
                            
                            _currentTeleportPosition = pos;
                        }
                        else if (!_selected)
                        {
                            for(int i = 0; i < _indicatorMaterials.Count; i++)
                            {
                                _indicatorMaterials[i].color = new Color(1, 0, 0, 0.5f);
                            }
                            
                        }
                    }
                }
            }).AddTo(this);

        controllerInput.TeleportConfirmLocation
            .Subscribe(input =>
            {
                if (_validTeleport && _teleportActive && _teleportAvailable.Value)
                {
                    _teleportAvailable.Value = false;
                    _selected = true;
                    Time.timeScale = 1;
                    teleportConfirmedSound.Play();
                    StartCoroutine(TeleportCooldown());
                    StartCoroutine(ChannelTeleport());
                }
            }).AddTo(this);
        
        controllerInput.CancelTeleport
            .Subscribe(input =>
            {
                if (_teleportActive)
                {
                    StartCoroutine(TeleportCooldown());
                    _teleportActive = false;
                    _player.MoveLock = false;
                    _player.AimLock = false;
                    _player.ShootLock = false;
                    _selected = false;
                    _vcam.Follow = vcamTarget;
                    Time.timeScale = 1;
                    teleportCanceledSound.Play();
                    ToggleTeleportIndicator();
                    controllerInput.ToggleTeleportInput(false);
                }
            }).AddTo(this);
    }

    private IEnumerator TeleportCooldown()
    {
        yield return new WaitForCooldown(_teleportConfig.cooldown, _teleportCooldownPercentage);
        _teleportAvailable.Value = true;
    }

    private IEnumerator ChannelTeleport()
    {
        yield return new WaitForDuration(_teleportConfig.channelTime, _teleportDurationPercentage);
        _vcam.Follow = vcamTarget;
        playerGameObject.transform.position = _currentTeleportPosition;
        _teleportActive = false;
        ToggleTeleportIndicator();
        _selected = false;
        _player.MoveLock = false;
        _player.AimLock = false;
        _player.ShootLock = false;
        controllerInput.ToggleTeleportInput(false);
    }
    public ReactiveProperty<bool> TeleportEnabled
    {
        get => _teleportEnabled;
        set => _teleportEnabled = value;
    }

    public ReactiveProperty<bool> TeleportAvailable
    {
        get => _teleportAvailable;
        set => _teleportAvailable = value;
    }
    
    public ReactiveProperty<float> TeleportCooldownPercentage => _teleportCooldownPercentage;
    public ReactiveProperty<float> TeleportDurationPercentage => _teleportDurationPercentage;

    public bool TeleportActive
    {
        get => _teleportActive;
        set => _teleportActive = value;
    }

    public void ResetOnNewLevel()
    {
        _teleportAvailable.Value = true;
        _teleportCooldownPercentage.Value = 1;
        _teleportDurationPercentage.Value = 1;
        _teleportActive = false;
        teleportIndicator.SetActive(false);
        StopAllCoroutines();
        _currentTeleportPosition = playerGameObject.transform.position;
    }

    public float GetTimescaleFactor()
    {
        return _teleportConfig.timescaleFactor;
    }

    public void ToggleTeleportIndicator()
    {
        if (teleportIndicator.activeSelf)
        {
            teleportIndicator.SetActive(false);
        }
        else
        {
            teleportIndicator.SetActive(true);
        }
    }

    public void SetTeleportConfig(TeleportConfig teleportConfig)
    {
        this._teleportConfig = teleportConfig;
    }
    
    private Vector3 ApplyCameraRotationToVector(Vector3 vector)
    {
        var camRot = _cam.transform.rotation.eulerAngles;
        return Quaternion.AngleAxis(camRot.y, Vector3.up) * vector;
    }
}
