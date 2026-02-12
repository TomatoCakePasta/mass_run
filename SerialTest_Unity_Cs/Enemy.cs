using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 3f;

    void Update()
    {
        // 下へ移動
        transform.Translate(Vector2.down * speed * Time.deltaTime);

        // 画面下（Y=-6以下）に行ったら消す
        if (transform.position.y < -6f)
        {
            Destroy(gameObject);
        }
    }

    // 当たり判定
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 弾（Bulletタグがついているもの）に当たったら
        if (other.CompareTag("Bullet"))
        {
            Destroy(other.gameObject); // 弾を消す
            Destroy(gameObject);       // 自分（敵）を消す
        }
    }
}