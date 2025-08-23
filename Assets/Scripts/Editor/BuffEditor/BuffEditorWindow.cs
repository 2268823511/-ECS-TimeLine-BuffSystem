using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class BuffEditorWindow : EditorWindow
{
    private Vector2 scrollPosition;
    private BuffConfig currentBuffConfig;
    private List<BuffConfig> allBuffConfigs = new List<BuffConfig>();
    
    [MenuItem("Tools/Buff Editor")]
    public static void ShowWindow()
    {
        BuffEditorWindow window = GetWindow<BuffEditorWindow>("Buff编辑器");
        window.minSize = new Vector2(600, 400);
    }

    private void OnEnable()
    {
        RefreshBuffList();
    }

    private void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        DrawBuffList();
        DrawBuffEditor();
        EditorGUILayout.EndHorizontal();
    }

    private void DrawBuffList()
    {
        EditorGUILayout.BeginVertical(GUILayout.Width(200));
        
        GUILayout.Label("Buff列表", EditorStyles.boldLabel);
        
        if (GUILayout.Button("创建新Buff"))
        {
            CreateNewBuff();
        }
        
        if (GUILayout.Button("刷新列表"))
        {
            RefreshBuffList();
        }
        
        EditorGUILayout.Space();
        
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        
        foreach (var config in allBuffConfigs)
        {
            if (config == null) continue;
            
            if (GUILayout.Button($"{config.buffClassName} (ID:{config.buffId})", 
                currentBuffConfig == config ? EditorStyles.miniButtonMid : EditorStyles.miniButton))
            {
                currentBuffConfig = config;
            }
        }
        
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
    }

    private void DrawBuffEditor()
    {
        EditorGUILayout.BeginVertical();
        
        if (currentBuffConfig == null)
        {
            GUILayout.Label("请选择一个Buff进行编辑", EditorStyles.centeredGreyMiniLabel);
            EditorGUILayout.EndVertical();
            return;
        }
        
        GUILayout.Label($"编辑Buff: {currentBuffConfig.buffClassName}", EditorStyles.boldLabel);
        
        EditorGUILayout.Space();
        
        // 显示Buff基本信息（只读）
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.IntField("Buff ID", currentBuffConfig.buffId);
        EditorGUILayout.TextField("类名", currentBuffConfig.buffClassName);
        EditorGUI.EndDisabledGroup();
        
        EditorGUILayout.Space();
        
        // 可编辑的BuffData配置
        GUILayout.Label("Buff数据配置", EditorStyles.boldLabel);
        
        SerializedObject serializedConfig = new SerializedObject(currentBuffConfig);
        SerializedProperty buffDataProperty = serializedConfig.FindProperty("buffDataConfig");
        
        EditorGUILayout.PropertyField(buffDataProperty, true);
        
        if (serializedConfig.ApplyModifiedProperties())
        {
            EditorUtility.SetDirty(currentBuffConfig);
        }
        
        EditorGUILayout.Space();
        
        // 操作按钮
        EditorGUILayout.BeginHorizontal();
        
        if (GUILayout.Button("生成Buff类代码"))
        {
            GenerateBuffClass(currentBuffConfig);
        }
        
        if (GUILayout.Button("删除Buff"))
        {
            if (EditorUtility.DisplayDialog("确认删除", $"确定要删除Buff {currentBuffConfig.buffClassName}吗？", "删除", "取消"))
            {
                DeleteBuff(currentBuffConfig);
            }
        }
        
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();
    }

    private void CreateNewBuff()
    {
        string path = "Assets/BuffConfigs";
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        
        int newId = GetNextBuffId();
        string configPath = $"{path}/Buff{newId}.asset";
        
        BuffConfig newConfig = ScriptableObject.CreateInstance<BuffConfig>();
        newConfig.buffId = newId;
        newConfig.buffClassName = $"Buff{newId}";
        newConfig.buffDataConfig.buffTypeId = newId;
        newConfig.buffDataConfig.buffName = $"Buff{newId}";
        
        AssetDatabase.CreateAsset(newConfig, configPath);
        AssetDatabase.SaveAssets();
        
        RefreshBuffList();
        currentBuffConfig = newConfig;
        
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = newConfig;
    }

    private void GenerateBuffClass(BuffConfig config)
    {
        string className = config.buffClassName;
        string generatedCode = BuffTemplateManager.GenerateBuffClass(config);
        
        if (string.IsNullOrEmpty(generatedCode))
        {
            EditorUtility.DisplayDialog("错误", "生成Buff类失败，请检查模板文件", "确定");
            return;
        }
        
        // 修改这里：去掉Generated子文件夹
        string directoryPath = "Assets/Scripts/Moudle/Buff";
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
        
        string filePath = $"{directoryPath}/{className}.cs";
        File.WriteAllText(filePath, generatedCode);
        
        AssetDatabase.Refresh();
        EditorUtility.DisplayDialog("成功", $"已生成Buff类: {className}.cs\n路径: {filePath}", "确定");
    }

    private void RefreshBuffList()
    {
        allBuffConfigs.Clear();
        string[] guids = AssetDatabase.FindAssets("t:BuffConfig");
        
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            BuffConfig config = AssetDatabase.LoadAssetAtPath<BuffConfig>(path);
            if (config != null)
            {
                allBuffConfigs.Add(config);
            }
        }
        
        allBuffConfigs.Sort((a, b) => a.buffId.CompareTo(b.buffId));
    }

    private int GetNextBuffId()
    {
        int maxId = 1000;
        foreach (var config in allBuffConfigs)
        {
            if (config != null && config.buffId >= maxId)
            {
                maxId = config.buffId + 1;
            }
        }
        return maxId;
    }

    private void DeleteBuff(BuffConfig config)
    {
        string assetPath = AssetDatabase.GetAssetPath(config);
        AssetDatabase.DeleteAsset(assetPath);
        
        // 修改这里：去掉Generated子文件夹
        string classPath = $"Assets/Scripts/Moudle/Buff/{config.buffClassName}.cs";
        if (File.Exists(classPath))
        {
            AssetDatabase.DeleteAsset(classPath);
        }
        
        RefreshBuffList();
        currentBuffConfig = null;
        AssetDatabase.Refresh();
    }
}
