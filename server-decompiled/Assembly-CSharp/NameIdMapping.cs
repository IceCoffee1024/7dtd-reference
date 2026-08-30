using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

// Token: 0x02000B83 RID: 2947
public class NameIdMapping : IMemoryPoolableObject, IDisposable
{
	// Token: 0x17000969 RID: 2409
	// (get) Token: 0x0600593A RID: 22842 RVA: 0x00222F84 File Offset: 0x00221184
	public Dictionary<string, int>.Enumerator NamesToIdsIterator
	{
		get
		{
			return this.namesToIds.GetEnumerator();
		}
	}

	// Token: 0x0600593B RID: 22843 RVA: 0x0000640C File Offset: 0x0000460C
	public NameIdMapping()
	{
	}

	// Token: 0x0600593C RID: 22844 RVA: 0x00222F91 File Offset: 0x00221191
	public NameIdMapping(string _filename, int _maxIds)
	{
		this.InitMapping(_filename, _maxIds);
	}

	// Token: 0x0600593D RID: 22845 RVA: 0x00222FA4 File Offset: 0x002211A4
	public void InitMapping(string _filename, int _maxIds)
	{
		this.filename = _filename;
		this.path = Path.GetDirectoryName(_filename);
		if (this.namesToIds == null)
		{
			this.namesToIds = new Dictionary<string, int>(_maxIds);
		}
		if (this.idsToNames == null || this.idsToNames.Length < _maxIds)
		{
			this.idsToNames = new string[_maxIds];
		}
	}

	// Token: 0x0600593E RID: 22846 RVA: 0x00222FF8 File Offset: 0x002211F8
	public void AddMapping(int _id, string _name, bool _force = false)
	{
		lock (this)
		{
			if (this.idsToNames[_id] == null || _force)
			{
				this.idsToNames[_id] = _name;
				this.namesToIds[_name] = _id;
				this.isDirty = true;
			}
		}
	}

	// Token: 0x0600593F RID: 22847 RVA: 0x0022305C File Offset: 0x0022125C
	public int GetIdForName(string _name)
	{
		lock (this)
		{
			int num;
			if (this.namesToIds.TryGetValue(_name, out num))
			{
				return num;
			}
		}
		return -1;
	}

	// Token: 0x06005940 RID: 22848 RVA: 0x002230A8 File Offset: 0x002212A8
	public string GetNameForId(int _id)
	{
		string text;
		lock (this)
		{
			text = this.idsToNames[_id];
		}
		return text;
	}

	// Token: 0x06005941 RID: 22849 RVA: 0x002230E8 File Offset: 0x002212E8
	public ArrayListMP<int> createIdTranslationTable(Func<string, int> _getDstId, NameIdMapping.MissingEntryCallbackDelegate _onMissingDestination = null)
	{
		ArrayListMP<int> arrayListMP = new ArrayListMP<int>(MemoryPools.poolInt, Block.MAX_BLOCKS);
		int[] items = arrayListMP.Items;
		for (int i = 0; i < items.Length; i++)
		{
			items[i] = -1;
		}
		for (int j = 0; j < this.idsToNames.Length; j++)
		{
			string text = this.idsToNames[j];
			if (text != null)
			{
				int num = _getDstId(text);
				if (num < 0)
				{
					if (_onMissingDestination == null)
					{
						Log.Error(string.Format("Creating id translation table from \"{0}\" failed: Entry \"{1}\" ({2}) in source map is unknown.", this.filename, text, j));
						return null;
					}
					num = _onMissingDestination(text, j);
					if (num < 0)
					{
						return null;
					}
				}
				items[j] = num;
			}
		}
		return arrayListMP;
	}

	// Token: 0x06005942 RID: 22850 RVA: 0x00223188 File Offset: 0x00221388
	public int ReplaceNames([TupleElementNames(new string[] { "oldName", "newName" })] IEnumerable<ValueTuple<string, string>> _replacementList)
	{
		int num = 0;
		lock (this)
		{
			foreach (ValueTuple<string, string> valueTuple in _replacementList)
			{
				string item = valueTuple.Item1;
				string item2 = valueTuple.Item2;
				int num2;
				if (this.namesToIds.TryGetValue(item, out num2))
				{
					this.idsToNames[num2] = item2;
					this.namesToIds.Remove(item);
					this.namesToIds[item2] = num2;
					num++;
				}
			}
			if (num > 0)
			{
				this.isDirty = true;
			}
		}
		return num;
	}

	// Token: 0x06005943 RID: 22851 RVA: 0x00223248 File Offset: 0x00221448
	public void SaveIfDirty(bool _async = true)
	{
		if (this.isDirty)
		{
			if (_async)
			{
				ThreadManager.AddSingleTask(delegate(ThreadManager.TaskInfo _info)
				{
					this.WriteToFile();
				}, null, null, true);
				return;
			}
			this.WriteToFile();
		}
	}

	// Token: 0x06005944 RID: 22852 RVA: 0x00223274 File Offset: 0x00221474
	public void WriteToFile()
	{
		try
		{
			if (this.filename == null)
			{
				Log.Error("Can not save mapping, no filename specified");
			}
			else
			{
				if (!SdDirectory.Exists(this.path))
				{
					SdDirectory.CreateDirectory(this.path);
				}
				using (Stream stream = SdFile.Open(this.filename, FileMode.Create, FileAccess.Write, FileShare.Read))
				{
					using (PooledBinaryWriter pooledBinaryWriter = MemoryPools.poolBinaryWriter.AllocSync(false))
					{
						pooledBinaryWriter.SetBaseStream(stream);
						pooledBinaryWriter.BaseStream.Seek(0L, SeekOrigin.Begin);
						this.SaveToWriter(pooledBinaryWriter);
					}
				}
			}
		}
		catch (Exception ex)
		{
			Log.Error("Could not save file '" + this.filename + "': " + ex.Message);
			Log.Exception(ex);
		}
	}

