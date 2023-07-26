using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;

public class SecondLifeRoom : MonoBehaviour
{
    [SerializeField] private GameObject conditionTextObject;
    [SerializeField] private GameObject abilityControlsInstruction;
    [SerializeField] private GameObject textBox;
    [SerializeField] private GameObject button;
    [SerializeField, TextArea] private String textboxText;

    private PlayerCharacterController _player;

    private void OnTriggerEnter(Collider other)
    {
        _player = other.GetComponent<PlayerCharacterController>();
        if (_player == null) 
            return;

        textBox.GetComponentInChildren<TextMeshProUGUI>().SetText(textboxText);
        abilityControlsInstruction.GetComponent<TextMeshProUGUI>().SetText("");

        _player.ShootLock = true;
        _player.MoveLock = true;
        _player.AimLock = true;
        _player.TutEnableSecondLife();
        textBox.SetActive(true);
        button.SetActive(true);
        conditionTextObject.SetActive(false);
        abilityControlsInstruction.SetActive(false);
    }
}
