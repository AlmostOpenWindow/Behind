#if UNITY_EDITOR
using System.Linq;
using UnityEngine;

namespace FlatKit {
public static class SubAssetMaterial {
    public static Material GetOrCreate(Object settings, string shaderName) {
        var settingsPath = UnityEditor.AssetDatabase.GetAssetPath(settings);
        var subAssets = UnityEditor.AssetDatabase.LoadAllAssetsAtPath(settingsPath);
        const string subAssetName = "Effect Material";
        var existingMaterial = subAssets.FirstOrDefault(o => o.name == subAssetName) as Material;
        if (existingMaterial != null) return existingMaterial;

        var shader = Shader.Find(shaderName);
        if (shader == null) return null;

        var newMaterial = new Material(shader) { name = subAssetName };
        try {
            UnityEditor.AssetDatabase.AddObjectToAsset(newMaterial, settings);
            UnityEditor.AssetDatabase.ImportAsset(settingsPath);
        }
        catch {
            // ignored
        }

        return newMaterial;
    }
}
}
#endif