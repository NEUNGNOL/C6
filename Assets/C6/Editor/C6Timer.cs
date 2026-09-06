using System;
using UnityEditor;
using UnityEngine;

namespace C6.EditorTool
{
    public static class C6Timer
    {
        const string kElapsedKey = "C6.ElapsedSeconds";
        const string kStageKey = "C6.StageIndex";
        const string kTotalKey = "C6.TotalSeconds";
        public static float TotalSeconds { get; private set; }
        const int kMaxStage = 4;

        // 테스트용: true면 간격이 5초가 됨. 완성되면 false로 변경 필요.
        const bool kTestMode = true;
        const float kTestIntervalSecs = 5f;

        static double _lastTick;
        static int _lastLoggedSecond = -1;

        public static float ElapsedSeconds { get; private set; }
        public static int StageIndex { get; private set; }

        public static event Action OnStageChanged;
        public static event Action OnRecovered;

        public static float StageIntervalSeconds =>
            kTestMode ? kTestIntervalSecs : C6Settings.StageIntervalMinutes * 60f;

        public static void Init()
        {
            ElapsedSeconds = SessionState.GetFloat(kElapsedKey, 0f);
            StageIndex = SessionState.GetInt(kStageKey, 0);
            TotalSeconds = SessionState.GetFloat(kTotalKey, 0f);
            _lastTick = EditorApplication.timeSinceStartup;
        }

        public static void Tick()
        {
            double now = EditorApplication.timeSinceStartup;

            // 최악 단계면 더 이상 누적 안 함 (버튼 누를 때까지 대기)
            if (StageIndex >= kMaxStage) { _lastTick = now; return; }

            float delta = (float)(now - _lastTick);
            _lastTick = now;

            ElapsedSeconds += delta;
            TotalSeconds += delta;
            SessionState.SetFloat(kElapsedKey, ElapsedSeconds);
            SessionState.SetFloat(kTotalKey, TotalSeconds);

            // 간격을 넘었으면 한 단계 악화
            if (ElapsedSeconds >= StageIntervalSeconds)
            {
                ElapsedSeconds -= StageIntervalSeconds;   // 넘친 시간은 이월
                StageIndex = Mathf.Min(StageIndex + 1, kMaxStage);

                SessionState.SetInt(kStageKey, StageIndex);
                SessionState.SetFloat(kElapsedKey, ElapsedSeconds);

                Debug.Log($"단계 악화! 현재 {StageIndex} / {kMaxStage} 단계");
                OnStageChanged?.Invoke();
            }

            int currentSecond = Mathf.FloorToInt(ElapsedSeconds);
            if (currentSecond != _lastLoggedSecond)
            {
                _lastLoggedSecond = currentSecond;
                Debug.Log($"경과 {currentSecond}초 (단계 {StageIndex})");
            }
        }

        // 나중에 스트레칭 완료 버튼이 호출할 함수
        public static void Recover()
        {
            StageIndex = 0;
            ElapsedSeconds = 0f;
            SessionState.SetInt(kStageKey, 0);
            SessionState.SetFloat(kElapsedKey, 0f);

            Debug.Log("회복! 0단계로 돌아감");
            OnRecovered?.Invoke();
        }
    }
}