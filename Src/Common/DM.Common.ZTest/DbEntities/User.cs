using System;

namespace DM.Common.ZTest.DbEntities
{
    public class User
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime CreatedTime { get; set; }
        public bool IsAdmin { get; set; }
    }
}