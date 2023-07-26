using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveController : MonoBehaviour
{
     [SerializeField] Renderer _dissolveRenderer; 
     float offset = 0f;
     private bool buildup = true; 

     // Start is called before the first frame update
    void Start()
    {
        _dissolveRenderer.material.shader = Shader.Find("Custom/DissolveShader");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (offset >= 1)
        {
            buildup = false; 
        }
        else if(offset <= 0)
        {
            buildup = true; 
        }

        if (buildup)
        {
            offset += 0.015f;
        }
        else if (!buildup)
        {
            offset -= 0.015f;
        }
        _dissolveRenderer.material.SetFloat("_Range", offset);
    }
}
