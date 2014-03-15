using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Carrier.Utility;
using Carrier.ViewModel;

namespace Carrier
{
    public sealed partial class MainWindow
    {
        /// <summary>
        ///   按键事件
        /// </summary>
        /// <param name = "e"></param>
        protected override void OnKeyUp(KeyEventArgs e)
        {
            var code = new CaptainCode();
            if (code.IsCompletedBy(e.Key))
            {
            }
            base.OnKeyUp(e);
        }

        /// <summary>
        ///   关闭窗口
        /// </summary>
        /// <param name = "e"></param>
        protected override void OnClosing(CancelEventArgs e)
        {
            if (
                (MessageBox.Show("Exit?", "Exit", MessageBoxButton.OKCancel, MessageBoxImage.Question,
                                 MessageBoxResult.OK) == MessageBoxResult.OK))
            {
                Notifier.ShutDown();
                int pcount = MoniterViewModel.Instance.ExecutingCaches.Count;
                new ShuttingDown(pcount).ShowDialog();
            }
            else
            {
                e.Cancel = true;
            }
            base.OnClosing(e);
        }

        /// <summary>
        ///   拖拽放下
        /// </summary>
        /// <param name = "e"></param>
        protected override void OnDrop(DragEventArgs e)
        {
            var files = e.Data.GetData(DataFormats.FileDrop) as Array;
            if (files == null)
            {
                return;
            }
            foreach (object file in files)
            {
                _dataProcessor.AddApplication(file.ToString());
                Notifier.ShowTip(file + " in");
            }
            base.OnDrop(e);
        }

        /// <summary>
        ///   拖入
        /// </summary>
        /// <param name = "e"></param>
        protected override void OnDragEnter(DragEventArgs e)
        {
            e.Effects = e.Data.GetDataPresent(DataFormats.FileDrop)
                            ? DragDropEffects.All
                            : DragDropEffects.None;
            base.OnDragEnter(e);
        }
    }
}