using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;

    void Update()
    {
        // 上へ移動
        transform.Translate(Vector2.up * speed * Time.deltaTime);

        // 画面外（Y=6以上）に出たら消す（メモリ節約）
        if (transform.position.y > 6f)
        {
            Destroy(gameObject);
        }
    }
}