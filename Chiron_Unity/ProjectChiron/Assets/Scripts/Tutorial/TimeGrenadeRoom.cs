using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;

public class TimeGrenadeRoom : MonoBehaviour
{
        [SerializeField] private GameObject conditionTextObject;
    [SerializeField] private GameObject textBox;
    [SerializeField] private GameObject button;
    [SerializeField] private GameObject abilityControlsInstruction;
    [SerializeField, TextArea] private String textboxText;
    [SerializeField, TextArea] private String instructionText;
    [SerializeField, TextArea] private String conditionText;
    [SerializeField] private SlowFieldConfig slowFieldConfig;
    private PlayerCharacterControllerInput _inputActions;
    private PlayerCharacterController _player;

    private bool _roomEnabled;

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
        _inputActions = gameObject.AddComponent<PlayerCharacterControllerInput>();

        conditionTextObject.SetActive(true);
        textBox.GetComponentInChildren<TextMeshProUGUI>().SetText(textboxText);
        abilityControlsInstruction.GetComponent<TextMeshProUGUI>().SetText(instructionText);
        conditionTextObject.GetComponent<TextMeshProUGUI>().SetText(
            "" + conditionText + slowFieldConfig.tutExecutionCount.Value + "/" + slowFieldConfig.tutConditionCount);

        _player.ShootLock = true;
        _player.MoveLock = true;
        _player.AimLock = true;
        _player.TutEnableSlowField();
        textBox.SetActive(true);
        button.SetActive(true);

        _inputActions.SlowField
            .Subscribe(input =>
            {
                //if(slowFieldConfig.tutExecutionCount.Value < slowFieldConfig.tutConditionCount)
                //if(_roomEnabled && _player.SlowFieldAvailable.Value)
                   // StartCoroutine(FadeCondText());
                
                if(abilityControlsInstruction.activeSelf)
                    abilityControlsInstruction.SetActive(false);
            }).AddTo(this);

        slowFieldConfig.tutExecutionCount
            .Subscribe(input =>
            {
                conditionTextObject.GetComponent<TextMeshProUGUI>().text =
                    "" + conditionText + slowFieldConfig.tutExecutionCount.Value + "/" + slowFieldConfig.tutConditionCount;
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
        slowFieldConfig.tutExecutionCount.Dispose();
        abilityControlsInstruction.SetActive(false);
        conditionTextObject.SetActive(false);
        _roomEnabled = false;
    }
}
