using UnityEngine;

namespace Veyrath.Core
{
    /// <summary>One place to identify and filter project messages in the Unity Console.</summary>
    public static class VeyrathLogger
    {
        public static void Info(string category, string message)
        {
            Debug.Log(Format(category, message));
        }

        public static void Warning(string category, string message)
        {
            Debug.LogWarning(Format(category, message));
        }

        public static void Error(string category, string message, Object context = null)
        {
            if (context == null) Debug.LogError(Format(category, message));
            else Debug.LogError(Format(category, message), context);
        }

        private static string Format(string category, string message)
        {
            return "[Veyrath/" + category + "] " + message;
        }
    }
}
