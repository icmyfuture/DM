using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using DM.Common.Extensions;

namespace DM.Client.WPF.Controls.SimpleControls.CheckNode
{
    /// <summary>
    /// Tree的CheckBoxNode节点的数据源，封装了对节点的操作。
    /// </summary>
    [Serializable]
    public class CheckNode : INotifyPropertyChanged
    {
        #region Constructor

        /// <summary>
        /// 空构造函数。
        /// </summary>
        public CheckNode()
        {
            SubNodes.CollectionChanged += SubNodes_CollectionChanged;
        }

        /// <summary>
        /// 深拷贝构造函数。
        /// </summary>
        /// <param name="node">被拷贝的节点。</param>
        /// <param name="parentNode">父节点。</param>
        /// <param name="isTagDeepClone">Tag是否需要深拷贝。</param>
        public CheckNode(CheckNode node, CheckNode parentNode = null, bool isTagDeepClone = false)
            : this()
        {
            _id = ID;
            _isChecked = node.IsChecked;
            _isExpanded = node.IsExpanded;
            _isLasyNode = node.IsLasyNode;
            _isVisibility = node.IsVisibility;
            _name = node.Name;
            _parentNode = parentNode;
            if (isTagDeepClone)
                _tag = node.Tag.DeepClone();
            else
                _tag = node.Tag;

            _subNodes = node.DeepCloneSubNodes(this, isTagDeepClone);
        }

        #endregion

        #region Fields

        private Guid _id = Guid.NewGuid();
        private bool? _isChecked = false;
        private bool _isExpanded;
        private bool _isVisibility = true;
        private bool _isLasyNode;
        private string _name;
        private CheckNode _parentNode;
        private ObservableCollection<CheckNode> _subNodes = new ObservableCollection<CheckNode>();
        private object _tag;

        #endregion Fields

        #region Events

        /// <summary>
        /// 属性改变事件。
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Events

        #region Properties Public

        /// <summary>
        /// Guid
        /// </summary>
        public Guid ID
        {
            get { return _id; }
            set
            {
                if (_id == value)
                    return;

                _id = value;
                OnPropertyChanged("ID");
            }
        }

        /// <summary>
        /// IsVisibility
        /// </summary>
        public bool IsVisibility
        {
            get { return _isVisibility; }
            set
            {
                if (_isVisibility == value)
                    return;

                _isVisibility = value;
                OnPropertyChanged("IsVisibility");
            }
        }

        /// <summary>
        /// 选中状态。
        /// </summary>
        public bool? IsChecked
        {
            get { return _isChecked; }
            set
            {
                if (_isChecked == value)
                    return;

                OnIsCheckedChanged(value);
            }
        }

        /// <summary>
        /// 是否展开。
        /// </summary>
        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                if (_isExpanded == value)
                    return;

