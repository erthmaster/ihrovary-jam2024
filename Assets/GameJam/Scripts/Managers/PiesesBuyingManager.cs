using System;
using GameJam.Managers;
using GameJam.UI;
using UnityEngine;
using Zenject;

namespace GameJam
{

    public class PiesesBuyingManager : MonoBehaviour
    {
        [SerializeField] private BuyPieceButton _buyPawnButton;
        [SerializeField] private BuyPieceButton _buyKingButton;
        [SerializeField] private BuyPieceButton _buyRookButton;
        [SerializeField] private BuyPieceButton _buyBishopButton;
        [SerializeField] private BuyPieceButton _buyKnightButton;
        [SerializeField] private BuyPieceButton _buyQueenButton;

        [Inject] private PlayerManager _playerManager;
        [SerializeField] int _queenPrice;

        [SerializeField] int _knightPrice;

        [SerializeField] int _bishopPrice;

        [SerializeField] int _rookPrice;

        [SerializeField] int _kingPrice;

        [SerializeField] int _pawnPrice;

        [Inject] ManaManager _manaManager;
        public void BuyQueen() => BuyPiece(_queenPrice, PlayerManager.ChessPiece.Queen);
        public void BuyKnight() => BuyPiece(_knightPrice, PlayerManager.ChessPiece.Knight);
        public void BuyBishop() => BuyPiece(_bishopPrice, PlayerManager.ChessPiece.Bishop);
        public void BuyRook() => BuyPiece(_rookPrice, PlayerManager.ChessPiece.Rook);
        public void BuyKing() => BuyPiece(_kingPrice, PlayerManager.ChessPiece.King);
        public void BuyPawn() => BuyPiece(_pawnPrice, PlayerManager.ChessPiece.Pawn);

        private void Start()
        {
            _buyPawnButton.Button.onClick.AddListener(() => BuyPawn());
            _buyKingButton.Button.onClick.AddListener(() => BuyKing());
            _buyRookButton.Button.onClick.AddListener(() => BuyRook());
            _buyBishopButton.Button.onClick.AddListener(() => BuyBishop());
            _buyKnightButton.Button.onClick.AddListener(() => BuyKnight());
            _buyQueenButton.Button.onClick.AddListener(() => BuyQueen());

            _buyPawnButton.CostText.text = _pawnPrice.ToString();
            _buyKingButton.CostText.text = _kingPrice.ToString();
            _buyRookButton.CostText.text = _rookPrice.ToString();
            _buyBishopButton.CostText.text = _bishopPrice.ToString();
            _buyKnightButton.CostText.text = _knightPrice.ToString();
        }
        private void BuyPiece(int price, PlayerManager.ChessPiece piecetype)
        {
            if (_playerManager.CurrentChessType == piecetype)
                return;
            if (_manaManager.mana < price)
            {
                Debug.Log("Not Enough Mana!");
                return;
            }

            _manaManager.mana -= price;

            _playerManager.TurnInTo(piecetype);
        }
    }
}