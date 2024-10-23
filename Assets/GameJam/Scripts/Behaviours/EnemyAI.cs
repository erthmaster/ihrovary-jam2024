using GameJam.Managers;
using System.Collections;
using UnityEngine;
using static GameJam.Managers.PlayerManager;

namespace GameJam.Behaviours
{
    public class EnemyAI : MonoBehaviour
    {
        public PlayerManager.ChessPiece CurrentChessType = PlayerManager.ChessPiece.Pawn;

        [field: SerializeField] public int Row { get; set; }
        [field: SerializeField] public int Column { get;  set; }
        [field: SerializeField] public Sprite[] Skins { get; private set; }
        [field: SerializeField] public bool IsEverMoved { get; private set; }
        [field: SerializeField] public GameObject BlackBreak { get; private set; }
        public PlayerAudioManager audioManager;
        public  PlayerManager pl;
        [SerializeField] private ParticleSystem _paricle;
        public Animator An; 
        public ScoreManager scoreManager;

        [SerializeField]private int[] enemyCost;//8 10 10 10 4 7

        public Items items;

        //--ANimations
        private int IdleAnimation = Animator.StringToHash("Player_Idle");
        private int DieAnimation = Animator.StringToHash("PieceDie");
        private int WalkAnimation = Animator.StringToHash("Player_Walk");
        private int RookWalkAnimation = Animator.StringToHash("Player_RookWalk");
        private int KnightWalkAnimation = Animator.StringToHash("Player_KnightWalk");
        //--ANimations




        public EnemyState CurrentState;
        public float DetectDist = 20;
        public void Die()
        {
            pl.OnWalk -= EnemyWalk;
            CurrentState = EnemyState.FUCKINGDEAD;
            switch (CurrentChessType)
            {
                case ChessPiece.King:
                    scoreManager.AddScore(enemyCost[0]);
                    break;
                case ChessPiece.Queen:
                    scoreManager.AddScore(enemyCost[1]);
                    break;
                case ChessPiece.Rook:
                    scoreManager.AddScore(enemyCost[2]);
                    break;
                case ChessPiece.Bishop:
                    scoreManager.AddScore(enemyCost[3]);
                    break;
                case ChessPiece.Pawn:
                    scoreManager.AddScore(enemyCost[4]);
                    break;
                case ChessPiece.Knight:
                    scoreManager.AddScore(enemyCost[5]);
                    break;
                default:
                    break;
            }
            An.Play(DieAnimation);

            Destroy(gameObject,2);
        }

        public enum EnemyState
        {
            Idle,
            AttackingPlayer,
            RandomMove,
            FUCKINGDEAD
        }

