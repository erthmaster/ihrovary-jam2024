using GameJam.Managers;
using System;
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
        [field:SerializeField] public ParticleSystem WhiteBreak { get; private set; }
        [field:SerializeField] public ParticleSystem BlackBreak { get; private set; }
        //[Inject] GameManager _Manager чогось ц€ д≥ч не працюЇ тому € зроблю по конченому;
        public GameManager _Manager ;





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
            if (IsHole)
            {
                Destroy(gameObject);
                return;
            }
            if (IsBlack)
               An.Play("TileDestroy_Black");
            else
                An.Play("TileDestroy");

            CancelInvoke();
            Invoke(nameof(Fade), 0.4f);


        }
        private void Fade()
        {

            if (IsBlack)
            {
                Instantiate(BlackBreak, transform.position, Quaternion.identity);
            }
            else
            {
                Instantiate(WhiteBreak, transform.position, Quaternion.identity);
            }


            _Manager.TilePool.Release(this);
        }




    }
}