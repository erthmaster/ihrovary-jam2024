using GameJam.Behaviours;
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

        [SerializeField] private Transform _tPlayer;
        [SerializeField] private ShakeZone _shakeZone;
        [SerializeField] private ParticleSystem _WhiteBreak;
        [SerializeField] private ParticleSystem _BlackBreak;

        public float _destroyBoardTileAnimSeconds;
        [SerializeField] private float _smallShakeDistance;

        private CameraMovement _cMovement;

        private void Start()
        {

            _cMovement = Camera.main.GetComponent<CameraMovement>();
        }

        public void UpdateTickRate(float NewTickRate)
        {
            TickRate = NewTickRate;

        }
        public void ResetDestroyer()
        {
            _boardDestroyer.transform.position = new Vector3(-0.235400006f, -5.65969992f, 0);
            TickRate = 0.25f;

            player.ResetSpeed();
        }

        private void FixedUpdate()
        {
            if(!pauseManager.IsPaused)
                _boardDestroyer.transform.position += Vector3.up * TickStep * TickRate;
        }   

        public void BreakTiles()
        {
            Collider2D[] ccs = Physics2D.OverlapCircleAll(_boardDestroyer.transform.position, 4);
            if (ccs.Any(n => n.transform.GetComponent<Player>() != null))
            {
                player.GameOver();
            }
            player.AddSpeed();
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
            Invoke(nameof(DestroyParticle), _destroyBoardTileAnimSeconds);
        }
    

    public void DestroyParticle()
    {
        _BlackBreak.Play();
        _WhiteBreak.Play();
        if (_shakeZone.playerIn)
            _cMovement.Shake();
        else if (_tPlayer.position.y < _shakeZone.transform.position.y + _smallShakeDistance)
            _cMovement.SmallShake();
    }
    }
}