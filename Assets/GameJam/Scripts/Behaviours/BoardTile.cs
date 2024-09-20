using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameJam.Behaviours
{
    public class BoardTile : MonoBehaviour
    {
        [field:SerializeField] public bool IsBlack { get; private set; }
        [field:SerializeField] public bool IsHole {get; private set;}
        [field:SerializeField] public int Row { get; set; }
        [field:SerializeField] private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void Construct(bool isBlack, bool isHole)
        {
            IsBlack = isBlack;
            IsHole = isHole;
            
            _spriteRenderer.color = IsBlack ? Color.black : Color.white;
            
            if(isHole) _spriteRenderer.color = Color.red;
        }
    }
}