using GameJam.Behaviours;
using UnityEngine;
using Zenject;

namespace GameJam.Behaviours
{
    public class CameraMovement : MonoBehaviour
    {
        [Inject] private Player _player;
        [SerializeField] private float _followSpeed;
        [SerializeField] private Vector3 _offset;

        void LateUpdate()
        {
            Vector3 targetPosition = _player.gameObject.transform.position + _offset;
            Vector2 move = Vector2.Lerp(transform.position, targetPosition, _followSpeed * Time.deltaTime);
            float clampedY = Mathf.Clamp(move.y, 5.5f, float.MaxValue);
            transform.position = new(0, clampedY, -10);
        }
    }
}