using GameJam.Behaviours;
using UnityEngine;

namespace GameJam.Behaviours
{
    public class SingleItem : Item
    {
        [Inject] ItemsEffects itemsEffects;
        public override void PickUp()
        {
            if(Name == "FreezeEnemies")
            {

            }
            else if (Name == "")
            {

            }
        }
    }
}