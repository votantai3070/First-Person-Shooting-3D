// Script tự động setup realistic materials
using UnityEditor;
using UnityEngine;

public class SetupWeaponMaterials : EditorWindow
{
    [MenuItem("Tools/Setup Realistic Weapon Materials")]
    static void SetupMaterials()
    {
        string[] guids = AssetDatabase.FindAssets("t:Weapon", new[] { "Assets/_Prefab" });

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

            foreach (Renderer renderer in prefab.GetComponentsInChildren<Renderer>())
            {
                Material mat = renderer.sharedMaterial;
                if (mat != null)
                {
                    mat.shader = Shader.Find("Universal Render Pipeline/Lit");

                    // Metal parts
                    if (mat.name.Contains("Metal") || mat.name.Contains("Barrel"))
                    {
                        mat.SetColor("_BaseColor", new Color(0.235f, 0.235f, 0.235f)); // Dark gray
                        mat.SetFloat("_Metallic", 0.9f);
                        mat.SetFloat("_Smoothness", 0.6f);
                    }
                    // Plastic/Polymer parts
                    else if (mat.name.Contains("Plastic") || mat.name.Contains("Grip"))
                    {
                        mat.SetColor("_BaseColor", new Color(0.1f, 0.1f, 0.1f)); // Black
                        mat.SetFloat("_Metallic", 0.0f);
                        mat.SetFloat("_Smoothness", 0.35f);
                    }
                    // Wood parts
                    else if (mat.name.Contains("Wood"))
                    {
                        mat.SetColor("_BaseColor", new Color(0.361f, 0.227f, 0.129f)); // Brown
                        mat.SetFloat("_Metallic", 0.0f);
                        mat.SetFloat("_Smoothness", 0.25f);
                    }

                    EditorUtility.SetDirty(mat);
                }
            }
        }

        AssetDatabase.SaveAssets();
        Debug.Log("Weapon materials setup complete!");
    }
}
