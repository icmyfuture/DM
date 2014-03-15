#region Import

using System.Collections.Generic;

#endregion

/// <summary>
///   出库请求类
/// </summary>
public class DCMExportRequest
{
    #region 构造函数

    /// <summary>
    ///   构造函数
    /// </summary>
    public DCMExportRequest()
    {
        Authentication = new DCMExportRequestAuthentication();
        TargetInfo = new TargetType();
        RequestContent = new RequestContentType();
    }

    #endregion

    /// <summary>
    ///   授权信息
    /// </summary>
    public DCMExportRequestAuthentication Authentication { get; set; }

    /// <summary>
    ///   回迁目标及反馈地址信息
    /// </summary>
    public TargetType TargetInfo { get; set; }

    /// <summary>
    ///   回迁请求信息
    /// </summary>
    public RequestContentType RequestContent { get; set; }
}

#region 授权信息类

/// <summary>
///   授权信息类
/// </summary>
public class DCMExportRequestAuthentication
{
    #region 构造函数

    #endregion

    /// <summary>
    ///   UserToken
    /// </summary>
    public string UserToken { get; set; }
}

#endregion

#region 回迁请求信息类

/// <summary>
///   回迁请求信息类
/// </summary>
public class RequestContentType
{
    #region 构造函数

    #endregion

    /// <summary>
    ///   素材唯一标识
    /// </summary>
    public string ContentID { get; set; }

    /// <summary>
    ///   类型：素材，Clip；节目，Pgm
    /// </summary>
    public string EntityType { get; set; }

    /// <summary>
    ///   素材类型ID(可选 与 EntityType互斥)
    /// </summary>
    public long EntityTypeID { get; set; }

    /// <summary>
    ///   打点信息，入点，单位：百纳秒
    /// </summary>
    public long TrimIn { get; set; }

    /// <summary>
    ///   打点信息，出点，单位：百纳秒
    /// </summary>
    public long Duration { get; set; }

    /// <summary>
    ///   格式，MXF：FILETYPE_MXF；AVI+WAV：FILETYPE_HIGHAVI#FILETYPE_HIGHWAV
    /// </summary>
    public string Format { get; set; }

    /// <summary>
    ///   素材名称
    /// </summary>
    public string EntityName { get; set; }

    /// <summary>
    ///   素材帧率
    /// </summary>
    public string FrameRate { get; set; }

    /// <summary>
    ///   RequiredMetadatas
    /// </summary>
    public List<RequiredMetadataType> RequiredMetadatas { get; set; }

    /// <summary>
    ///   IsNeedCatalogueData
    /// </summary>
    public bool IsNeedCatalogueData { get; set; }
}

#endregion

#region 回迁目标及反馈地址信息

/// <summary>
///   回迁目标及反馈地址信息
/// </summary>
public class TargetType
{
    #region 构造函数

    /// <summary>
    ///   构造函数
    /// </summary>
    public TargetType()
    {
        TargetNotify = new TargetNotifyType();
        LocationInfo = new LocationInfoType();
    }

    #endregion

    /// <summary>
    ///   目标系统
    /// </summary>
    public string TargetSystemID { get; set; }

    /// <summary>
    ///   目标设备
    /// </summary>
    public string TargetDevice { get; set; }

    /// <summary>
    ///   如果提供这个,则回复内容以此为准
    /// </summary>
    public TargetNotifyType TargetNotify { get; set; }

    /// <summary>
    ///   如果有这个信息,则以此作为目标位置
    /// </summary>
    public LocationInfoType LocationInfo { get; set; }
}

#endregion

#region 目标位置信息类 

/// <summary>
///   目标位置信息类
/// </summary>
public class LocationInfoType
{
    #region 构造函数

    #endregion

    /// <summary>
    ///   如果有这个,则以这个为准；共享盘符：盘符:\\文件夹路径；FTP：ftp://用户名:密码@文件夹路径；IP：\\主机名\文件夹路径
    /// </summary>
    public string TargetPnameixInfo { get; set; }

    /// <summary>
    ///   AccessUser
    /// </summary>
    public string AccessUser { get; set; }

    /// <summary>
    ///   AccessPwd
    /// </summary>
    public string AccessPwd { get; set; }
}

#endregion

#region 回复内容类

/// <summary>
///   回复内容类
/// </summary>
public class TargetNotifyType
{
    #region 构造函数

    #endregion

    /// <summary>
    ///   回复接口
    /// </summary>
    public string CallbackInterface { get; set; }

    /// <summary>
    ///   回复方法
    /// </summary>
    public string CallbackMethod { get; set; }
}

#endregion

#region RequiredMetadataType

/// <summary>
///   RequiredMetadataType
/// </summary>
public class RequiredMetadataType
{
    #region 构造函数

    #endregion

    /// <summary>
    ///   MetadataTypeID
    /// </summary>
    public string MetadataTypeID { get; set; }
}

#endregion