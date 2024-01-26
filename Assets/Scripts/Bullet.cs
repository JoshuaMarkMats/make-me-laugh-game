using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : AutoDestroyPoolableObject
{
    public Vector2 direction = Vector2.zero;
    public float speed = 1f;
    public int damage = 3;

    private void FixedUpdate()
    {
        transform.position += (Vector3)direction * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerController>(out var player))
        {
            player.ChangeHealth(-damage);
        }
        gameObject.SetActive(false);
    }
}
