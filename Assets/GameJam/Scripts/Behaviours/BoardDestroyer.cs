using System;
using GameJam.Behaviours;
using UnityEngine;
using PrimeTween;
using Zenject;
using GameJam.Managers;

public class BoardDestroyer : MonoBehaviour
{
    [Inject] BoardDestroyerManager manager;
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.TryGetComponent<BoardTile>(out var boardTile))
        {
            boardTile.Delete();
            manager.BreakTiles();
        }
    }
}
