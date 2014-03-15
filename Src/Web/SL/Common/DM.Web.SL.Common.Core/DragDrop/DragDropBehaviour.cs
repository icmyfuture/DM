using System.Windows;
using DragEventArgs = DM.Web.SL.Common.Core.DragDrop.DragEventArgs;
using DragEventHandler = DM.Web.SL.Common.Core.DragDrop.DragEventHandler;

namespace DM.Web.SL.Common.Core.DragDrop
{
    #region Imports

    

    #endregion

    /// <summary>
    ///   Adapts a <see cref = "DependencyObject" /> to <see cref = "IDropTarget" />.
    /// </summary>
    public sealed class DragDropBehaviour : IDropTarget
    {
        #region Fields

        private static readonly DependencyProperty DragDropBehaviourProperty = DependencyProperty.RegisterAttached(
            "DragDropBehaviour", typeof (DragDropBehaviour), typeof (DragDropBehaviour), new PropertyMetadata(null));

        private readonly object _target;

        #endregion

        /// <summary>
        ///   Initializes a new instance of the <see cref = "DragDropBehaviour" /> class.
        /// </summary>
        /// <param name = "target">The <see cref = "DependencyObject" /> to adapt.</param>
        private DragDropBehaviour(DependencyObject target)
        {
            #region Validate Arguments

            Guard.ArgumentNull("target", target);

            #endregion

            _target = target;
        }

        #region IDropTarget Members

        void IDropTarget.Drop(DragEventArgs e)
        {
            var handler = Drop;
            if (handler != null)
            {
                handler(_target, e);
            }
        }

        void IDropTarget.DragOver(DragEventArgs e)
        {
            var handler = DragOver;
            if (handler != null)
            {
                handler(_target, e);
            }
        }

        void IDropTarget.DragLeave(DragEventArgs e)
        {
            var handler = DragLeave;
            if (handler != null)
            {
                handler(_target, e);
            }
        }

        void IDropTarget.DragEnter(DragEventArgs e)
        {
            var handler = DragEnter;
            if (handler != null)
            {
                handler(_target, e);
            }
        }

        public object Target
        {
            get { return _target; }
        }

        #endregion

        /// <summary>
        ///   Gets the <see cref = "DragDropBehaviour" /> for the specified <see cref = "DependencyObject" />
        /// </summary>
        /// <param name = "dependencyObject"></param>
        /// <returns></returns>
        public static DragDropBehaviour GetBehaviour(DependencyObject dependencyObject)
        {
            return (DragDropBehaviour) dependencyObject.GetValue(DragDropBehaviourProperty);
        }

        /// <summary>
        ///   Set the <see cref = "DragDropBehaviour" /> for the specified <see cref = "DependencyObject" />
        /// </summary>
        /// <param name = "dependencyObject"></param>
        /// <param name = "value"></param>
        private static void SetBehaviour(DependencyObject dependencyObject, DragDropBehaviour value)
        {
            dependencyObject.SetValue(DragDropBehaviourProperty, value);
        }

        /// <summary>
        ///   Creates a new <see cref = "DragDropBehaviour" /> and associates it with the specified object.
        /// </summary>
        /// <param name = "target"></param>
        /// <returns></returns>
        /// <remarks>
        ///   An behaviour is associated with a <see cref = "DependencyObject" /> via the <see cref = "DragDropBehaviourProperty" /> 
        ///   attached property. Any previous association will be overwritten.
        /// </remarks>
        public static DragDropBehaviour GetOrCreateBehaviour(DependencyObject target)
        {
            var behaviour = GetBehaviour(target);
            if (behaviour == null)
            {
                behaviour = new DragDropBehaviour(target);
                SetBehaviour(target, behaviour);
            }
            return behaviour;
        }

        public event DragEventHandler Drop;
        public event DragEventHandler DragOver;
        public event DragEventHandler DragEnter;
        public event DragEventHandler DragLeave;
    }
}