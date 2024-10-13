using GameJam.Behaviours;
using GameJam.UI;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using Zenject;

namespace GameJam.Managers
{
    public class BoardDestroyerManager : MonoBehaviour
    {
        [Inject] BoardDestroyer _boardDestroyer;
        [Inject] PlayerManager player;
        [Inject] PauseManager pauseManager;
        [SerializeField] private float TickRate;
        [SerializeField] private float MaxTickRate;
        [SerializeField] private float TickStep;
        public float speed;

        private void Start()
        {
            StartCoroutine(Tick());
        }

        public void UpdateTickRate(float NewTickRate)
        {
            TickRate = NewTickRate;

        }
        private IEnumerator Tick()
        {
            while (true)
            {

                if (!pauseManager.IsPaused)
                {


                    _boardDestroyer.transform.position += _boardDestroyer.transform.up * TickStep;
                    Collider2D[] ccs = Physics2D.OverlapCircleAll(_boardDestroyer.transform.position, 4);
                    if (ccs.Any(n => n.transform.GetComponent<Player>() != null))
                    {
                        player.GameOver();
                    }
                    foreach (var item in ccs)
                    {
                        if (item.TryGetComponent<EnemyAI>(out EnemyAI ai))
                        {
                            ai.Die();
                        }
                    }
                    if (TickRate < MaxTickRate)
                    {
                        UpdateTickRate(TickRate + speed);
                    }



                }
                yield return new WaitForSeconds(60 / TickRate);

            }
        }
    }
}