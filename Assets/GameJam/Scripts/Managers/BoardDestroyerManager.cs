using GameJam.Behaviours;
using System;
using System.Linq;
using UnityEngine;
using Zenject;

namespace GameJam.Managers
{
    public class BoardDestroyerManager : MonoBehaviour
    {
        [Inject] BoardDestroyer _boardDestroyer;
        [Inject] PlayerManager player;
        [SerializeField] private float TickRate;
        [SerializeField] private float MaxTickRate;
        [SerializeField] private float TickStep;


        private void Start()
        {
            InvokeRepeating(nameof(Tick), 2, 60 / TickRate);
        }

        public void UpdateTickRate(float NewTickRate)
        {
            CancelInvoke();
            TickRate = NewTickRate;
            InvokeRepeating(nameof(Tick), 2, 60 / TickRate);

        }
        private void Tick()
        {
            _boardDestroyer.transform.position += _boardDestroyer.transform.up*TickStep;
            if (Physics2D.OverlapCircleAll(_boardDestroyer.transform.position, 4).Any(n=>n.transform.GetComponent<Player>()!=null))
            {
                player.GameOver();
            }
            if (TickRate < MaxTickRate)
            {
                UpdateTickRate(TickRate + 1);
            }
                
        }
    }
}