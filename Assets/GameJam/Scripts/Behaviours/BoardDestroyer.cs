using System;
using GameJam.Behaviours;
using UnityEngine;

public class BoardDestroyer : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("sdsad");
        if (other.gameObject.TryGetComponent<BoardTile>(out var boardTile))
        {
            Destroy(boardTile.gameObject);
        }
    }
}
