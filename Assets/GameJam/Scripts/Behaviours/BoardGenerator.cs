using System;
using GameJam.Behaviours;
using GameJam.Managers;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;
using Random = UnityEngine.Random;

namespace GameJam.Board
{
    public class BoardGenerator : MonoBehaviour
    {
        [Inject] PlayerManager _playerManager;
        [SerializeField] private GameObject _boardTilePrefab;
        [SerializeField] private Transform _boardTilesParent;
        [SerializeField] private int _boardWidth;
        [SerializeField] private int _boardStartHeight;
        [SerializeField] private int _maxBoardHeightFromPlayer;
        [SerializeField] private int _rows;
        [SerializeField] private float _holeTileChance;

        private void Update()
        {

            if (_playerManager.IsEverMoved && _playerManager.PlayerTile.Row > _rows - _maxBoardHeightFromPlayer)
            {

                int needToAdd = _playerManager.PlayerTile.Row - (_rows - _maxBoardHeightFromPlayer);
                for (int i = 0; i < needToAdd; i++)
                {
                    GenerateNewRow();

                }
            }
        }

        public BoardTile[,] GenerateStartBoard()
        {
            BoardTile[,] boardTiles = new BoardTile[_boardWidth, _boardStartHeight];
            for (int i = 0; i < _boardStartHeight; i++)
            {
                for (int x = 0; x < _boardWidth; x++)
                {
                    Vector3 tileScale = _boardTilePrefab.transform.localScale;

                    GameObject tile = Instantiate(_boardTilePrefab,
                        new Vector2(x * tileScale.x - ((_boardWidth - 1) * tileScale.x) / 2, _rows * tileScale.y),
                        Quaternion.identity, _boardTilesParent);
                    if (_rows % 2 == 0)
                        tile.GetComponent<BoardTile>().Construct(x % 2 == 0, false);
                    else
                        tile.GetComponent<BoardTile>().Construct(x % 2 != 0, false);
                    tile.GetComponent<BoardTile>().Row = _rows;
                    tile.GetComponent<BoardTile>()._spriteRenderer.sortingOrder = -_rows;
                    tile.GetComponent<BoardTile>().Collum = x;
                    boardTiles[x, i] = tile.GetComponent<BoardTile>();
                }
                _rows++;
            }
            GenerateNewRow();
            GenerateNewRow();
            GenerateNewRow();
            return boardTiles;
        }


        void GenerateNewRow()
        {
            for (int x = 0; x < _boardWidth; x++)
            {
                Vector3 tileScale = _boardTilePrefab.transform.localScale;
                //Debug.Log(x);
                GameObject tile = Instantiate(_boardTilePrefab,
                    new Vector2(x * tileScale.x - ((_boardWidth-1) * tileScale.x)/2, _rows * tileScale.y),
                    Quaternion.identity, _boardTilesParent);

                if(_rows % 2 == 0)
                    tile.GetComponent<BoardTile>().Construct(x % 2 == 0, Random.value < _holeTileChance);
                else
                    tile.GetComponent<BoardTile>().Construct(x % 2 != 0, Random.value < _holeTileChance);

                tile.GetComponent<BoardTile>().Collum = x;

                tile.GetComponent<BoardTile>().Row = _rows;
                tile.GetComponent<BoardTile>()._spriteRenderer.sortingOrder = -_rows;
            }
            _rows++;
        }
    }
}