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

// Token: 0x0200152F RID: 5423
public class XmlFile
{
	// Token: 0x17001408 RID: 5128
	// (get) Token: 0x0600AA21 RID: 43553 RVA: 0x003F4968 File Offset: 0x003F2B68
	// (set) Token: 0x0600AA22 RID: 43554 RVA: 0x003F4970 File Offset: 0x003F2B70
	public bool Loaded
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x0600AA23 RID: 43555 RVA: 0x003F4979 File Offset: 0x003F2B79
	public XmlFile(XmlFile _orig)
	{
		this.Directory = _orig.Directory;
		this.Filename = _orig.Filename;
		this.Loaded = _orig.Loaded;
		this.XmlDoc = new XDocument(_orig.XmlDoc);
	}

	// Token: 0x0600AA24 RID: 43556 RVA: 0x003F49B8 File Offset: 0x003F2BB8
	public XmlFile(string _name)
	{
		this.Directory = GameIO.GetGameDir("Data/Config");
		this.Filename = ((!_name.EndsWith(".xml", StringComparison.OrdinalIgnoreCase)) ? (_name + ".xml") : _name);
		this.load(this.Directory, this.Filename, false);
	}

	// Token: 0x0600AA25 RID: 43557 RVA: 0x003F4A10 File Offset: 0x003F2C10
	public XmlFile(string _text, string _directory, string _filename, bool _throwExc = false)
	{
		this.Directory = _directory;
		this.Filename = _filename;
		this.toXml(_text, _filename, _throwExc);
	}

	// Token: 0x0600AA26 RID: 43558 RVA: 0x003F4A30 File Offset: 0x003F2C30
	public XmlFile(TextAsset _ta)
	{
		using (MemoryStream memoryStream = new MemoryStream(_ta.bytes))
		{
			this.load(memoryStream, _ta.name, false);
		}
	}

	// Token: 0x0600AA27 RID: 43559 RVA: 0x003F4A7C File Offset: 0x003F2C7C
	public XmlFile(byte[] _data, bool _throwExc = false)
	{
		using (MemoryStream memoryStream = new MemoryStream(_data))
		{
			this.load(memoryStream, null, _throwExc);
		}
	}

	// Token: 0x0600AA28 RID: 43560 RVA: 0x003F4ABC File Offset: 0x003F2CBC
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

	// Token: 0x0600AA29 RID: 43561 RVA: 0x003F4B40 File Offset: 0x003F2D40
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

	// Token: 0x0600AA2A RID: 43562 RVA: 0x003F4BB1 File Offset: 0x003F2DB1
	public XmlFile(Stream _stream)
	{
		this.load(_stream, null, false);
	}

	// Token: 0x0600AA2B RID: 43563 RVA: 0x003F4BC4 File Offset: 0x003F2DC4
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

	// Token: 0x0600AA2C RID: 43564 RVA: 0x003F4C3C File Offset: 0x003F2E3C
	public byte[] SerializeToBytes(bool _minified = false, Encoding _encoding = null)
	{
		PooledExpandableMemoryStream pooledExpandableMemoryStream = MemoryPools.poolMemoryStream.AllocSync(true);
		this.SerializeToStream(pooledExpandableMemoryStream, _minified, _encoding);
		byte[] array = pooledExpandableMemoryStream.ToArray();
		MemoryPools.poolMemoryStream.FreeSync(pooledExpandableMemoryStream);
		return array;
	}

	// Token: 0x0600AA2D RID: 43565 RVA: 0x003F4C70 File Offset: 0x003F2E70
	public void SerializeToFile(string _path, bool _minified = false, Encoding _encoding = null)
	{
		using (Stream stream = SdFile.Create(_path))
		{
			this.SerializeToStream(stream, _minified, _encoding);
		}
	}

	// Token: 0x0600AA2E RID: 43566 RVA: 0x003F4CAC File Offset: 0x003F2EAC
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

	// Token: 0x0600AA2F RID: 43567 RVA: 0x003F4CFC File Offset: 0x003F2EFC
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

	// Token: 0x0600AA30 RID: 43568 RVA: 0x003F4D18 File Offset: 0x003F2F18
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

	// Token: 0x0600AA31 RID: 43569 RVA: 0x003F4D8C File Offset: 0x003F2F8C
	[PublicizedFrom(EAccessModifier.Private)]
	public void load(byte[] _bytes, bool _throwExc = false)
	{
		using (MemoryStream memoryStream = new MemoryStream(_bytes))
		{
			this.load(memoryStream, null, _throwExc);
		}
	}

	// Token: 0x0600AA32 RID: 43570 RVA: 0x003F4DC8 File Offset: 0x003F2FC8
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

	// Token: 0x0600AA33 RID: 43571 RVA: 0x003F4E2C File Offset: 0x003F302C
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

	// Token: 0x0600AA34 RID: 43572 RVA: 0x003F4EC4 File Offset: 0x003F30C4
	public void RemoveComments()
	{
		this.XmlDoc.DescendantNodes().OfType<XComment>().Remove<XComment>();
	}

	// Token: 0x0600AA35 RID: 43573 RVA: 0x003F4EDB File Offset: 0x003F30DB
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

	// Token: 0x0600AA36 RID: 43574 RVA: 0x003F4F0D File Offset: 0x003F310D
	public int ClearXpathResults()
	{
		int count = this.tempXpathMatchList.Count;
		this.tempXpathMatchList.Clear();
		return count;
	}

	// Token: 0x0600AA37 RID: 43575 RVA: 0x003F4F28 File Offset: 0x003F3128
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

	// Token: 0x04007DD7 RID: 32215
	public readonly string Directory;

	// Token: 0x04007DD8 RID: 32216
	public readonly string Filename;

	// Token: 0x04007DDA RID: 32218
	public XDocument XmlDoc;

	// Token: 0x04007DDB RID: 32219
	[PublicizedFrom(EAccessModifier.Private)]
	public List<XObject> tempXpathMatchList;
}
