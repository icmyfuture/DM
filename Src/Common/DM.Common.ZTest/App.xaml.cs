﻿using DM.Common.WPF;

namespace DM.Common.ZTest
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App
    {
        public App()
        {
            AppHelper.Instance.Attach(this, () => new UnityTest().Test());
        }
    }
}
