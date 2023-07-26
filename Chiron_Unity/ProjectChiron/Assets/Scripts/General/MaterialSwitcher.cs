using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MaterialSwitcher : MonoBehaviour
{
    [SerializeField] private List<Material> switchableMaterials;

    [Range(0, 1)] [SerializeField] private float switchState;

    [SerializeField] private bool switchingActive = false;

    [Range(0, 1)] [SerializeField] private float targetSwitchState = 0;
    [SerializeField] float valueChangePerSecond = 1;

    [SerializeField] private AnimationCurve animationCurve;
    
    private void Awake()
    {
        switchingActive = true;
    }

    public void Update()
    {
        if (!switchingActive)
            return;

        if (targetSwitchState > switchState)
        {
            switchState += valueChangePerSecond * Time.deltaTime;
            if (switchState > targetSwitchState)
                switchState = targetSwitchState;
        }else if (targetSwitchState < switchState)
        {
            switchState -= valueChangePerSecond * Time.deltaTime;
            if (switchState < targetSwitchState)
                switchState = targetSwitchState;
        }
        
        foreach (Material switchableMaterial in switchableMaterials)
        {
            switchableMaterial.SetFloat("_switch", switchState);
        }
    }

    private void OnApplicationQuit()
    {
        switchingActive = false;
    }

    public void SetSwitch(float f)
    {
        targetSwitchState = animationCurve.Evaluate(f);
        //Debug.Log($"switch value {f} -> {targetSwitchState}");
    }
}
