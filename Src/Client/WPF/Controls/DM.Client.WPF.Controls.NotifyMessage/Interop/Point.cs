using System.Runtime.InteropServices;

namespace DM.Client.WPF.Controls.NotifyMessage.Interop
{
    /// <summary>
  /// Win API struct providing coordinates for a single point.
  /// </summary>
  [StructLayout(LayoutKind.Sequential)]
  public struct Point
  {
    public int X;
    public int Y;
  }
}