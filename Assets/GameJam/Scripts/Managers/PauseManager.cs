using GameJam.Board;
using GameJam.UI;
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
        [Inject] Items items;
        [Inject] ManaManager manamanager;
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
            Restart();
            IsPaused = true;
            _menuObj.SetActive(false);
            
            Menu.SetBool("IsGame", false );
        }
        public void PAUSEONGAMEOVER()
        {
            IsPaused = true;
            isgameover = true;
            _menuObj.SetActive(false);
        }
        public void Restart()
        {
            StartCoroutine(generator.ResetAllBoard());
            scoreManager.SetZeroScore();
            items.RestartItems();
            manamanager.mana = 0;
            IsPaused = false;
            isgameover = false;
        }
    }
}