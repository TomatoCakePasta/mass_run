using UnityEngine;
using UnityEngine.UI; // スライダーを使うために必要
using System.Collections;
using System.Collections.Generic;
using CriWare;

public class SoundManager : MonoBehaviour
{
    private CriAtomExPlayer atomExPlayer;

    private CriAtomExAcb atomExAcb;
    private CriAtomExAcb atomExAcb1;

    private CriAtomExPlayback currentPlayback;

    public Slider speedSlider; // ルームランナーの速度シミュレーション用

    public float minInterval = 0.1f; // 最高速度の時の発音間隔（秒）
    public float maxInterval = 1.0f; // 最低速度の時の発音間隔（秒）

    private float timer = 0f;
    private bool arp = false; // アルペジオ再生中かどうか
    private int count = 0;

    IEnumerator Start()
    {
        /* キューシートファイルのロード待ち */
        while (CriAtom.CueSheetsAreLoading)
        {
            yield return null;
        }

        /* AtomExPlayerの生成 */
        atomExPlayer = new CriAtomExPlayer();

        /* Cue情報の取得 */
        atomExAcb = CriAtom.GetAcb("CueSheet_0");

        atomExAcb1 = CriAtom.GetAcb("CueSheet_1");
    }

    void Update()
    {
        // ==========================================
        // フェーズ1：スライダーの速度に合わせてアルペジオを鳴らす
        // ==========================================

        if (count >= 24) atomExPlayer.SetCue(atomExAcb, "Arp2");
        else atomExPlayer.SetCue(atomExAcb, "Arp1");

        if (arp && speedSlider != null)
        {
            float speed = speedSlider.value; // スライダーの値 (0.0 〜 1.0)

            // 速度が少しでも出ている時だけ鳴らす
            if (speed > 0.01f)
            {
                // 速度に応じて発音間隔を計算 (Lerp: 速いほど間隔が短くなる)
                float currentInterval = Mathf.Lerp(maxInterval, minInterval, speed);

                timer += Time.deltaTime;
                if (timer >= currentInterval)
                {
                    atomExPlayer.Start(); // 単音を1回鳴らす
                    timer = 0f; // タイマーリセット
                    count++;
                    if (count >= 48) count = 0;
                }
            }
        }
    }

    // ==========================================
    // ボタン1用：「タイトル）」
    // ==========================================
    public void StartPhase1()
    {
        atomExPlayer.SetCue(atomExAcb, "drone");
        if (atomExPlayer.GetStatus() == CriAtomExPlayer.Status.Stop) atomExPlayer.Start();
    }

    // ==========================================
    // ボタン2用：「アルペジオ」
    // ==========================================
    public void StartPhase2()
    {
        if (atomExPlayer.GetStatus() == CriAtomExPlayer.Status.Playing) atomExPlayer.Stop();
        atomExPlayer.SetCue(atomExAcb1, "coin_insert");
        atomExPlayer.Start();

        arp = true;

    }

    // ==========================================
    // ボタン3用：「ループ」
    // ==========================================
    public void StartPhase3()
    {
        arp = false; // アルペジオを止める
        atomExPlayer.Stop();
        atomExPlayer.SetCue(atomExAcb, "all");
        currentPlayback = atomExPlayer.Start();
    }

    // ==========================================
    // ボタン4用：「エンディング」
    // ==========================================
    public void GoToEnding()
    {
        // 再生中のMainFlowに対して、「次はインデックス2(Ending)のブロックへ行ってね」と指示
        currentPlayback.SetNextBlockIndex(2);
    }

    // アプリ終了時にメモリを解放する（お作法として入れておきます）
    void OnDestroy()
    {
        if (atomExPlayer != null)
        {
            atomExPlayer.Dispose();
            atomExPlayer = null;
        }
    }
}