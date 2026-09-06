using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace C6.EditorTool
{
    public class C6Window : EditorWindow
    {
        public static C6Window Instance { get; private set; }

        Label _totalLabel;
        VisualElement _faceImage;
        Label _stageLabel;
        Label _timerLabel;

        [MenuItem("Tools/C6")]
        public static void Open()
        {
            var w = GetWindow<C6Window>("C6");
            w.minSize = new Vector2(220, 260);
            w.Show();
        }

        void OnEnable()
        {
            Instance = this;
            EditorApplication.update += Refresh;
        }

        void OnDisable()
        {
            EditorApplication.update -= Refresh;
            if (Instance == this) Instance = null;
        }

        void CreateGUI()
        {
            var uxml = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                "Assets/C6/Editor/UI/C6Window.uxml");
            uxml.CloneTree(rootVisualElement);

            var uss = AssetDatabase.LoadAssetAtPath<StyleSheet>(
                "Assets/C6/Editor/UI/C6Window.uss");
            rootVisualElement.styleSheets.Add(uss);

            _totalLabel = rootVisualElement.Q<Label>("total-label");
            _faceImage = rootVisualElement.Q<VisualElement>("face-image");
            _stageLabel = rootVisualElement.Q<Label>("stage-label");
            _timerLabel = rootVisualElement.Q<Label>("timer-label");

            var recoverBtn = rootVisualElement.Q<Button>("recover-button");
            recoverBtn.clicked += C6Timer.Recover;

            Refresh();
        }
        void Refresh()
        {
            if (_faceImage == null) return;

            var tex = C6Art.Get(C6Timer.StageIndex);
            if (tex != null)
                _faceImage.style.backgroundImage = new StyleBackground(tex);
            _stageLabel.text = $"{C6Timer.StageIndex} / 4 단계";

            int tm = Mathf.FloorToInt(C6Timer.TotalSeconds / 60f);
            int ts = Mathf.FloorToInt(C6Timer.TotalSeconds % 60f);
            _totalLabel.text = $"작업 {tm:00}:{ts:00}";

            if (C6Timer.StageIndex >= 4)
            {
                _timerLabel.text = "당신을 항상 지지하는 C6을 기억해주세요!";
                _timerLabel.AddToClassList("urgent");
            }
            else
            {
                float remain = Mathf.Max(0f, C6Timer.StageIntervalSeconds - C6Timer.ElapsedSeconds);
                int m = Mathf.FloorToInt(remain / 60f);
                int s = Mathf.FloorToInt(remain % 60f);
                _timerLabel.text = $"다음 단계까지 {m:00}:{s:00}";
                _timerLabel.RemoveFromClassList("urgent");
            }
        }
    }
}