using GameJam.Board;
using GameJam.Managers;
using System;
using System.Linq;
using UnityEngine;
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
        [field:SerializeField] public Sprite SelectSprite { get; private set; }
        [field:SerializeField] public ParticleSystem WhiteBreak { get; private set; }
        [field:SerializeField] public ParticleSystem BlackBreak { get; private set; }
        //[Inject] GameManager _Manager ������ �� �� �� ������ ���� � ������ �� ���������;
        public GameManager _Manager ;
        public BoardGenerator gen ;

        public void Construct(bool isBlack, bool isHole)
        {
            IsBlack = isBlack;
            IsHole = isHole;

            string animname = isHole ?
                "TileIdleHole" :
                isBlack ? "TileIdleBlack" : "TileIdle";
            An.Play(animname);
        }
        public void Delete()
        {
            if (IsBlack)
               An.Play("TileDestroy_Black");
            else
                An.Play("TileDestroy");

            CancelInvoke();
            Invoke(nameof(Fade), 0.4f);

        }
        public void Fade()
        {
            TileDeleteObjects();
            if (IsBlack)
            {
                Instantiate(BlackBreak, transform.position, Quaternion.identity);
            }
            else
            {
                Instantiate(WhiteBreak, transform.position, Quaternion.identity);
            }

            Camera.main.GetComponent<CameraMovement>().Shake();
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
            if(!IsBlack)
                An.Play("TileSelect");
            else
                An.Play("TileSelectBlack");
        }

        public void DeSelect()
        {
            if (!gameObject.activeSelf)
                return;
            string animname = IsHole ?
                "TileIdleHole" :
                IsBlack ? "TileIdleBlack" : "TileIdle";
            An.Play(animname);
        }

    }
}