using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Windows;
using System.Windows.Resources;
using System.Xml;
using DM.Web.SL.Controls.Window.Entities;

namespace DM.Web.SL.Controls.Window
{
    /// <summary>
    ///   加载XAP文件
    /// </summary>
    public class ApplicationXAPHelper : IDisposable
    {
        #region Fields

        /// <summary>
        ///   单一实例
        /// </summary>
        public static ApplicationXAPHelper Instance = new ApplicationXAPHelper();

        private static readonly Dictionary<string, Assembly> _DicAssemblys = new Dictionary<string, Assembly>();
        private readonly Uri _manifestUri = new Uri("AppManifest.xaml", UriKind.Relative);

        private string _appDll;
        private string _appType;
        private ApplicationInfo _applicationInfo;
        private WebClient client;

        #endregion Fields

        #region Events

        private event EventHandler LoadCompleted;

        #endregion Events

        #region Methods

        /// <summary>
        ///   析构
        /// </summary>
        public void Dispose()
        {
            if (_applicationInfo != null)
            {
                _applicationInfo.Instance = null;
                _applicationInfo = null;
            }
            _appType = null;
            _appDll = null;
            LoadCompleted = null;
            if (client != null)
            {
                client.OpenReadCompleted -= ClientOpenReadCompleted;
                client = null;
            }

            #region 内存释放

            //GCManager.Instance.FreeMemory(this);

            #endregion
        }

        /// <summary>
        ///   加载Xap文件
        /// </summary>
        /// <param name = "applicationInfo">应用程序信息</param>
        /// <param name = "loadCompleted">完成回调事件</param>
        public void LoadXap(ApplicationInfo applicationInfo, EventHandler loadCompleted)
        {
            #region 防止修改
            _applicationInfo = applicationInfo;
            #endregion

            LoadCompleted = loadCompleted;
            _appType = applicationInfo.TypeName;
            _appDll = applicationInfo.TypeName.Split(';')[1];
            if (_DicAssemblys.ContainsKey(_applicationInfo.ApplicationID))
            {
                CompletedCallBack();
            }
            else
            {
                try
                {
                    client = new WebClient();
                    client.OpenReadCompleted += ClientOpenReadCompleted;

                    //打开打包的xap文件
                    client.OpenReadAsync(new Uri(applicationInfo.XapName, UriKind.Relative));
                }
                catch (Exception e)
                {
                    Dispose();
                    throw e;
                }
            }
        }

        private void ClientOpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            TryLoadXap(e.Result);
            Dispose();
        }

        /// <summary>
        ///   创建返回实例
        /// </summary>
        private void CompletedCallBack()
        {
            if (LoadCompleted != null)
            {
                //======================
                //转换此assembly为UIElement
                UIElement myData = _DicAssemblys[_applicationInfo.ApplicationID].CreateInstance(_appType.Split(';')[0]) as UIElement;
                _applicationInfo.Instance = myData;
                LoadCompleted(_applicationInfo, EventArgs.Empty);
            }
        }

        /// <summary>
        ///   获取XamlStream
        /// </summary>
        /// <param name = "stream"></param>
        /// <returns></returns>
        private IEnumerable<Uri> GetAssemblyUrisFromDeploymentXamlStream(Stream stream)
        {
            using (var xmlReader = XmlReader.Create(stream))
            {
                while (xmlReader.Read())
                {
                    if (xmlReader.NodeType == XmlNodeType.Element
                        && xmlReader.Name == "AssemblyPart")
                        yield return new Uri(xmlReader.GetAttribute("Source"), UriKind.Relative);
                }
                xmlReader.Close();
            }
            //GCManager.Instance.FreeMemory();
        }

        /// <summary>
        ///   解析Xap
        /// </summary>
        /// <param name = "xapStream"></param>
        private void TryLoadXap(Stream xapStream)
        {
            try
            {
                if ( !_DicAssemblys.ContainsKey( _applicationInfo.ApplicationID ) )
                {
                    if ( !_applicationInfo.XapName.ToUpper().EndsWith( ".DLL" ) )
                    {
                        StreamResourceInfo xapSri = new StreamResourceInfo( xapStream, "application/x-silverlight-app" );
                        var manifestSri = Application.GetResourceStream( xapSri, _manifestUri );
                        Assembly asm = null;
                        foreach ( var asmUri in GetAssemblyUrisFromDeploymentXamlStream( manifestSri.Stream ) )
                        {
                            var assemblySri = Application.GetResourceStream( xapSri, asmUri );
                            var asmPart = new AssemblyPart();
                            if ( asmUri.ToString().ToLower().Equals( _appDll.ToLower() + ".dll" ) )
                            {
                                asm = asmPart.Load( assemblySri.Stream );
                            }
                            else
                            {
                                asmPart.Load( assemblySri.Stream );
                            }
                        }
                        _DicAssemblys.Add( _applicationInfo.ApplicationID, asm );
                        xapSri.Stream.Close();
                    }
                    else
                    {
                        AssemblyPart part = new AssemblyPart();
                        Assembly asm = part.Load( xapStream );
                        
                        _DicAssemblys.Add( _applicationInfo.ApplicationID, asm );
                    }
                }
                CompletedCallBack();
                //GCManager.Instance.FreeMemory(this);
            }
            catch (Exception ex)
            {
                //ServiceProxy.AppLog.WriteLog("", "TryLoadXap Exception:" + ex.Message, ex.StackTrace, LogLevel.SysError);
            }
        }

        #endregion Methods
    }
}