using System;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Authentication.PlayerAccounts;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.SceneManagement;


public class AuthenticationManager : MonoBehaviour
{
    public event Action<PlayerInfo, string> OnSignedIn;
    private PlayerInfo playerInfo;

    async void Awake()
    {
        await UnityServices.InitializeAsync();
        PlayerAccountService.Instance.SignedIn += SignIn;
    }

    public async void SignIn()
    {
        //await signInAnonymous();
        try
        {
            var accessToken = PlayerAccountService.Instance.AccessToken;
            await SignInWithUnityAsync(accessToken);
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    async Task signInAnonymous()
    {
        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            print("Sign in sccessfully");
            print("Player Id:" + AuthenticationService.Instance.PlayerId);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        catch (AuthenticationException ex)
        {
            Debug.LogException(ex);
        }
    }
    public async Task InitSignIn()
    {
        await PlayerAccountService.Instance.StartSignInAsync();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    async Task SignInWithUnityAsync(string accessToken)
    {
        try
        {
            await AuthenticationService.Instance.SignInWithUnityAsync(accessToken);
            Debug.Log("SignIn is successful.");
        }
        catch (AuthenticationException ex)
        {
            // Compare error code to AuthenticationErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            // Compare error code to CommonErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
    }
}
