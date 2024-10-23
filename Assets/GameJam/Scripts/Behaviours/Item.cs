using GameJam.Managers;
using UnityEngine;

namespace GameJam.Behaviours
{
    public class Item : MonoBehaviour
    {
        public PlayerManager playerManager;
        public Items _itemsEffects;
        [SerializeField] float spawnChance;
        public string AnimationOnPickUp = "itemDestroy";
        public string Name;

        private bool isUsed = false;

        [SerializeField] private Animator _anim;

        private void OnTriggerStay2D(Collider2D collider)
        {
            if (playerManager == null)
                return;
            if (collider.GetComponent<Player>() && !playerManager.Moving)
            {
                CheckName(Name);
            }
        }
        private void CheckName(string name)
        {
            if (!isUsed)
            {
                isUsed = true;
                if (name == "Coin")
                    _itemsEffects.AddCoin();
                else if (name == "FreezeEnemies")
                    _itemsEffects.FreezedDelay();
                else if (name == "DoubleCoins")
                    _itemsEffects.DoubleGoldDelay();
                else if (name == "ManaRecov")
                    _itemsEffects.IncrManaDelay();
                else if (name == "RandomSpell")
                    _itemsEffects.RandomEffect();
                else if (name == "RandomFigure")
                    _itemsEffects.RandomFigure();
                else if (name == "Mana1")
                    _itemsEffects.AddMana(1);
                else if (name == "Mana2")
                    _itemsEffects.AddMana(2);
                else if (name == "Mana3")
                    _itemsEffects.AddMana(3);


                _anim.Play(AnimationOnPickUp,-1,0);
            }
        }
        //ÑÞÄÈ ÉÄÅ ÀÍ²ÌÀÖ²ß ²ËÞØÓØÓØÓÑÜÊÀ
        public void Fade()
        {
            Destroy(gameObject);
        }
        public bool TrySpawn()
        {
            return Random.value > (1 - spawnChance / 100);
        }
    }
}