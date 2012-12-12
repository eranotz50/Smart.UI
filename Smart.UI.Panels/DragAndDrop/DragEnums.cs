namespace Smart.UI.Panels
{

    #region ENUMS

    /*
    /// <summary>
    /// Определяет драгит ли себя, перента или еще кого-то
    /// </summary>
    public enum DragTarget
    {
        Self,
        Parent,
    }
    
    /// <summary>
    /// Определяет драгится ли элемент на ближайшем драгпенелродителе или поднимается на самый верх
    /// </summary>
    public enum DragCanvas
    {
        //Default,
        NearestParent,
        HighestPanel,

    }
  
*/

    public enum DockMode
    {
        Default,
        NoDock,
        DockOnFreeSpace,
        DockEverywhere
    }

    /// <summary>
    /// Определяет режим драгинга: свободны, горизонтальный или вертикальный или скроллер
    /// </summary>
    public enum DragMode
    {
        Free,
        Horizontal,
        Vertical,
        None,
        Custom
    }

    #endregion
}