using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using UnityEngine;

// Token: 0x020014FB RID: 5371
public class XmlFile
{
	// Token: 0x170013B7 RID: 5047
	// (get) Token: 0x0600A916 RID: 43286 RVA: 0x003ED710 File Offset: 0x003EB910
	// (set) Token: 0x0600A917 RID: 43287 RVA: 0x003ED718 File Offset: 0x003EB918
	public bool Loaded
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x0600A918 RID: 43288 RVA: 0x003ED721 File Offset: 0x003EB921
	public XmlFile(XmlFile _orig)
	{
		this.Directory = _orig.Directory;
		this.Filename = _orig.Filename;
		this.Loaded = _orig.Loaded;
		this.XmlDoc = new XDocument(_orig.XmlDoc);
	}

	// Token: 0x0600A919 RID: 43289 RVA: 0x003ED760 File Offset: 0x003EB960
	public XmlFile(string _name)
	{
		this.Directory = GameIO.GetGameDir("Data/Config");
		this.Filename = ((!_name.EndsWith(".xml", StringComparison.OrdinalIgnoreCase)) ? (_name + ".xml") : _name);
		this.load(this.Directory, this.Filename, false);
	}

	// Token: 0x0600A91A RID: 43290 RVA: 0x003ED7B8 File Offset: 0x003EB9B8
	public XmlFile(string _text, string _directory, string _filename, bool _throwExc = false)
	{
		this.Directory = _directory;
		this.Filename = _filename;
		this.toXml(_text, _filename, _throwExc);
	}

	// Token: 0x0600A91B RID: 43291 RVA: 0x003ED7D8 File Offset: 0x003EB9D8
	public XmlFile(TextAsset _ta)
	{
		using (MemoryStream memoryStream = new MemoryStream(_ta.bytes))
		{
			this.load(memoryStream, _ta.name, false);
		}
	}

	// Token: 0x0600A91C RID: 43292 RVA: 0x003ED824 File Offset: 0x003EBA24
	public XmlFile(byte[] _data, bool _throwExc = false)
	{
		using (MemoryStream memoryStream = new MemoryStream(_data))
		{
			this.load(memoryStream, null, _throwExc);
		}
	}

	// Token: 0x0600A91D RID: 43293 RVA: 0x003ED864 File Offset: 0x003EBA64
	public XmlFile(string _directory, string _file, bool _loadAsync = false, bool _throwExc = false)
	{
		XmlFile <>4__this = this;
		this.Directory = _directory;
		this.Filename = ((!_file.EndsWith(".xml", StringComparison.OrdinalIgnoreCase)) ? (_file + ".xml") : _file);
		if (!_loadAsync)
		{
			this.load(_directory, this.Filename, false);
			return;
		}
		ThreadManager.AddSingleTask(delegate(ThreadManager.TaskInfo _)
		{
			<>4__this.load(_directory, <>4__this.Filename, false);
		}, null, null, true);
	}

	// Token: 0x0600A91E RID: 43294 RVA: 0x003ED8E8 File Offset: 0x003EBAE8
	public XmlFile(string _directory, string _file, Action<Exception> _doneCallback)
	{
		XmlFile <>4__this = this;
		this.Directory = _directory;
		this.Filename = ((!_file.EndsWith(".xml", StringComparison.OrdinalIgnoreCase)) ? (_file + ".xml") : _file);
		ThreadManager.AddSingleTask(delegate(ThreadManager.TaskInfo _)
		{
			try
			{
				<>4__this.load(_directory, <>4__this.Filename, false);
				_doneCallback(null);
			}
			catch (Exception ex)
			{
				_doneCallback(ex);
			}
		}, null, null, true);
	}

	// Token: 0x0600A91F RID: 43295 RVA: 0x003ED959 File Offset: 0x003EBB59
	public XmlFile(Stream _stream)
	{
		this.load(_stream, null, false);
	}

