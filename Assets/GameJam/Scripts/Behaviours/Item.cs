using GameJam.Managers;
using UnityEngine;
using Zenject;

namespace GameJam.Behaviours
{
    public class Item : MonoBehaviour
    {
        [Inject] PlayerManager playerManager;
        [Inject] private ItemsEffects _itemsEffects;

        public string Name;

        private void OnTriggerStay2D(Collider2D collider)
        {
            Debug.Log("1");
            if (collider.GetComponent<Player>() && !playerManager.Moving)
            {
                Debug.Log("1");
                CheckName(Name);
            }
        }
        private void CheckName(string name)
        {
            if (name == "FreezeEnemies")
            {
                FreezeEnemies();
            }
            else if(name == "DoubleCoins")
            {
                DoubleCoins();
            }
        }

        private void FreezeEnemies()
        {
            _itemsEffects.IsFreezed = true;
            Invoke(nameof(_itemsEffects.FreezedDelay), _itemsEffects.CDFreezed);
            Destroy(gameObject);
        }
        private void DoubleCoins()
        {
            _itemsEffects.IsDoubleGold = true;
            Invoke(nameof(_itemsEffects.DoubleGoldDelay), _itemsEffects.CDDoubleGold);
            Destroy(gameObject);
        }
    }
}