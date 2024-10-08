using System.Collections;
using UnityEngine;

namespace GameJam.Managers
{
    public class ItemsEffects : MonoBehaviour
    {
        public bool IsFreezed;
        public bool IsDoubleGold;
        public bool IsIncrManaSpeed;

        public float CDFreezed;
        public float CDDoubleGold;
        public float CDIncrManaSpeed;

        public void FreezedDelay()
        {
            IsFreezed = false;
        }
        public void DoubleGoldDelay()
        {
            IsDoubleGold = false;
        }
        public void IncrManaDelay()
        {
            IsIncrManaSpeed = false;
        }
    }
}

