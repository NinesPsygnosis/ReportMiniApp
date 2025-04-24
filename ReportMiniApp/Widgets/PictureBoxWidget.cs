namespace ReportMiniApp
{
    using System;
    using System.IO;
    using System.Net.Http;
    using System.Xml.Linq;

    public class PictureBoxWidget : IReportWidget
    {
        public string WidgetId { get; }
        public string Title { get; private set; }
        public string ImagePath { get; private set; }
        public string Alignment { get; private set; }

        public PictureBoxWidget(string widgetId)
        {
            WidgetId = widgetId;
        }

        public void ConfigureFromXml(XElement config)
        {
            Title = GetElementValue(config, "Title", "Untitled Image");
            ImagePath = GetElementValue(config, "ImagePath", "");
            Alignment = GetElementValue(config, "Alignment", "Center");

            bool isRemote = ResolveImagePath(ImagePath, out string resolvedPath);

            if (isRemote)
            {
                FetchRemoteImage(resolvedPath);
            }
        }

        private string GetElementValue(XElement parent, string name, string defaultValue)
        {
            var element = parent.Element(name);
            return element != null ? element.Value : defaultValue;
        }

        private bool ResolveImagePath(string path, out string resolvedPath)
        {
            resolvedPath = path;

            Uri uri;
            if (Uri.TryCreate(path, UriKind.Absolute, out uri))
            {
                if (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps)
                {
                    return true;
                }
            }

            if (Path.IsPathRooted(path))
            {
                resolvedPath = path;
                return false;
            }

            resolvedPath = Path.Combine(Directory.GetCurrentDirectory(), path);
            return false;
        }

        private void FetchRemoteImage(string url)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var response = client.GetAsync(url).Result;
                }
            }
            catch (Exception ex)
            {
            }
            
            // [Truncated] - Cache image for use
        }

        public void Render()
        {
            Console.WriteLine($"[PictureBox: {Title}] Image={ImagePath}, Align={Alignment}");
        }
    }
}
