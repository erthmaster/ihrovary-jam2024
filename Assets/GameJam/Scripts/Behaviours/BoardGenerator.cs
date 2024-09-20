using System;
using GameJam.Managers;
using GameJam.ScriptableObjects;
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
            Debug.Log(_playerManager);
            if (_playerManager.IsEverMoved && _playerManager.PlayerTile.Row > _rows - _maxBoardHeightFromPlayer)
            {
                int needToAdd = _playerManager.PlayerTile.Row - (_rows - _maxBoardHeightFromPlayer);
                for (int i = 0; i < needToAdd; i++)
                {
                    GenerateNewRow();
                }
            }
        }

        public void GenerateStartBoard()
        {
            for (int x = 0; x < _boardWidth; x++)
            {
                Vector3 tileScale = _boardTilePrefab.transform.localScale;
                
                GameObject tile = Instantiate(_boardTilePrefab,
                    new Vector2(x * tileScale.x - ((_boardWidth-1) * tileScale.x)/2, _rows * tileScale.y),
                    Quaternion.identity, _boardTilesParent);
                if(_rows % 2 == 0)
                    tile.AddComponent<BoardTile>().Construct(x % 2 == 0, false);
                else
                    tile.AddComponent<BoardTile>().Construct(x % 2 != 0, false);
                tile.GetComponent<BoardTile>().Row = _rows;
            }
            _rows++;
        }

        void GenerateNewRow()
        {
            for (int x = 0; x < _boardWidth; x++)
            {
                Vector3 tileScale = _boardTilePrefab.transform.localScale;
                
                GameObject tile = Instantiate(_boardTilePrefab,
                    new Vector2(x * tileScale.x - ((_boardWidth-1) * tileScale.x)/2, _rows * tileScale.y),
                    Quaternion.identity, _boardTilesParent);
                if(_rows % 2 == 0)
                    tile.AddComponent<BoardTile>().Construct(x % 2 == 0, Random.value < _holeTileChance);
                else
                    tile.AddComponent<BoardTile>().Construct(x % 2 != 0, Random.value < _holeTileChance);
                tile.GetComponent<BoardTile>().Row = _rows;
            }
            _rows++;
        }
    }
}