using System;
using UnityEngine;
using Zenject;

namespace GameJam.Managers
{
    public class BoardDestroyerManager : MonoBehaviour
    {
        [Inject] BoardDestroyer _boardDestroyer;
        [SerializeField] private float TickRate;
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
        }
    }
}