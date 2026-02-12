using UnityEngine;
using TMPro; // TextMeshProを使うために必要
using UnityEngine.SceneManagement; // シーン読み込みに必要

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI messageText; // 画面の文字
    private bool isGameOver = false;
    bool shootValue;

    void Update()
    {
        shootValue = ArduinoManager.Instance.currentShootState;

        // ゲームオーバーかクリア時に「Rキー」でリスタート
        if (isGameOver && Input.GetKeyDown(KeyCode.R))SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        else if (isGameOver && shootValue)SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GameOver()
    {
        if (isGameOver) return; // 二重呼び出し防止

        isGameOver = true;
        messageText.text = "GAME OVER\n<size=30>Press 'R' to Restart</size>";
        messageText.color = Color.red;
    }

    public void GameClear()
    {
        if (isGameOver) return;

        isGameOver = true;
        messageText.text = "GAME CLEAR!\n<size=30>Press 'R' to Restart</size>";
        messageText.color = Color.yellow;
    }
}