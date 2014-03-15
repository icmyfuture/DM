using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization.Formatters.Soap;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;

namespace DM.Common.Utility
{
    /// <summary>
    /// 对象序列化类
    /// </summary>
    public class XmlHelper
    {
        /// <summary>
        /// 
        /// </summary>
        public static Hashtable MXmlSerializerList = new Hashtable();

        private XmlHelper()
        {
        }

        /// <summary>
        /// 获取一个XmlNode节点，如果该节点未空则抛出异常。
        /// </summary>
        /// <param name="xnNode">被查询的节点</param>
        /// <param name="sXPath">查询XPath</param>
        /// <returns>查询结果</returns>
        public static XmlNode GetXmlNode(XmlNode xnNode, string sXPath)
        {
            if (xnNode == null)
            {
                const string sError = "XmlHelper.GetXmlNode() failed, input XML node is empty.";
                throw new Exception(sError);
            }

            XmlNode xn = xnNode.SelectSingleNode(sXPath);
            if (xn == null)
            {
                string sError = string.Format("Execute XPath failed, no XML node was found! /r/nXPath:({0})/r/nXML:({0})", sXPath);
                throw new Exception(sError);
            }

            return xn;
        }

        /// <summary>
        /// 获取一个XML节点的Value，如果该节点不存在则返回默认值
        /// </summary>
        /// <param name="xnNode">被查询的节点</param>
        /// <param name="sXPath">查询的XPath</param>
        /// <param name="sDefault">默认值</param>
        /// <returns>查询的结果</returns>
        public static string GetXmlValue(XmlNode xnNode, string sXPath, string sDefault)
        {
            if (xnNode == null)
            {
                const string sError = "XmlHelper.GetXmlValue() failed, input XML node is empty.";
                throw new Exception(sError);
            }

            XmlNode xn = xnNode.SelectSingleNode(sXPath);
            if (xn == null)
            {
                return sDefault;
            }

            return xn.Value;
        }

        /// <summary>
        /// 获取一个XML节点的InnerText，如果该节点不存在则返回默认值
        /// </summary>
        /// <param name="xnNode">被查询的节点</param>
        /// <param name="sXPath">查询的XPath</param>
        /// <param name="sDefault">默认值</param>
        /// <returns>查询的结果</returns>		
        public static string GetXmlText(XmlNode xnNode, string sXPath, string sDefault)
        {
            if (xnNode == null)
            {
                const string sError = "XmlHelper.GetXmlText() failed, input XML node is empty.";
                throw new Exception(sError);
            }

            XmlNode xn = xnNode.SelectSingleNode(sXPath);
            if (xn == null)
            {
                return sDefault;
            }

            return xn.InnerText;
        }

        /// <summary>
        /// 将一个对象转换为XML
        /// </summary>
        /// <param name="o">对象</param>
        /// <returns>XML字符串</returns>
        public static string Object2Xml(object o)
        {
            return Object2Xml(o, o.GetType());
        }

        public static string Object2Xml(object o, Type type)
        {
            try
            {
                string retVal;
                lock (MXmlSerializerList.SyncRoot)
                {
                    XmlSerializer serializer;
                    if (MXmlSerializerList.Contains(type) == false)
                    {
                        serializer = new XmlSerializer(type);
                        MXmlSerializerList.Add(type, serializer);
                    }
                    else
                    {
                        serializer = (XmlSerializer)MXmlSerializerList[type];
                    }
                    var s = new MemoryStream();
                    serializer.Serialize(s, o);
                    s.Seek(0, SeekOrigin.Begin);

                    TextReader tr = new StreamReader(s);

                    retVal = tr.ReadToEnd();

                    tr.Close();
                    s.Close();
                }
                return retVal;
            }
            catch (Exception e)
            {
                const string sError = "Serialize object to XML failed.";
                throw new Exception(sError, e);
            }
        }

