using System;
using System.Configuration;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;

namespace DM.Common.ZTest
{
    public class UnityTest : ITest
    {
        public void Test()
        {
            IUnityContainer container = new UnityContainer();
            var section = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
            section.Configure(container);
            var computer = container.Resolve<IComputer>();
            computer.Tell();
        }
    }

    internal interface IComputer
    {
        void Tell();
    }

    public class Computer : IComputer
    {
        private readonly ICpu _cpu;

        public Computer(ICpu cpu)
        {
            _cpu = cpu;
        }

        public void Tell()
        {
            Console.WriteLine(_cpu.Name);
        }
    }

    public interface ICpu
    {
        string Name { get; }
    }

    public class IntelCpu : ICpu
    {
        public string Name
        {
            get { return "Intel"; }
        }
    }
}