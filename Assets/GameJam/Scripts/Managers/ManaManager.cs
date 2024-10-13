using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace GameJam.UI
{
    public class ManaManager : MonoBehaviour
    {
        [SerializeField]private int _maxMana;
        [Inject] PauseManager PauseManager;
        public float mana;
        [SerializeField] private float _speed;

        [SerializeField] private Slider _slider;
        [SerializeField] private TMP_Text _manaText;
        private void Start()
        {
            mana = 0;
        }
        private void FixedUpdate()
        {
            if (PauseManager.IsPaused)
                return;
            if (mana < _maxMana)
                mana += _speed * Time.deltaTime;
            if (mana < 0)
                mana = 0;
            _slider.value = mana;
            _manaText.text = Mathf.Floor(mana).ToString();
        }
    }
}