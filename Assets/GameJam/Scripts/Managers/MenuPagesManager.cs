using UnityEngine;

namespace GameJam.UI
{
    public class MenuPagesManager : MonoBehaviour
    {
        [SerializeField] private GameObject _menu;
        [SerializeField] private GameObject _settings;
        [SerializeField] private GameObject _customisation;
        [SerializeField] private GameObject _updates;
        public void OpenMenu()
        {
            _menu.SetActive(true);
            _settings.SetActive(true);
            _customisation.SetActive(true);
            _updates.SetActive(true);
        }
    }
}
