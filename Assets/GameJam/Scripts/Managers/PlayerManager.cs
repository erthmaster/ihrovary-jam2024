using System;
using System.Collections;
using GameJam.Behaviours;
using UnityEngine;
using Zenject;
using static UnityEngine.ParticleSystem;

namespace GameJam.Managers
{
    public class PlayerManager : MonoBehaviour
    {
        [Inject] private Player _player;
        [Inject] private PlayerAudioManager audioManager;
        public ChessPiece CurrentChessType = ChessPiece.Pawn;
        public enum ChessPiece
        {
            King,
            Queen,
            Rook,
            Bishop,
            Pawn,
            Knight,

        }
        [field:SerializeField] public BoardTile PlayerTile { get; private set; }
        [field:SerializeField] public int Row { get; private set; }
        [field:SerializeField] public int Column { get; private set; }
        [field:SerializeField] public bool IsEverMoved { get; private set; }
        [field:SerializeField] public Sprite[] Skins { get; private set; }
        
        [SerializeField] private ParticleSystem _paricle;
        [SerializeField] private GameObject gameOverObj;
        private void Start()
        {
            RaycastHit2D hit = Physics2D.Raycast(_player.transform.position, Vector2.zero);
            if (hit.collider && hit.collider.gameObject.TryGetComponent<BoardTile>(out BoardTile tile) && !tile.IsHole)
            {
                if (!IsEverMoved) IsEverMoved = true;
                PlayerTile = tile;
                Row = tile.Row;
                Column = tile.Collum;
            }
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
                if(hit.collider && hit.collider.gameObject.TryGetComponent<BoardTile>(out BoardTile boardTileComponent) && !boardTileComponent.IsHole)
                {
                    TryMoveTo(boardTileComponent);
                }
            }

            if (PlayerTile == null)
            {
                GameOver();
            }
        }

        void GameOver()
        {
            gameOverObj.SetActive(true);
        }
        
        public void TurnInTo(ChessPiece piece)
        {
            Debug.Log($" TurnedInTo: {piece} Was: {CurrentChessType}");
            CurrentChessType = piece;
            switch (CurrentChessType)
            {
                case ChessPiece.King:
                    _player.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Skins[1];
                    break;
                case ChessPiece.Queen:
                    _player.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Skins[5];
                    break;
                case ChessPiece.Rook:
                    _player.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Skins[3];
                    break;
                case ChessPiece.Bishop:
                    _player.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Skins[2];
                    break;
                case ChessPiece.Pawn:
                    _player.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Skins[0];
                    break;
                case ChessPiece.Knight:
                    _player.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Skins[4];
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
                    if (TryWalkPawn(Row, Column, tile.Row, tile.Collum)) MoveTo(tile);
                    break;
                case ChessPiece.Knight:
                    if (TryWalkKnight(Row, Column, tile.Row, tile.Collum)) MoveTo(tile);
                    break;
                default:
                    break;
            }
        }
        public void MoveTo(BoardTile tile)
        {
            if (CurrentChessType != ChessPiece.Knight && CheckForHoles(tile))
                return;
            switch (CurrentChessType)
            {
                case ChessPiece.King:
                    _player.transform.GetChild(0).GetComponent<Animator>().Play("Player_Walk");
                    break;
                case ChessPiece.Queen:
                    _player.transform.GetChild(0).GetComponent<Animator>().Play("Player_RookWalk");
                    break;
                case ChessPiece.Rook:
                    _player.transform.GetChild(0).GetComponent<Animator>().Play("Player_RookWalk");
                    break;
                case ChessPiece.Bishop:
                    _player.transform.GetChild(0).GetComponent<Animator>().Play("Player_RookWalk");
                    break;
                case ChessPiece.Pawn:
                    _player.transform.GetChild(0).GetComponent<Animator>().Play("Player_Walk");
                    break;
                case ChessPiece.Knight:
                    _player.transform.GetChild(0).GetComponent<Animator>().Play("Player_KnightWalk");
                    break;
                default:
                    break;
            }
            StartCoroutine( Walk(tile.transform.position, 5));

            if (!IsEverMoved) IsEverMoved = true;
            PlayerTile = tile;
            Row = tile.Row;
            Column = tile.Collum;
        }
        
