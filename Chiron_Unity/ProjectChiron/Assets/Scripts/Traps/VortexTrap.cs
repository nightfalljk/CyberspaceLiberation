using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class VortexTrap : MonoBehaviour
{
    //TODO: Extend this to all classes? Might make the game more chaotic
    
    private Transform vortexCenter;
    [SerializeField] private VortexTrapConfig vortexTrapConfig;

    private Vector3 _playerPos;
    private Vector3 _vortexPos;
    
    private Vector3 _dir;
    private PlayerCharacterController _player;
    private bool _pulledIn;
    private bool _trapped;
    private bool _playerSet;
    private Vector3 _lastPos;
    private bool _firstUpdate;
    private void Awake()
    {
        _player = null;
        _trapped = false;
        _pulledIn = false;
        _playerSet = false;
        _firstUpdate = true;
        
        _dir = Vector3.zero;
        _lastPos = Vector3.zero;
        //_vortexPos = new Vector3(vortexCenter.position.x, 0, vortexCenter.position.z);
    }

    private void Update()
    {
        if (_playerSet)
        {
            vortexCenter = this.gameObject.transform;
            _vortexPos = new Vector3(vortexCenter.position.x, 0, vortexCenter.position.z);
            var currentPlayerPos = _player.gameObject.transform.position;
            var dist = (currentPlayerPos - _vortexPos).magnitude;
            if (!_pulledIn &&  dist > vortexTrapConfig.spitThreshold)
            {
                _player.Move(_dir, vortexTrapConfig.pullSpeed);
            }
            
            else if (!_pulledIn && dist < vortexTrapConfig.spitThreshold)
            {
                _pulledIn = true;
                StartCoroutine(Duration());
                _lastPos = _player.gameObject.transform.position;
                _dir = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
            }

            if (_trapped && _pulledIn)
            {
                if (!_firstUpdate && ((currentPlayerPos - _lastPos).normalized - _dir).magnitude > vortexTrapConfig.directionThreshold)
                {
                    PreemptiveCancel();
                }
                else
                {
                    _lastPos = _player.gameObject.transform.position;
                    _firstUpdate = false;
                    _player.Move(_dir, vortexTrapConfig.spitSpeed);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        _player = other.GetComponent<PlayerCharacterController>();
        if (_player != null)
        {
            _trapped = true;
            _player = other.GetComponent<PlayerCharacterController>();
            _player.MoveLock = true;
            _player.AimLock = true;
            _playerSet = true;

            _playerPos = new Vector3(_player.transform.position.x, 0, _player.transform.position.z);
            _dir = (_vortexPos - _playerPos).normalized;
        }
    }

    private void PreemptiveCancel()
    {
        StopAllCoroutines();
        _player.MoveLock = false;
        _player.AimLock = false;
        _player.ShootLock = false;
        _playerSet = false;
        _pulledIn = false;
        _trapped = false;
        _firstUpdate = true;
        _player = null;
    }

    private IEnumerator Duration()
    {
        _player.ShootLock = true;
        yield return new WaitForSeconds(vortexTrapConfig.duration);
        _player.MoveLock = false;
        _player.AimLock = false;
        _player.ShootLock = false;
        _firstUpdate = true;
        _playerSet = false;
        _pulledIn = false;
        _trapped = false;
        _player = null;
    }
}
