using EasyDriverPlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MayCatServer
{
    public class LedTags
    {
        public ITagCore PhanTram { get; set; }
        public ITagCore CL { get; set; }
        public ITagCore MT { get; set; }
        public ITagCore Run_min { get; set; }
        public ITagCore Run_ss { get; set; }
        public ITagCore Stop_min { get; set; }
        public ITagCore Stop_ss { get; set; }
        public ITagCore TieuDe { get; set; }
        public ITagCore SoLanDung { get; set; }
        public ITagCore TocDoTB { get; set; }
        public ITagCore TocDoDon { get; set; }

        public static LedTags Instance { get; } = new LedTags();

        public string StationName { get; set; } = "Local Station";
        public string ChannelName { get; set; } = "Led";
        public string DeviceName { get; set; } = "Device";

        private ITagCore GetTag(string tagName)
        {
            if (EasyProject.Instance.Project.Browse(new string[] { StationName, ChannelName, DeviceName }, 0) is IHaveTag device)
            {
                return device.Tags.Find(tagName);
            }
            return null;
        }

        public void GetTags()
        {
            foreach (PropertyInfo propertyInfo in GetType().GetProperties())
            {
                if (propertyInfo.PropertyType == typeof(ITagCore))
                {
                    propertyInfo.SetValue(this, GetTag(propertyInfo.Name));
                }
            }
        }
    }
}
