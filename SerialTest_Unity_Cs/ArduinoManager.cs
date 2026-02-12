using UnityEngine;
using System.IO.Ports;
using System;

public class ArduinoManager : MonoBehaviour
{
    // どこからでもアクセスできるようにする（シングルトン）
    public static ArduinoManager Instance;

    [Header("Arduino設定")]
    public string portName = "COM4"; // ※ポート番号を確認してください
    public int baudRate = 9600;

    // 自機が受け取るための変数
    public int  currentPotValue = 512; // 初期値（真ん中）
    public bool currentShootState = false;   // 初期値（離している）

    private SerialPort serialPort;

    void Awake()
    {
        // シーンが変わっても自分を壊さない設定
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // これが魔法の呪文です
            OpenConnection();
        }
        else
        {
            // すでに通信担当がいるなら、新しく作られた自分は消える
            Destroy(gameObject);
        }
    }

    void OpenConnection()
    {
        try
        {
            serialPort = new SerialPort(portName, baudRate);
            serialPort.ReadTimeout = 20; // 読み込み待ち時間を短く
            serialPort.Open();
            Debug.Log("Serial Port Opened");
        }
        catch (Exception e)
        {
            Debug.LogError("ポートが開けません: " + e.Message);
        }
    }

    void Update()
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            try
            {
                // データ読み取り
                string data = serialPort.ReadLine();

                if (data == "S") currentShootState = true;
                else
                {
                    currentPotValue = int.Parse(data);
                    currentShootState = false;
                }
            }
            catch (TimeoutException) { } // データがない時は無視
            catch (Exception e)
            {
                Debug.LogWarning("読み取りエラー: " + e.Message);
            }
        }
    }

    // ゲーム終了時のみポートを閉じる
    void OnApplicationQuit()
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
        }
    }
}