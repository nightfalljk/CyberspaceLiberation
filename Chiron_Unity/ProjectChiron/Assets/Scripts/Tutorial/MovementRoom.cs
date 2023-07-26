using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;

public class MovementRoom : MonoBehaviour
{
    [SerializeField] private GameObject textBox;
    [SerializeField] private GameObject button;
    [SerializeField] private GameObject abilityControlsInstruction;
    [SerializeField, TextArea] private String textboxText;
    [SerializeField, TextArea] private String instructionText;
    [SerializeField, TextArea] private String secondaryInstructionText;
    private PlayerCharacterControllerInput _inputActions;
    [SerializeField] private PlayerCharacterController _player;
    private bool _moved;
    private bool _displayed;

    private void Awake()
    {
        _player.EnterTutorial();
        _moved = false;
        _displayed = false;
        _inputActions = gameObject.AddComponent<PlayerCharacterControllerInput>();
        _player.ResetAfterTutorial();
        
        textBox.GetComponentInChildren<TextMeshProUGUI>().SetText(textboxText);
        abilityControlsInstruction.GetComponent<TextMeshProUGUI>().SetText(instructionText);

        _player.ShootLock = true;
        _player.MoveLock = true;
        _player.AimLock = true;
        _player.EnterTutorial();
        textBox.SetActive(true);
        button.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        
        _player = other.GetComponent<PlayerCharacterController>();
        if (_player == null) 
            return;
        _player.EnterTutorial();
        _inputActions = gameObject.AddComponent<PlayerCharacterControllerInput>();
        _moved = false;
        _displayed = false;

        textBox.GetComponentInChildren<TextMeshProUGUI>().SetText(textboxText);
        abilityControlsInstruction.GetComponent<TextMeshProUGUI>().SetText(instructionText);

        _player.ShootLock = true;
        _player.MoveLock = true;
        _player.AimLock = true;
        _player.EnterTutorial();
        textBox.SetActive(true);
        button.SetActive(true);
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (!textBox.activeSelf)
            StartCoroutine(SwitchInstruction());
    }

    private IEnumerator SwitchInstruction()
    {
        yield return new WaitForSeconds(2f);
        abilityControlsInstruction.GetComponent<TextMeshProUGUI>().SetText(secondaryInstructionText);
        StartCoroutine(DisableInstruction());
    }

    private IEnumerator DisableInstruction()
    {
        yield return new WaitForSeconds(2f);
        abilityControlsInstruction.SetActive(false);
    }
    
    private void OnTriggerExit(Collider other)
    {
        _moved = false;
        _displayed = false;
        abilityControlsInstruction.SetActive(false);
        StopAllCoroutines();
    }
}
