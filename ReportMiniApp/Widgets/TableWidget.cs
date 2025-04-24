namespace ReportMiniApp
{
    using System;
    using System.Linq;
    using System.Xml.Linq;

    public class TableWidget : IReportWidget
    {
        public string WidgetId { get; }
        public string Title { get; private set; }
        public int Rows { get; private set; }
        public int Columns { get; private set; }
        public string DefaultValue { get; private set; }
        public string[,] Data { get; private set; }

        public TableWidget(string widgetId)
        {
            WidgetId = widgetId;
        }

        public void ConfigureFromXml(XElement config)
        {
            Title = config.Element("Title")?.Value ?? "Untitled Table";

            if (!int.TryParse(config.Element("Rows")?.Value, out var rows))
                rows = 5;

            if (!int.TryParse(config.Element("Columns")?.Value, out var columns))
                columns = 3;

            Rows = rows;
            Columns = columns;
            DefaultValue = config.Element("DefaultValue")?.Value ?? string.Empty;
            Data = new string[Rows, Columns];

            var cellsElement = config.Element("Cells");
            if (cellsElement != null)
            {
                var rowElements = cellsElement.Elements("Row");
                int r = 0;

                foreach (var row in rowElements)
                {
                    var cellElements = row.Elements("Cell").ToList();
                    for (int c = 0; c < cellElements.Count && c < Columns; c++)
                    {
                        if (r < Rows)
                        {
                            Data[r, c] = cellElements[c].Value;
                        }
                    }
                    r++;
                    if (r >= Rows) break;
                }

                // Fill remaining empty cells with DefaultValue
                for (int i = 0; i < Rows; i++)
                {
                    for (int j = 0; j < Columns; j++)
                    {
                        if (string.IsNullOrEmpty(Data[i, j]))
                        {
                            Data[i, j] = DefaultValue;
                        }
                    }
                }
            }
            else
            {
                // No <Cells> provided — use default for entire table
                for (int r = 0; r < Rows; r++)
                    for (int c = 0; c < Columns; c++)
                        Data[r, c] = DefaultValue;
            }
        }

        public void Render()
        {
            Console.WriteLine($"[TableWidget: {Title}] Initialized {Rows}x{Columns} cells.");
        }
    }
}
