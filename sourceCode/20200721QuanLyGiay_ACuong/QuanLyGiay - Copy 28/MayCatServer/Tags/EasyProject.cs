using EasyDriver.Core;
using EasyDriverPlugin;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MayCatServer
{
    public class EasyProject
    {
        public static EasyProject Instance { get; } = new EasyProject();

        public IEasyScadaProject Project { get; set; }
        public DriverManagerService DriverManagerService { get; set; }

        public EasyProject()
        {
            DriverManagerService = new DriverManagerService();
            Project = ProjectManagerService.OpenProject("ServerProject.json");
            InitProject();
        }

        private void InitProject()
        {
            // Loop qua các đối tương con của project mới để xử lý
            foreach (var item in Project.Childs)
            {
                // Nếu đối tương là một LocalStation thì khới tạo các DriverPlugin tương ứng với các Channel có trong LocalStation
                if (item is LocalStation localStation)
                {
                    foreach (var i in localStation.Childs)
                    {
                        if (i is IChannelCore channel)
                        {
                            string driverPath = channel.DriverPath;
                            string driverName = Path.GetFileName(driverPath);
                            driverPath = $"{Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)}\\{driverName}";

                            // Thêm driver tương ứng với channel vào DriverManagerService
                            IEasyDriverPlugin driver = DriverManagerService.AddDriver(channel, driverPath);
                            if (driver != null)
                            {
                                // Khởi động driver bằng hàm connect
                                driver?.Connect();
                                var dataTypes = driver.GetSupportDataTypes().ToList();
                                foreach (var tag in channel.GetAllTags())
                                {
                                    tag.DataType = dataTypes.FirstOrDefault(x => x.Name == tag.DataTypeName);
                                    WriteTagExtensions.cache.Add(tag.Path, tag);
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    public class ProjectManagerService
    {
        public static IEasyScadaProject OpenProject(string path)
        {
            string resJson = File.ReadAllText(path);
            IEasyScadaProject project = JsonConvert.DeserializeObject<IEasyScadaProject>(resJson, new EasyScadaProjectJsonConverter());
            project.ProjectPath = path;
            project.Name = Path.GetFileNameWithoutExtension(path);
            project.CreatedDate = File.GetCreationTime(path);
            project.ModifiedDate = File.GetLastWriteTime(path);
            project.AcceptChanges();
            return project;
        }
    }

    public class DriverManagerService 
    {
        public DriverManagerService()
        {
            DriverPoll = new Dictionary<IChannelCore, IEasyDriverPlugin>();
        }

        public Dictionary<IChannelCore, IEasyDriverPlugin> DriverPoll { get; private set; }


        public IEasyDriverPlugin AddDriver(IChannelCore channel, IEasyDriverPlugin driver)
        {
            if (DriverPoll.ContainsKey(channel))
                return DriverPoll[channel];
            else
            {
                DriverPoll[channel] = driver;
            }
            return driver;
        }

        public IEasyDriverPlugin AddDriver(IChannelCore channel, string driverPath)
        {
            if (GetDriver(channel) == null)
            {
                IEasyDriverPlugin driver = AssemblyHelper.LoadAndCreateInstance<IEasyDriverPlugin>(driverPath);
                if (driver != null)
                {
                    driver.Channel = channel;
                    return AddDriver(channel, driver);
                }
            }
            return null;
        }

        public IEasyDriverPlugin GetDriver(ICoreItem item)
        {
            IChannelCore channel = GetChannel(item);
            if (DriverPoll.ContainsKey(channel))
                return DriverPoll[channel];
            return null;
        }

        public async void RemoveDriver(IChannelCore channel)
        {
            await Task.Run(() =>
            {
                IEasyDriverPlugin driver = GetDriver(channel);
                if (driver != null)
                {
                    DriverPoll.Remove(channel);
                    driver.Dispose();
                }
            });
        }

        private IChannelCore GetChannel(ICoreItem item)
        {
            if (item == null)
                return null;
            if (item is IChannelCore channel)
                return channel;
            return GetChannel(item.Parent);
        }
    }
}
