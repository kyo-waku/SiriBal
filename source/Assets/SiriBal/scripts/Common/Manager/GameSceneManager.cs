using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Generic;

namespace Generic.Manager{

    // Note: "SceneManager" has alrady used as the class name of the Unity official library.
    public class GameSceneManager
    {        
        public DefinedErrors ChangeScene(GameScenes next)
        {
            SceneManager.LoadScene(next.ToString());

            DataManager.PrevScene = DataManager.CurrentScene;
            DataManager.CurrentScene = next;            
            return DefinedErrors.Pass;
        }

        public DefinedErrors BackToPrevScene()
        {
            var result = ChangeScene(DataManager.PrevScene);
            return result;
        }

    }

}