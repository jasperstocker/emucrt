using System.IO;
using UnityEngine;

namespace EmulationCRT
{
    public class SettingsLoadSave
    {
        // private const string FileExtension = ".emuset";
        //
        // public static void SaveSettings(Settings data)
        // {
        //     // Save the full path to the file.
        //     string saveFile = $"{Application.persistentDataPath}/{data.name}.emuset";
        //     string json = JsonUtility.ToJson(data);
        //     File.WriteAllText(saveFile, json);
        // }
        //
        // public static void LoadSettings(Settings data)
        // {
        //     // Save the full path to the file.
        //     string saveFile = $"{Application.persistentDataPath}/{data.name}.emuset";
        //     string json = JsonUtility.ToJson(data);
        //     File.WriteAllText(saveFile, json);
        // }
        //
        // public static string[] GetAllSettings()
        // {
        //     var ext = new List<string> { FileExtension };
        //     var myFiles = Directory
        //         .EnumerateFiles(dir, "*.*", SearchOption.AllDirectories)
        //         .Where(s => ext.Contains(Path.GetExtension(s).TrimStart(".").ToLowerInvariant()));
        // }
    }
}