	// Token: 0x06005945 RID: 22853 RVA: 0x00223354 File Offset: 0x00221554
	public byte[] SaveToArray()
	{
		byte[] array;
		using (PooledExpandableMemoryStream pooledExpandableMemoryStream = MemoryPools.poolMemoryStream.AllocSync(true))
		{
			using (PooledBinaryWriter pooledBinaryWriter = MemoryPools.poolBinaryWriter.AllocSync(false))
			{
				pooledBinaryWriter.SetBaseStream(pooledExpandableMemoryStream);
				this.SaveToWriter(pooledBinaryWriter);
			}
			array = pooledExpandableMemoryStream.ToArray();
		}
		return array;
	}

	// Token: 0x06005946 RID: 22854 RVA: 0x002233C4 File Offset: 0x002215C4
	[PublicizedFrom(EAccessModifier.Private)]
	public void SaveToWriter(BinaryWriter _writer)
	{
		_writer.Write(1);
		lock (this)
		{
			int num = 0;
			long position = _writer.BaseStream.Position;
			_writer.Write(num);
			for (int i = 0; i < this.idsToNames.Length; i++)
			{
				string text = this.idsToNames[i];
				if (text != null)
				{
					_writer.Write(i);
					_writer.Write(text);
					num++;
				}
			}
			_writer.BaseStream.Position = position;
			_writer.Write(num);
			_writer.BaseStream.Position = _writer.BaseStream.Length;
			this.isDirty = false;
		}
	}

	// Token: 0x06005947 RID: 22855 RVA: 0x00223480 File Offset: 0x00221680
	public bool LoadFromFile()
	{
		bool flag;
		try
		{
			if (this.filename == null)
			{
				Log.Error("Can not load mapping, no filename specified");
				flag = false;
			}
			else if (!SdFile.Exists(this.filename))
			{
				flag = false;
			}
			else
			{
				using (Stream stream = SdFile.OpenRead(this.filename))
				{
					using (PooledBinaryReader pooledBinaryReader = MemoryPools.poolBinaryReader.AllocSync(false))
					{
						pooledBinaryReader.SetBaseStream(stream);
						this.LoadFromReader(pooledBinaryReader);
					}
				}
				flag = true;
			}
		}
		catch (Exception ex)
		{
			Log.Error("Could not load file '" + this.filename + "': " + ex.Message);
			Log.Exception(ex);
			flag = false;
		}
		return flag;
	}

	// Token: 0x06005948 RID: 22856 RVA: 0x0022354C File Offset: 0x0022174C
	public bool LoadFromArray(byte[] _data)
	{
		bool flag;
		try
		{
			using (MemoryStream memoryStream = new MemoryStream(_data))
			{
				using (PooledBinaryReader pooledBinaryReader = MemoryPools.poolBinaryReader.AllocSync(false))
				{
					pooledBinaryReader.SetBaseStream(memoryStream);
					this.LoadFromReader(pooledBinaryReader);
				}
			}
			flag = true;
		}
		catch (Exception ex)
		{
			Log.Error("Could not load mapping from array: " + ex.Message);
			Log.Exception(ex);
			flag = false;
		}
		return flag;
	}

	// Token: 0x06005949 RID: 22857 RVA: 0x002235E0 File Offset: 0x002217E0
	[PublicizedFrom(EAccessModifier.Private)]
	public void LoadFromReader(BinaryReader _reader)
	{
		_reader.ReadInt32();
		lock (this)
		{
			Array.Clear(this.idsToNames, 0, this.idsToNames.Length);
			this.namesToIds.Clear();
			int num = _reader.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				int num2 = _reader.ReadInt32();
				string text = _reader.ReadString();
				this.idsToNames[num2] = text;
				this.namesToIds[text] = num2;
			}
			this.isDirty = false;
		}
	}

	// Token: 0x0600594A RID: 22858 RVA: 0x00223680 File Offset: 0x00221880
	public void Reset()
	{
		this.filename = null;
		this.path = null;
		if (this.idsToNames != null)
		{
			Array.Clear(this.idsToNames, 0, this.idsToNames.Length);
		}
		if (this.namesToIds != null)
		{
			this.namesToIds.Clear();
		}
		this.isDirty = false;
	}

	// Token: 0x0600594B RID: 22859 RVA: 0x000027FC File Offset: 0x000009FC
	public void Cleanup()
	{
	}

	// Token: 0x0600594C RID: 22860 RVA: 0x002236D1 File Offset: 0x002218D1
	[PublicizedFrom(EAccessModifier.Private)]
	public void System.IDisposable.Dispose()
	{
		MemoryPools.poolNameIdMapping.FreeSync(this);
	}

	// Token: 0x04004552 RID: 17746
	[PublicizedFrom(EAccessModifier.Private)]
	public const int FILE_VERSION = 1;

	// Token: 0x04004553 RID: 17747
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<string, int> namesToIds;

	// Token: 0x04004554 RID: 17748
	[PublicizedFrom(EAccessModifier.Private)]
	public string[] idsToNames;

	// Token: 0x04004555 RID: 17749
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isDirty;

	// Token: 0x04004556 RID: 17750
	[PublicizedFrom(EAccessModifier.Private)]
	public string path;

	// Token: 0x04004557 RID: 17751
	[PublicizedFrom(EAccessModifier.Private)]
	public string filename;

	// Token: 0x02000B84 RID: 2948
	// (Invoke) Token: 0x0600594F RID: 22863
	public delegate int MissingEntryCallbackDelegate(string _entryName, int _sourceId);
}