        private bool CheckForHoles(BoardTile tile)
        {
            Vector2 direction = (tile.transform.position - PlayerTile.transform.position).normalized;
            float distance = Vector2.Distance(PlayerTile.transform.position, tile.transform.position);
            RaycastHit2D hit = Physics2D.Raycast(tile.transform.position, direction, distance);
            
            print(distance + " : " + direction);
            
            if (hit.collider //&& hit.collider.transform.position == new Vector3(hit.point.x, hit.point.y, 0)
                             && hit.collider.gameObject.TryGetComponent<BoardTile>(out var tileComponent) && tileComponent.IsHole)
            {
                Debug.Log("Cannot move through the hole!");
                return false;
            }
            Debug.Log("yep");
            return true;
        }

        private IEnumerator Walk(Vector3 pos,float speed)
        {
            float time = 0;


            while (time < 1)
            {
                float t = InOutQuint(time);
                _player.transform.position = Vector3.Lerp(_player.transform.position, pos, t);
                time += Time.deltaTime * speed;
                yield return null;
            }
            transform.position = pos;
            _player.transform.GetChild(0).GetComponent<Animator>().Play("Player_Idle");
        }

        public static float GptEase(float t)
        {
            return 0.5f - 0.5f * Mathf.Cos(t * Mathf.PI);
        }
        public static float InOutQuint(float t)
        {
            if (t < 0.5) return InQuint(t * 2) / 2;
            return 1 - InQuint((1 - t) * 2) / 2;
        } //coolest one
        public static float InQuint(float t) => t * t * t * t * t;


        public static bool TryWalkKing(int rowpos, int collumpos,int rowTo,int collumTo)
        {

            bool IsCloseByX = (collumpos - 1 == collumTo )|| (collumpos + 1 == collumTo) || collumpos == collumTo;
            bool IsCloseByY = (rowpos - 1 == rowTo) || (rowpos + 1 == rowTo) || (rowTo == rowpos);
            return ( IsCloseByX && IsCloseByY );
        }
        public static bool TryWalkPawn(int rowPos, int columnPos, int rowTo, int columnTo)
        {

            bool IsCloseByX = columnPos == columnTo;
            bool IsCloseByY = (rowPos + 1 == rowTo) || (rowTo == rowPos);
            return (IsCloseByY&&IsCloseByX);
        }
        public static bool TryWalkRook(int rowpos, int collumpos, int rowTo, int collumTo)
        {

            bool IsSameCollum = collumpos==collumTo;
            bool IsSameRow = rowTo == rowpos;
            if (IsSameCollum == true && IsSameRow == true)
                return false;
            return IsSameCollum || IsSameRow;
        }
        public static bool TryWalkBishop(int rowpos, int collumpos, int rowTo, int collumTo)
        {
            bool isonthesamedigonal = Mathf.Abs(collumpos - collumTo) == Mathf.Abs(rowpos - rowTo);
            return isonthesamedigonal;

        }
        public static bool TryWalkKnight(int rowpos, int collumpos, int rowTo, int collumTo)
        {

            bool isinbottomright = (rowpos - 2 == rowTo && collumpos + 1 == collumTo)|| (rowpos - 1 == rowTo && collumpos + 2 == collumTo);
            bool isinupperleft = (rowpos +2 == rowTo && collumpos - 1 == collumTo)|| (rowpos + 1 == rowTo && collumpos - 2 == collumTo);
            bool isinbottomleft = (rowpos -2 == rowTo && collumpos - 1 == collumTo)|| (rowpos - 1 == rowTo && collumpos - 2 == collumTo);
            bool isinupperright = (rowpos +2 == rowTo && collumpos + 1 == collumTo)|| (rowpos + 1 == rowTo && collumpos + 2 == collumTo);

            return isinupperleft||isinbottomright||isinbottomleft||isinupperright;
        }
        public static bool TryWalkQueen(int rowpos, int collumpos, int rowTo, int collumTo)
        {


            return  TryWalkBishop(rowpos, collumpos, rowTo, collumTo) || TryWalkRook(rowpos,collumpos,rowTo,collumTo);
        }
    }
}