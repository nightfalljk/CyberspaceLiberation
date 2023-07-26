using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class WaitForDuration : CustomYieldInstruction
{
    private float _duration;
    private float _timer;
    private ReactiveProperty<float> _percentage;

    public WaitForDuration(float duration, ReactiveProperty<float> percentage)
    {
        _percentage = percentage;
        _duration = duration;
        Reset();
    }

    public override bool keepWaiting
    {
        get
        {
            float timer = _timer;
            _percentage.Value = Mathf.Clamp01(_timer / _duration);
            _timer += Time.deltaTime;
            
            return timer < _duration;
        }
        
    }

    public override void Reset()
    {
        base.Reset();
        _timer = 0;
    }
}
