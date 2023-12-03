using UnityEditor;
using UnityEngine;

namespace Editor.Packages
{
    public class EmbedPackageEditorWindow : EditorWindow
    {
        public string editorWindowText = "Enter package name: ";
        private string textInput;
        void OnGUI()
        {
            textInput = EditorGUILayout.TextField(editorWindowText, textInput);
            
            if (GUILayout.Button("JUST DO IT!"))
                EmbedPackage.EmbedPackageByName(textInput);
 
            if (GUILayout.Button("Abort"))
                Close();
            
            this.Repaint();
        }
 
        [MenuItem("Window/Embed Package by Name")]
        static void CreateProjectCreationWindow()
        {
            var window = ScriptableObject.CreateInstance<EmbedPackageEditorWindow>();
            window.ShowUtility();
        }
    }
}