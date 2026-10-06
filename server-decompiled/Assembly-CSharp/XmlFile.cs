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

// Token: 0x0200151D RID: 5405
public class XmlFile
{
	// Token: 0x170013FC RID: 5116
	// (get) Token: 0x0600A9C9 RID: 43465 RVA: 0x003F2564 File Offset: 0x003F0764
	// (set) Token: 0x0600A9CA RID: 43466 RVA: 0x003F256C File Offset: 0x003F076C
	public bool Loaded
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x0600A9CB RID: 43467 RVA: 0x003F2575 File Offset: 0x003F0775
	public XmlFile(XmlFile _orig)
	{
		this.Directory = _orig.Directory;
		this.Filename = _orig.Filename;
		this.Loaded = _orig.Loaded;
		this.XmlDoc = new XDocument(_orig.XmlDoc);
	}

	// Token: 0x0600A9CC RID: 43468 RVA: 0x003F25B4 File Offset: 0x003F07B4
	public XmlFile(string _name)
	{
		this.Directory = GameIO.GetGameDir("Data/Config");
		this.Filename = ((!_name.EndsWith(".xml", StringComparison.OrdinalIgnoreCase)) ? (_name + ".xml") : _name);
		this.load(this.Directory, this.Filename, false);
	}

	// Token: 0x0600A9CD RID: 43469 RVA: 0x003F260C File Offset: 0x003F080C
	public XmlFile(string _text, string _directory, string _filename, bool _throwExc = false)
	{
		this.Directory = _directory;
		this.Filename = _filename;
		this.toXml(_text, _filename, _throwExc);
	}

	// Token: 0x0600A9CE RID: 43470 RVA: 0x003F262C File Offset: 0x003F082C
	public XmlFile(TextAsset _ta)
	{
		using (MemoryStream memoryStream = new MemoryStream(_ta.bytes))
		{
			this.load(memoryStream, _ta.name, false);
		}
	}

	// Token: 0x0600A9CF RID: 43471 RVA: 0x003F2678 File Offset: 0x003F0878
	public XmlFile(byte[] _data, bool _throwExc = false)
	{
		using (MemoryStream memoryStream = new MemoryStream(_data))
		{
			this.load(memoryStream, null, _throwExc);
		}
	}

	// Token: 0x0600A9D0 RID: 43472 RVA: 0x003F26B8 File Offset: 0x003F08B8
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

	// Token: 0x0600A9D1 RID: 43473 RVA: 0x003F273C File Offset: 0x003F093C
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

	// Token: 0x0600A9D2 RID: 43474 RVA: 0x003F27AD File Offset: 0x003F09AD
	public XmlFile(Stream _stream)
	{
		this.load(_stream, null, false);
	}

	// Token: 0x0600A9D3 RID: 43475 RVA: 0x003F27C0 File Offset: 0x003F09C0
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

	// Token: 0x0600A9D4 RID: 43476 RVA: 0x003F2838 File Offset: 0x003F0A38
	public byte[] SerializeToBytes(bool _minified = false, Encoding _encoding = null)
	{
		PooledExpandableMemoryStream pooledExpandableMemoryStream = MemoryPools.poolMemoryStream.AllocSync(true);
		this.SerializeToStream(pooledExpandableMemoryStream, _minified, _encoding);
		byte[] array = pooledExpandableMemoryStream.ToArray();
		MemoryPools.poolMemoryStream.FreeSync(pooledExpandableMemoryStream);
		return array;
	}

	// Token: 0x0600A9D5 RID: 43477 RVA: 0x003F286C File Offset: 0x003F0A6C
	public void SerializeToFile(string _path, bool _minified = false, Encoding _encoding = null)
	{
		using (Stream stream = SdFile.Create(_path))
		{
			this.SerializeToStream(stream, _minified, _encoding);
		}
	}

	// Token: 0x0600A9D6 RID: 43478 RVA: 0x003F28A8 File Offset: 0x003F0AA8
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

	// Token: 0x0600A9D7 RID: 43479 RVA: 0x003F28F8 File Offset: 0x003F0AF8
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

	// Token: 0x0600A9D8 RID: 43480 RVA: 0x003F2914 File Offset: 0x003F0B14
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

	// Token: 0x0600A9D9 RID: 43481 RVA: 0x003F2988 File Offset: 0x003F0B88
	[PublicizedFrom(EAccessModifier.Private)]
	public void load(byte[] _bytes, bool _throwExc = false)
	{
		using (MemoryStream memoryStream = new MemoryStream(_bytes))
		{
			this.load(memoryStream, null, _throwExc);
		}
	}

	// Token: 0x0600A9DA RID: 43482 RVA: 0x003F29C4 File Offset: 0x003F0BC4
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

	// Token: 0x0600A9DB RID: 43483 RVA: 0x003F2A28 File Offset: 0x003F0C28
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

	// Token: 0x0600A9DC RID: 43484 RVA: 0x003F2AC0 File Offset: 0x003F0CC0
	public void RemoveComments()
	{
		this.XmlDoc.DescendantNodes().OfType<XComment>().Remove<XComment>();
	}

	// Token: 0x0600A9DD RID: 43485 RVA: 0x003F2AD7 File Offset: 0x003F0CD7
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

	// Token: 0x0600A9DE RID: 43486 RVA: 0x003F2B09 File Offset: 0x003F0D09
	public int ClearXpathResults()
	{
		int count = this.tempXpathMatchList.Count;
		this.tempXpathMatchList.Clear();
		return count;
	}

	// Token: 0x0600A9DF RID: 43487 RVA: 0x003F2B24 File Offset: 0x003F0D24
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

	// Token: 0x04007D96 RID: 32150
	public readonly string Directory;

	// Token: 0x04007D97 RID: 32151
	public readonly string Filename;

	// Token: 0x04007D99 RID: 32153
	public XDocument XmlDoc;

	// Token: 0x04007D9A RID: 32154
	[PublicizedFrom(EAccessModifier.Private)]
	public List<XObject> tempXpathMatchList;
}
