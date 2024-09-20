using GameJam.Behaviours;
using GameJam.Board;
using UnityEngine;
using Zenject;

namespace GameJam.Managers
{
    public class GameManager : MonoBehaviour
    {
        [Inject] private Player _player;
        [Inject] private BoardGenerator _boardGenerator;

        void Awake()
        {
            _boardGenerator.GenerateStartBoard();
        }
    }
}