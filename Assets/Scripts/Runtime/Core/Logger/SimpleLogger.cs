using UnityEngine;

namespace Runtime.Core.Logger
{
    public class SimpleLogger : ILogger
    {
        void ILogger.Log(string message) =>
            Debug.Log(message);

        void ILogger.LogWarning(string message) =>
            Debug.LogWarning(message);

        void ILogger.LogError(string message) =>
            Debug.LogError(message);
    }
}