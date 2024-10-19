using GameJam.Board;
using GameJam.Managers;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;
using Zenject;

namespace GameJam.Behaviours
{
    public class BoardTile : MonoBehaviour
    {
        [field:SerializeField] public bool IsBlack { get; private set; }
        [field:SerializeField] public bool IsHole {get; private set;}
        [field:SerializeField] public int Row { get; set; }

        [field:SerializeField] public Animator An;
        [field:SerializeField] public SpriteRenderer _spriteRenderer;

        [field:SerializeField] public int Collum { get; set; }
        
        [field:SerializeField] public Sprite BlackSprite { get; private set; }
        [field:SerializeField] public Sprite WhiteSprite { get; private set; }
        [field:SerializeField] public SpriteRenderer SelectSprite { get; private set; }
        [field:SerializeField] public ParticleSystem WhiteBreak { get; private set; }
        [field:SerializeField] public ParticleSystem BlackBreak { get; private set; }
        //[Inject] GameManager _Manager ������ �� �� �� ������ ���� � ������ �� ���������;
        public GameManager _Manager ;
        public BoardGenerator gen ;

        public void Construct(bool isBlack, bool isHole)
        {
            An.enabled = false;
            IsBlack = isBlack;
            IsHole = isHole;

            _spriteRenderer.sprite = isHole ?
                null :
                isBlack ? BlackSprite : WhiteSprite;
            SelectSprite.enabled = false;
        }
        public void Delete()
        {
            SelectSprite.enabled = false;
            if (IsHole)
            {
                An.enabled = true;
                if (IsBlack)
                    An.Play("TileDestroy_Black");
                else
                    An.Play("TileDestroy");
            }


            CancelInvoke();
            Invoke(nameof(Fade), 0.4f);

        }
        public void Fade()
        {
            TileDeleteObjects();
            /*if (IsBlack)
            {
                Instantiate(BlackBreak, transform.position, Quaternion.identity);
            }
            else
            {
                Instantiate(WhiteBreak, transform.position, Quaternion.identity);
            }*/ //now in BoardDestroyer

            //Camera.main.GetComponent<CameraMovement>().Shake();
            _Manager.TilePool.Release(this);
        }

        public void Fade2()
        {
            TileDeleteObjects();



            _Manager.TilePool.Release(this);



        }
        public void TileDeleteObjects()
        {
            foreach (var s in Physics2D.OverlapCircleAll(transform.position,1))
            {
                if (s.TryGetComponent(out Item item))
                {
                    item.Fade();
                }
                if (s.TryGetComponent(out EnemyAI ai))
                {
                    ai.Die();
                }
                
            }
        }
        public void Select()
        {
            if (!gameObject.activeSelf)
                return;
            if (IsHole) return;
            SelectSprite.enabled = true;


        }
        public void SetSpriteOrder(int order) 
        {
            _spriteRenderer.sortingOrder = order;
            SelectSprite.sortingOrder = order+1;

        }
        public void DeSelect()
        {
            if (!gameObject.activeSelf)
                return;
            SelectSprite.enabled = false;

        }

    }
}