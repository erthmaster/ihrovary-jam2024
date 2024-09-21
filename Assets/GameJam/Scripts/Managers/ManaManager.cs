using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace GameJam.UI
{
    public class ManaManager : MonoBehaviour
    {
        public float ManaAmount;
        
        [SerializeField] private int _maxMana;
        [SerializeField] private float _speed;
        [SerializeField] private Slider _slider;
        private void Start()
        {
            // ManaAmount = _maxMana;
        }
        private void FixedUpdate()
        {
            if (ManaAmount < _maxMana)
                ManaAmount += _speed * Time.fixedDeltaTime;
    
            ManaAmount = Mathf.Clamp(ManaAmount, 0, _maxMana);
            _slider.value = ManaAmount;
        }
    }
}