        public void SetInitPosition()
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.zero);
            if (hit.collider && hit.collider.gameObject.TryGetComponent<BoardTile>(out BoardTile tile) && !tile.IsHole)
            {
                Row = tile.Row;
                Column = tile.Collum;
            }
        }

        private void Start()
        {
            //Invoke(nameof(SetInitPosition), 1f); 
            pl.OnWalk += EnemyWalk;
            TurnInTo(CurrentChessType);
            An = transform.GetChild(0).GetComponent<Animator>();
        }

        public void ChangeState(EnemyState a)
        {
            CurrentState = a;
        }
        public void EnemyWalk()
        {

            if (items._isFreezed) return;

            switch (CurrentState)
            {
                case EnemyState.Idle:
                    break;
                case EnemyState.AttackingPlayer:
                    BoardTile walktile = GetDesiredWalkTile();
                    if (walktile == null)
                        return;
                    
                    MoveTo(walktile);

                    break;
                case EnemyState.RandomMove:
                    break;
                default:
                    break;
            }

            if (Vector2.Distance(pl.PlayerTile.transform.position, transform.position) < DetectDist)
                ChangeState(EnemyState.AttackingPlayer);




        }

        private BoardTile GetDesiredWalkTile()
        {
            if (transform == null)
                return null;
            Collider2D[] bts = Physics2D.OverlapCircleAll(transform.position, 25);

            BoardTile winner = null;
            float leastdist = 1323;
            foreach (var item in bts)
            {

                if (item.TryGetComponent(out BoardTile tile))
                {
                    if (tile.IsHole)
                        continue;
                    if (!CheckIfCanWalkThere(tile))
                    {
                        continue;
                    }
                    if (tile == pl.PlayerTile)
                    {
                        return tile;
                    }
                    if (Vector2.Distance(tile.transform.position, transform.position)< leastdist)
                    {
                        winner = tile;
                        leastdist = Vector2.Distance(tile.transform.position, transform.position);
                    }


                    
                }
            }

            return winner;
        }

        public bool CheckIfCanWalkThere(BoardTile tile)
        {
            switch (CurrentChessType)
            {
                case ChessPiece.King:
                    return TryWalkKing(Row, Column, tile.Row, tile.Collum);
                case ChessPiece.Queen:
                    return TryWalkQueen(Row, Column, tile.Row, tile.Collum);
                case ChessPiece.Rook:
                    return TryWalkRook(Row, Column, tile.Row, tile.Collum);
                case ChessPiece.Bishop:
                    return TryWalkBishop(Row, Column, tile.Row, tile.Collum);
                case ChessPiece.Pawn:
                    if (tile.gameObject == pl.PlayerTile.gameObject)
                    {
                        return TryWalkPawnEnemyKILL(Row, Column, tile.Row, tile.Collum);
                    }
                    else
                        return TryWalkPawnEnemy(Row, Column, tile.Row, tile.Collum);
                case ChessPiece.Knight:
                    return TryWalkKnight(Row, Column, tile.Row, tile.Collum);
                default:
                    break;
            }
            return false;
        }
        public static bool TryWalkPawnEnemy(int rowPos, int columnPos, int rowTo, int columnTo)
        {
            if (rowPos == rowTo && columnPos == columnTo)
                return false;
            bool IsCloseByX = columnPos == columnTo;
            bool IsCloseByY = (rowPos - 1 == rowTo) || (rowTo == rowPos);

            return (IsCloseByY && IsCloseByX);
        }
        public static bool TryWalkPawnEnemyKILL(int rowPos, int columnPos, int rowTo, int columnTo)
        {

            if (rowPos == rowTo && columnPos == columnTo)
                return false;
            bool IsCloseByX = (columnPos + 1 == columnTo) || (columnPos - 1 == columnTo);
            bool IsCloseByY = (rowPos - 1 == rowTo);

            return (IsCloseByY && IsCloseByX);
        }
        public void TurnInTo(ChessPiece piece)
        {
            CurrentChessType = piece;
            switch (CurrentChessType)
            {
                case ChessPiece.King:
                    transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Skins[1];

                    break;
                case ChessPiece.Queen:
                    transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Skins[5];

                    break;
                case ChessPiece.Rook:
                    transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Skins[3];
                    
                    break;
                case ChessPiece.Bishop:
                    transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Skins[2];

                    break;
                case ChessPiece.Pawn:
                    transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Skins[0];
                    break;
                case ChessPiece.Knight:
                    transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Skins[4];
 
                    break;
                default:
                    break;
            }

        }




        private bool CheckForHoles(BoardTile tile)
        {
            if (CurrentChessType == ChessPiece.Knight)
                return true;

            Vector2 origin = transform.position;
            Vector2 target = tile.transform.position;
            Vector2 direction = (target - origin).normalized;
            float distance = Vector2.Distance(origin, target);

            RaycastHit2D[] hits = Physics2D.RaycastAll(origin, direction, distance);

            foreach (var hit in hits)
            {
                if (hit.collider != null && hit.collider.gameObject != tile.gameObject
                                         && hit.collider.gameObject.TryGetComponent<BoardTile>(out var tileComponent) && tileComponent.IsHole)
                {
                    //Debug.Log("Cannot move through the hole!");
                    return false;
                }
            }

            return true;
        }
        public void MoveTo(BoardTile tile)
        {
            if (!CheckForHoles(tile))
                return;
            if (CurrentState == EnemyState.FUCKINGDEAD)
                return;
            switch (CurrentChessType)
            {
                case ChessPiece.King:
                    An.Play(WalkAnimation);
                    break;
                case ChessPiece.Queen:
                    An.Play(WalkAnimation);
                    break;
                case ChessPiece.Rook:
                    An.Play(RookWalkAnimation);
                    break;
                case ChessPiece.Bishop:
                    An.Play(WalkAnimation);
                    break;
                case ChessPiece.Pawn:
                    An.Play(WalkAnimation);
                    break;
                case ChessPiece.Knight:
                    An.Play(KnightWalkAnimation);
                    break;
                default:
                    break;
            }
            StartCoroutine(Walk(tile.transform.position, 3));

            if (!IsEverMoved) IsEverMoved = true;

            Row = tile.Row;
            Column = tile.Collum;

        }

        private IEnumerator Walk(Vector3 pos, float speed)
        {
            if (items._isFreezed) yield break;

            float time = 0;



            while (time < 1)
            {

                float t = InOutQuint(time);
                transform.position = Vector3.Lerp(transform.position, pos, t);
                time += Time.deltaTime * speed;
                yield return null;
                if (CurrentState == EnemyState.FUCKINGDEAD)
                    yield break;
            }
            transform.position = pos;

            audioManager.Walk();
            transform.GetChild(0).GetComponent<Animator>().Play(IdleAnimation);
            Instantiate(_paricle, transform.position, Quaternion.identity);
            if(Column== pl.Column && Row == pl.Row)
            {
                pl.GameOver();
            }
            pl.ShowSelect();
        }


    }
}