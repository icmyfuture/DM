using System;
using System.IO;
using DM.Common.Data;
using DM.Common.ZTest.DbEntities;
using log4net.Config;

namespace DM.Common.ZTest
{
    public class DataTest : ITest
    {
        private readonly string _config;
        private readonly string _connectionString;

        public DataTest()
        {
            _config = string.Format(@"{0}\hibernate.config", Environment.CurrentDirectory);
            _connectionString = string.Format(@"Data Source={0}\App_Data\dm.db;Version=3", Environment.CurrentDirectory);

            string log4NetConfigFile = Path.Combine(Environment.CurrentDirectory, "log4net.xml");
            XmlConfigurator.Configure(new FileInfo(log4NetConfigFile));
        }

        public void Test()
        {
            using (var session = new SessionManager(_config, _connectionString).Data())
            {
                for (var i = 0; i < 10; i++)
                {
                    var user = new User { Name = "DE", CreatedTime = DateTime.Now, IsAdmin = true };
                    session.Save(user);
                }
                session.Flush();
            }
        }
    }
}