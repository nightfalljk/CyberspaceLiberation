using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

[ExecuteInEditMode()]
public class Tooltip : MonoBehaviour
{
    //private PlayerInputActions _input;
    public InputSystemUIInputModule inputModule;
    
    public TextMeshProUGUI headerField;
    public TextMeshProUGUI contentField;
    public LayoutElement layoutElement;
    public int characterWrapLimit = 80;
    public RectTransform rectTransform;
    
    private void Awake()
    {
        inputModule = FindObjectOfType<InputSystemUIInputModule>();
        //rectTransform = GetComponent<RectTransform>();
    }

    public Vector2 mousePosition
    {
        get
        {
            if (inputModule == null)
            {
                return new Vector2();
            }
            else
            {
                return
                    inputModule.point.action.ReadValue<Vector2>(); //_input.Teleport.Move.ReadValue<Vector2>();// Character. .point.action.ReadValue<Vector2>();
            }
        }
    }

    void Update()
    {
        if (Application.isEditor)
        {
            SetSize();
        }
        SetPosition();
    }

    private void SetSize()
    {
        int headerLength = headerField.text.Length;
        int contentLength = contentField.text.Length;

        layoutElement.enabled = (headerLength > characterWrapLimit || contentLength > characterWrapLimit) ? true : false;
    }

    private void SetPosition()
    {
        Vector2 position = mousePosition;//Input.mousePosition;

        float pivotX = position.x / Screen.width;
        float pivotY = position.y / Screen.height;
        
        rectTransform.pivot = new Vector2(pivotX, pivotY);
        transform.position = position;
        
    }
    
    public void SetText(string content, string header = "")
    {
        if (string.IsNullOrEmpty(header))
        {
            headerField.gameObject.SetActive(false);
        }
        else
        {
            headerField.gameObject.SetActive(true);
            headerField.text = header;
        }

        contentField.text = content;
        SetSize();
    }
}
