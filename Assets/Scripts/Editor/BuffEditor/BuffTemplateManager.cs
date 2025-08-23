using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public static class BuffTemplateManager
{
    private const string TEMPLATE_PATH = "Assets/Scripts/Editor/BuffEditor/Templates/BuffClassTemplate.txt";
    
    public static string GenerateBuffClass(BuffConfig config)
    {
        string templateContent = LoadTemplate();
        if (string.IsNullOrEmpty(templateContent))
        {
            Debug.LogError($"无法加载模板文件: {TEMPLATE_PATH}");
            return "";
        }
        
        var replacements = CreateReplacementDictionary(config);
        return ReplaceTemplateVariables(templateContent, replacements);
    }
    
    private static string LoadTemplate()
    {
        if (!File.Exists(TEMPLATE_PATH))
        {
            CreateDefaultTemplate();
        }
        
        try
        {
            return File.ReadAllText(TEMPLATE_PATH);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"读取模板文件失败: {e.Message}");
            return "";
        }
    }
    
    private static Dictionary<string, string> CreateReplacementDictionary(BuffConfig config)
    {
        var data = config.buffDataConfig;
        
        // 添加调试信息
        string conflictIdsString = string.Join(", ", data.conflictBuffIds);
        UnityEngine.Debug.Log($"[BuffTemplate] 冲突BuffIDs: [{conflictIdsString}] (数量: {data.conflictBuffIds.Count})");
        
        return new Dictionary<string, string>
        {
            {"{CLASS_NAME}", config.buffClassName},
            {"{BUFF_TYPE_ID}", data.buffTypeId.ToString()},
            {"{MAX_LV}", data.maxLv.ToString()},
            {"{BUFF_LV_TYPE}", data.buffLvType.ToString()},
            {"{BUFF_TICK_TYPE}", data.buffTickType.ToString()},
            {"{BASE_DURATION}", data.baseDuration.ToString("F1")},
            {"{BASE_CD_TIME}", data.baseCDTime.ToString("F1")},
            {"{BASE_INTERVAL}", data.baseInterval.ToString("F1")},
            {"{CONFLICT_TYPE}", data.conflictType.ToString()},
            {"{CONFLICT_BUFF_IDS}", conflictIdsString}, // 使用处理过的字符串
            {"{BUFF_PRIORITY}", data.buffPriority.ToString()},
            {"{REDUCE_LV}", data.reduceLv.ToString()},
            {"{BUFF_NAME}", data.buffName.Replace("\"", "\\\"")},
            {"{BUFF_DESC}", data.buffDesc.Replace("\"", "\\\"").Replace("\n", "\\n")},
            {"{BUFF_ICON_PATH}", data.buffIconPath},
            {"{BUFF_EFFECT_PATH}", data.buffEffectPath}
        };
    }
    
    private static string ReplaceTemplateVariables(string template, Dictionary<string, string> replacements)
    {
        string result = template;
        foreach (var replacement in replacements)
        {
            result = result.Replace(replacement.Key, replacement.Value);
        }
        return result;
    }
    
    private static void CreateDefaultTemplate()
    {
        string directoryPath = Path.GetDirectoryName(TEMPLATE_PATH);
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
        
        // 如果模板文件不存在，会使用上面第3步的内容自动创建
        AssetDatabase.Refresh();
        Debug.Log($"已创建默认模板文件: {TEMPLATE_PATH}");
    }
}
