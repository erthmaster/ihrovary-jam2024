using GameJam.Behaviours;
using GameJam.Board;
using UnityEngine;
using Zenject;

namespace GameJam.Managers
{
    public class GameManager : MonoBehaviour
    {
        [field:SerializeField] public Sprite BlackSprite { get; private set; }
        [field:SerializeField] public Sprite WhiteSprite { get; private set; }
        
        [field:SerializeField] public Sprite BlackHoleSprite { get; private set; }
        [field:SerializeField] public Sprite WhiteHoleSprite { get; private set; }
        
        [Inject] private PlayerManager _playerManager;
        [Inject] private BoardGenerator _boardGenerator;

        void Awake()
        {
            BoardTile[,] boardTiles = _boardGenerator.GenerateStartBoard();
            _playerManager.MoveTo(boardTiles[3, 1]);
        }
    }
}