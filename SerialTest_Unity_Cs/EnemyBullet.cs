using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed = 5f;

    void Update()
    {
        transform.Translate(Vector2.down * speed * Time.deltaTime);

        if (transform.position.y < -6f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // ŠÄ“Â‚É•ñ
            FindObjectOfType<GameManager>().GameOver();

            Destroy(other.gameObject); // ©‹@‚ğÁ‚·
            Destroy(gameObject);       // ’e‚ğÁ‚·
        }
    }
}