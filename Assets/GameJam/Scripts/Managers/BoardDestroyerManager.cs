using System;
using UnityEngine;
using Zenject;

namespace GameJam.Managers
{
    public class BoardDestroyerManager : MonoBehaviour
    {
        [Inject] BoardDestroyer _boardDestroyer;
        [SerializeField] private float _speed;
        
        void Update()
        {
            Transform destroyerTransform = _boardDestroyer.transform;
            destroyerTransform.Translate(new Vector2(0, _speed * Time.deltaTime));
        }
    }
}