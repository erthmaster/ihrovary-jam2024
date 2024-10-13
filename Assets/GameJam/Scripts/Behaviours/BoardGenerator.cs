using System;
using System.Collections;
using System.Collections.Generic;
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
        [SerializeField] private float _EnemyTileChance;
        [SerializeField] private EnemyAI Enemy;
        [Inject] GameManager _Manager;
        [Inject] PlayerAudioManager Audiomanager;
        [Inject] PlayerManager playerManager;
        [Inject] private Items _itemsEffects;
        public List<BoardTile> tiles = new List<BoardTile>();
        public Item[] items;
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
        public IEnumerator ResetAllBoard()
        {
            for (int i = 0; i < tiles.Count; i++)
            {
                tiles[i].Fade();
            }
            yield return new WaitForSeconds(1f);
            GenerateStartBoard();
        }

        public BoardTile[,] GenerateStartBoard()
        {
            BoardTile[,] boardTiles = new BoardTile[_boardWidth, _boardStartHeight];
            for (int i = 0; i < _boardStartHeight; i++)
            {
                for (int x = 0; x < _boardWidth; x++)
                {
                    Vector3 tileScale = _boardTilePrefab.transform.localScale;
                    BoardTile tile = _Manager.TilePool.Get();
                    tile.transform.SetPositionAndRotation(new Vector2(x * tileScale.x - ((_boardWidth - 1) * tileScale.x) / 2, _rows * tileScale.y),
                        Quaternion.identity);
                    tile.transform.SetParent(_boardTilesParent);
                    
                    if (_rows % 2 == 0)
                        tile.Construct(x % 2 == 0, false);
                    else
                        tile.Construct(x % 2 != 0, false);
                    tile.Row = _rows;
                    tile._spriteRenderer.sortingOrder = -_rows;
                    tile.Collum = x;
                    tile._Manager=_Manager;
                    tile.gen=this;
                    boardTiles[x, i] = tile;
                    tiles.Add(tile);
                }
                _rows++;
            }
            GenerateNewRow();
            GenerateNewRow();
            GenerateNewRow();
            //_playerManager.Invoke(nameof(_playerManager.SetInitPosition), 1f);

            return boardTiles;
        }



        void GenerateNewRow()
        {
            for (int x = 0; x < _boardWidth; x++)
            {
                Vector3 tileScale = _boardTilePrefab.transform.localScale;
                //Debug.Log(x);
                BoardTile tile = _Manager.TilePool.Get();
                tile.transform.SetPositionAndRotation(new Vector2(x * tileScale.x - ((_boardWidth - 1) * tileScale.x) / 2, _rows * tileScale.y),
                    Quaternion.identity);
                tile.transform.SetParent(_boardTilesParent);
                    

                if (_rows % 2 == 0)
                    tile.Construct(x % 2 == 0, Random.value < _holeTileChance);
                else
                    tile.Construct(x % 2 != 0, Random.value < _holeTileChance);


                bool spawed = default;
                foreach (var item in items)
                {
                    if (tile.IsHole)
                        break;
                    if (item.TrySpawn()){
                        spawed = true;
                        Instantiate(item, tile.transform.position, Quaternion.identity);
                        break;
                    }
                    item._itemsEffects = _itemsEffects;
                    item.playerManager = playerManager;
                }
                if (!spawed)
                {
                    if (Random.value < _EnemyTileChance && !tile.IsHole)
                    {
                        var v = Instantiate(Enemy, tile.transform.position, Quaternion.identity);
                        v.CurrentChessType = (PlayerManager.ChessPiece)Random.Range(0, 6);
                        v.pl = _playerManager;
                        v.audioManager = Audiomanager;
                    }
                }


                tile.Collum = x;
                tile._Manager = _Manager;
                tile.Row = _rows;
                tile.gen = this;
                tile._spriteRenderer.sortingOrder = -_rows;
                tiles.Add(tile);
            }
            _rows++;
        }
    }
}