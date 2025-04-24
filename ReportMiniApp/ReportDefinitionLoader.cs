namespace ReportMiniApp
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Xml.Linq;

    public class ReportDefinitionLoader
    {
        private readonly HashSet<string> _ids = new();

        public List<IReportWidget> LoadFromXml(string xmlFilePath)
        {
            var doc = XDocument.Parse(File.ReadAllText(xmlFilePath));
            var widgets = new List<IReportWidget>();

            foreach (XElement widgetElement in doc?.Root?.Elements("Widget"))
            {
                var typeStr = widgetElement.Element("Type")?.Value;
                var id = widgetElement.Element("WidgetId")?.Value;
                var config = widgetElement.Element("Config");

                if (typeStr is null || id is null || config is null)
                    throw new InvalidOperationException("Invalid widget entry.");

                if (!_ids.Add(id))
                    throw new InvalidOperationException($"Duplicate WidgetId: {id}");

                var type = Type.GetType(typeStr, throwOnError: true);

                if (Activator.CreateInstance(type, id) is IReportWidget instance)
                {
                    instance.ConfigureFromXml(config);
                    widgets.Add(instance);
                }
            }

            return widgets;
        }
    }
}