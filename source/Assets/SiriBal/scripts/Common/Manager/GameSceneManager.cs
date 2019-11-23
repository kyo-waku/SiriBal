using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Generic;

namespace Generic.Manager{

    // Note: "SceneManager" has alrady used as the class name of the Unity official library.
    public class GameSceneManager
    {

        // Current Scene
        public static GameScenes _currentScene = GameScenes.None;

        // Before Scene
        public static GameScenes _beforeScene = GameScenes.None;
        
        public static DefinedErrors ChangeScene(GameScenes next)
        {
            SceneManager.LoadScene(next.ToString());

            _beforeScene = _currentScene;
            _currentScene = next;            
            return DefinedErrors.Pass;
        }

        public static DefinedErrors BackToBeforeScene()
        {
            var result = ChangeScene(_beforeScene);
            return result;
        }

    }

}