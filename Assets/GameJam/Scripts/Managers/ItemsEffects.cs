using GameJam.UI;
using UnityEngine;
using Zenject;
using UnityEngine.UI;
using static GameJam.Managers.PlayerManager;

namespace GameJam.Managers
{
    public class Items : MonoBehaviour
    {
        [Inject] ManaManager manaManager;
        [Inject] ScoreManager scoreManager;
        [Inject] PlayerManager playerManager;

        [SerializeField] private int _coinsInMoment;

        public bool _isFreezed { get; private set; }
        public bool _isDoubleGold { get; private set; }
        public bool _isIncrManaSpeed { get; private set; }

        [SerializeField] private float _CDFreezed;
        [SerializeField] private float _CDDoubleGold;
        [SerializeField] private float _CDIncrManaSpeed;

        [SerializeField] private Image _FreezedImage;
        [SerializeField] private Image _DoubleGoldImage;
        [SerializeField] private Image _IncrManaSpeedImage;

        private float TFreezed;
        private float TDoubleGold;
        private float TIncrManaSpeed;
        public void RestartItems()
        {
            TFreezed = 0;
            TDoubleGold = 0;
            TIncrManaSpeed = 0;
        }
        private void FixedUpdate()
        {
            if (_isFreezed)
            {
                TFreezed -= 0.02f;
                _FreezedImage.fillAmount = TFreezed/_CDFreezed;
                if (TFreezed <= 0) OffFreezed();
            }
            if( _isDoubleGold)
            {
                TDoubleGold -= 0.02f;
                _DoubleGoldImage.fillAmount = TDoubleGold / _CDDoubleGold;
                if (TDoubleGold <= 0) OffDoubleGold();
            }
            if (_isIncrManaSpeed)
            {
                TIncrManaSpeed -= 0.02f;
                _IncrManaSpeedImage.fillAmount = TIncrManaSpeed / _CDIncrManaSpeed;
                if (TIncrManaSpeed <= 0) OffDoubleGold();
            }
        }
        public void FreezedDelay()//for all visual effects on camera and animations
        {
            TFreezed = _CDFreezed;
            _FreezedImage.gameObject.SetActive(true);
            _isFreezed = true;
        }
        private void OffFreezed()
        {
            _FreezedImage.gameObject.SetActive(false);
            _isFreezed = false;
        }
        public void DoubleGoldDelay()
        {
            _DoubleGoldImage.gameObject.SetActive(true);
            TDoubleGold = _CDDoubleGold;
            _isDoubleGold = true;
            
        }
        private void OffDoubleGold()
        {
            _DoubleGoldImage.gameObject.SetActive(false);
            _isDoubleGold = false;
        }
        public void IncrManaDelay()
        {
            _IncrManaSpeedImage.gameObject.SetActive(true);
            TIncrManaSpeed = _CDIncrManaSpeed;
            _isIncrManaSpeed = true;
        }
        private void OffIncrManaDelay()
        {
            _IncrManaSpeedImage.gameObject.SetActive(false);
            _isIncrManaSpeed = false;
        }
        public void RandomFigure()
        {
            //+sound +animation
            playerManager.TurnInToRandomFigure();
        }
        public void RandomEffect()
        {
            //+sound +animation


        }
        public void AddMana(int count)
        {
            manaManager.mana += count;
            //+sound +animation
        }
        public void AddCoin()
        {
            scoreManager.AddMoney();
            //use _coinsInMoment
            //addCoin
        }
    }
}