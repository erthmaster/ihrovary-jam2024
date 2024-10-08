using GameJam.Managers;
using UnityEngine;
using Zenject;

namespace GameJam.Behaviours
{
    public abstract class Item : MonoBehaviour
    {
        [Inject] PlayerManager playerManager;

        private void OnCollisionStay(Collision collision)
        {
            if(collision.gameObject.GetComponent<Player>() && !playerManager.isMoving)
            {
                PickUp();
            }
        }
        public abstract void PickUp();
    }
}