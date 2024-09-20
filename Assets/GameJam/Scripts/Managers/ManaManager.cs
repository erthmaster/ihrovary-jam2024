using UnityEngine;
using UnityEngine.UI;

namespace GameJam.UI
{
    public class ManaManager : MonoBehaviour
    {
        [SerializeField]private int _maxMana;

        public float mana;
        [SerializeField] private float _speed;


        [SerializeField] private Slider _slider;
        private void Start()
        {
            mana = _maxMana;
        }
        private void FixedUpdate()
        {
            if (mana < _maxMana)
                mana += _speed;
            if (mana < 0)
                mana = 0;
                _slider.value = mana;
        }
    }
}