        public static string Object2Xml(object o, Type type, XmlSerializerNamespaces np)
        {
            try
            {
                string retVal;
                lock (MXmlSerializerList.SyncRoot)
                {
                    XmlSerializer serializer;
                    if (MXmlSerializerList.Contains(type) == false)
                    {
                        serializer = new XmlSerializer(type);
                        MXmlSerializerList.Add(type, serializer);
                    }
                    else
                    {
                        serializer = (XmlSerializer)MXmlSerializerList[type];
                    }
                    var s = new MemoryStream();
                    serializer.Serialize(s, o, np);
                    s.Seek(0, SeekOrigin.Begin);

                    TextReader tr = new StreamReader(s);

                    retVal = tr.ReadToEnd();

                    tr.Close();
                    s.Close();
                }
                return retVal;
            }
            catch (Exception e)
            {
                const string sError = "Serialize object to XML failed.";
                throw new Exception(sError, e);
            }
        }

        public static string Object2SoapXml(object o)
        {
            if (o == null)
            {
                return null;
            }

            var bf = new SoapFormatter();
            var ms = new MemoryStream();
            try
            {
                bf.Serialize(ms, o);
                ms.Position = 0;
                string s = Encoding.UTF8.GetString(ms.ToArray());
                return s;
            }
            catch (Exception e)
            {
                const string sError = "Convert object into SOAP XML failed.";
                throw new Exception(sError, e);
            }
            finally
            {
                ms.Close();
            }
        }

        public static object SoapXml2Object(string sXml)
        {
            if (string.IsNullOrEmpty(sXml))
            {
                return null;
            }

            var bf = new SoapFormatter();
            byte[] bt = Encoding.UTF8.GetBytes(sXml);
            var ms = new MemoryStream(bt);
            try
            {
                object ts = bf.Deserialize(ms);
                return ts;
            }
            catch (Exception e)
            {
                const string sError = "Convert SOAP XML into object failed.";
                throw new Exception(sError, e);
            }
            finally
            {
                ms.Close();
            }
        }

        //		public static object AnyTypeXmlNode2Object(object oAnyType, Type tpObject)
        //		{
        //			if (!(oAnyType is Array))
        //			{
        //				return null;
        //			}
        //			
        //			Array oa = (Array) oAnyType;
        //			if (oa.Length == 0)
        //			{
        //				return null;
        //			}
        //			
        //			if (oa.Length > 1)
        //			{
        //				throw new Exception("Source format error, can't convert to target object");
        //			}
        //			
        //			if (!(oa.GetValue(0) is XmlNode))
        //			{
        //				throw new Exception("Source format error, can't convert to target object");
        //			}
        //			
        //			XmlNode xn = (XmlNode) oa.GetValue(0);
        //			return Xml2Object(xn.OuterXml, tpObject);
        //		}
        //		
        //		public static object Object2AnyTypeXmlNode(object obj, Type tpObject)
        //		{
        //			if (obj == null)
        //			{
        //				return new object();
        //			}
        //			
        //			string sData = Object2Xml(obj, tpObject);
        //			XmlDocument xd = new XmlDocument();
        //			xd.LoadXml(sData);			
        //			
        //			Array oa = Array.CreateInstance(typeof(XmlElement), 1);
        //			oa.SetValue(xd.DocumentElement, 0);
        //			return oa;
        //		}		

        /// <summary>
        /// 对象序列化二进制字节数组
        /// </summary>
        /// <returns></returns>
        public static byte[] Object2BinaryByte(object obj)
        {
            var oMemStream = new MemoryStream();
            byte[] bData;
            try
            {
                var bf = new BinaryFormatter();
                bf.Serialize(oMemStream, obj);
                bData = oMemStream.ToArray();
            }
            //string iInfo = Encoding.ASCII.GetString(bData);
            finally
            {
                oMemStream.Close();
            }
            return bData;
        }

        /// <summary>
        /// 二进制字节数组序列化对象
        /// </summary>
        /// <param name="bData"></param>
        /// <returns></returns>
        public static object BinaryByteToObject(byte[] bData)
        {
            var oMemStream = new MemoryStream(bData);
            object obj;
            try
            {
                oMemStream.Position = 0;
                var bf = new BinaryFormatter();
                obj = bf.Deserialize(oMemStream);
            }
            finally
            {
                oMemStream.Close();
            }
            return obj;
        }

