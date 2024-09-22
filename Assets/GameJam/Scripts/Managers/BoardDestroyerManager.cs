using System;
using System.Collections;
using UnityEngine;
using Zenject;

namespace GameJam.Managers
{
    public class BoardDestroyerManager : MonoBehaviour
    {
        [Inject] BoardDestroyer _boardDestroyer;
        [Inject] MusicChangeManager _musicChangeManager;
        [SerializeField] private float TickRate;
        [SerializeField] private float MaxTickRate;
        [SerializeField] private float TickStep;


        private void Start()
        {
            StartCoroutine(Tick());
        }

        public void UpdateTickRate(float NewTickRate)
        {
            TickRate = NewTickRate;
            if(TickRate > 50)
                _musicChangeManager.currentIndexNeed = 1;
            else if(TickRate > 60)
                _musicChangeManager.currentIndexNeed = 2;
            else if (TickRate > 70)
                _musicChangeManager.currentIndexNeed = 3;
            else if (TickRate > 80)
                _musicChangeManager.currentIndexNeed = 4;
            else if (TickRate > 90)
                _musicChangeManager.currentIndexNeed = 5;
        }
        IEnumerator Tick()
        {
            while(true)
            {
                yield return new WaitForSeconds(60 / TickRate);
                _boardDestroyer.transform.position += _boardDestroyer.transform.up * TickStep;

                if (TickRate < MaxTickRate)
                {
                    UpdateTickRate(TickRate + 1);
                }
            }
        }
    }
}