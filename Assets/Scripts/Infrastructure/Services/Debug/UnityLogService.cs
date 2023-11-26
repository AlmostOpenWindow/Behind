using UnityEngine;

namespace Services.Debug
{
    public class UnityLogService : ILogService
    {
        public void Log(string message)
            => UnityEngine.Debug.Log(message);

        public void Warning(string message)
            =>  UnityEngine.Debug.LogWarning(message);

        public void Error(string message)
            =>  UnityEngine.Debug.LogError(message);

        public void ErrorFormat(string message, params object[] formatValues)
            =>  UnityEngine.Debug.LogErrorFormat(message, formatValues);
    }
}