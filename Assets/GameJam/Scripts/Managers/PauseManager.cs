using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameJam.UI
{
    public class PauseManager : MonoBehaviour
    {
        public bool IsPaused { get; set; } = true;
        [SerializeField] private GameObject _menuObj;
        [SerializeField] private GameObject _settingsObj;
        public bool isgameover = false;

        [SerializeField] private GameObject Menu;
        private void Update()
        {
            if (isgameover)
                return;
            if (IsPaused)
            {
                //Time.timeScale = 0.0f;
                //if(!_settingsObj.activeInHierarchy)
                //    _menuObj.SetActive(true);
            }
            else
            {
                DisPause();
            }
        }
        public void Pause()
        { 
            IsPaused = true;
        }
        public void DisPause()
        {
            IsPaused = false;
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
            Menu.SetActive(true);
        }
        public void GoSettings()
        {
            _settingsObj.SetActive(true);
            _menuObj.SetActive(false);
        }
        public void GoPause()
        {
            _settingsObj.SetActive(false);
            _menuObj.SetActive(true);
        }
        public void PAUSEONGAMEOVER()
        {
            IsPaused = true;
            isgameover = true;
            _menuObj.SetActive(false);
        }
        public void Restart()
        {
            SceneManager.LoadScene(1);
        }
    }
}