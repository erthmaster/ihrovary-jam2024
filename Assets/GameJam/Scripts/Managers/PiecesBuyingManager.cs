using GameJam.UI;
using UnityEngine;
using Zenject;

namespace GameJam
{
    public class PiecesBuyingManager : MonoBehaviour
    {
        [SerializeField] int _queenPrice;

        [SerializeField] int _knightPrice;

        [SerializeField] int _bishopPrice;

        [SerializeField] int _rookPrice;

        [SerializeField] int _kingPrice;

        [Inject] ManaManager _manaManager;
        public void BuyQueen() => BuyPiece(_queenPrice);
        public void BuyKight() => BuyPiece(_knightPrice);
        public void BuyBishop() => BuyPiece(_bishopPrice);
        public void BuyRook() => BuyPiece(_rookPrice);
        public void BuyKing() => BuyPiece(_kingPrice);

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) BuyKing();
            if (Input.GetKeyDown(KeyCode.Alpha2)) BuyRook();
            if (Input.GetKeyDown(KeyCode.Alpha3)) BuyBishop();
            if (Input.GetKeyDown(KeyCode.Alpha4)) BuyKight();
            if (Input.GetKeyDown(KeyCode.Alpha5)) BuyQueen();
        }
        private void BuyPiece(int price)
        {
            if (_manaManager.ManaAmount < price)
            {
                Debug.Log("Not Enough Mana!");
                return;
            }
            _manaManager.ManaAmount -= price;
            //����� �����
        }
    }
}