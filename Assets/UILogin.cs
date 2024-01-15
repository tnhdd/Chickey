using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.UI;

public class UILogin : MonoBehaviour
{
    [SerializeField] Button loginButton;
    [SerializeField] AuthenticationManager authenticationManager;

    private void OnEnable()
    {
        loginButton.onClick.AddListener(loginButtonPressed);
        authenticationManager.OnSignedIn += LogInController_OnSignedIn;
    }

    private void LogInController_OnSignedIn(PlayerInfo playerInfo, string playerName)
    {
        Debug.Log("id: " + playerInfo.Id + playerName);

    }

    private async void loginButtonPressed()
    {
        await authenticationManager.InitSignIn();
    }
    private void OnDisable()
    {
        loginButton.onClick?.RemoveListener(loginButtonPressed);
        authenticationManager.OnSignedIn -= LogInController_OnSignedIn;

    }
}
