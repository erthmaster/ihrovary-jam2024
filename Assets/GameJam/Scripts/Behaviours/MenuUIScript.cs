using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameJam.UI
{
    public class MenuUIScript : MonoBehaviour
    {
        public void LoadGame()
        {
            SceneManager.LoadScene(1);
        }
    }
}