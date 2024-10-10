using GameJam.UI;
using System.Collections;
using UnityEngine;
using Zenject;

namespace GameJam.Managers
{
    public class Items : MonoBehaviour
    {
        [Inject] ManaManager manaManager;
        [SerializeField] private int _coinsInMoment;

        [SerializeField] private bool _isFreezed;
        [SerializeField] private bool _isDoubleGold;
        [SerializeField] private bool _isIncrManaSpeed;

        [SerializeField] private float _CDFreezed;
        [SerializeField] private float _CDDoubleGold;
        [SerializeField] private float _CDIncrManaSpeed;
        public void _StartCoroutine(string name)
        {
            StartCoroutine(name);
        }
        private IEnumerator FreezedDelay()//for all visual effects on camera and animations
        {
            _isFreezed = true;
            yield return new WaitForSeconds(_CDFreezed);
            _isFreezed = false;
        }
        private IEnumerator DoubleGoldDelay()
        {
            _isDoubleGold = true;
            yield return new WaitForSeconds(_CDDoubleGold);
            _isDoubleGold = false;
        }
        private IEnumerator IncrManaDelay()
        {
            yield return new WaitForSeconds(_CDIncrManaSpeed);
            _isIncrManaSpeed = false;
        }
        public void RandomFigure()
        {
            //+sound +animation
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
            //use _coinsInMoment
            //addCoin
        }
    }
}