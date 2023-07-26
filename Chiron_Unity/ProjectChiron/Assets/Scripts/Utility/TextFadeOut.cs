using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextFadeOut : CustomYieldInstruction
{
    private float _duration;
    private float _timer;
    private TextMeshProUGUI _text;
    
    public TextFadeOut(TextMeshProUGUI text, float duration)
    {
        _duration = duration;
        _text = text;
        Reset();
    }

    public override bool keepWaiting
    {
        get
        {
            float timer = _timer;
            var percentage = Mathf.Clamp01(_timer / _duration);
            _timer += Time.deltaTime;
            var col = _text.faceColor;
            _text.faceColor = new Color32(col.r, col.g, col.b, (byte) ((1 - percentage) * 255.0));
            
            return timer < _duration;
        }
    }

    public override void Reset()
    {
        base.Reset();
        _timer = 0;
    }
}
