using UnityEditor;
using UnityEngine;

namespace Configs
{
    public class BaseConfig : ScriptableObject
    {
        [SerializeField] private string _comment;

#if UNITY_EDITOR
        [SerializeField] 
        private string _customName;
#endif
        [SerializeField] 
        private int _id;
        
        public int ConfigID => _id;
        
#if UNITY_EDITOR
        private void OnValidate()
        {
            UpdateConfigId();
        }

        public override string ToString()
        {
            if (_customName.Length > 0)
                return _customName;
            return base.ToString();
        }
#endif

        public void UpdateConfigId()
        {
#if UNITY_EDITOR
            if (!AssetDatabase.TryGetGUIDAndLocalFileIdentifier(this, out var guid, out long id))
                return;
            _id = guid.GetHashCode();
#else 
            Debug.LogError("Not access to this");
#endif
        }
    }
}