using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;

public class TeleportRoom : MonoBehaviour
{
    [SerializeField] private GameObject textBox;
    [SerializeField] private GameObject button;
    [SerializeField] private GameObject abilityControlsInstruction;
    [SerializeField, TextArea] private String textboxText;
    [SerializeField, TextArea] private String instructionText;
    [SerializeField,TextArea] private String secondInstructionText;

    private PlayerCharacterControllerInput _inputActions;
    private PlayerCharacterController _player;
    private bool _teleportEnabled;
    private bool _teleportExecute;

    private void OnTriggerEnter(Collider other)
    {
        _player = other.GetComponent<PlayerCharacterController>();
        if (_player == null) 
            return;
        
        _inputActions = gameObject.AddComponent<PlayerCharacterControllerInput>();
        _teleportEnabled = false;
        _teleportExecute = false;

        textBox.GetComponentInChildren<TextMeshProUGUI>().SetText(textboxText);
        abilityControlsInstruction.GetComponent<TextMeshProUGUI>().SetText(instructionText);

        _player.ShootLock = true;
        _player.MoveLock = true;
        _player.AimLock = true;
        _player.TutEnableTeleport();
        textBox.SetActive(true);
        button.SetActive(true);

        _inputActions.TeleportEnable
            .Where(input => input == true)
            .Subscribe(input =>
                {
                    if (!_teleportEnabled)
                    {
                        _teleportEnabled = true;
                        abilityControlsInstruction.GetComponent<TextMeshProUGUI>().SetText(secondInstructionText);
                    }
                }).AddTo(this);

        _inputActions.TeleportConfirmLocation
            .Subscribe(input =>
            {
                if (!_teleportExecute)
                {
                    _teleportExecute = true;
                    if (abilityControlsInstruction.activeSelf)
                        abilityControlsInstruction.SetActive(false);
                }
            }).AddTo(this);

        _inputActions.CancelTeleport
            .Subscribe(input =>
            {
                if (!_teleportExecute)
                {
                    _teleportExecute = true;
                    if (abilityControlsInstruction.activeSelf)
                        abilityControlsInstruction.SetActive(false);
                }
            }).AddTo(this);


    }

    private void OnTriggerExit(Collider other)
    {
        //_teleportEnabled = false;
        //_teleportExecute = false;
        if (abilityControlsInstruction.activeSelf)
            abilityControlsInstruction.SetActive(false);
    }


}
