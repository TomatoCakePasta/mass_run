using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public GameObject bulletPrefab; // 弾のプレハブを入れる枠

    void Update()
    {
        if (ArduinoManager.Instance == null) return;

        float x = 0;
        Vector3 pos = transform.position;

        int potValue = ArduinoManager.Instance.currentPotValue;
        bool shootValue = ArduinoManager.Instance.currentShootState;


        // 左右移動
        x = Input.GetAxisRaw("Horizontal");
        transform.Translate(Vector2.right * x * speed * Time.deltaTime);
        pos.x = Mathf.Clamp(pos.x, -8f, 8f);
        // 発射（スペースキー）
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        }
        if (shootValue) Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        else
        {
            x = Map(potValue, 0, 1023, -8f, 8f);
            pos.x = x;
            transform.position = pos;
        }
    }        

    float Map(float value, float start1, float stop1, float start2, float stop2)
    {
        return start2 + (stop2 - start2) * ((value - start1) / (stop1 - start1));
    }
}