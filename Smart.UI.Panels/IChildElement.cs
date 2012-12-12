using Smart.UI.Panels;
using Smart.Classes.Subjects;

namespace Smart.UI.Panels
{
    /// <summary>
    /// ������������ ����� ���������� �� �����, ���� ��� ������������
    /// </summary>
    public interface IChildElement
    {
        SimpleSubject<SimplePanel> OnAddedToStage { get; set; }
        SimpleSubject<SimplePanel> OnRemovedFromStage { get; set; }
    }
}