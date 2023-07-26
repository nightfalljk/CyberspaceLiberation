#if UNITY_EDITOR

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ShaderHelper : MonoBehaviour
{
    public Material newMat;
    [Header("Vapor Wave")]
    public Material oldMat0;
    [Header("Outrun")]
    public Material oldMat1;

    //public int index;

    //public ShaderGraphVfxAsset a;
    //public List<string> PropertyNames
    
//    public Shader shader1;
//    public Shader shader2;
//    
//    public string PropertyNameNew;
//    public string PropertyNameOld;
    public void Start()
    {
        //ShaderUtils
    }

    public void CopyMatsSame()
    {
        MaterialProperty[] mp0 = MaterialEditor.GetMaterialProperties(new[] {oldMat0});
        MaterialProperty[] mp1 = MaterialEditor.GetMaterialProperties(new[] {oldMat1});
        
        //MaterialProperty[] mpNew = MaterialEditor.GetMaterialProperties(new[] {newMat});
        for (var i = 0; i < mp0.Length; i++)
        {
            MaterialProperty materialProperty0 = mp0[i];
            MaterialProperty materialProperty1 = mp1[i];
            string name = materialProperty0.name;
            Debug.Log(name);
            if (String.Compare(name, 0, "unity", 0, 5) != 0)
            {
                bool f = newMat.HasProperty(name);
                bool s = newMat.HasProperty(name + "_1");
                if (! (f && s))
                {
                    if(!f)
                        Debug.Log($"New Mat does not have --{name}--" );
                    if(!s)
                        Debug.Log($"New Mat does not have --{name}_1--" );
                    continue;
                }
                    
                switch (materialProperty0.type)
                {
                    case MaterialProperty.PropType.Color:
                        newMat.SetColor(name, materialProperty0.colorValue);
                        newMat.SetColor(name + "_1", materialProperty1.colorValue);
                        break;
                    case MaterialProperty.PropType.Vector:
                        newMat.SetVector(name, materialProperty0.vectorValue);
                        newMat.SetVector(name + "_1", materialProperty1.vectorValue);
                        break;
                    case MaterialProperty.PropType.Float:
                        newMat.SetFloat(name, materialProperty0.floatValue);
                        newMat.SetFloat(name + "_1", materialProperty1.floatValue);
                        break;
                    case MaterialProperty.PropType.Range:
                        //newMat.SetColor(name, materialProperty0.colorValue);
                        //newMat.SetColor(name + " (1)", materialProperty1.colorValue);
                        Debug.LogWarning("No Range Copy implemented");
                        break;
                    case MaterialProperty.PropType.Texture:
                        newMat.SetTexture(name, materialProperty0.textureValue);
                        newMat.SetTexture(name + "_1", materialProperty1.textureValue);
                        break;
                    case MaterialProperty.PropType.Int:
                        newMat.SetInt(name, materialProperty0.intValue);
                        newMat.SetInt(name + "_1", materialProperty1.intValue);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

            }
        }
        
        //newMat.Se
    }

    public void CopyShaderPropertiesSolo()
    {
        MaterialProperty[] mp0 = MaterialEditor.GetMaterialProperties(new[] {oldMat0});
        MaterialProperty[] mp1 = MaterialEditor.GetMaterialProperties(new[] {oldMat1});
        
        //MaterialProperty[] mpNew = MaterialEditor.GetMaterialProperties(new[] {newMat});
        for (var i = 0; i < mp0.Length + mp1.Length; i++)
        {
            MaterialProperty materialProperty =  i < mp0.Length ? mp0[i] : mp1[i - (mp0.Length -1)];
            //MaterialProperty materialProperty1 = mp1[i];
            string name = materialProperty.name;
            Debug.Log(name);
            if (String.Compare(name, 0, "unity", 0, 5) != 0)
            {
                bool f = newMat.HasProperty(name);
                //bool s = newMat.HasProperty(name + "_1");
                if (! (f))
                {
                    Debug.Log($"New Mat does not have --{name}--" );
                    continue;
                }
                    
                switch (materialProperty.type)
                {
                    case MaterialProperty.PropType.Color:
                        newMat.SetColor(name, materialProperty.colorValue);
                        //newMat.SetColor(name + "_1", materialProperty1.colorValue);
                        break;
                    case MaterialProperty.PropType.Vector:
                        newMat.SetVector(name, materialProperty.vectorValue);
                        //newMat.SetVector(name + "_1", materialProperty1.vectorValue);
                        break;
                    case MaterialProperty.PropType.Float:
                        newMat.SetFloat(name, materialProperty.floatValue);
                        //newMat.SetFloat(name + "_1", materialProperty1.floatValue);
                        break;
                    case MaterialProperty.PropType.Range:
                        //newMat.SetColor(name, materialProperty0.colorValue);
                        //newMat.SetColor(name + " (1)", materialProperty1.colorValue);
                        Debug.LogWarning("No Range Copy implemented");
                        break;
                    case MaterialProperty.PropType.Texture:
                        newMat.SetTexture(name, materialProperty.textureValue);
                        //newMat.SetTexture(name + "_1", materialProperty1.textureValue);
                        break;
                    case MaterialProperty.PropType.Int:
                        newMat.SetInt(name, materialProperty.intValue);
                        //newMat.SetInt(name + "_1", materialProperty1.intValue);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

            }
        }
        
    }
}

[CustomEditor(typeof(ShaderHelper))]
public class EditorShaderHelper : Editor
{
    public override void OnInspectorGUI()
    {
        ShaderHelper myTarget = (ShaderHelper) target;
        base.OnInspectorGUI();
        GUILayout.Label("New Material has to have two times the same Properties with the second one _1 at the end ");
        if (GUILayout.Button("Copy Material Properties from two identical"))
        {
            myTarget.CopyMatsSame();
        }
        GUILayout.Label("New Material has to have exactly the same Properties");
        if (GUILayout.Button("Copy Material Properties from two different"))
        {
            myTarget.CopyShaderPropertiesSolo();
        }
    }
}
#endif