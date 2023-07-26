using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;

public class FinalTutorialRoom : MonoBehaviour
{

    [SerializeField] private GameObject conditionTextObject;
    [SerializeField] private GameObject textBox;
    [SerializeField] private GameObject button;
    [SerializeField] private GameObject abilityControlsInstruction;
    [SerializeField, TextArea] private String textboxText;
    [SerializeField, TextArea] private String instructionText;
    private PlayerCharacterControllerInput _inputActions;
    private PlayerCharacterController _player;
    private bool _shot;
    

    private void OnTriggerEnter(Collider other)
    {
        _player = other.GetComponent<PlayerCharacterController>();
        if (_player == null) 
            return;

        _shot = false;
        _inputActions = gameObject.AddComponent<PlayerCharacterControllerInput>();

        textBox.GetComponentInChildren<TextMeshProUGUI>().SetText(textboxText);
        abilityControlsInstruction.GetComponent<TextMeshProUGUI>().SetText(instructionText);

        _player.ShootLock = true;
        _player.MoveLock = true;
        _player.AimLock = true;
        _player.TutEnableWeaponBoost();
        textBox.SetActive(true);
        button.SetActive(true);
        conditionTextObject.SetActive(false);

        _inputActions.Shoot
            .Subscribe(input =>
            {
                if (!_shot)
                {
                    _shot = true;
                    abilityControlsInstruction.SetActive(false);
                }
            }).AddTo(this);
    }

    private void OnTriggerExit(Collider other)
    {
        abilityControlsInstruction.SetActive(false);
    }
}