        /// <summary>
        /// 对象序列化二进制字节数组转换的ASCII码
        /// </summary>
        /// <returns></returns>
        public static string Object2BinaryString(object obj)
        {
            if (obj == null)
            {
                return null;
            }

            var oMemStream = new MemoryStream();
            string sInfo;
            try
            {
                var bf = new BinaryFormatter();
                bf.Serialize(oMemStream, obj);
                byte[] bData = oMemStream.ToArray();

                //sInfo = Encoding.UTF8.GetString(bData);
                sInfo = AlphaEncoder.Instance.Encode(bData);
            }
            finally
            {
                oMemStream.Close();
            }
            return sInfo;
        }

        /// <summary>
        /// 二进制字节数组转换的Ascii序列化对象
        /// </summary>
        /// <returns></returns>
        public static object BinaryStringToObject(string sData)
        {
            if (string.IsNullOrEmpty(sData))
            {
                return null;
            }

            //byte[] bData = Encoding.UTF8.GetBytes(sData);
            MemoryStream oMemStream = null;
            object obj;
            try
            {
                byte[] bData = AlphaEncoder.Instance.Decode(sData);
                oMemStream = new MemoryStream(bData) {Position = 0};
                var bf = new BinaryFormatter();
                obj = bf.Deserialize(oMemStream);
            }
            catch (Exception)
            {
                //如果 按照二进制来序列化实体有问题话 就用Soap来序列化实体
                obj = SoapXml2Object(sData);
            }
            finally
            {
                if (oMemStream != null)
                {
                    oMemStream.Close();
                }
            }
            return obj;
        }

        public static bool IsEmpty(string str)
        {
            bool bEmpty = false;
            if (str == null)
            {
                bEmpty = true;
            }
            else
            {
                if (str.Length <= 0)
                {
                    bEmpty = true;
                }
            }
            return bEmpty;
        }
        public static string GetXmlValue(XmlNode xn, string sDefault)
        {
            if (xn == null)
            {
                return sDefault;
            }
            return xn.Value;
        }

        public static XmlNode GetXmlNode(string sXPath, XmlNode xn)
        {
            if (xn == null)
            {
                throw new XmlException(string.Format("Searched Node is null！XPath executing failed.(XPath:{0})", sXPath));
            }

            XmlNode xnRes = xn.SelectSingleNode(sXPath);
            if (xnRes == null)
            {
                throw new XmlException(string.Format("Searched value is not exist！(XPath:{0} \r\n XML:{1})", sXPath, xn.OuterXml));
            }

            return xnRes;
        }

        public static void SaveTextFile(string sFileName, string sString)
        {
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(sFileName, false, Encoding.UTF8);
                sw.Write(sString);
                sw.Close();
            }
            catch (Exception e)
            {
                var strBuilder = new StringBuilder("SaveTextFile SaveConfig Error: ");
                strBuilder.Append(e);
                throw new Exception(strBuilder.ToString());
            }
            finally
            {
                if (sw != null)
                    sw.Close();
            }
        }

