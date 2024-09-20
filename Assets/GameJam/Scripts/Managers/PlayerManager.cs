using System;
using GameJam.Behaviours;
using UnityEngine;
using Zenject;

namespace GameJam.Managers
{
    public class PlayerManager : MonoBehaviour
    {
        [Inject] private Player _player;
        [field:SerializeField] public BoardTile PlayerTile { get; private set; }
        [field:SerializeField] public int Row { get; private set; }
        [field:SerializeField] public bool IsEverMoved { get; private set; }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
                if(hit.collider && hit.collider.gameObject.TryGetComponent<BoardTile>(out BoardTile boardTileComponent) && !boardTileComponent.IsHole)
                {
                    MoveTo(boardTileComponent);
                }
            }
        }

        public void MoveTo(BoardTile tile)
        {
            if(!IsEverMoved) IsEverMoved = true;
            _player.transform.position = tile.transform.position;
            PlayerTile = tile;
            Row = tile.Row;
        }
    }
}