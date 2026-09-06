using UnityEditor;
using UnityEngine;

namespace C6.EditorTool
{
    public static class C6Art
    {
        static Texture2D[] _faces;

        public static Texture2D Get(int stage)
        {
            Load();
            stage = Mathf.Clamp(stage, 0, 4);
            return _faces[stage];
        }

        static void Load()
        {
            if (_faces != null) return;   // 이미 불러왔으면 건너뜀

            _faces = new Texture2D[5];
            for (int i = 0; i < 5; i++)
            {
                _faces[i] = AssetDatabase.LoadAssetAtPath<Texture2D>(
                    $"Assets/C6/Editor/Art/c6_stage{i}.png");
            }
        }
    }
}