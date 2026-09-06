using UnityEditor;
using UnityEngine;

namespace C6.EditorTool
{
    public static class C6Settings
    {
        const string kStageIntervalKey = "C6.StageIntervalMinutes";

        // 몇 분마다 한 단계씩 지칠지 (기본 15분)
        public static float StageIntervalMinutes
        {
            get => EditorPrefs.GetFloat(kStageIntervalKey, 15f);
            set => EditorPrefs.SetFloat(kStageIntervalKey, Mathf.Clamp(value, 5f, 60f));
        }
    }
}