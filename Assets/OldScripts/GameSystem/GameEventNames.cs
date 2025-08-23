namespace GameSystem
{
    /// <summary>
    /// 游戏事件名称常量 - 使用类名+动作的命名规范
    /// </summary>
    public static class GameEventNames
    {
        #region Buff UI相关事件
        
        public const string BuffUI_DataUpdated = "BuffUI_DataUpdated";
        public const string BuffUI_ListUpdated = "BuffUI_ListUpdated";
        public const string BuffUI_ItemSelected = "BuffUI_ItemSelected";
        
        #endregion
        
        #region Player UI相关事件
        
        public const string PlayerUI_DataUpdated = "PlayerUI_DataUpdated";
        
        #endregion
        
        #region 原始Buff事件（如果需要更底层的控制）
        
        public const string BuffBase_Added = "BuffBase_Added";
        public const string BuffBase_Removed = "BuffBase_Removed";
        public const string BuffBase_StateChanged = "BuffBase_StateChanged";
        
        #endregion
    }
}







