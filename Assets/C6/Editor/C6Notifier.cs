using UnityEngine;

namespace C6.EditorTool
{
    public static class C6Notifier
    {
        public static void OnStageChanged()
        {
            var window = C6Window.Instance;
            if (window == null) return;   // 창이 닫혀있으면 무시

            string message = C6Timer.StageIndex >= 4
                ? "💀 C6가 뻗었어요! 스트레칭 해주세요"
                : $"😣 C6가 힘들어합니다 ({C6Timer.StageIndex}/4)";

            window.ShowNotification(new GUIContent(message), 3.0);
            window.Repaint();
        }
    }
}