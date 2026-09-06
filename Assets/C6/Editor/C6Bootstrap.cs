using UnityEditor;

namespace C6.EditorTool
{
    [InitializeOnLoad]
    public static class C6Bootstrap
    {
        static C6Bootstrap()
        {
            C6Timer.Init();
            EditorApplication.update += C6Timer.Tick;
            C6Timer.OnStageChanged += C6Notifier.OnStageChanged;
        }
    }
}