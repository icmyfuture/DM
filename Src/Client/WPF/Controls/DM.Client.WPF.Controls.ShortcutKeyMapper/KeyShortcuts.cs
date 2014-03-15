using System;
using System.Collections.Generic;
using System.Windows.Input;
using MvvmLight.Helpers;

namespace DM.Client.WPF.Controls.ShortcutKeyMapper
{
    /// <summary>
    /// 记录快捷键的实体
    /// </summary>
    public class KeyShortcuts
    {

        /// <summary>
        /// 快捷键实体
        /// </summary>
        /// <param name="keyShortcutsType">快捷键类型</param>
        /// <param name="shortCuts">快捷键组合</param>
        /// <param name="excute">回调方法</param>
        public KeyShortcuts(KeyShortcutsTypes keyShortcutsType, string shortCuts, WeakAction<object> excute)
        {
            AccessKeys = new List<Key>();
            ModifierKey = new List<ModifierKeys>();
            KeyShortcutsType = keyShortcutsType;
            ShortCuts = shortCuts;
            Excute = excute;
       
        }

        /// <summary>
        /// 快捷类型
        /// </summary>
        public KeyShortcutsTypes KeyShortcutsType
        {
            get;
            private set;
        }

        private string m_shortCuts;

        /// <summary>
        /// 快捷键组合的字符串
        /// </summary>
        public string ShortCuts
        {
            get { return m_shortCuts; }
            private set
            {
                m_shortCuts = value;
                AnalysishortCuts();
            }
        }

        /// <summary>
        /// 订阅着的回调方法
        /// </summary>
        public WeakAction<object> Excute
        {
            get;
            private set;
        }

        /// <summary>
        /// 用户的按键
        /// </summary>
        internal List<Key> AccessKeys
        {
            get;
            private set;
        }

        /// <summary>
        /// 功能按键
        /// </summary>
        internal List<ModifierKeys> ModifierKey
        {
            get;
            private set;
        }

        private void AnalysishortCuts()
        {
            if (string.IsNullOrEmpty(m_shortCuts))
                throw new ArgumentException("Gesture Parameter format error.");
            m_shortCuts = m_shortCuts.Replace("Ctrl", "Control");
            //获取所有键集合
            var keys = m_shortCuts.Split('+');
            foreach (var key in keys)
            {
                ModifierKeys modifierKey;
                //不严格区分大小写
                Enum.TryParse(key, true, out modifierKey);
                if (modifierKey != ModifierKeys.None)
                    ModifierKey.Add(modifierKey);
                else
                {
                    Key k;
                    //不严格区分大小写.
                    Enum.TryParse(key, true, out k);
                    if (k != Key.None)
                        AccessKeys.Add(k);
                    else
                    {
                        //抛出异常
                        throw new ArgumentException("Gesture Parameter format error.");
                    }
                }
            }
        }
    }
}