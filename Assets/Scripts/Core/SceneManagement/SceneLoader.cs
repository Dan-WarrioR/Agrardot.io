using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.SceneManagement
{
    public static class SceneLoader
    {
        private const LoadSceneMode SceneLoadMode = LoadSceneMode.Additive;

        private static readonly HashSet<string> LoadedScenes = new();

        private static Scene _forceActiveScene;
        private static Scene _previousActive;
        
        public static Scene ForceActiveScene
        {
            get => _forceActiveScene;
            set
            {
                _previousActive = _forceActiveScene;
                _forceActiveScene = value;
                if (_forceActiveScene.IsValid() && _forceActiveScene.isLoaded)
                {
                    SceneManager.SetActiveScene(_forceActiveScene);
                }
            }
        }
        
        static SceneLoader()
        {
            LoadedScenes.Clear();

            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                if (scene.isLoaded)
                {
                    LoadedScenes.Add(scene.name);
                }
            }

            SceneManager.activeSceneChanged += OnActiveSceneChanged;
        }

        
        
        public static bool IsSceneLoaded(string sceneName)
        {
            return SceneManager.GetSceneByName(sceneName).IsValid();
        }

        public static void LoadScene(string sceneName, Action callback = null, bool allowSceneActivation = true)
        {
            if (IsSceneLoaded(sceneName))
            {
                Debug.LogError($"Scene with name {sceneName} already exists!");
                return;
            }
            
            var operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            operation.allowSceneActivation = allowSceneActivation;
            operation.completed += OnLoad;

            void OnLoad(AsyncOperation asyncOperation)
            {
                asyncOperation.completed -= OnLoad;
                LoadedScenes.Add(sceneName);
                callback?.Invoke();
            }
        }

        public static void UnloadScene(string sceneName, Action callback = null)
        {
            if (!IsSceneLoaded(sceneName))
            {
                Debug.LogError($"Can't unload scene with name {sceneName} because it's not loaded!");
                return;
            }
            
            var operation = SceneManager.UnloadSceneAsync(sceneName);
            operation.completed += OnUnload;

            void OnUnload(AsyncOperation asyncOperation)
            {
                asyncOperation.completed -= OnUnload;
                LoadedScenes.Remove(sceneName);
                callback?.Invoke();
                Resources.UnloadUnusedAssets();
                GC.Collect();
            }
        }
        
        

        private static void OnActiveSceneChanged(Scene prev, Scene next)
        {
            if (!ForceActiveScene.isLoaded || next == ForceActiveScene)
            {
                return; 
            }
            SceneManager.SetActiveScene(ForceActiveScene);
        }
    }
}