	// Token: 0x0600A920 RID: 43296 RVA: 0x003ED96C File Offset: 0x003EBB6C
	public string SerializeToString(bool _minified = false)
	{
		string text;
		using (StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture))
		{
			using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter, XmlFile.GetWriterSettings(!_minified, Encoding.UTF8)))
			{
				this.XmlDoc.WriteTo(xmlWriter);
			}
			text = stringWriter.ToString();
		}
		return text;
	}

	// Token: 0x0600A921 RID: 43297 RVA: 0x003ED9E4 File Offset: 0x003EBBE4
	public byte[] SerializeToBytes(bool _minified = false, Encoding _encoding = null)
	{
		PooledExpandableMemoryStream pooledExpandableMemoryStream = MemoryPools.poolMemoryStream.AllocSync(true);
		this.SerializeToStream(pooledExpandableMemoryStream, _minified, _encoding);
		byte[] array = pooledExpandableMemoryStream.ToArray();
		MemoryPools.poolMemoryStream.FreeSync(pooledExpandableMemoryStream);
		return array;
	}

	// Token: 0x0600A922 RID: 43298 RVA: 0x003EDA18 File Offset: 0x003EBC18
	public void SerializeToFile(string _path, bool _minified = false, Encoding _encoding = null)
	{
		using (Stream stream = SdFile.Create(_path))
		{
			this.SerializeToStream(stream, _minified, _encoding);
		}
	}

	// Token: 0x0600A923 RID: 43299 RVA: 0x003EDA54 File Offset: 0x003EBC54
	public void SerializeToStream(Stream _stream, bool _minified = false, Encoding _encoding = null)
	{
		if (_encoding == null)
		{
			_encoding = Encoding.UTF8;
		}
		using (XmlWriter xmlWriter = XmlWriter.Create(_stream, XmlFile.GetWriterSettings(!_minified, _encoding)))
		{
			this.XmlDoc.WriteTo(xmlWriter);
		}
	}

	// Token: 0x0600A924 RID: 43300 RVA: 0x003EDAA4 File Offset: 0x003EBCA4
	[PublicizedFrom(EAccessModifier.Private)]
	public static XmlWriterSettings GetWriterSettings(bool _indent, Encoding _encoding)
	{
		return new XmlWriterSettings
		{
			Encoding = _encoding,
			Indent = _indent,
			OmitXmlDeclaration = true
		};
	}

	// Token: 0x0600A925 RID: 43301 RVA: 0x003EDAC0 File Offset: 0x003EBCC0
	[PublicizedFrom(EAccessModifier.Private)]
	public void toXml(string _data, string _filename = null, bool _throwExc = false)
	{
		try
		{
			this.XmlDoc = XDocument.Parse(_data, LoadOptions.SetLineInfo);
			this.Loaded = true;
		}
		catch (Exception ex)
		{
			if (_throwExc)
			{
				throw;
			}
			Log.Error("Failed parsing XML" + ((!string.IsNullOrEmpty(_filename)) ? (" (" + _filename + ")") : "") + ":");
			Log.Exception(ex);
		}
	}

	// Token: 0x0600A926 RID: 43302 RVA: 0x003EDB34 File Offset: 0x003EBD34
	[PublicizedFrom(EAccessModifier.Private)]
	public void load(byte[] _bytes, bool _throwExc = false)
	{
		using (MemoryStream memoryStream = new MemoryStream(_bytes))
		{
			this.load(memoryStream, null, _throwExc);
		}
	}

	// Token: 0x0600A927 RID: 43303 RVA: 0x003EDB70 File Offset: 0x003EBD70
	[PublicizedFrom(EAccessModifier.Private)]
	public void load(string _directory, string _file, bool _throwExc = false)
	{
		if (_file == null)
		{
			SdFileInfo[] directory = GameIO.GetDirectory(_directory, "*.xml");
			if (directory.Length == 0)
			{
				return;
			}
			_file = directory[0].Name;
		}
		string text = _directory + "/" + _file;
		using (Stream stream = SdFile.OpenRead(text))
		{
			this.load(stream, text, _throwExc);
		}
	}

	// Token: 0x0600A928 RID: 43304 RVA: 0x003EDBD4 File Offset: 0x003EBDD4
	[PublicizedFrom(EAccessModifier.Private)]
	public void load(Stream _stream, string _name = null, bool _throwExc = false)
	{
		try
		{
			using (StreamReader streamReader = new StreamReader(_stream, Encoding.UTF8))
			{
				this.XmlDoc = XDocument.Load(streamReader, LoadOptions.SetLineInfo);
				this.Loaded = true;
			}
		}
		catch (Exception ex)
		{
			if (_throwExc)
			{
				throw;
			}
			Log.Error("Failed parsing XML" + ((!string.IsNullOrEmpty(_name)) ? (" (" + _name + ")") : "") + ":");
			Log.Exception(ex);
		}
	}

	// Token: 0x0600A929 RID: 43305 RVA: 0x003EDC6C File Offset: 0x003EBE6C
	public void RemoveComments()
	{
		this.XmlDoc.DescendantNodes().OfType<XComment>().Remove<XComment>();
	}

	// Token: 0x0600A92A RID: 43306 RVA: 0x003EDC83 File Offset: 0x003EBE83
	public bool GetXpathResults(string _xpath, out List<XObject> _matchList)
	{
		if (this.tempXpathMatchList == null)
		{
			this.tempXpathMatchList = new List<XObject>();
		}
		if (this.GetXpathResultsInList(_xpath, this.tempXpathMatchList))
		{
			_matchList = this.tempXpathMatchList;
			return true;
		}
		_matchList = null;
		return false;
	}

	// Token: 0x0600A92B RID: 43307 RVA: 0x003EDCB5 File Offset: 0x003EBEB5
	public int ClearXpathResults()
	{
		int count = this.tempXpathMatchList.Count;
		this.tempXpathMatchList.Clear();
		return count;
	}

	// Token: 0x0600A92C RID: 43308 RVA: 0x003EDCD0 File Offset: 0x003EBED0
	public bool GetXpathResultsInList(string _xpath, List<XObject> _matchList)
	{
		if (_matchList == null)
		{
			throw new ArgumentNullException("_matchList", "GetXpathResultsInList can not be called with a null _matchList argument");
		}
		_matchList.Clear();
		IEnumerable enumerable = this.XmlDoc.XPathEvaluate(_xpath) as IEnumerable;
		if (enumerable == null)
		{
			return false;
		}
		_matchList.AddRange(enumerable.Cast<XObject>());
		return _matchList.Count != 0;
	}

	// Token: 0x04007D13 RID: 32019
	public readonly string Directory;

	// Token: 0x04007D14 RID: 32020
	public readonly string Filename;

	// Token: 0x04007D16 RID: 32022
	public XDocument XmlDoc;

	// Token: 0x04007D17 RID: 32023
	[PublicizedFrom(EAccessModifier.Private)]
	public List<XObject> tempXpathMatchList;
}
