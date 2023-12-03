using UnityEditor;
using UnityEngine;

namespace Editor.Packages
{
    public class EmbedPackageEditorWindow : EditorWindow
    {
        public string editorWindowText = "Enter package name: ";
 
        void OnGUI()
        {
            GUILayout.Label(editorWindowText);
            string inputText = EditorGUILayout.TextField("");
 
            if (GUILayout.Button("JUST DO IT!"))
                EmbedPackage.EmbedPackageByName(inputText);
 
            if (GUILayout.Button("Abort"))
                Close();
        }
 
        [MenuItem("Window/Embed Package by Name")]
        static void CreateProjectCreationWindow()
        {
            var window = ScriptableObject.CreateInstance<EmbedPackageEditorWindow>();
            window.ShowUtility();
        }
    }
}