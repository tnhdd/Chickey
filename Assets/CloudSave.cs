using UnityEngine;

public class CloudSave : MonoBehaviour
{/*
    async void Awake()
    {
        try
        {
            await UnityServices.InitializeAsync();
            await SignInAnonymouslyAsync();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    async Task SignInAnonymouslyAsync()
    {
        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log("Sign in anonymously succeeded!");

            // Shows how to get the playerID
            Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");

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

    public async static void SaveGameData<T>(T inData, string key)
    {
        var data = new Dictionary<string, object> { { key, inData } };
        await CloudSaveService.Instance.Data.Player.SaveAsync(data);
    }*/
}
