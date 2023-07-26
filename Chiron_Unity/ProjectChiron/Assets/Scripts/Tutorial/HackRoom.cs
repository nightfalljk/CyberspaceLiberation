using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;

public class HackRoom : MonoBehaviour
{
    [SerializeField] private GameObject textBox;
    [SerializeField] private GameObject button;
    [SerializeField] private GameObject abilityControlsInstruction;
    [SerializeField, TextArea] private String textboxText;
    [SerializeField, TextArea] private String instructionText;
    [SerializeField,TextArea] private String secondInstructionText;

    private PlayerCharacterControllerInput _inputActions;
    private PlayerCharacterController _player;
    private Hack _hack;
    private bool _hacked;
    private bool _roomEnabled;

    private void OnTriggerEnter(Collider other)
    {
        _player = other.GetComponent<PlayerCharacterController>();
        if (_player == null) 
            return;
        
        _inputActions = gameObject.AddComponent<PlayerCharacterControllerInput>();
        _hack = _player.GetComponentInChildren<Hack>();
        _hacked = false;
        _roomEnabled = true;
        
        textBox.GetComponentInChildren<TextMeshProUGUI>().SetText(textboxText);
        abilityControlsInstruction.GetComponent<TextMeshProUGUI>().SetText(instructionText);

        _player.ShootLock = true;
        _player.MoveLock = true;
        _player.AimLock = true;
        _player.TutEnableHack();
        textBox.SetActive(true);
        button.SetActive(true);

        _hack.EnemiesInRange
            .Where(input => input != false)
            .Subscribe(input =>
            {    if(_roomEnabled)
                    abilityControlsInstruction.GetComponent<TextMeshProUGUI>().SetText(secondInstructionText);
            }).AddTo(this);

        _inputActions.Hack
            .Subscribe(input =>
            {
                if (!_hacked && _roomEnabled)
                {
                    StartCoroutine(DisableInstruction());
                    _hacked = true;
                }
            }).AddTo(this);

    }

    private IEnumerator DisableInstruction()
    {
        yield return new WaitForSeconds(1f);
        abilityControlsInstruction.SetActive(false);
    }
    
    private void OnTriggerExit(Collider other)
    {
        //_hacked = false;
        abilityControlsInstruction.SetActive(false);
        _roomEnabled = false;
    }
}
