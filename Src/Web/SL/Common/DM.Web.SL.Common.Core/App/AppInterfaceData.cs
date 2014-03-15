using System.Linq;
using System.Collections.Generic;

namespace DM.Web.SL.Common.Core.App
{
    /// <summary>
    ///   应用数据交换实体类
    /// </summary>
    /// <remarks>
    ///   所有应用都使用统一的数据结构
    /// </remarks>
    public class AppInterfaceData
    {
        #region 属性
        /// <summary>
        ///   备用字段1 冗余字段 可空
        /// </summary>
        public string Addtional { get; set; }

        /// <summary>
        ///   素材ID
        /// </summary>
        public string ContentID { get; set; }

        /// <summary>
        ///   素材ID long形的字符串
        /// </summary>
        public string EntityID { get; set; }

        /// <summary>
        ///   素材名称 冗余字段 可空
        /// </summary>
        public string EntityName { get; set; }

        /// <summary>
        ///   素材分类ID
        /// </summary>
        public string EntityTypeID { get; set; }

        /// <summary>
        /// 素材类型名称
        /// </summary>
        public string EntityTypeName { get; set; }

        /// <summary>
        ///   文件组 冗余字段 可空
        /// </summary>
        public string FileGroup { get; set; }

        /// <summary>
        ///   文件类型 冗余字段 可空
        /// </summary>
        public string FileType { get; set; }

        /// <summary>
        ///   层次ID(实体层请保持为0)
        /// </summary>
        public string LayerID { get; set; }

        /// <summary>
        ///   入点(如果未打点请不要赋值)
        /// </summary>
        public string MarkIn { get; set; }

        /// <summary>
        ///   出点(如果未打点请不要赋值)
        /// </summary>
        public string MarkOut { get; set; }

        /// <summary>
        ///   素材时长(如果非音视频 请空)
        /// </summary>
        public string Duration { get; set; }

        /// <summary>
        ///   操作者用户ID
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        ///   帧率(如果非音视频 请空)
        /// </summary>
        public string FrameRate { get; set; }

        /// <summary>
        ///   音频格式(如果非音视频 请空)
        /// </summary>
        public string AudioFormat { get; set; }
        /// <summary>
        ///   视频格式(如果非音视频 请空)
        /// </summary>
        public string VideoFormat { get; set; }


        /// <summary>
        ///   其他信息列表 冗余字段 可空
        /// </summary>
        public List<ExtensionItem> ExtensionItems { get; set; }

        #endregion

        #region 方法
        ///<summary>
        /// 添加扩展值
        ///</summary>
        ///<param name="key">键</param>
        ///<param name="value">值</param>
        public void AddExtension( string key, string value )
        {
            if (ExtensionItems == null)
            {
                ExtensionItems = new List<ExtensionItem>();
            }

            if (ExtensionItems.All(x => x.Key != key))
            {
                ExtensionItems.Add(new ExtensionItem
                                   {
                                       Key = key,
                                       Value = value
                                   });
            }
            else
            {
                ExtensionItem item = ExtensionItems.FirstOrDefault( x => x.Key == key );
                ExtensionItems.Remove( item );
                item.Value = value;
                ExtensionItems.Add(item);
            }
        }


        /// <summary>
        /// 获取扩展值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>值</returns>
        public string GetExtension( string key )
        {
            if (ExtensionItems == null || ExtensionItems.All(x => x.Key != key))
            {
                return "";
            }
            ExtensionItem item = ExtensionItems.FirstOrDefault(x => x.Key == key);
            return item.Value;
        }

        #endregion
    }

    #region 其他信息对象
    /// <summary>
    /// 其他信息对象
    /// </summary>
    public struct ExtensionItem
    {
        /// <summary>
        /// 键
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }
    }
    #endregion
}