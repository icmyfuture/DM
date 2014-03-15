using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml;

namespace DM.Client.WPF.Controls.DragDrop.DragDropFramework
{
    public class Utilities
    {
        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(ref Win32Point pos);

        /// <summary>
        /// Returns the position of the mouse cursor in screen coordinates
        /// </summary>
        /// <returns></returns>
        public static Point Win32GetCursorPos()
        {
            Win32Point position = new Win32Point();
            GetCursorPos(ref position);
            return new Point(position.X, position.Y);
        }

        public static T FindParentControlIncludingMe<T>(DependencyObject depObj)
          where T : DependencyObject
        {
            while (depObj != null)
            {
                if (depObj.GetType() == typeof(T))
                    return depObj as T;
                if (depObj is Visual)
                    depObj = VisualTreeHelper.GetParent(depObj);
                else if (depObj is FrameworkContentElement)
                    depObj = ((FrameworkContentElement)depObj).Parent;
                else
                    depObj = null;
            }

            return null;
        }

        public static object FindParentControlIncludingMe(DependencyObject depObj, Type t)
        {
            while (depObj != null)
            {
                if (depObj.GetType() == t)
                    return depObj;
                if (depObj is Visual)
                    depObj = VisualTreeHelper.GetParent(depObj);
                else if (depObj is FrameworkContentElement)
                    depObj = ((FrameworkContentElement)depObj).Parent;
                else
                    depObj = null;
            }

            return null;
        }

        public static T FindParentControlIncludingMe<T, TSourceContainer>(DependencyObject depObj)
            where T : DependencyObject
        {
            while (depObj != null)
            {
                if (depObj.GetType() == typeof(T))
                    return depObj as T;
                if (depObj.GetType() == typeof(TSourceContainer))
                    return null;
                if (depObj is Visual)
                    depObj = VisualTreeHelper.GetParent(depObj);
                else if (depObj is FrameworkContentElement)
                    depObj = ((FrameworkContentElement) depObj).Parent;
                else
                    depObj = null;
            }

            return null;
        }

        public static object FindParentControlIncludingMe(DependencyObject depObj, Type t, Type tSourceContainer)
        {
            while (depObj != null)
            {
                if (depObj.GetType() == t)
                    return depObj;
                if (depObj.GetType() == tSourceContainer)
                    return null;
                if (depObj is Visual)
                    depObj = VisualTreeHelper.GetParent(depObj);
                else if (depObj is FrameworkContentElement)
                    depObj = ((FrameworkContentElement) depObj).Parent;
                else
                    depObj = null;
            }

            return null;
        }

        /// <summary>
        /// Loops to find parent control of type <code>T</code>
        /// </summary>
        /// <typeparam name="T">Type of object to return</typeparam>
        /// <param name="depObj">Child of object where search begins</param>
        /// <returns>Object of type <code>T</code> or null</returns>
        public static T FindParentControlExcludingMe<T>(DependencyObject depObj)
            where T : DependencyObject
        {
            while (depObj != null)
            {
                if (depObj is Visual)
                    depObj = VisualTreeHelper.GetParent(depObj);
                else if (depObj is FrameworkContentElement)
                {
                    depObj = ((FrameworkContentElement) depObj).Parent;
                }
                else
                    depObj = null;
                if (depObj is T)
                    return depObj as T;
            }

            return null;
        }

        /// <summary>
        /// Serializes <code>obj</code> into a XAML string
        /// </summary>
        /// <param name="obj">Object to serialize</param>
        /// <returns>Serialized XAML version of <code>obj</code></returns>
        public static string SerializeObject(object obj)
        {
            return XamlWriter.Save(obj);
        }

        /// <summary>
        /// Converts XAML serialized string back into object
        /// </summary>
        /// <param name="xaml">XAML serialized object</param>
        /// <returns>Object represented by XAML string</returns>
        public static object DeserializeObject(string xaml)
        {
            return XamlReader.Load(new XmlTextReader(new StringReader(xaml)));
        }

        /// <summary>
        /// Clones an object
        /// </summary>
        /// <param name="obj">object to clone</param>
        /// <returns>Clone of <code>obj</code></returns>
        public static object CloneElement(object obj)
        {
            return DeserializeObject(SerializeObject(obj));
        }

        public static BitmapSource CreateImage(UIElement source, double scale)
        {
            double actualHeight = source.RenderSize.Height;
            double actualWidth = source.RenderSize.Width;

            double renderHeight = actualHeight * scale;
            double renderWidth = actualWidth * scale;

            RenderTargetBitmap renderTarget = new RenderTargetBitmap((int)renderWidth, (int)renderHeight, 96, 96, PixelFormats.Pbgra32);
            VisualBrush sourceBrush = new VisualBrush(source);

            DrawingVisual drawingVisual = new DrawingVisual();
            DrawingContext drawingContext = drawingVisual.RenderOpen();

            using (drawingContext)
            {
                drawingContext.PushTransform(new ScaleTransform(scale, scale));
                drawingContext.DrawRectangle(sourceBrush, null, new Rect(new Point(0, 0), new Point(actualWidth, actualHeight)));
            }
            renderTarget.Render(drawingVisual);

            return renderTarget;
        }

        #region Nested type: Win32Point

        [StructLayout(LayoutKind.Sequential)]
        private struct Win32Point
        {
            public readonly Int32 X;
            public readonly Int32 Y;
        } ;

        #endregion
    }
}