        public static string ReadTextFile(string sFileName)
        {
            TextReader tr = new StreamReader(sFileName);
            string sXML;
            try
            {
                sXML = tr.ReadToEnd();
            }
            finally
            {
                tr.Close();
            }
            return sXML;
        }
    }

    /// <summary>
    /// 简单的字母编码,快速的把二进制数据用16进制翻译成16进制的数字值
    /// </summary>
    public class AlphaEncoder
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        private AlphaEncoder()
        {
            for (int i = 0; i <= 255; i++)
            {
                string sInfo = i.ToString("X");
                if (sInfo.Length == 1) sInfo = "0" + sInfo;
                _htAlpha[sInfo] = (byte)i;
                _htNumber[(byte)i] = sInfo;
            }

            _htSixthNumber.Add('0', (byte)0);
            _htSixthNumber.Add('1', (byte)1);
            _htSixthNumber.Add('2', (byte)2);
            _htSixthNumber.Add('3', (byte)3);
            _htSixthNumber.Add('4', (byte)4);
            _htSixthNumber.Add('5', (byte)5);
            _htSixthNumber.Add('6', (byte)6);
            _htSixthNumber.Add('7', (byte)7);
            _htSixthNumber.Add('8', (byte)8);
            _htSixthNumber.Add('9', (byte)9);
            _htSixthNumber.Add('A', (byte)10);
            _htSixthNumber.Add('B', (byte)11);
            _htSixthNumber.Add('C', (byte)12);
            _htSixthNumber.Add('D', (byte)13);
            _htSixthNumber.Add('E', (byte)14);
            _htSixthNumber.Add('F', (byte)15);
        }

        public static AlphaEncoder Instance = new AlphaEncoder();

        private readonly Hashtable _htAlpha = new Hashtable();
        private readonly Hashtable _htNumber = new Hashtable();
        private readonly Hashtable _htSixthNumber = new Hashtable();
        /// <summary>
        /// 编码数据
        /// </summary>
        public string Encode(byte[] bData)
        {
            var sChar = new char[bData.Length * 2];
            for (int i = 0; i < bData.Length; i++)
            {
                byte bItem = bData[i];
                var sAlpha = (string)_htNumber[bItem];
                sChar[i * 2] = sAlpha[0];
                sChar[i * 2 + 1] = sAlpha[1];
            }
            return new string(sChar);
        }

        /// <summary>
        /// 解码数据
        /// </summary>
        public byte[] Decode(string sData)
        {
            var bData = new byte[sData.Length / 2];
            for (int i = 0; i < sData.Length; i = i + 2)
            {
                bData[i / 2] = (byte)((byte)_htSixthNumber[sData[i]] * 16 + (byte)_htSixthNumber[sData[i + 1]]);
                //cChar[0] = sData[i];
                //cChar[1] = sData[i+1];
                ////这个地方可以优化
                //bData[i / 2] = (byte)ht_Alpha[new string(cChar)];
            }
            return bData;
        }
    }

    #region XmlSmart
    public class XmlSmart
    {
        private XmlNode _mPp;
        private XmlNamespaceManager _xnm;
        public XmlSmart()
        {
        }

        public XmlSmart(XmlNode pp, XmlNamespaceManager x)
        {
            _mPp = pp;
            _xnm = x;
        }

        static public XmlSmart Read(Stream stream, Dictionary<string, string> mapName)
        {
            var result = new XmlSmart();
            var pDOC = new XmlDocument();
            pDOC.Load(stream);

            result._mPp = pDOC.DocumentElement;

            if (pDOC.DocumentElement != null && string.IsNullOrEmpty(pDOC.DocumentElement.NamespaceURI) == false)
            {
                result._xnm = new XmlNamespaceManager(pDOC.NameTable);
                result._xnm.AddNamespace("default", pDOC.DocumentElement.NamespaceURI);
            }
            if (mapName != null)
            {
                if (result._xnm == null)
                {
                    result._xnm = new XmlNamespaceManager(pDOC.NameTable);
                }

                foreach (KeyValuePair<string, string> kvp in mapName)
                {
                    result._xnm.AddNamespace(kvp.Key, kvp.Value);
                }
            }
            return result;
        }

        static public XmlSmart Read(string sXml, bool bLoadFile)
        {
            return Read(sXml, bLoadFile, null);
        }

        static public XmlSmart Read(string sXml, bool bLoadFile, Dictionary<string, string> mapName)
        {
            var result = new XmlSmart();
            var pDOC = new XmlDocument();

            if (bLoadFile)
            {
                pDOC.Load(sXml);
            }
            else
            {
                pDOC.LoadXml(sXml);
            }

            result._mPp = pDOC.DocumentElement;

            if (pDOC.DocumentElement != null && string.IsNullOrEmpty(pDOC.DocumentElement.NamespaceURI) == false)
            {
                result._xnm = new XmlNamespaceManager(pDOC.NameTable);
                result._xnm.AddNamespace("default", pDOC.DocumentElement.NamespaceURI);
            }
            if (mapName != null)
            {
                if (result._xnm == null)
                {
                    result._xnm = new XmlNamespaceManager(pDOC.NameTable);
                }

                foreach (KeyValuePair<string, string> kvp in mapName)
                {
                    result._xnm.AddNamespace(kvp.Key, kvp.Value);
                }
            }
            return result;
        }

        static public XmlSmart ReadFile(string sFileName, bool bAutoEnCoding)
        {
            if (bAutoEnCoding)
            {
                using (var sr = new FileStream(sFileName, FileMode.Open))
                {
                    using (new BinaryReader(sr))
                    {

                        var aabyte = new byte[sr.Length];
                        sr.Read(aabyte, 0, int.Parse(sr.Length.ToString(CultureInfo.InvariantCulture)));
                        string sXml = GetString(aabyte, new Encoding[] { });
                        return Read(sXml, false);

                    }
                }
            }
            return Read(sFileName, true);
        }

        //encodings参数可以不填 (那么将默认猜测gb2312,utf-8,unicode三种编码)
        static public string GetString(byte[] buffer, params Encoding[] encodings)
        {
            if (encodings.Length < 1)
            {
                encodings = new[] { Encoding.GetEncoding("gb2312"), Encoding.UTF8, Encoding.Unicode };
            }

            foreach (Encoding encoding in encodings)
            {
                //按此编码获取字符串
                string str = encoding.GetString(buffer);

                //反向转换一次，看是否可逆
                byte[] tempBytes = encoding.GetBytes(str);

                //若长度不一样，编码错误
                if (buffer.Length != tempBytes.Length)
                {
                    continue;
                }
                for (int i = 0; i < buffer.Length; i++)
                {
                    //若其中任何一个字节不一样，也说明编码错误
                    if (buffer[i] != tempBytes[i])
                    {
                        goto continueforeach;
                    }
                }

                return str;

            continueforeach:
                ;
            }
            throw new Exception("没有找到正确的编码或数据错误。");
        }

        public void RemoveNode(string bstrName)
        {
            XmlNode pNode = _mPp.SelectSingleNode(bstrName, _xnm);
            if (pNode != null)
            {
                _mPp.RemoveChild(pNode);
            }
        }

        public void Remove()
        {
            if (_mPp.ParentNode != null)
            {
                _mPp.ParentNode.RemoveChild(_mPp);
            }
        }

        public XmlSmart SetNode(string bstrName, object varValue)
        {
            XmlNode pNode = _mPp.SelectSingleNode(bstrName, _xnm);
            if (pNode != null)
            {
                pNode.InnerText = string.Format("{0}", varValue);
                if (pNode == _mPp)
                {
                    return this;
                }
                return new XmlSmart(pNode, _xnm);
            }
            return AddNode(bstrName, varValue);
        }

        public string GetMap(string bsName)
        {
            //m_pp.Attributes
            //m_pp.Attributes.Item(
            string bsResult = string.Empty;
            if(_mPp.Attributes!=null)
            {
                for (int i = 0; i < _mPp.Attributes.Count; i++)
                {

                    XmlNode xn = _mPp.Attributes.Item(i);

                    if (xn.Name == bsName)
                    {
                        bsResult = xn.InnerText;
                        break;
                    }
                }
            }
            return bsResult;
        }

        public Dictionary<string, string> GetMaps()
        {
            var result = new Dictionary<string, string>();
            if (_mPp.Attributes != null)
            {
                foreach (XmlNode xn in _mPp.Attributes)
                {
                    result.Add(xn.Name, xn.InnerText);
                }
            }
            return result;
        }


        public XmlSmart SetMap(string bstrName, object varValue)
        {
            if (_mPp.Attributes != null)
            {
                foreach (XmlNode xn in _mPp.Attributes)
                {
                    if (bstrName == xn.Name)
                    {
                        xn.InnerText = varValue.ToString();
                        return this;
                    }
                }
            }

            //Create a new attribute.

            if (_mPp.OwnerDocument != null)
            {
                XmlNode attr = _mPp.OwnerDocument.CreateNode(XmlNodeType.Attribute, bstrName, string.Empty);
                attr.InnerText = string.Format("{0}", varValue);

                //Add the attribute to the document.
                if (_mPp.Attributes != null) _mPp.Attributes.SetNamedItem(attr);
            }
            return this;
        }
        public void Save(string bstrFileName)
        {
            if (_mPp.OwnerDocument != null) _mPp.OwnerDocument.Save(bstrFileName);
        }

        public void Save(Stream stream)
        {
            if (_mPp.OwnerDocument != null) _mPp.OwnerDocument.Save(stream);
        }

        //read
        public string GetNode(string bstrName)
        {
            XmlNodeList pList = _mPp.SelectNodes(bstrName, _xnm);
            if (pList != null)
            {
                string bstrRet = "";
                for (int i = 0; i < pList.Count; i++)
                {

                    XmlNode pNode = pList.Item(i);

                    if (pNode != null)
                    {
                        string bstrValue = pNode.InnerText;
                        if (bstrValue.Length == 0)
                            continue;
                        if (bstrRet.Length > 0)
                            bstrRet = bstrRet + "," + bstrValue;
                        else
                            bstrRet = bstrValue;
                    }
                }
                return bstrRet;
            }
            return string.Empty;
        }

        public string GetValue()
        {
            return string.Format("{0}", _mPp.InnerText);
        }

        public void SetValue(string bsValue)
        {
            _mPp.InnerText = bsValue;
        }

        public string GetName()
        {
            return _mPp.Name;
        }

        public string GetXML()
        {
            return _mPp.OuterXml;
            //return m_pp.InnerXml;
        }

        /*
        XmlSmart MoveUp()
        {
            return this.MoveUp(1);
        }

        public void MoveUp(int nLevel)
        {
            for(int i=0;i<nLevel;i++)
            {
				
                XmlNode pNode=m_pCurNode.ParentNode;
                if(pNode != null)
                {		
                    m_pCurNode=pNode;
					
                    string bstrName=m_pCurNode.Name;
                }
                else
                    break;
            }
        }*/

        public XmlSmart Find(string bstrName)
        {
            if (_mPp != null)
            {
                XmlNode pNode = _mPp.SelectSingleNode(bstrName, _xnm);
                if (pNode != null)
                {
                    return new XmlSmart(pNode, _xnm);
                }
            }
            return null;
        }

        public XmlListSmart Finds()
        {
            return new XmlListSmart(_mPp.ChildNodes, _xnm);
        }
        public XmlListSmart Finds(string bstrName)
        {
            return new XmlListSmart(_mPp.SelectNodes(bstrName, _xnm), _xnm);
        }


        public XmlSmart Parent()
        {
            return new XmlSmart(_mPp.ParentNode, _xnm);
        }

        public void Sort(string sXpath)
        {
            var map = new SortedDictionary<string, XmlSmart>();
            XmlSmart xmlParent = null;
            foreach (XmlSmart x2 in Finds(sXpath))
            {
                if (xmlParent == null)
                {
                    xmlParent = x2.Parent();
                }
                map.Add(x2.GetMap("en"), x2);
                x2.Remove();

            }

            foreach (XmlSmart xs in map.Values)
            {
                if (xmlParent != null && xs != null)
                {
                    xmlParent.AddNode(xs);
                }
            }
            //xml.Find("en2cn/").AddNodes(map.Values);
        }

        public XmlSmart AddNode(string bstrName)
        {
            if (_mPp == null)
            {
                return Write(bstrName);
            }
            if (_mPp.OwnerDocument != null)
            {
                XmlElement spnewEle = _mPp.OwnerDocument.CreateElement(bstrName);
                _mPp.AppendChild(spnewEle);
                XmlNode spCurNode = spnewEle;
                //IXMLDOMNodePtr spCurNode = Pbase->m_pp->appendChild(spnewEle);	
                return new XmlSmart(spCurNode, _xnm);
            }
            return null;
        }

        public void AddRemark(string sRemark)
        {
            if (_mPp.OwnerDocument != null)
            {
                XmlComment newComment = _mPp.OwnerDocument.CreateComment(sRemark);
                _mPp.AppendChild(newComment);
            }
            //m_pp.OwnerDocument.InsertBefore(newComment,m_pp.OwnerDocument.DocumentElement);
        }

        public XmlSmart AddNode(string bstrName, object varValue)
        {
            if (_mPp.OwnerDocument != null)
            {
                XmlElement spnewEle = _mPp.OwnerDocument.CreateElement(bstrName);

                spnewEle.InnerText = string.Format("{0}", varValue);
                _mPp.AppendChild(spnewEle);
                XmlNode spCur = spnewEle;
                return new XmlSmart(spCur, _xnm);
            }
            return null;
        }

        public XmlSmart AddNode(XmlSmart oNode)
        {
            if (oNode._mPp.OwnerDocument != _mPp.OwnerDocument)
            {
                if (_mPp.OwnerDocument != null)
                {
                    XmlElement spnewEle = _mPp.OwnerDocument.CreateElement(oNode._mPp.Name);
                    string sXml = oNode._mPp.OuterXml;
                    spnewEle.InnerXml = sXml;
                    if (spnewEle.ChildNodes.Count > 0)
                    {
                        oNode._mPp = spnewEle.ChildNodes[0];
                    }
                }
            }


            _mPp.AppendChild(oNode._mPp);
            XmlNode spCur = oNode._mPp;
            return new XmlSmart(spCur, _xnm);


        }


        public XmlSmart Write(string bstrRoot)
        {
            var pDOC = new XmlDocument();


            //VARIANT_BOOL varBool=m_pDOC->loadXML(L"<?xml version=\"1.0\" ?>");	



            XmlElement spnewEle = pDOC.CreateElement(bstrRoot);
            _mPp = pDOC.AppendChild(spnewEle);
            return this;
        }

        public XmlSmart FindXmlns(string xpath)
        {
            var sb = new StringBuilder();
            foreach (var s in xpath.Split('/'))
            {
                if (sb.Length > 0)
                {
                    sb.Append('/');
                }
                if (s != string.Empty)
                {
                    if (s.Contains(":") == false)
                    {
                        sb.Append("default:");
                    }
                    sb.Append(s);
                }
            }
            //Console.WriteLine(sb.ToString());
            return Find(sb.ToString());
        }

        public string FindXmlnsValue(string xpath)
        {
            return FindXmlnsValue(xpath, null);
        }
        public string FindXmlnsValue(string xpath, string attribute)
        {
            var v = FindXmlns(xpath);
            if (v != null)
            {
                if (string.IsNullOrEmpty(attribute))
                {
                    return v.GetValue();
                }
                return v.GetMap(attribute);
            }
            return string.Empty;
        }
    }

    public class XmlSmartEnumerator : IEnumerator
    {
        private readonly IEnumerator _enumer;
        private readonly XmlNamespaceManager _xnm;

        public XmlSmartEnumerator(XmlNodeList pp, XmlNamespaceManager x)
        {
            _enumer = pp.GetEnumerator();
            _xnm = x;
        }

        #region IEnumerator 成员

        public object Current
        {
            get
            {
                return new XmlSmart((XmlNode)_enumer.Current, _xnm);
            }
        }

        public bool MoveNext()
        {
            return _enumer.MoveNext();
        }

        public void Reset()
        {
            _enumer.Reset();
        }

        #endregion
    }
    public class XmlListSmart : IEnumerable
    {
        private readonly XmlNodeList _mPp;
        private readonly XmlNamespaceManager _xnm;

        public XmlListSmart(XmlNodeList pp, XmlNamespaceManager x)
        {
            _mPp = pp;
            _xnm = x;
        }
        public int Count
        {
            get
            {
                return _mPp.Count;
            }
        }
        //public XmlSmart Item(int nIndex)
        //{
        //    return new XmlSmart(m_pp[nIndex]);
        //}






        #region IEnumerable 成员

        public IEnumerator GetEnumerator()
        {
            return new XmlSmartEnumerator(_mPp, _xnm);
        }

        #endregion
    }
    #endregion
}
