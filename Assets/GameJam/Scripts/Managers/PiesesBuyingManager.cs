using GameJam.UI;
using UnityEngine;
using Zenject;

namespace GameJam.Managers
{

    public class PiesesBuyingManager : MonoBehaviour
    {
        [SerializeField] private BuyPieceButton _buyPawnButton;
        [SerializeField] private BuyPieceButton _buyKingButton;
        [SerializeField] private BuyPieceButton _buyRookButton;
        [SerializeField] private BuyPieceButton _buyBishopButton;
        [SerializeField] private BuyPieceButton _buyKnightButton;
        [SerializeField] private BuyPieceButton _buyQueenButton;

        [SerializeField] private Animator _buyKingButtonAnimator;
        [SerializeField] private Animator _buyRookButtonAnimator;
        [SerializeField] private Animator _buyBishopButtonAnimator;
        [SerializeField] private Animator _buyKnightButtonAnimator;
        [SerializeField] private Animator _buyQueenButtonAnimator;

        private bool boolKingButtonAnimator;
        private bool boolRookButtonAnimator;
        private bool boolBishopButtonAnimator;
        private bool boolKnightButtonAnimator;
        private bool boolQueenButtonAnimator;


        [Inject] private PlayerManager _playerManager;
        [SerializeField] int _queenPrice;

        [SerializeField] int _knightPrice;

        [SerializeField] int _bishopPrice;

        [SerializeField] int _rookPrice;

        [SerializeField] int _kingPrice;

        [SerializeField] int _pawnPrice;

        [Inject] ManaManager _manaManager;
        public void BuyQueen()
        {
            BuyPiece(_queenPrice, PlayerManager.ChessPiece.Queen);
            DisableAll();
            _buyQueenButtonAnimator.Play("Activating");
            boolQueenButtonAnimator = true;
        }
        public void BuyKnight()
        {
            BuyPiece(_knightPrice, PlayerManager.ChessPiece.Knight);
            DisableAll();
            _buyKnightButtonAnimator.Play("Activating");
            boolKnightButtonAnimator = true;
        }
        public void BuyBishop()
        {
            BuyPiece(_bishopPrice, PlayerManager.ChessPiece.Bishop);
            DisableAll();
            _buyBishopButtonAnimator.Play("Activating");
            boolBishopButtonAnimator = false;
        }
        public void BuyRook()
        { 
            BuyPiece(_rookPrice, PlayerManager.ChessPiece.Rook);
            DisableAll();
            _buyRookButtonAnimator.Play("Activating");
            boolRookButtonAnimator = false;
        }
        public void BuyKing() 
        {
            BuyPiece(_kingPrice, PlayerManager.ChessPiece.King);
            DisableAll();
            _buyKingButtonAnimator.Play("Activating");
            boolKingButtonAnimator = false;
        }
        public void BuyPawn() 
        { 
            BuyPiece(_pawnPrice, PlayerManager.ChessPiece.Pawn);
            DisableAll();
        }

        public void DisableAll()
        {
            if(!boolKingButtonAnimator)
            {
                boolKingButtonAnimator = true;
                _buyKingButtonAnimator.Play("Disable");
            }
                
            if (!boolRookButtonAnimator)
            {
                boolRookButtonAnimator = true;
                _buyRookButtonAnimator.Play("Disable");
            }
                
            if (!boolBishopButtonAnimator)
            {
                boolBishopButtonAnimator = true;
                _buyBishopButtonAnimator.Play("Disable");
            }
                
            if (!boolKnightButtonAnimator)
            {
                boolKnightButtonAnimator = true;
                _buyKnightButtonAnimator.Play("Disable");
            }
                
            if (!boolQueenButtonAnimator)
            {
                boolQueenButtonAnimator = true;
                _buyQueenButtonAnimator.Play("Disable");
            }
        }
        private void NotEnoughMana(PlayerManager.ChessPiece piece)
        {
            switch (piece)
            {
                case PlayerManager.ChessPiece.King:
                    _buyKingButtonAnimator.Play("Not Enough Mana");
                    break;
                case PlayerManager.ChessPiece.Queen:
                    _buyQueenButtonAnimator.Play("Not Enough Mana");
                    break;
                case PlayerManager.ChessPiece.Rook:
                    _buyRookButtonAnimator.Play("Not Enough Mana");
                    break;
                case PlayerManager.ChessPiece.Bishop:
                    _buyBishopButtonAnimator.Play("Not Enough Mana");
                    break;
                case PlayerManager.ChessPiece.Knight:
                    _buyKnightButtonAnimator.Play("Not Enough Mana");
                    break;
                default:
                    break;
            }
        }

        private void Start()
        {
            _buyPawnButton.Button.onClick.AddListener(() => BuyPawn());
            _buyKingButton.Button.onClick.AddListener(() => BuyKing());
            _buyRookButton.Button.onClick.AddListener(() => BuyRook());
            _buyBishopButton.Button.onClick.AddListener(() => BuyBishop());
            _buyKnightButton.Button.onClick.AddListener(() => BuyKnight());
            _buyQueenButton.Button.onClick.AddListener(() => BuyQueen());
            // _buyPawnButton.CostText.text = _pawnPrice.ToString();
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
                NotEnoughMana(piecetype);
                return;
            }

            _manaManager.mana -= price;

            _playerManager.TurnInTo(piecetype);
        }
    }
}