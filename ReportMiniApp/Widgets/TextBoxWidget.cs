namespace ReportMiniApp
{
    using System;
    using System.Xml.Linq;

    public class TextBoxWidget : IReportWidget
    {
        public string WidgetId { get; }
        public string Title { get; private set; }
        public string InnerText { get; private set; }

        public TextBoxWidget(string widgetId)
        {
            WidgetId = widgetId;
        }

        public void ConfigureFromXml(XElement config)
        {
            Title = config.Element("Title")?.Value ?? "Untitled Text";
            InnerText = config.Element("InnerText")?.Value ?? "(empty)";
        }

        public void Render()
        {
            Console.WriteLine($"[TextBoxWidget: {Title}] {InnerText}");
        }
    }
}