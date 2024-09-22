using GameJam.Behaviours;
using GameJam.Board;
using UnityEngine;
using Zenject;
using UnityEngine.Pool;

namespace GameJam.Managers
{
    public class GameManager : MonoBehaviour
    {
        [Inject] private PlayerManager _playerManager;
        [Inject] private BoardGenerator _boardGenerator;
        public BoardTile Tile;
        public ObjectPool<BoardTile> TilePool;
        public GameState CurrentGameState = GameState.Playing;
        public enum GameState
        {
            Playing,
            Paused,
            GameOver,

        }


        void Awake()
        {
            TilePool = new ObjectPool<BoardTile>(() => { return Instantiate(Tile); },
            _Tile => { if (_Tile == null) return; _Tile.gameObject.SetActive(true); }, _tile => { if (_tile == null) return; _tile.gameObject.SetActive(false); }, _tile => { Destroy(_tile.gameObject); }, false, 50, 110);



            BoardTile[,] boardTiles = _boardGenerator.GenerateStartBoard();
            //_playerManager.MoveTo(boardTiles[3, 1]);
        }

    }
}