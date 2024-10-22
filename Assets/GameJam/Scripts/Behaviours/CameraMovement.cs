using GameJam.Behaviours;
using PrimeTween;
using UnityEngine;
using Zenject;

namespace GameJam.Behaviours
{
    public class CameraMovement : MonoBehaviour
    {
        [Inject] private Player _player;
        [SerializeField] private float _followSpeed;
        [SerializeField] private Vector3 _offset;
        
        [SerializeField] private float _shakeDuration;
        [SerializeField] private float _shakeStrength;
        [SerializeField] private float _shakeFactor;

        void FixedUpdate()
        {
            Vector3 targetPosition = _player.gameObject.transform.position + _offset;
            Vector2 move = Vector2.Lerp(transform.position, targetPosition, _followSpeed * Time.deltaTime);
            float clampedY = Mathf.Clamp(move.y, 5.5f, float.MaxValue);
            transform.position = new(0, clampedY, -10);
        }

        public void Shake()
        {
            Vector3 originalPosition = transform.localPosition;

            Tween.ShakeCamera(
                GetComponent<Camera>(),
                _shakeFactor,
                _shakeDuration,
                _shakeStrength
            );
        }
        public void SmallShake()
        {
            Vector3 originalPosition = transform.localPosition;

            Tween.ShakeCamera(
                GetComponent<Camera>(),
                _shakeFactor / 2,
                _shakeDuration,
                _shakeStrength / 2
            );
        }
    }
}