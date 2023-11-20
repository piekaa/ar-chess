using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginController : MonoBehaviour
{
    [SerializeField] private InputField login;
    [SerializeField] private InputField password;

    [SerializeField] private LoginData loginData;
    
    public void Login()
    {
        //todo in thread
        var (status, cookie) = Http.PostMultipartAndGetCookie("lichess.org", "/login", new Dictionary<string, string>
        {
            { "username", login.text },
            { "password", password.text },
            { "remember", "true" },
            { "token", "" },
        });

        if (status == 200)
        {
            loginData.cookie = cookie;
            
            EventSystem.Instance.Fire(EventName.LoggeIn, new());
            
            SceneManager.LoadScene("Lichess Ar");
        }
        else
        {
            EventSystem.Instance.Fire(EventName.Unauthorized, new());
        }
    }
}