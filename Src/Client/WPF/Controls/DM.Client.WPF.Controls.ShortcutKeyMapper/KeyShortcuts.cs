using System;
using System.Collections.Generic;
using System.Windows.Input;
using MvvmLight.Helpers;

namespace DM.Client.WPF.Controls.ShortcutKeyMapper
{
    /// <summary>
    /// ��¼��ݼ���ʵ��
    /// </summary>
    public class KeyShortcuts
    {

        /// <summary>
        /// ��ݼ�ʵ��
        /// </summary>
        /// <param name="keyShortcutsType">��ݼ�����</param>
        /// <param name="shortCuts">��ݼ����</param>
        /// <param name="excute">�ص�����</param>
        public KeyShortcuts(KeyShortcutsTypes keyShortcutsType, string shortCuts, WeakAction<object> excute)
        {
            AccessKeys = new List<Key>();
            ModifierKey = new List<ModifierKeys>();
            KeyShortcutsType = keyShortcutsType;
            ShortCuts = shortCuts;
            Excute = excute;
       
        }

        /// <summary>
        /// �������
        /// </summary>
        public KeyShortcutsTypes KeyShortcutsType
        {
            get;
            private set;
        }

        private string m_shortCuts;

        /// <summary>
        /// ��ݼ���ϵ��ַ���
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
        /// �����ŵĻص�����
        /// </summary>
        public WeakAction<object> Excute
        {
            get;
            private set;
        }

        /// <summary>
        /// �û��İ���
        /// </summary>
        internal List<Key> AccessKeys
        {
            get;
            private set;
        }

        /// <summary>
        /// ���ܰ���
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
            //��ȡ���м�����
            var keys = m_shortCuts.Split('+');
            foreach (var key in keys)
            {
                ModifierKeys modifierKey;
                //���ϸ����ִ�Сд
                Enum.TryParse(key, true, out modifierKey);
                if (modifierKey != ModifierKeys.None)
                    ModifierKey.Add(modifierKey);
                else
                {
                    Key k;
                    //���ϸ����ִ�Сд.
                    Enum.TryParse(key, true, out k);
                    if (k != Key.None)
                        AccessKeys.Add(k);
                    else
                    {
                        //�׳��쳣
                        throw new ArgumentException("Gesture Parameter format error.");
                    }
                }
            }
        }
    }
}