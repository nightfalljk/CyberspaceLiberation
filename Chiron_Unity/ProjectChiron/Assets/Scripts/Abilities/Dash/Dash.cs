using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class Dash : MonoBehaviour
{
    private DashConfig _dashConfig;
    private bool _dashing;
    private ReactiveProperty<bool> _dashRdy;
    private ReactiveProperty<bool> _doubleDashRdy;
    private ReactiveProperty<bool> _dashEnabled;
    private ReactiveProperty<float> _cooldownPercentage;
    private ReactiveProperty<float> _durationPercentage;
    
    
    
    public void SetDashConfig(DashConfig dashConfig)
    {
        this._dashConfig = dashConfig;
    }

    private void Awake()
    {
        _dashing = false;
        _dashRdy = new ReactiveProperty<bool>();
        _doubleDashRdy = new ReactiveProperty<bool>();
        _dashEnabled = new ReactiveProperty<bool>();
        _cooldownPercentage = new ReactiveProperty<float>();
        _durationPercentage = new ReactiveProperty<float>();
        _cooldownPercentage.Value = 1;
        _durationPercentage.Value = 1;
        _dashRdy.Value = true;
        _doubleDashRdy.Value = true;
        _dashEnabled.Value = true;
    }

    public IEnumerator FirstDash()
    {
        if (_dashConfig.tutExecutionCount.Value < _dashConfig.tutConditionCount)
        {
            _dashConfig.tutExecutionCount.Value++;
            if(_dashConfig.tutExecutionCount.Value == _dashConfig.tutConditionCount)
                _dashConfig.tutCondition.Value = true;
        }

        _dashRdy.Value = false;
        yield return new WaitForDuration(_dashConfig.duration, _durationPercentage);
        _dashing = false;
        yield return new WaitForCooldown(_dashConfig.cooldown, _cooldownPercentage);
        _dashRdy.Value = true;
        _doubleDashRdy.Value = true;
    }

    public IEnumerator DoubleDash()
    {
        if (_dashConfig.tutExecutionCount.Value < _dashConfig.tutConditionCount)
        {
            _dashConfig.tutExecutionCount.Value++;
            if(_dashConfig.tutExecutionCount.Value == _dashConfig.tutConditionCount)
                _dashConfig.tutCondition.Value = true;
        }

        _doubleDashRdy.Value = false;
        yield return new WaitForDuration(_dashConfig.duration, _durationPercentage);
        _dashing = false;
    }

    public void ResetOnNewLevel()
    {
        _dashing = false;
        _cooldownPercentage.Value = 1;
        _durationPercentage.Value = 1;
        _dashRdy.Value = true;
        _doubleDashRdy.Value = true;
    }

    public bool Dashing
    {
        get => _dashing;
        set => _dashing = value;
    }

    public ReactiveProperty<bool> DashRdy
    {
        get => _dashRdy;
        set => _dashRdy = value;
    }

    public ReactiveProperty<bool> DoubleDashRdy
    {
        get => _doubleDashRdy;
        set => _doubleDashRdy = value;
    }

    public float DashDuration => _dashConfig.duration;

    public ReactiveProperty<bool> DashEnabled
    {
        get => _dashEnabled;
        set => _dashEnabled = value;
    }

    public ReactiveProperty<float> CooldownPercentage => _cooldownPercentage;

    public ReactiveProperty<float> DurationPercentage => _durationPercentage;
}
