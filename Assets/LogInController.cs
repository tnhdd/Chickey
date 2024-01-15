using System;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Authentication.PlayerAccounts;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogInController : MonoBehaviour
{
    public event Action<PlayerInfo, string> OnSignedIn;
    private PlayerInfo playerInfo;

    async void Awake()
    {
        await UnityServices.InitializeAsync();

        PlayerAccountService.Instance.SignedIn += SignedIn;
    }

    async void SignedIn()
    {

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
            playerInfo = AuthenticationService.Instance.PlayerInfo;
            var name = await AuthenticationService.Instance.GetPlayerNameAsync();
            OnSignedIn?.Invoke(playerInfo, name);
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

    private void OnDestroy()
    {
        PlayerAccountService.Instance.SignedIn -= SignedIn;
    }
}
