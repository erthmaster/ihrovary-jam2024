using GameJam.Behaviours;
using GameJam.Board;
using UnityEngine;
using Zenject;

namespace GameJam.Managers
{
    public class GameManager : MonoBehaviour
    {
        [Inject] private PlayerManager _playerManager;
        [Inject] private BoardGenerator _boardGenerator;

        void Awake()
        {
            BoardTile[,] boardTiles = _boardGenerator.GenerateStartBoard();
            _playerManager.MoveTo(boardTiles[3, 1]);
        }
    }
}