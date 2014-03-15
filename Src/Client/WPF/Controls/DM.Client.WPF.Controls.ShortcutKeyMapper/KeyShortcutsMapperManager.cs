#region Note
// ------------------------------------------------------------------
//  版           权:     Copyright © Sobey 2011
//  文   件    名:     KeyShortcutsMapperManager
//  创   建    者:   xiangmaojun
//  创 建 日 期:   2011-09-19 14:37 
//  邮           箱:      xiangmaojun@newstepinfo.cn
//  描           述:                 
// ------------------------------------------------------------------
#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Input;
using MvvmLight.Helpers;

namespace DM.Client.WPF.Controls.ShortcutKeyMapper
{
    /// <summary>
    /// 键盘方案映射器
    /// </summary>
    public static class KeyShortcutsMapperManager
    {
        /// <summary>
        /// 上一个键盘接收类型
        /// </summary>
        public static KeyShortcutsTypes PreviewActiveKeyShortcutsType
        {
            get;
            private set;
        }

        private static KeyShortcutsTypes m_currentActiveKeyShortcutsType;

        private static KeyShortcutsTypes CurrentActiveKeyShortcutsType
        {
            get { return m_currentActiveKeyShortcutsType; }
            set
            {
                if (m_currentActiveKeyShortcutsType == value)
                    return;
                //记录上一次的键盘接收类型. 枚举是值类型. 所以可以直接复制
                PreviewActiveKeyShortcutsType = m_currentActiveKeyShortcutsType;
                //记录当前的键盘接收类型
                m_currentActiveKeyShortcutsType = value;
            }
        }

        /// <summary>
        /// 更新当前被激活的快捷键管理类
        /// </summary>
        /// <param name="currentActiveKeyShortcutsType"></param>
        public static void SetKeyShortcutsType(KeyShortcutsTypes currentActiveKeyShortcutsType)
        {
            CurrentActiveKeyShortcutsType = currentActiveKeyShortcutsType;
        }

        /// <summary>
        /// 快捷键的缓存池
        /// </summary>
        private static readonly Dictionary<KeyShortcutsTypes, List<KeyShortcuts>> KeyShortcuts = new Dictionary<KeyShortcutsTypes, List<KeyShortcuts>>();

        /// <summary>
        /// 注册快捷键
        /// </summary>
        /// <param name="keyShortcutsType"></param>
        /// <param name="shortCuts"></param>
        /// <param name="excute"></param>
        public static void RegistorShortcuts(KeyShortcutsTypes keyShortcutsType, string shortCuts, WeakAction<object> excute)
        {

            //判断是否包含当前Type的快捷键映射
            if (!KeyShortcuts.ContainsKey(keyShortcutsType))
            {
                List<KeyShortcuts> keyshorts = new List<KeyShortcuts>
                                               {
                                                   new KeyShortcuts(keyShortcutsType, shortCuts, excute)
                                               };
                KeyShortcuts.Add(keyShortcutsType, keyshorts);
            }
            else
            {
                var keyshorts = new KeyShortcuts(keyShortcutsType, shortCuts, excute);
                var keys = KeyShortcuts[keyShortcutsType];
                bool isExist = keys.IndexOf(keyshorts) > -1;
                if (isExist)
                {
                    throw new Exception(string.Format("{0} is already exist with {1} types", shortCuts, keyShortcutsType));
                }
                keys.Add(keyshorts);
            }
        }

        /// <summary>
        /// 注销快捷键
        /// </summary>
        /// <param name="keyShortcutsType"></param>
        public static void UnRegistorShortcuts(KeyShortcutsTypes keyShortcutsType)
        {
            if (KeyShortcuts.ContainsKey(keyShortcutsType))
            {
                KeyShortcuts.Remove(keyShortcutsType);
            }
        }

        /// <summary>
        /// 设置键盘输入源
        /// </summary>
        /// <param name="e"></param>
        public static void SetInputKeySource(KeyEventArgs e)
        {
            if (e.Key == Key.System && e.SystemKey == Key.F4)
            {
                //e.Handled = true;
            }
            else
            {
                HandleKeyDown(e);
            }
        }

        private static long _lasttime;
        private static string _lastShortKey;
        /// <summary>
        /// 处理键盘事件
        /// </summary>
        /// <param name="e"></param>
        private static void HandleKeyDown(KeyEventArgs e)
        {
            Trace.WriteLine(string.Format("Key{0} Pressed", e.Key));

            if (!KeyShortcuts.ContainsKey(CurrentActiveKeyShortcutsType))
                return;
            var keys = KeyShortcuts[CurrentActiveKeyShortcutsType];
            foreach (var shortcutkey in keys)
            {
                if (InvokeKeyDown(e, shortcutkey))
                    break;
            }
        }
        /// <summary>
        /// 响应键盘事件
        /// </summary>
        /// <param name="e"></param>
        /// <param name="shortcutkey"></param>
        /// <returns></returns>
        private static bool InvokeKeyDown(KeyEventArgs e, KeyShortcuts shortcutkey)
        {
            var keyFlag = false;
            foreach (var key in shortcutkey.AccessKeys)
            {
                if (e.KeyStates == Keyboard.GetKeyStates(key) && e.KeyStates != KeyStates.None)
                {
                    keyFlag = true;
                }
                else
                {
                    keyFlag = false;
                    break;
                }
            }
            var modifierKeyFlag = shortcutkey.ModifierKey.Count == 0 ? true : false;
            foreach (var modifierKeyse in shortcutkey.ModifierKey)
            {
                if ((Keyboard.Modifiers & modifierKeyse) == modifierKeyse && Keyboard.Modifiers != ModifierKeys.None)
                {
                    modifierKeyFlag = true;
                }
                else
                {
                    modifierKeyFlag = false;
                    break;
                }
            }
            //满足快捷键组合条件
            if (keyFlag && modifierKeyFlag)
            {
                if (_lastShortKey == shortcutkey.ShortCuts)
                {
                    long time = Environment.TickCount - _lasttime;
                    if (time < 100)
                    {
                        _lasttime = Environment.TickCount;
                        e.Handled = true;
                        return false;
                    }
                }
                shortcutkey.Excute.Execute();
                _lasttime = Environment.TickCount;
                if (string.IsNullOrEmpty(_lastShortKey))
                    _lastShortKey = shortcutkey.ShortCuts;
                e.Handled = true;
                return true;
            }
            return false;
        }
    }
}