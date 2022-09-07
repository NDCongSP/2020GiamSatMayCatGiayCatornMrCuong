using EasyDriverPlugin;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MayCatServer
{
    public static class WriteTagExtensions
    {
        public static Quality Write(this ITagCore tagCore, string value)
        {
            try
            {
                if (tagCore != null && tagCore.Quality == Quality.Good)
                {
                    var driver = EasyProject.Instance.DriverManagerService.GetDriver(tagCore);
                    return driver.Write(tagCore, value);

                    //if (tagCore.Path.StartsWith("Local Station/Channel/Song") ||
                    //    tagCore.Path.StartsWith("Local Station/Channel/MayMen"))
                    //{
                    //    Task.Run(() =>
                    //    {
                    //        var driver = EasyProject.Instance.DriverManagerService.GetDriver(tagCore);
                    //        //return driver.Write(tagCore, value);
                    //    });
                    //    return Quality.Good;
                    //}
                    //else
                    //{
                    //    var driver = EasyProject.Instance.DriverManagerService.GetDriver(tagCore);
                    //    return driver.Write(tagCore, value);
                    //}
                }
                return Quality.Bad;
            }
            catch { return Quality.Bad; }
        }

        public static List<Quality> WriteMultiTag(List<WriteCommand> writeCommands, int soLanThuToiDa = 1)
        {
            Stopwatch sw = new Stopwatch();
            sw.Restart();
            List<Quality> result = new List<Quality>();
            if (writeCommands.Count > 0)
            {
                for (int i = 0; i < soLanThuToiDa; i++)
                {
                    int count = 0;
                    foreach (var item in writeCommands)
                    {
                        var res = WriteTagExtensions.Write(item.PathToTag, item.Value);
                        if (res == Quality.Good)
                            count++;
                        result.Add(res);

                    }
                    if (count == writeCommands.Count)
                        break;
                    else
                        continue;
                }
            }
            sw.Stop();
            Debug.WriteLine($"Write {writeCommands.Count} item take time: {sw.ElapsedMilliseconds}");
            return result;
        }

        public static Quality Write(string path, string value)
        {
            if (cache.ContainsKey(path))
            {
                if (cache[path] is ITagCore tag)
                    return tag.Write(value);
            }
            return Quality.Bad;
        }

        public static Hashtable cache = new Hashtable();
    }
}
