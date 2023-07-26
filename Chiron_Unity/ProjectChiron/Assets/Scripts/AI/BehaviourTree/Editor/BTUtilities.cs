using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

public static class BTUtilities
{
    /// <summary>C#'s Script Icon [The one MonoBhevaiour Scripts have].</summary>
    private static Texture2D scriptIcon = (EditorGUIUtility.IconContent("cs Script Icon").image as Texture2D);
    
    
    /// <summary>Creates a new C# Class.</summary>
    [MenuItem("Assets/Create/BT/BTNode C# Script", false, 89)]
    private static void CreateBTNode() {
        string[] guids = AssetDatabase.FindAssets("BTNodeTemplate.cs");
        if (guids.Length == 0) {
            Debug.LogWarning("BTNodeTemplate.cs.txt not found in asset database");
            return;
        }
        string path = AssetDatabase.GUIDToAssetPath(guids[0]);
        CreateFromTemplate(
            "BTNodeNew.cs",
            path
        );
    }
    
    /// <summary>Creates a new C# Class.</summary>
    [MenuItem("Assets/Create/BT/XNode C# Script", false, 89)]
    private static void CreateXNode() {
        string[] guids = AssetDatabase.FindAssets("XNodeTemplate.cs");
        if (guids.Length == 0) {
            Debug.LogWarning("XNodeTemplate.cs.txt not found in asset database");
            return;
        }
        string path = AssetDatabase.GUIDToAssetPath(guids[0]);
        CreateFromTemplate(
            "XNodeNew.cs",
            path
        );
    }
    public static void CreateFromTemplate(string initialName, string templatePath) {
        ProjectWindowUtil.StartNameEditingIfProjectWindowExists(
            0,
            ScriptableObject.CreateInstance<DoCreateCodeFile>(),
            initialName,
            scriptIcon,
            templatePath
        );
    }
    /// Inherits from EndNameAction, must override EndNameAction.Action
    public class DoCreateCodeFile : UnityEditor.ProjectWindowCallback.EndNameEditAction {
        public override void Action(int instanceId, string pathName, string resourceFile) {
            Object o = CreateScript(pathName, resourceFile);
            ProjectWindowUtil.ShowCreatedAsset(o);
        }
    }
    
    /// <summary>Creates Script from Template's path.</summary>
    internal static UnityEngine.Object CreateScript(string pathName, string templatePath) {
        string className = Path.GetFileNameWithoutExtension(pathName).Replace(" ", string.Empty);
        string templateText = string.Empty;

        UTF8Encoding encoding = new UTF8Encoding(true, false);

        if (File.Exists(templatePath)) {
            /// Read procedures.
            StreamReader reader = new StreamReader(templatePath);
            templateText = reader.ReadToEnd();
            reader.Close();

            templateText = templateText.Replace("#SCRIPTNAME#", className);
            templateText = templateText.Replace("#NOTRIM#", string.Empty);
            /// You can replace as many tags you make on your templates, just repeat Replace function
            /// e.g.:
            /// templateText = templateText.Replace("#NEWTAG#", "MyText");

            /// Write procedures.

            StreamWriter writer = new StreamWriter(Path.GetFullPath(pathName), false, encoding);
            writer.Write(templateText);
            writer.Close();

            AssetDatabase.ImportAsset(pathName);
            return AssetDatabase.LoadAssetAtPath(pathName, typeof(Object));
        } else {
            Debug.LogError(string.Format("The template file was not found: {0}", templatePath));
            return null;
        }
    }
}

