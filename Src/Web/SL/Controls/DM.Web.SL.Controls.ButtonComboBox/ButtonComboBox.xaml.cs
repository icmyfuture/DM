using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DM.Web.SL.Controls.ButtonComboBox
{
    ///<summary>
    /// ButtonComboBox
    ///</summary>
    public partial class ButtonComboBox
    {
        #region 变量

        private List<ButtonComboBoxMethod> _listMethod = new List<ButtonComboBoxMethod>();
        private bool _isItemExecution;

        /// <summary>
        /// 
        /// </summary>
        public int SelectedIndex { get; set; }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        public event RoutedEventHandler SelectionChanged;

        #region 初始化

        ///<summary>
        /// 初始化
        ///</summary>
        public ButtonComboBox()
        {
            InitializeComponent();
        }

        #endregion

        #region ButtonComboBox调用方法

        ///<summary>
        /// 对ButtonComboBox添加变量与方法
        ///</summary>
        ///<param name="list">下拉菜单中变量与方法</param>
        /// <param name="isItemExecution">选择下拉列表时是否执行事件</param>
        /// <param name="selectedIndex">初始项</param>
        public void Add(List<ButtonComboBoxMethod> list, bool isItemExecution, int selectedIndex)
        {
            if (list.Count <= selectedIndex || selectedIndex < 0)
                selectedIndex = 0;
            var selectList = new List<string>();
            _listMethod = list;
            _isItemExecution = isItemExecution;
            SelectedIndex = selectedIndex;
            if (list.Count != 0)
            {
                selectList.AddRange(list.Select(item => item.ItemName));

                Button_ComboBox.SelectionChanged += ButtonComboBoxSelectionChanged;
                Button_ComboBox.ItemsSource = selectList;
                Button_ComboBox.SelectedItem = selectList[selectedIndex];
            }
        }

        ///<summary>
        /// 对ButtonComboBox添加变量与方法
        ///</summary>
        ///<param name="list">下拉菜单中变量与方法</param>
        /// <param name="isItemExecution">选择下拉列表时是否执行事件</param>
        public void Add(List<ButtonComboBoxMethod> list, bool isItemExecution)
        {
            Add(list, isItemExecution, 0);
        }

        ///<summary>
        /// 对ButtonComboBox添加变量与方法
        ///</summary>
        ///<param name="list">下拉菜单中变量与方法</param>
        public void Add(List<ButtonComboBoxMethod> list)
        {
            Add(list, false, 0);
        }

        #endregion

        #region 按钮操作

        /// <summary>
        /// ComboBox中选项发生改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedIndex = Button_ComboBox.SelectedIndex;
            if (SelectedIndex >= 0)
            {
                if (SelectionChanged != null)
                    SelectionChanged(this, e);
                string str = Button_ComboBox.SelectedItem.ToString();
                Button_ComboBox_Value.Content = str;
                Button_ComboBox_Value.Focus();
                if (_isItemExecution)
                {
                    foreach (ButtonComboBoxMethod list in _listMethod)
                    {
                        if (list.ItemName == str)
                        {
                            if (list.ItemEventHandler != null)
                            {
                                list.ItemEventHandler(new object(), EventArgs.Empty);
                            }
                        }
                    }
                }
            }
        }

        private void ButtonComboBoxValueClick(object sender, RoutedEventArgs e)
        {
            if (_listMethod.Count != 0)
            {
                string itemName;
                if (Button_ComboBox.SelectedItem == null)
                {
                    itemName = _listMethod[SelectedIndex].ItemName;
                    foreach (ButtonComboBoxMethod list in _listMethod)
                    {
                        if (list.ItemName == itemName)
                        {
                            if (list.ItemEventHandler != null)
                            {
                                list.ItemEventHandler(new object(), EventArgs.Empty);
                            }
                        }
                    }
                }
                else
                {
                    itemName = Button_ComboBox.SelectedItem.ToString();
                    foreach (ButtonComboBoxMethod list in _listMethod)
                    {
                        if (list.ItemName == itemName)
                        {
                            if (list.ItemEventHandler != null)
                            {
                                list.ItemEventHandler(new object(), EventArgs.Empty);
                            }
                        }
                    }
                }
            }
        }

        private void ButtonComboBoxSelectClick(object sender, RoutedEventArgs e)
        {
            Button_ComboBox.IsDropDownOpen = true;
        }


        private void ButtonComboBoxSelectMouseEnter(object sender, MouseEventArgs e)
        {
            Button_ComboBox_Select.Focus();
        }

        #endregion
    }
}
