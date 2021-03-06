﻿using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;

namespace Genso.API.Editor {

    public class GensoEditor
    {
        private static string buildRoot = Regex.Replace(Application.dataPath, "Assets", "") + "/Build/";
        private static string copyRoot = Application.dataPath + "/Post Build/";

        [MenuItem("Assets/Create/Game Config")]
        public static void CreateConfig()
        {
            CreateAsset<Config>();
        }

        [MenuItem("Assets/Create/Character Data")]
        public static void CreateCharacterData() {
            CreateAsset<CharacterData>();
        }

        [MenuItem("Genso/Clear Player Prefs %#c")]
        public static void ClearPlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
            Debug.Log("Player Prefs Cleared.");
        }

        [MenuItem("Genso/Build Windows", false, 51)]
        public static void BuildWindows()
        {
            string buildFolder = buildRoot + "Windows/";
            string copyFolder = copyRoot + "Windows/";
            Build(buildFolder, BuildTarget.StandaloneWindows, copyFolder);
            buildFolder = buildRoot + "Windows_64/";
            Build(buildFolder, BuildTarget.StandaloneWindows64, copyFolder);
        }

        [MenuItem("Genso/Build Mac", false, 51)]
        public static void BuildMac()
        {
            string buildFolder = buildRoot + "Mac/";
            string copyFolder = copyRoot + "Mac/";
            Build(buildFolder, BuildTarget.StandaloneOSXUniversal, copyFolder);
        }

        [MenuItem("Genso/Build Linux", false, 51)]
        public static void BuildLinux() {
            string buildFolder = buildRoot + "Linux/";
            string copyFolder = copyRoot + "Linux/";
            Build(buildFolder, BuildTarget.StandaloneLinuxUniversal, copyFolder);
        }

        [MenuItem("Genso/Build All", false, 101)]
        public static void BuildAll() {
            BuildWindows();
            BuildMac();
            BuildLinux();
        }

        public static void Build(string path, BuildTarget target, string copyPath = null) {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            
            // Delete everything in build folder
            DirectoryInfo directory = new DirectoryInfo(path);
            foreach(FileInfo file in directory.GetFiles())
                file.Delete();
            foreach(DirectoryInfo subdirectory in directory.GetDirectories())
                subdirectory.Delete(true);

            string executablePath = path + "genso";
            switch (target) {
                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneWindows64:
                    executablePath += ".exe";
                    break;
                case BuildTarget.StandaloneLinux:
                case BuildTarget.StandaloneLinux64:
                case BuildTarget.StandaloneLinuxUniversal:
                    executablePath += ".x86";
                    break;
            }

            EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
            string[] scenePaths = new string[scenes.Length];
            for (var i = 0; i < scenes.Length; i++)
                scenePaths[i] = scenes[i].path;

            BuildPipeline.BuildPlayer(scenePaths, executablePath, target, BuildOptions.None);

            if (copyPath == null)
                return;

            // Copy all Post Build files to output directory

            //Now Create all of the directories
            foreach (string dirPath in Directory.GetDirectories(copyPath, "*", 
                SearchOption.AllDirectories))
                Directory.CreateDirectory(dirPath.Replace(copyPath, path));

            //Copy all the files & Replaces any files with the same name
            foreach (string newPath in Directory.GetFiles(copyPath,
                                                          "*.*",
                                                          SearchOption.AllDirectories)) {
                if(!newPath.Contains(".meta"))
                    File.Copy(newPath, newPath.Replace(copyPath, path), true);   
            }
        }

        /// <summary>
        /// Create new asset from <see cref="ScriptableObject"/> type with unique name at
        /// selected folder in project window. Asset creation can be cancelled by pressing
        /// escape key when asset is initially being named.
        /// </summary>
        /// <typeparam name="T">Type of scriptable object.</typeparam>
        public static void CreateAsset<T>() where T : ScriptableObject
        {
            var asset = ScriptableObject.CreateInstance<T>();
            ProjectWindowUtil.CreateAsset(asset, "New " + typeof(T).Name + ".asset");
        }
    }


}