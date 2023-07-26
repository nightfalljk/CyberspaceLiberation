using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class WaitForCooldown : CustomYieldInstruction
{
    private float _cooldown;
    private float _timer;
    private ReactiveProperty<float> _percentage;

    public WaitForCooldown(float cooldown, ReactiveProperty<float> percentage)
    {
        _percentage = percentage;
        _cooldown = cooldown;
        Reset();
    }

    public override bool keepWaiting
    {
        get
        {
            float timer = _timer;
            _percentage.Value = Mathf.Clamp01(_timer / _cooldown);
            _timer += Time.deltaTime;
            
            return timer < _cooldown;
        }
        
    }

    public override void Reset()
    {
        base.Reset();
        _timer = 0;
    }
}
