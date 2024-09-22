using GameJam.Behaviours;
using GameJam.Managers;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using Zenject;
using static GameJam.Managers.PlayerManager;

namespace GameJam.Behaviours
{
    public class EnemyAI : MonoBehaviour
    {
        public PlayerManager.ChessPiece CurrentChessType = PlayerManager.ChessPiece.Pawn;

        [field: SerializeField] public int Row { get; private set; }
        [field: SerializeField] public int Column { get; private set; }
        [field: SerializeField] public Sprite[] Skins { get; private set; }
        [field: SerializeField] public bool IsEverMoved { get; private set; }
        public PlayerAudioManager audioManager;
        public  PlayerManager pl;
        [SerializeField] private ParticleSystem _paricle;

        public EnemyState CurrentState;
        public float DetectDist = 20;


        public enum EnemyState
        {
            Idle,
            AttackingPlayer,
            RandomMove
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
            Invoke(nameof(SetInitPosition), 0.3f);
            pl.OnWalk += EnemyWalk;
            TurnInTo(CurrentChessType);
        }

        public void ChangeState(EnemyState a)
        {
            CurrentState = a;
        }
        public void EnemyWalk()
        {


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
                    if(tile.Collum == pl.Column&&tile.Row == pl.Row)
                        return TryWalkPawnKill(Row, Column, tile.Row, tile.Collum);
                    return TryWalkPawn2(Row, Column, tile.Row, tile.Collum);
                case ChessPiece.Knight:
                    return TryWalkKnight(Row, Column, tile.Row, tile.Collum);
                default:
                    break;
            }
            return false;
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



        public void TryMoveTo(BoardTile tile)
        {
            switch (CurrentChessType)
            {
                case ChessPiece.King:
                    if (TryWalkKing(Row, Column, tile.Row, tile.Collum)) MoveTo(tile);
                    break;
                case ChessPiece.Queen:
                    if (TryWalkQueen(Row, Column, tile.Row, tile.Collum)) MoveTo(tile);
                    break;
                case ChessPiece.Rook:
                    if (TryWalkRook(Row, Column, tile.Row, tile.Collum)) MoveTo(tile);
                    break;
                case ChessPiece.Bishop:
                    if (TryWalkBishop(Row, Column, tile.Row, tile.Collum)) MoveTo(tile);
                    break;
                case ChessPiece.Pawn:
                    if (!IsEverMoved)
                    { if (TryWalkPawn2Inital(Row, Column, tile.Row, tile.Collum)) { MoveTo(tile); } }
                    else
                    { if (TryWalkPawn2(Row, Column, tile.Row, tile.Collum)) { MoveTo(tile); } }
                    break;
                case ChessPiece.Knight:
                    if (TryWalkKnight(Row, Column, tile.Row, tile.Collum)) MoveTo(tile);
                    break;
                default:
                    break;
            }
        }

        public void Die()
        {
            Destroy(gameObject);
        }
        public static bool TryWalkPawn2(int rowPos, int columnPos, int rowTo, int columnTo)
        {

            if (rowPos == rowTo && columnPos == columnTo)
                return false;
            bool IsCloseByX = columnPos == columnTo;
            bool IsCloseByY = (rowPos - 1 == rowTo) || (rowTo == rowPos);

            return (IsCloseByY && IsCloseByX);
        }
        public static bool TryWalkPawn2Inital(int rowPos, int columnPos, int rowTo, int columnTo)
        {

            if (rowPos == rowTo && columnPos == columnTo)
                return false;
            bool IsCloseByX = columnPos == columnTo;
            bool IsCloseByY = (rowPos - 1 == rowTo) || (rowPos - 2 == rowTo) || (rowTo == rowPos);

            return (IsCloseByY && IsCloseByX);
        }

        public static bool TryWalkPawnKill(int rowPos, int columnPos, int rowTo, int columnTo)
        {

            if (rowPos == rowTo && columnPos == columnTo)
                return false;
            bool IsCloseByX = (columnPos+1 == columnTo) || (columnPos - 1 == columnTo);
            bool IsCloseByY = (rowPos - 1 == rowTo) || (rowTo == rowPos);

            return (IsCloseByY && IsCloseByX);
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
            switch (CurrentChessType)
            {
                case ChessPiece.King:
                    transform.GetChild(0).GetComponent<Animator>().Play("Player_Walk");
                    break;
                case ChessPiece.Queen:
                    transform.GetChild(0).GetComponent<Animator>().Play("Player_Walk");
                    break;
                case ChessPiece.Rook:
                    transform.GetChild(0).GetComponent<Animator>().Play("Player_RookWalk");
                    break;
                case ChessPiece.Bishop:
                    transform.GetChild(0).GetComponent<Animator>().Play("Player_Walk");
                    break;
                case ChessPiece.Pawn:
                    transform.GetChild(0).GetComponent<Animator>().Play("Player_Walk");
                    break;
                case ChessPiece.Knight:
                    transform.GetChild(0).GetComponent<Animator>().Play("Player_KnightWalk");
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
            float time = 0;



            while (time < 1)
            {
                float t = InOutQuint(time);
                transform.position = Vector3.Lerp(transform.position, pos, t);
                time += Time.deltaTime * speed;
                yield return null;
            }
            transform.position = pos;

            audioManager.Walk();
            transform.GetChild(0).GetComponent<Animator>().Play("Player_Idle");
            Instantiate(_paricle, transform.position, Quaternion.identity);
            if(Column== pl.Column && Row == pl.Row)
            {
                pl.GameOver();
            }
        }


    }
}