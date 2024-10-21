using GameJam.Managers;
using UnityEditor;
using UnityEngine;
using Zenject;

namespace GameJam.UI
{
    
    public class DispauseAnimEvent : MonoBehaviour
    {
        [Inject] PauseManager pauseManager;
        [SerializeField] private Animator anim;
        private void DisPause()
        {
            pauseManager.IsPaused = false;
        }
        public void StartPlayAnim()
        {
            anim.SetBool("IsGame", true);
        }
    }
}