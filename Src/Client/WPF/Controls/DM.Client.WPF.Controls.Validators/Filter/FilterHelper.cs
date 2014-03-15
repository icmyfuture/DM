using System.Diagnostics;
using System.Windows.Input;

namespace DM.Client.WPF.Controls.Validators.Filter
{
    public static class FilterHelper
    {
        /// <summary>
        /// 是否是允许的键，如
        /// ctrl, Back, Tab, Enter, Shift, Ctrl, Alt, CapsLock, Escape, PageUp, PageDown
        /// End, Home, Left, Up, Right, Down, Insert, Delete
        /// Fx keys。
        /// </summary>
        /// <param name="key">被检测的键。</param>
        /// <returns>是返回true，否则返回false。</returns>
        public static bool IsValidOtherKey(Key key)
        {
            // 允许ctrl键。
            Debug.WriteLine(key.ToString());
            if ((Keyboard.Modifiers & ModifierKeys.Control) != 0)
            {
                return true;
            }

            // 允许
            // Back, Tab, Enter, Shift, Ctrl, Alt, CapsLock, Escape, PageUp, PageDown
            // End, Home, Left, Up, Right, Down, Insert, Delete
            // 不允许 space!
            // 允许 Fx keys
            if ((key < Key.D0 && key != Key.Space)
                || (key > Key.Z && key < Key.NumPad0))
            {
                return true;
            }

            // 剩下的都不允许。
            return false;
        }
    }
}