using GameJam.Board;
using UnityEngine;
using Zenject;

namespace GameJam.Managers
{
    public class PauseManager : MonoBehaviour
    {
        public bool IsPaused { get; set; } = true;
        [SerializeField] private GameObject _menuObj;
        public bool isgameover = false;

        [SerializeField] private Animator Menu;

        [Inject] BoardGenerator generator;
        [Inject] ScoreManager scoreManager;
        public void Pause()
        { 
            IsPaused = true;
            _menuObj.SetActive(true);
        }
        public void DisPause()
        {
            IsPaused = false;
            _menuObj.SetActive(false);
        }
        public void GoMenu()
        {
            StartCoroutine(generator.ResetAllBoard());
            scoreManager.SetZeroScore();
            _menuObj.SetActive(false);
            
            Menu.SetBool("IsGame", false );
            IsPaused = true;//я хз чому ispaused не працює йопт
        }
        public void PAUSEONGAMEOVER()
        {
            IsPaused = true;
            isgameover = true;
            _menuObj.SetActive(false);
        }
        public void UNPAUSEONGAMEOVER()
        {
            IsPaused = false;
            isgameover = false;
            _menuObj.SetActive(true);
        }
        public void Restart()
        {
            StartCoroutine(generator.ResetAllBoard());
            scoreManager.SetZeroScore();
        }
    }
}