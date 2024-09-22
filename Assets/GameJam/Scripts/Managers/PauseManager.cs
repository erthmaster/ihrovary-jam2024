using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace GameJam.UI
{
    public class PauseManager : MonoBehaviour
    {
        public bool IsPaused { get;  set; }
        [SerializeField] private GameObject _menuObj;
        [SerializeField] private GameObject _settingsObj;
        public bool isggd;
        private void Update()
        {
            if (isggd)
                return;
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                IsPaused = !IsPaused;
            }
            if (IsPaused)
            {
                Time.timeScale = 0.0f;
                if(!_settingsObj.activeInHierarchy)
                    _menuObj.SetActive(true);
            }
            else
            {
                DisPause();
            }
        }
        public void DisPause()
        {
            if (_settingsObj.activeInHierarchy)
            {
                _settingsObj.SetActive(false);
            }
            Time.timeScale = 1.0f;
            _menuObj.SetActive(false);
        }
        public void GoMenu()
        {
            IsPaused = false;
            SceneManager.LoadScene(0);
        }
        public void GoSettings()
        {
            _settingsObj.SetActive(true);
            _menuObj.SetActive(false);
        }
        public void PAUSEONGAMEOVER()
        {
            IsPaused = true;
            isggd = true;
            Time.timeScale = 0.0f;
        }
        public void Restart()
        {
            SceneManager.LoadScene(1);
        }
    }
}