                _isExpanded = value;
                OnPropertyChanged("IsExpanded");
            }
        }

        /// <summary>
        /// 是否是用于延时加载的节点。
        /// </summary>
        public bool IsLasyNode
        {
            get { return _isLasyNode; }
        }

        private bool _hasSubNodes;

        /// <summary>
        /// 是否有子节点。
        /// </summary>
        public bool HasSubNodes
        {
            get { return _hasSubNodes; }
            private set
            {
                if (_hasSubNodes == value)
                    return;

                _hasSubNodes = value;
                OnPropertyChanged("HasSubNodes");
            }
        }

        /// <summary>
        /// 用于显示节点的文字。
        /// </summary>
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name == value)
                    return;

                _name = value;
                OnPropertyChanged("Name");
            }
        }

        /// <summary>
        /// 父节点。
        /// </summary>
        public CheckNode ParentNode
        {
            get { return _parentNode; }
            set
            {
                if (_parentNode == value)
                    return;

                _parentNode = value;
                OnPropertyChanged("ParentNode");
            }
        }

        /// <summary>
        /// 子节点。
        /// </summary>
        public ObservableCollection<CheckNode> SubNodes
        {
            get { return _subNodes; }
            set
            {
                if (_subNodes == value)
                    return;

                _subNodes = value;
                OnPropertyChanged("SubNodes");
            }
        }

        /// <summary>
        /// 绑定在节点上的数据。
        /// </summary>
        public object Tag
        {
            get { return _tag; }
            set
            {
                if (_tag == value)
                    return;

                _tag = value;
                OnPropertyChanged("Tag");
            }
        }

        #endregion Properties Public

        #region Methods Public

        /// <summary>
        /// 如果没有子节点，加一个空的子节点，如果有子节点，递归调用子节点的该方法。
        /// </summary>
        public void AddLasyNodeForSub()
        {
            if (IsLasyNode)
                return;

            if (SubNodes.Count == 0)
            {
                SubNodes.Add(new CheckNode { ParentNode = this, _isLasyNode = true, Name = "Loading..."});
                return;
            }

            foreach (var subNode in SubNodes)
                subNode.AddLasyNodeForSub();
        }

        /// <summary>
        /// 递归查找选中和实心叶子节点。
        /// </summary>
        public void FindCheckedOrCheckNullleaves(List<CheckNode> leaves)
        {
            if (IsLasyNode)
                return;

            foreach (var subNode in SubNodes)
            {
                if (subNode.IsLasyNode)
                    break;
                subNode.FindCheckedOrCheckNullleaves(leaves);
            }

            if (IsCheckedOrCheckNullLeaf())
                leaves.Add(this);
        }

        /// <summary>
        /// 获取选中和实心叶子节点。
        /// </summary>
        /// <returns>返回获取的叶子节点。</returns>
        public List<CheckNode> GetCheckedLeaves()
        {
            if (IsLasyNode)
                return new List<CheckNode>();

            List<CheckNode> ret = new List<CheckNode>();
            FindCheckedOrCheckNullleaves(ret);

            //如果没有叶子，当前节点可能是叶子。
            if (ret.Count == 0 && IsCheckedOrCheckNullLeaf())
                ret.Add(this);

            return ret;
        }

        /// <summary>
        /// 获取父节点的链表，包括本节点。
        /// </summary>
        /// <returns>返回获取的父节点的链表，包括本节点。</returns>
        public IEnumerable<CheckNode> GetParentNodeLinkIncludeSelf()
        {
            CheckNode curr = this;
            yield return curr;
            while (curr.ParentNode != null)
            {
                curr = curr.ParentNode;
                yield return curr;
            }
        }

        /// <summary>
        /// 获取根节点。
        /// </summary>
        /// <returns>返回获取的根节点。</returns>
        public CheckNode GetRootNode()
        {
            var list = GetParentNodeLinkIncludeSelf().ToList();
            return list[list.Count - 1];
        }

        /// <summary>
        /// 是否有一个延迟加载子节点。
        /// </summary>
        /// <returns>有返回ture，否则返回false。</returns>
        public bool HasLasyNodeForSub()
        {
            if (SubNodes.Count == 1 && SubNodes[0].IsLasyNode)
                return true;
            return false;
        }

        /// <summary>
        /// 所有的子节点是否选中。
        /// </summary>
        /// <returns>子节点全部选中是返回ture，否则返回false。</returns>
        public bool IsAllSubNodesChecked()
        {
            return IsAllSubNodesChecked(this);
        }

        /// <summary>
        /// 递归所有的子节点是否选中。
        /// </summary>
        /// <param name="source">发起递归的源对象。</param>
        /// <returns>子节点全部选中是返回ture，否则返回false。</returns>
        public bool IsAllSubNodesChecked(CheckNode source)
        {
            foreach (var subNode in SubNodes)
            {
                if (subNode.IsLasyNode)
                    return false;

                if (!subNode.IsAllSubNodesChecked(source))
                    return false;
            }

            if (this == source)
                return true;

            //选中
            if (IsChecked.HasValue && IsChecked.Value)
                return true;

            return false;
        }

        /// <summary>
        /// 所有的子节点是否未选中。
        /// </summary>
        /// <returns>子节点全部未选中返回true，否则返回false。</returns>
        public bool IsAllSubNodesUnChecked()
        {
            return IsAllSubNodesUnChecked(this);
        }

        /// <summary>
        /// 递归所有的所有的子节点是否未选中。
        /// </summary>
        /// <param name="source">发起递归的源对象。</param>
        /// <returns>子节点全部选中是返回ture，否则返回false。</returns>
        public bool IsAllSubNodesUnChecked(CheckNode source)
        {
            foreach (var subNode in SubNodes)
            {
                if (subNode.IsLasyNode)
                    break;

                if (!subNode.IsAllSubNodesUnChecked(source))
                    return false;
            }

            if (source == this)
                return true;

            //未选中
            if (IsChecked != null && !IsChecked.Value)
                return true;

            return false;
        }

        /// <summary>
        /// 是否是选中或实心状态的叶子节点。
        /// </summary>
        /// <returns>是返回true，否则返回false。</returns>
        public bool IsCheckedOrCheckNullLeaf()
        {
            //如果未选中忽略本节点。
            if (IsChecked.HasValue && !IsChecked.Value)
                return false;

            //下面是处理"选中"或"实心"状态的节点:
            //如果子节点都没有"选中"。
            if (IsAllSubNodesUnChecked())
                return true;

            return false;
        }

        /// <summary>
        /// 所有的平行节点是否选中。
        /// </summary>
        /// <returns>所有的平行节点全部选中返回true，否则返回false。</returns>
        public bool IsOtherAllParallelNodesChecked()
        {
            if (ParentNode == null)
                throw new Exception("IsOtherAllParallelNodesChecked faild, do not have a ParentNode.");

            foreach (var node in ParentNode.SubNodes)
            {
                //不判断自己。
                if (node == this)
                    continue;
                if (!node.IsChecked.HasValue || !node.IsChecked.Value)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// 所有的平行节点是否未选中。
        /// </summary>
        /// <returns>所有的平行节点全部未选中返回true，否则返回false。</returns>
        public bool IsOtherAllParallelNodesUnChecked()
        {
            if (ParentNode == null)
                throw new Exception("IsOtherAllParallelNodesUnChecked faild, do not have a ParentNode.");

            foreach (var node in ParentNode.SubNodes)
            {
                //不判断自己。
                if (node == this)
                    continue;
                if (!node.IsChecked.HasValue || node.IsChecked.Value)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// 不触发OnIsCheckedChanged。
        /// </summary>
        public void SetIsCheckedWithoutOnIsCheckedChanged(bool? isChecked)
        {
            _isChecked = isChecked;
            OnPropertyChanged("IsChecked");
        }

        #endregion Methods Public

        #region Methods Private

        private void OnIsCheckedChanged(bool? isChecked)
        {
            SetCurrNode(isChecked);
            SetSubNodes(isChecked);
            SetParentNodes(isChecked);
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void SetCurrNode(bool? currNodeIsChecked)
        {
            //选中
            if (currNodeIsChecked.HasValue && currNodeIsChecked.Value && !IsAllSubNodesChecked())
                SetIsChecked(null);
            else
                SetIsChecked(currNodeIsChecked);
        }

        private void SetIsChecked(bool? isChecked)
        {
            _isChecked = isChecked;
            OnPropertyChanged("IsChecked");
        }

        private void SetIsCheckedAndSetParent(bool? isChecked)
        {
            _isChecked = isChecked;

            SetParentNodes(_isChecked);
            OnPropertyChanged("IsChecked");
        }

        private void SetIsCheckedAndSetSub(bool? isChecked)
        {
            _isChecked = isChecked;
            SetSubNodes(_isChecked);
            OnPropertyChanged("IsChecked");
        }

        private void SetParentNodes(bool? currNodeIsChecked)
        {
            if (ParentNode == null)
                return;

            //实心
            if (currNodeIsChecked == null)
            {
                ParentNode.SetIsCheckedAndSetParent(null);
                return;
            }

            //选中
            if (currNodeIsChecked.Value)
            {
                if (IsAllSubNodesChecked() && IsOtherAllParallelNodesChecked())
                    ParentNode.SetIsCheckedAndSetParent(true);
                else
                    ParentNode.SetIsCheckedAndSetParent(null);
                return;
            }

            //反选
            //if (!currNodeIsChecked.Value)
            if (IsAllSubNodesUnChecked() && IsOtherAllParallelNodesUnChecked())
                ParentNode.SetIsCheckedAndSetParent(false);
            else
                ParentNode.SetIsCheckedAndSetParent(null);
        }

        private void SetSubNodes(bool? currNodeIsChecked)
        {
            //反选
            if (currNodeIsChecked.HasValue && !currNodeIsChecked.Value)
            {
                foreach (var subNode in SubNodes)
                    subNode.SetIsCheckedAndSetSub(false);
            }
        }

        private void SubNodes_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (SubNodes.Count > 0 && !SubNodes[0].IsLasyNode)
                HasSubNodes = true;
            HasSubNodes = false;
        }

        private ObservableCollection<CheckNode> DeepCloneSubNodes(CheckNode parentNode, bool isTagDeepClone)
        {
            ObservableCollection<CheckNode> ret = new ObservableCollection<CheckNode>();
            foreach (var subNode in SubNodes)
                ret.Add(new CheckNode(subNode, parentNode, isTagDeepClone));

            return ret;
        }

        #endregion Methods Private
    }
}