using System;
using System.Collections;
using GameJam.Behaviours;
using UnityEngine;
using Zenject;
using TMPro;
using System.Threading.Tasks;
using GameJam.UI;
using System.Linq;
using GameJam.Board;

namespace GameJam.Managers
{
    public class PlayerManager : MonoBehaviour
    {
        [Inject] private Player _player;
        [Inject] private PlayerAudioManager audioManager;
        [Inject] private PauseManager pauseManager;
        [Inject] private BoardGenerator _gen;
        [Inject] private GameManager ___;
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
        [field: SerializeField] public float MoveCoolDown { get; private set; }
        [field: SerializeField] public float MaxMoveCoolDown { get; private set; }
        [field: SerializeField] public BoardTile PlayerTile { get; private set; }
        [field: SerializeField] public int Row { get; private set; }
        [field: SerializeField] public int Column { get; private set; }
        [field: SerializeField] public bool IsEverMoved { get; set; }
        [field: SerializeField] public Sprite[] Skins { get; private set; }

        public bool Moving = false;
        private bool canMove = true;

        [SerializeField] private TMP_Text _currentMovesText;
        [SerializeField] private int[] _movesCount;
        private int currentMoves;

        [SerializeField] private Animator _turnIntoAnim;
        [SerializeField] private ParticleSystem _paricle;
        public event Action OnWalk;

        public LayerMask Mask;

        [SerializeField] private Animator _animCards;

        [Inject] ScoreManager scoreManager;




        //--ANimations
        private int IdleAnimation = Animator.StringToHash("Player_Idle");
        private int DieAnimation = Animator.StringToHash("PieceDie");
        private int WalkAnimation = Animator.StringToHash("Player_Walk");
        private int RookWalkAnimation = Animator.StringToHash("Player_RookWalk");
        private int KnightWalkAnimation = Animator.StringToHash("Player_KnightWalk");
        //--ANimations



        public void Deselect()
        {

            foreach (var tile in _gen.tiles)
            { if (tile != null) tile.DeSelect(); }
        }
        public void ShowSelect()
        {
            //Deselect();

            foreach (var tile in _gen.tiles)
            {
                if (!SelectCheckHoleAndStop(tile))
                    continue;

                if(CurrentChessType == ChessPiece.Knight)
                {
                    if (TryWalkKnight(Row, Column, tile.Row, tile.Collum))
                    {
                            tile.Select();
                    }
                }

                if (!CheckForHoles(tile))
                {
                    continue;
                }

                switch (CurrentChessType)
                {


                    case ChessPiece.King:
                        if (TryWalkKing(Row, Column, tile.Row, tile.Collum))
                        {
                            tile.Select();
                        }
                        break;
                    case ChessPiece.Queen:
                        if (TryWalkQueen(Row, Column, tile.Row, tile.Collum))
                        {
                            tile.Select();
                        }
                        break;
                    case ChessPiece.Rook:
                        if (TryWalkRook(Row, Column, tile.Row, tile.Collum))
                        {
                            tile.Select();
                        }
                        break;
                    case ChessPiece.Bishop:
                        if (TryWalkBishop(Row, Column, tile.Row, tile.Collum))
                        {
                            tile.Select();
                        }
                        break;
                    case ChessPiece.Pawn:
                        if (Physics2D.OverlapCircleAll(tile.transform.position, 0.5f).Any(n => n.GetComponent<EnemyAI>() != null))
                        {

                            if (TryWalkPawnKILL(Row, Column, tile.Row, tile.Collum)) {tile.Select(); }
                        }
                        if (!IsEverMoved && TryWalkPawnInital(Row, Column, tile.Row, tile.Collum))
                        {
                            var tile2 = _gen.tiles.FirstOrDefault((n) => n.Row == tile.Row - 1 && n.Collum == n.Collum);
                            if (tile2 != null)
                            {
                                if (Physics2D.OverlapCircleAll(tile2.transform.position, 1).Any(n => n.GetComponent<EnemyAI>() != null))
                                {
                                    return;
                                }
                            }


                            if ((Physics2D.OverlapCircleAll(tile.transform.position, 1).All(n => n.GetComponent<EnemyAI>() == null)))
                                tile.Select();
                        }
                        if (TryWalkPawn(Row, Column, tile.Row, tile.Collum))
                        {

                            if ((Physics2D.OverlapCircleAll(tile.transform.position, 1).All(n => n.GetComponent<EnemyAI>() == null)))
                                tile.Select();
                        }

                        break;

                    default:
                        break;
                }
            }
        }
        private bool SelectCheckHoleAndStop(BoardTile tile)//õóéíÿ  ïðàöþº)
        {
            return !tile.IsHole;
        }

