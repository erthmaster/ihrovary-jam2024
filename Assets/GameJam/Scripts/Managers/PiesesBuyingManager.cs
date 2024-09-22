using GameJam.Managers;
using GameJam.UI;
using UnityEngine;
using Zenject;

namespace GameJam
{

    public class PiesesBuyingManager : MonoBehaviour


    {

        [Inject] private PlayerManager _playerManager;
        [Inject] private PauseManager _pauseManager;
        [SerializeField] int _queenPrice;

        [SerializeField] int _knightPrice;

        [SerializeField] int _bishopPrice;

        [SerializeField] int _rookPrice;

        [SerializeField] int _kingPrice;

        [Inject] ManaManager _manaManager;
        public void BuyQueen() => BuyPiece(_queenPrice,PlayerManager.ChessPiece.Queen);
        public void BuyKight() => BuyPiece(_knightPrice,PlayerManager.ChessPiece.Knight);
        public void BuyBishop() => BuyPiece(_bishopPrice,PlayerManager.ChessPiece.Bishop);
        public void BuyRook() => BuyPiece(_rookPrice,PlayerManager.ChessPiece.Rook);
        public void BuyKing() => BuyPiece(_kingPrice,PlayerManager.ChessPiece.King) ;

        private void Update()
        {
            //if (_pauseManager.IsPaused) return;

            if (Input.GetKeyDown(KeyCode.Alpha1)) BuyKing();
            if (Input.GetKeyDown(KeyCode.Alpha2)) BuyRook();
            if (Input.GetKeyDown(KeyCode.Alpha3)) BuyBishop();
            if (Input.GetKeyDown(KeyCode.Alpha4)) BuyKight();
            if (Input.GetKeyDown(KeyCode.Alpha5)) BuyQueen();
        }
        private void BuyPiece(int price,PlayerManager.ChessPiece piecetype)
        {
            if (_playerManager.CurrentChessType == piecetype)
                return;
            if (_manaManager.mana < price)
            {
                Debug.Log("Not Enough Mana!");
                return;
            }

            _manaManager.mana -= price;
            //метод заміни

            _playerManager.TurnInTo(piecetype);
            _manaManager.mana -= price;

        }
    }
}