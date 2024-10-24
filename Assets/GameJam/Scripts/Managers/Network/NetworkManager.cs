using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;

namespace GameJam.Network
{
    public class NetworkManager : MonoBehaviour
    {
        private PlayGamesClientConfiguration clientConfig;
        private void Start()
        {
            Configure();
            SignInto(SignInInteractivity.CanPromptOnce, clientConfig);
        }
        internal void Configure()
        {
            clientConfig = new PlayGamesClientConfiguration.Builder().Build();
        }
        internal void SignInto(SignInInteractivity interactivity, PlayGamesClientConfiguration config)
        {
            config = clientConfig;
            PlayGamesPlatform.InitializeInstance(config);
            PlayGamesPlatform.Activate();

            PlayGamesPlatform.Instance.Authenticate(interactivity, (code) =>
            {
                Debug.Log("Authenticating...");
                if(code == SignInStatus.Success)
                {
                    Debug.Log("Successfully Authenticated");
                }
                else
                {
                    Debug.Log("Failed to Authenticate");
                }
            });
        }
    }
}