        public void SetInitPosition()
        {

            RaycastHit2D hit = Physics2D.Raycast(_player.transform.position, Vector2.zero, 100, Mask);
            if (hit.collider && hit.collider.gameObject.TryGetComponent<BoardTile>(out BoardTile tile))
            {
                PlayerTile = tile;
                Row = tile.Row;
                Column = tile.Collum;
            }

            ShowSelect();
        }


        [SerializeField] private GameObject gameOverObj;


        private void Update()
        {
            if (Row == -1 || Column == -1)
                SetInitPosition();
            if (Input.GetMouseButtonDown(0) && MoveCoolDown >= MaxMoveCoolDown)
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D[] hits = Physics2D.RaycastAll(mousePosition, Vector2.zero);
                RaycastHit2D hit = default;
                hit = hits.FirstOrDefault((h) => { return h.collider.GetComponent<BoardTile>(); });
                if (hit.collider && hit.collider.gameObject.TryGetComponent<BoardTile>(out BoardTile boardTileComponent) && !boardTileComponent.IsHole)
                {
                    TryMoveTo(boardTileComponent);
                }
            }

            MoveCoolDown = Mathf.Clamp(MoveCoolDown + Time.deltaTime, 0, MaxMoveCoolDown);

        }
        public void UnGameOver()
        {
            Deselect();
            _player.transform.GetChild(0).GetComponent<Animator>().Play(IdleAnimation);
            if (PlayerTile != null)
                Instantiate(PlayerTile.WhiteBreak, _player.transform.position, Quaternion.identity);
            gameOverObj.SetActive(false);
            pauseManager.UNPAUSEONGAMEOVER();
            ___.CurrentGameState = GameManager.GameState.Playing;
            _player.transform.position = new Vector3(0, 6, 0);
        }
        public void GameOver()
        {

            Deselect();
            _player.transform.GetChild(0).GetComponent<Animator>().Play(DieAnimation);
            if (PlayerTile != null)
                Instantiate(PlayerTile.WhiteBreak, _player.transform.position, Quaternion.identity);

            scoreManager.ShowEndScore();
            gameOverObj.SetActive(true);
            pauseManager.PAUSEONGAMEOVER();
            ___.CurrentGameState = GameManager.GameState.GameOver;
        }
        async private Task TurnInAnimation()
        {
            Debug.Log("TurnInAnimation");
            Deselect();
            _turnIntoAnim.gameObject.SetActive(true);
            _turnIntoAnim.SetBool("Turn", true);
            await Task.Delay(1000);
        }
        IEnumerator WaitEndOfAnim()
        {
            Debug.Log("waitforofAnim");
            Deselect();
            yield return new WaitForSeconds(0.2f);
            _turnIntoAnim.SetBool("Turn", false);
            _turnIntoAnim.gameObject.SetActive(false);
        }
        private void ChangeAnim(string nameActiveAnim)//cards anim
        {
            _animCards.SetBool("Pawn", false);
            _animCards.SetBool("King", false);
            _animCards.SetBool("Rook", false);
            _animCards.SetBool("Bishop", false);
            _animCards.SetBool("Knight", false);
            _animCards.SetBool("Queen", false);

            _animCards.SetBool(nameActiveAnim, true);
        }
        async public void TurnInTo(ChessPiece piece)
        {

            Debug.Log($" TurnedInTo: {piece} Was: {CurrentChessType}");
            if (pauseManager.IsPaused)
                return;
            canMove = false;

            CurrentChessType = piece;
            switch (CurrentChessType)
            {
                case ChessPiece.King:
                    ChangeAnim("King");
                    break;
                case ChessPiece.Queen:
                    ChangeAnim("Queen");
                    break;
                case ChessPiece.Rook:
                    ChangeAnim("Rook");
                    break;
                case ChessPiece.Bishop:
                    ChangeAnim("Bishop");
                    break;
                case ChessPiece.Pawn:
                    ChangeAnim("Pawn");
                    break;
                case ChessPiece.Knight:
                    ChangeAnim("Knight");
                    break;
                default:
                    break;
            }
            await TurnInAnimation();

            StartCoroutine(WaitEndOfAnim());
            canMove = true;


            switch (CurrentChessType)
            {
                case ChessPiece.King:
                    _player.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Skins[1];
                    currentMoves = _movesCount[0];
                    break;
                case ChessPiece.Queen:
                    _player.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Skins[5];
                    currentMoves = _movesCount[4];
                    break;
                case ChessPiece.Rook:
                    _player.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Skins[3];
                    currentMoves = _movesCount[2];
                    break;
                case ChessPiece.Bishop:
                    _player.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Skins[2];
                    currentMoves = _movesCount[1];
                    break;
                case ChessPiece.Pawn:
                    _player.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Skins[0];
                    currentMoves = 0;
                    break;
                case ChessPiece.Knight:
                    _player.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Skins[4];
                    currentMoves = _movesCount[3];
                    break;
                default:
                    break;
            }
            if (CurrentChessType == ChessPiece.Pawn)
                IsEverMoved = false;
            if (___.CurrentGameState != GameManager.GameState.GameOver)
            {
                ShowMoves();
                ShowSelect();
            }

        }

