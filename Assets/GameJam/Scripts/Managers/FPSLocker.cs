using UnityEngine;

namespace GameJam.Managers
{
    public class FPSLocker : MonoBehaviour
    {
        void Start()
        {
            Application.targetFrameRate = 60;
        }
    }
}