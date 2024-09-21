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

        [field:SerializeField] public SpriteRenderer _spriteRenderer;

        [field:SerializeField] public int Collum { get; set; }
        
        [field:SerializeField] public Sprite BlackSprite { get; private set; }
        [field:SerializeField] public Sprite WhiteSprite { get; private set; }
        



        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void Construct(bool isBlack, bool isHole)
        {
            IsBlack = isBlack;
            IsHole = isHole;

            _spriteRenderer.sprite = isHole ?
                null :
                isBlack ? BlackSprite : WhiteSprite;

        }





    }
}