        public void TryMoveTo(BoardTile tile)
        {
            int temp_row = Row;

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
                    if (Physics2D.OverlapCircleAll(tile.transform.position, 1).Any(n => n.GetComponent<EnemyAI>() != null))
                    {
                        if (TryWalkPawnKILL(Row, Column, tile.Row, tile.Collum)) { MoveTo(tile); }
                    }
                    if (!IsEverMoved && Physics2D.OverlapCircleAll(tile.transform.position, 1).All((n) => n.GetComponent<EnemyAI>() == null) && TryWalkPawnInital(Row, Column, tile.Row, tile.Collum))
                    {

                        var tile2 = _gen.tiles.FirstOrDefault((n) => n.Row == tile.Row - 1 && n.Collum == n.Collum);
                        if (tile2 != null)
                        {

                            if (Physics2D.OverlapCircleAll(tile2.transform.position, 1).Any(n => n.GetComponent<EnemyAI>() != null))
                            {
                                Debug.Log("FDSF");
                                return;
                            }

                        }


                        MoveTo(tile);
                    }

                    if (TryWalkPawn(Row, Column, tile.Row, tile.Collum) && Physics2D.OverlapCircleAll(tile.transform.position, 1).All((n) => n.GetComponent<EnemyAI>() == null)) { MoveTo(tile); }
                    break;
                case ChessPiece.Knight:
                    if (TryWalkKnight(Row, Column, tile.Row, tile.Collum)) MoveTo(tile);
                    break;
                default:
                    break;

            }
            if (Row > temp_row)
                scoreManager.AddScore(Row - temp_row);
        }


        public void MoveTo(BoardTile tile)
        {
            if (pauseManager.IsPaused)
                return;
            if (!canMove) return;

            if (!CheckForHoles(tile))
                return;
            Deselect();
            switch (CurrentChessType)
            {
                case ChessPiece.King:
                    _player.transform.GetChild(0).GetComponent<Animator>().Play(WalkAnimation);
                    break;
                case ChessPiece.Queen:
                    _player.transform.GetChild(0).GetComponent<Animator>().Play(WalkAnimation);
                    break;
                case ChessPiece.Rook:
                    _player.transform.GetChild(0).GetComponent<Animator>().Play(RookWalkAnimation);
                    break;
                case ChessPiece.Bishop:
                    _player.transform.GetChild(0).GetComponent<Animator>().Play(WalkAnimation);
                    break;
                case ChessPiece.Pawn:
                    _player.transform.GetChild(0).GetComponent<Animator>().Play(WalkAnimation);
                    break;
                case ChessPiece.Knight:
                    _player.transform.GetChild(0).GetComponent<Animator>().Play(KnightWalkAnimation);
                    break;
                default:
                    break;
            }

            Moving = true;

            StartCoroutine(Walk(tile.transform.position, 3));

            if (!IsEverMoved) IsEverMoved = true;
            PlayerTile = tile;
            Row = tile.Row;
            Column = tile.Collum;
            MoveCoolDown = 0;
        }

        private bool CheckForHoles(BoardTile tile)
        {
            if (CurrentChessType == ChessPiece.Knight)
                return true;
            if (PlayerTile == null)
                return false;
            Vector2 origin = PlayerTile.transform.position;
            Vector2 target = tile.transform.position;
            Vector2 direction = (target - origin).normalized;
            float distance = Vector2.Distance(origin, target);
            //Debug.Log(distance);


            RaycastHit2D[] hits = Physics2D.RaycastAll(origin, direction, distance);

            foreach (var hit in hits)
            {
                if (hit.collider != null && hit.collider.gameObject != tile.gameObject
                                         && hit.collider.gameObject.TryGetComponent<BoardTile>(out var tileComponent) && tileComponent.IsHole)
                {
                    Debug.Log("Cannot move through the hole!");
                    return false;
                }
            }

            return true;
        }

        private IEnumerator Walk(Vector3 pos, float speed)
        {
            Debug.Log("WALK");
            Deselect();
            float time = 0;
            if (pauseManager.IsPaused)
                yield break;
            if (CurrentChessType != ChessPiece.Pawn)
            {
                currentMoves--;
                ShowMoves();
            }

            while (time < 1)
            {
                float t = InOutQuint(time);
                _player.transform.position = Vector3.Lerp(_player.transform.position, pos, t);
                time += Time.deltaTime * speed;
                yield return null;
            }
            transform.position = pos;

            Moving = false;

            audioManager.Walk();
            _player.transform.GetChild(0).GetComponent<Animator>().Play("Player_Idle");
            Instantiate(_paricle, transform.position, Quaternion.identity);
            if (currentMoves <= 0 && CurrentChessType != ChessPiece.Pawn)
            {
                TurnInTo(ChessPiece.Pawn);
            }
            OnWalk?.Invoke();
            foreach (var v in Physics2D.OverlapCircleAll(transform.position, 1.2f))
            {
                if (v.TryGetComponent(out EnemyAI ai))
                {
                    ai.Die();
                }
            }

            ShowSelect();
        }
        private void ShowMoves()
        {
            char dot = '.';
            if (currentMoves <= 0)
                _currentMovesText.text = " ";
            else
                _currentMovesText.text = new string(dot, currentMoves);
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


        public static bool TryWalkKing(int rowpos, int collumpos, int rowTo, int collumTo)
        {
            if (rowpos == rowTo && collumpos == collumTo)
                return false;
            bool IsCloseByX = (collumpos - 1 == collumTo) || (collumpos + 1 == collumTo) || collumpos == collumTo;
            bool IsCloseByY = (rowpos - 1 == rowTo) || (rowpos + 1 == rowTo) || (rowTo == rowpos);
            return (IsCloseByX && IsCloseByY);
        }
        public static bool TryWalkPawn(int rowPos, int columnPos, int rowTo, int columnTo)
        {

            if (rowPos == rowTo && columnPos == columnTo)
                return false;
            bool IsCloseByX = columnPos == columnTo;
            bool IsCloseByY = (rowPos + 1 == rowTo) || (rowTo == rowPos);

            return (IsCloseByY && IsCloseByX);
        }
        public static bool TryWalkPawnInital(int rowpos, int collumpos, int rowTo, int collumTo)
        {
            if (rowpos == rowTo && collumpos == collumTo)
                return false;
            bool IsCloseByX = collumpos == collumTo;
            bool IsCloseByY = (rowpos + 1 == rowTo) || (rowTo == rowpos) || (rowpos + 2 == rowTo);
            return (IsCloseByY && IsCloseByX);
        }
        public static bool TryWalkRook(int rowpos, int collumpos, int rowTo, int collumTo)
        {
            if (rowpos == rowTo && collumpos == collumTo)
                return false;
            bool IsSameCollum = collumpos == collumTo;
            bool IsSameRow = rowTo == rowpos;
            if (IsSameCollum == true && IsSameRow == true)
                return false;
            return IsSameCollum || IsSameRow;
        }
        public static bool TryWalkPawnKILL(int rowPos, int columnPos, int rowTo, int columnTo)
        {

            if (rowPos == rowTo && columnPos == columnTo)
                return false;
            bool IsCloseByX = (columnPos + 1 == columnTo) || (columnPos - 1 == columnTo);
            bool IsCloseByY = (rowPos + 1 == rowTo);

            return (IsCloseByY && IsCloseByX);
        }
        public static bool TryWalkBishop(int rowpos, int collumpos, int rowTo, int collumTo)
        {
            if (rowpos == rowTo && collumpos == collumTo)
                return false;
            bool isonthesamedigonal = Mathf.Abs(collumpos - collumTo) == Mathf.Abs(rowpos - rowTo);
            return isonthesamedigonal;

        }
        public static bool TryWalkKnight(int rowpos, int collumpos, int rowTo, int collumTo)
        {
            if (rowpos == rowTo && collumpos == collumTo)
                return false;
            bool isinbottomright = (rowpos - 2 == rowTo && collumpos + 1 == collumTo) || (rowpos - 1 == rowTo && collumpos + 2 == collumTo);
            bool isinupperleft = (rowpos + 2 == rowTo && collumpos - 1 == collumTo) || (rowpos + 1 == rowTo && collumpos - 2 == collumTo);
            bool isinbottomleft = (rowpos - 2 == rowTo && collumpos - 1 == collumTo) || (rowpos - 1 == rowTo && collumpos - 2 == collumTo);
            bool isinupperright = (rowpos + 2 == rowTo && collumpos + 1 == collumTo) || (rowpos + 1 == rowTo && collumpos + 2 == collumTo);

            return isinupperleft || isinbottomright || isinbottomleft || isinupperright;
        }
        public static bool TryWalkQueen(int rowpos, int collumpos, int rowTo, int collumTo)
        {

            if (rowpos == rowTo && collumpos == collumTo)
                return false;
            return TryWalkBishop(rowpos, collumpos, rowTo, collumTo) || TryWalkRook(rowpos, collumpos, rowTo, collumTo);
        }
    }
}