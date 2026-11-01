using System;
using System.IO;
using UnityEngine;

namespace Veyrath.Core
{
    public static class SaveGameStore
    {
        public static bool TryWrite(string path, SaveData data, out string error)
        {
            error = null;
            string temporaryPath = path + ".tmp";
            try
            {
                string directory = Path.GetDirectoryName(path);
                if (!string.IsNullOrEmpty(directory)) Directory.CreateDirectory(directory);
                File.WriteAllText(temporaryPath, JsonUtility.ToJson(data, true));
                if (File.Exists(path)) File.Replace(temporaryPath, path, null);
                else File.Move(temporaryPath, path);
                return true;
            }
            catch (Exception exception)
            {
                error = exception.Message;
                if (File.Exists(temporaryPath)) File.Delete(temporaryPath);
                return false;
            }
        }

        public static bool TryRead(string path, out SaveData data, out string error)
        {
            data = null;
            error = null;
            try
            {
                if (!File.Exists(path)) return false;
                data = JsonUtility.FromJson<SaveData>(File.ReadAllText(path));
                if (data == null || !data.IsSupportedSchema()) throw new InvalidDataException("Unsupported save schema.");
                if (data.schemaVersion == 1) data.schemaVersion = SaveData.CurrentSchemaVersion;
                return true;
            }
            catch (Exception exception)
            {
                error = exception.Message;
                return false;
            }
        }
    }
}
