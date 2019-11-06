using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginButtonController : MonoBehaviour
{
    // Define
    private enum GameScene{
        None = 0,
        Main,
        Login,
        Score,
        Thankyou,
    }

    // Common values
    private string _mainMessage = "";
    private bool _isMessageChanged = false;
    private GameScene _currentScene = GameScene.Login;
    private bool _isSceneChanged = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(_isSceneChanged)
        {
            ChangeScene(_currentScene);
            _isSceneChanged = false;
        }
        
        if (!System.String.IsNullOrEmpty(_mainMessage))
        {
            ShowMessageOnScreen(_mainMessage);
            _isMessageChanged = false;
        }
    }

    public void LoginToSiribal()
    {
        // Firebase Authentication Static Instance
        Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        auth.SignOut();
        
        // Email & Password from input field
        var email = GameObject.Find("EmailInputField").GetComponent<InputField>().text;
        var password = GameObject.Find("PasswordInputField").GetComponent<InputField>().text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            _mainMessage = "Invalid email or password. Please check your email or password again.";
            _isMessageChanged = true;
            return;
        }

        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            if(task.Exception == null){
                if (task.IsCanceled) {
                    Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                    return;
                }
                if (task.IsFaulted) {
                    Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                    return;
                }
                Firebase.Auth.FirebaseUser newUser = task.Result;
                Debug.LogFormat("User signed in successfully: {0} ({1})",
                    newUser.DisplayName, newUser.UserId);

                if (newUser.IsEmailVerified)
                {
                    _mainMessage = "success";
                    _isMessageChanged = true;
                    _currentScene = GameScene.Main;
                    _isSceneChanged = true;
                }
                else
                {
                    newUser.SendEmailVerificationAsync();
                    _mainMessage = "This emali has not verified yet. We've sent you email again. Please verify from the link.";
                    _isMessageChanged = true;
                }
            }
            else{
                foreach(var innerExc in task.Exception.InnerExceptions)
                {
                    _mainMessage = innerExc.InnerException.Message;
                }
                _isMessageChanged = true;
            }
        });
    }

    public void GuestLogin()
    {
        _mainMessage = "success";
        _isMessageChanged = true;
        _currentScene = GameScene.Main;
        _isSceneChanged = true;
    }
    public void CreateNewUser()
    {
        // Email & Password from input field
        var email = GameObject.Find("EmailInputField").GetComponent<InputField>().text;
        var password = GameObject.Find("PasswordInputField").GetComponent<InputField>().text;

        // Firebase Authentication Static Instance
        Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password)){
            _mainMessage = "Invalid email or password. Please check your email or password again.";
            _isMessageChanged = true;
            return;
        }

        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(
            task => {
                if (task.Exception == null)
                {
                    if (task.IsCanceled) {
                        Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                        return;
                    }
                    if (task.IsFaulted) {
                        Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                        return;
                    }
                    // Firebase user has been created.
                    Firebase.Auth.FirebaseUser newUser = task.Result;
                    Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                        newUser.DisplayName, newUser.UserId);
                    newUser.SendEmailVerificationAsync();
                    
                    _mainMessage = "success";
                    _isMessageChanged = true;
                    _currentScene = GameScene.Thankyou;
                    _isSceneChanged = true;
                }
                else
                {
                    foreach(var innerExc in task.Exception.InnerExceptions)
                    {
                        _mainMessage = innerExc.InnerException.Message;
                    }
                    _isMessageChanged = true;
                }
            }
        );
    }
    void ChangeScene(GameScene eGameScene)
    {    
        switch(eGameScene)
        {
            case GameScene.Login:
                SceneManager.LoadScene("LoginForm");
                break;
            case GameScene.Main:
                SceneManager.LoadScene("SeriousBalloon");
                break;
            case GameScene.Thankyou:
                SceneManager.LoadScene("ThankYou");
                break;
            case GameScene.Score:
            case GameScene.None:
            default:
                Debug.Assert(false, "NotImpl");
                break;
        }
    }

    void ShowMessageOnScreen(string message)
    {
        var mesObject = GameObject.Find("Message").GetComponent<Text>();
        mesObject.text = message;
    }
}

