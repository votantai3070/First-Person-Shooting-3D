using UnityEditor;
using UnityEngine;

public class FixShaders : EditorWindow
{
    [MenuItem("Tools/Fix Weapon Shaders")]
    static void FixAllShaders()
    {
        // Lấy tất cả materials trong project
        string[] guids = AssetDatabase.FindAssets("t:_Material", new[] { "Assets/" });

        int fixedShader = 0;
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Material mat = AssetDatabase.LoadAssetAtPath<Material>(path);

            if (mat != null && mat.shader.name.Contains("Standard"))
            {
                // Đổi sang URP Lit shader
                mat.shader = Shader.Find("Universal Render Pipeline/Lit");
                EditorUtility.SetDirty(mat);
                fixedShader++;
            }
        }

        AssetDatabase.SaveAssets();
        Debug.Log($"Fixed {fixedShader} materials!");
    }
}
