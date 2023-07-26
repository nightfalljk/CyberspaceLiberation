using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;

public class WeaponBoostRoom : MonoBehaviour
{
    [SerializeField] private GameObject conditionTextObject;
    [SerializeField] private GameObject textBox;
    [SerializeField] private GameObject button;
    [SerializeField] private GameObject abilityControlsInstruction;
    [SerializeField, TextArea] private String textboxText;
    [SerializeField, TextArea] private String instructionText;
    [SerializeField, TextArea] private String conditionText;
    [SerializeField] private WeaponBoostConfig weaponBoostConfig;
    private PlayerCharacterControllerInput _inputActions;
    private PlayerCharacterController _player;
    private bool _roomEnabled;
    private bool _boosted;

    private void Awake()
    {
        var textCol = conditionTextObject.GetComponent<TextMeshProUGUI>().faceColor;
        conditionTextObject.GetComponent<TextMeshProUGUI>().faceColor = new Color32(textCol.r, textCol.g, textCol.b, (byte)(0f));
        _roomEnabled = false;
        
    }

    private void OnTriggerEnter(Collider other)
    {
        _player = other.GetComponent<PlayerCharacterController>();
        if (_player == null) 
            return;
        
        _roomEnabled = true;
        _boosted = false;
        _inputActions = gameObject.AddComponent<PlayerCharacterControllerInput>();

        textBox.GetComponentInChildren<TextMeshProUGUI>().SetText(textboxText);
        abilityControlsInstruction.GetComponent<TextMeshProUGUI>().SetText(instructionText);
        conditionTextObject.GetComponent<TextMeshProUGUI>().SetText(conditionText);

        _player.ShootLock = true;
        _player.MoveLock = true;
        _player.AimLock = true;
        _player.TutEnableWeaponBoost();
        textBox.SetActive(true);
        button.SetActive(true);

        weaponBoostConfig.tutCondition
            .Where(input => input != false)
            .Subscribe(input =>
            {
                if (_roomEnabled && !_boosted)
                {
                    _boosted = true;
                    //StartCoroutine(FadeCondText());
                    abilityControlsInstruction.SetActive(false);
                }
            }).AddTo(this);
    }

    private IEnumerator FadeCondText()
    {
        yield return new TextFadeIn(conditionTextObject.GetComponent<TextMeshProUGUI>(), 1);
        yield return new WaitForSeconds(2);
        yield return new TextFadeOut(conditionTextObject.GetComponent<TextMeshProUGUI>(), 1);
    }

    private void OnTriggerExit(Collider other)
    {
        weaponBoostConfig.tutCondition.Dispose();
        conditionTextObject.SetActive(false);
        abilityControlsInstruction.SetActive(false);
        _roomEnabled = false;
    }
}
