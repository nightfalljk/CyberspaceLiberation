using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;

public class DashRoom : MonoBehaviour
{
    [SerializeField] private GameObject conditionTextObject;
    [SerializeField] private GameObject textBox;
    [SerializeField] private GameObject button;
    [SerializeField] private GameObject abilityControlsInstruction;
    [SerializeField, TextArea] private String textboxText;
    [SerializeField, TextArea] private String instructionText;
    [SerializeField, TextArea] private String conditionText;
    [SerializeField] private DashConfig dashConfig;
    private PlayerCharacterControllerInput _inputActions;
    private PlayerCharacterController _player;

    private void Awake()
    {
        var textCol = conditionTextObject.GetComponent<TextMeshProUGUI>().faceColor;
        conditionTextObject.GetComponent<TextMeshProUGUI>().faceColor = new Color32(textCol.r, textCol.g, textCol.b, (byte)(0f));
    }

    private void OnTriggerEnter(Collider other)
    {
        _player = other.GetComponent<PlayerCharacterController>();
        if (_player == null) 
            return;
        
        _inputActions = gameObject.AddComponent<PlayerCharacterControllerInput>();

        textBox.GetComponentInChildren<TextMeshProUGUI>().SetText(textboxText);
        abilityControlsInstruction.GetComponent<TextMeshProUGUI>().SetText(instructionText);
        conditionTextObject.GetComponent<TextMeshProUGUI>().SetText(
            "" + conditionText + dashConfig.tutExecutionCount.Value + "/" + dashConfig.tutConditionCount);

        _player.ShootLock = true;
        _player.MoveLock = true;
        _player.AimLock = true;
        _player.TutEnableDash();
        textBox.SetActive(true);
        button.SetActive(true);

        _inputActions.Dash
            .Where(input => input == true)
            .Subscribe(input =>
            {
                //if(dashConfig.tutExecutionCount.Value < dashConfig.tutConditionCount)
                    //StartCoroutine(FadeCondText());
                
                if(abilityControlsInstruction.activeSelf)
                    abilityControlsInstruction.SetActive(false);
            }).AddTo(this);

        dashConfig.tutExecutionCount
            .Subscribe(input =>
            {
                conditionTextObject.GetComponent<TextMeshProUGUI>().text =
                    "" + conditionText + dashConfig.tutExecutionCount.Value + "/" + dashConfig.tutConditionCount;
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
        _inputActions.Dash.Dispose();
        dashConfig.tutExecutionCount.Dispose();
        abilityControlsInstruction.SetActive(false);
        conditionTextObject.SetActive(false);
    }
}
