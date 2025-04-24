namespace ReportMiniApp
{
    using System.Xml.Linq;
    public interface IReportWidget
    {
        string WidgetId { get; }
        void ConfigureFromXml(XElement config);
        void Render();
    }
}
