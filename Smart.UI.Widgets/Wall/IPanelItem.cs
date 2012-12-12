namespace Smart.UI.Widgets
{
    public enum ContentSize
    {
        Small, // небольшой размер
        Medium, // половина стенки (в горизонтальной половина высоты стенки, в вертикальной - половина ширины)
        Large, // на колонку/строку один контрол
        Auto, // автоматически высчитывать по переданной функции
        Custom // тупо брать размер контрола. 
    }

    public interface IPanelItem
    {
        ContentSize ContentSize { get; set; }
    }
}