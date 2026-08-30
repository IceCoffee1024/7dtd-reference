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

// Token: 0x020014E0 RID: 5344
public class XmlFile
{
	// Token: 0x1700137E RID: 4990
	// (get) Token: 0x0600A819 RID: 43033 RVA: 0x003E83D0 File Offset: 0x003E65D0
	// (set) Token: 0x0600A81A RID: 43034 RVA: 0x003E83D8 File Offset: 0x003E65D8
	public bool Loaded
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x0600A81B RID: 43035 RVA: 0x003E83E1 File Offset: 0x003E65E1
	public XmlFile(XmlFile _orig)
	{
		this.Directory = _orig.Directory;
		this.Filename = _orig.Filename;
		this.Loaded = _orig.Loaded;
		this.XmlDoc = new XDocument(_orig.XmlDoc);
	}

	// Token: 0x0600A81C RID: 43036 RVA: 0x003E8420 File Offset: 0x003E6620
	public XmlFile(string _name)
	{
		this.Directory = GameIO.GetGameDir("Data/Config");
		this.Filename = ((!_name.EndsWith(".xml", StringComparison.OrdinalIgnoreCase)) ? (_name + ".xml") : _name);
		this.load(this.Directory, this.Filename, false);
	}

	// Token: 0x0600A81D RID: 43037 RVA: 0x003E8478 File Offset: 0x003E6678
	public XmlFile(string _text, string _directory, string _filename, bool _throwExc = false)
	{
		this.Directory = _directory;
		this.Filename = _filename;
		this.toXml(_text, _filename, _throwExc);
	}

	// Token: 0x0600A81E RID: 43038 RVA: 0x003E8498 File Offset: 0x003E6698
	public XmlFile(TextAsset _ta)
	{
		using (MemoryStream memoryStream = new MemoryStream(_ta.bytes))
		{
			this.load(memoryStream, _ta.name, false);
		}
	}

	// Token: 0x0600A81F RID: 43039 RVA: 0x003E84E4 File Offset: 0x003E66E4
	public XmlFile(byte[] _data, bool _throwExc = false)
	{
		using (MemoryStream memoryStream = new MemoryStream(_data))
		{
			this.load(memoryStream, null, _throwExc);
		}
	}

	// Token: 0x0600A820 RID: 43040 RVA: 0x003E8524 File Offset: 0x003E6724
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

	// Token: 0x0600A821 RID: 43041 RVA: 0x003E85A8 File Offset: 0x003E67A8
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

	// Token: 0x0600A822 RID: 43042 RVA: 0x003E8619 File Offset: 0x003E6819
	public XmlFile(Stream _stream)
	{
		this.load(_stream, null, false);
	}

	// Token: 0x0600A823 RID: 43043 RVA: 0x003E862C File Offset: 0x003E682C
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

	// Token: 0x0600A824 RID: 43044 RVA: 0x003E86A4 File Offset: 0x003E68A4
	public byte[] SerializeToBytes(bool _minified = false, Encoding _encoding = null)
	{
		PooledExpandableMemoryStream pooledExpandableMemoryStream = MemoryPools.poolMemoryStream.AllocSync(true);
		this.SerializeToStream(pooledExpandableMemoryStream, _minified, _encoding);
		byte[] array = pooledExpandableMemoryStream.ToArray();
		MemoryPools.poolMemoryStream.FreeSync(pooledExpandableMemoryStream);
		return array;
	}

	// Token: 0x0600A825 RID: 43045 RVA: 0x003E86D8 File Offset: 0x003E68D8
	public void SerializeToFile(string _path, bool _minified = false, Encoding _encoding = null)
	{
		using (Stream stream = SdFile.Create(_path))
		{
			this.SerializeToStream(stream, _minified, _encoding);
		}
	}

	// Token: 0x0600A826 RID: 43046 RVA: 0x003E8714 File Offset: 0x003E6914
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

	// Token: 0x0600A827 RID: 43047 RVA: 0x003E8764 File Offset: 0x003E6964
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

	// Token: 0x0600A828 RID: 43048 RVA: 0x003E8780 File Offset: 0x003E6980
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

	// Token: 0x0600A829 RID: 43049 RVA: 0x003E87F4 File Offset: 0x003E69F4
	[PublicizedFrom(EAccessModifier.Private)]
	public void load(byte[] _bytes, bool _throwExc = false)
	{
		using (MemoryStream memoryStream = new MemoryStream(_bytes))
		{
			this.load(memoryStream, null, _throwExc);
		}
	}

	// Token: 0x0600A82A RID: 43050 RVA: 0x003E8830 File Offset: 0x003E6A30
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

	// Token: 0x0600A82B RID: 43051 RVA: 0x003E8894 File Offset: 0x003E6A94
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

	// Token: 0x0600A82C RID: 43052 RVA: 0x003E892C File Offset: 0x003E6B2C
	public void RemoveComments()
	{
		this.XmlDoc.DescendantNodes().OfType<XComment>().Remove<XComment>();
	}

	// Token: 0x0600A82D RID: 43053 RVA: 0x003E8943 File Offset: 0x003E6B43
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

	// Token: 0x0600A82E RID: 43054 RVA: 0x003E8975 File Offset: 0x003E6B75
	public int ClearXpathResults()
	{
		int count = this.tempXpathMatchList.Count;
		this.tempXpathMatchList.Clear();
		return count;
	}

	// Token: 0x0600A82F RID: 43055 RVA: 0x003E8990 File Offset: 0x003E6B90
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

	// Token: 0x04007C3A RID: 31802
	public readonly string Directory;

	// Token: 0x04007C3B RID: 31803
	public readonly string Filename;

	// Token: 0x04007C3D RID: 31805
	public XDocument XmlDoc;

	// Token: 0x04007C3E RID: 31806
	[PublicizedFrom(EAccessModifier.Private)]
	public List<XObject> tempXpathMatchList;
}
