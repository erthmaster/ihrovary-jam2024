using System;
using GameJam.Behaviours;
using UnityEngine;
using PrimeTween;

public class BoardDestroyer : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.TryGetComponent<BoardTile>(out var boardTile))
        {
            boardTile.Delete();
        }
    }
}
