using System.Collections.Generic;
using System.Windows.Input;

namespace Carrier.Utility
{
    ///<summary>
    ///  ↑↑↓↓←→←→BA
    ///</summary>
    internal sealed class CaptainCode
    {
        private readonly List<Key> _keys = new List<Key>
            {
                Key.Up,
                Key.Up,
                Key.Down,
                Key.Down,
                Key.Left,
                Key.Right,
                Key.Left,
                Key.Right,
                Key.B,
                Key.A
            };

        private int _mPosition = -1;

        ///<summary>
        ///</summary>
        private int Position
        {
            get { return _mPosition; }
            set { _mPosition = value; }
        }

        ///<summary>
        ///</summary>
        ///<param name = "key"></param>
        ///<returns></returns>
        public bool IsCompletedBy(Key key)
        {
            if (_keys[Position + 1] == key)
            {
                // move to next
                Position++;
            }
            else if (Position == 1 && key == Key.Up)
            {
                // stay where we are
            }
            else if (_keys[0] == key)
            {
                // restart at 1st
                Position = 0;
            }
            else
            {
                // no match in sequence
                Position = -1;
            }

            if (Position == _keys.Count - 1)
            {
                Position = -1;
                return true;
            }

            return false;
        }
    }
}