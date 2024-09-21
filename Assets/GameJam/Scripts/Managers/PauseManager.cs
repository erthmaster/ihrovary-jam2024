using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace GameJam
{
    public class PauseManager : MonoBehaviour
    {
        public bool IsPaused { get; private set; }
        [SerializeField] private GameObject menuObj;
        [SerializeField] private GameObject settingsObj;
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                IsPaused = !IsPaused;
            }
            if (IsPaused)
            {
                Time.timeScale = 0.0f;
                menuObj.SetActive(true);
            }
            else
            {
                DisPause();
            }
        }
        public void DisPause()
        {
            if (settingsObj.activeInHierarchy)
            {
                settingsObj.SetActive(false);
            }
            Time.timeScale = 1.0f;
            menuObj.SetActive(false);
        }
        public void GoMenu()
        {
            SceneManager.LoadScene(0);
        }
        public void GoSettings()
        {
            settingsObj.SetActive(true);
            menuObj.SetActive(false);
        }
    }
}