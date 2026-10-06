using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

// Token: 0x02000B95 RID: 2965
public class NameIdMapping : IMemoryPoolableObject, IDisposable
{
	// Token: 0x1700098B RID: 2443
	// (get) Token: 0x06005954 RID: 22868 RVA: 0x00226210 File Offset: 0x00224410
	public Dictionary<string, int>.Enumerator NamesToIdsIterator
	{
		get
		{
			return this.namesToIds.GetEnumerator();
		}
	}

	// Token: 0x06005955 RID: 22869 RVA: 0x0000640C File Offset: 0x0000460C
	public NameIdMapping()
	{
	}

	// Token: 0x06005956 RID: 22870 RVA: 0x0022621D File Offset: 0x0022441D
	public NameIdMapping(string _filename, int _maxIds)
	{
		this.InitMapping(_filename, _maxIds);
	}

	// Token: 0x06005957 RID: 22871 RVA: 0x00226230 File Offset: 0x00224430
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

	// Token: 0x06005958 RID: 22872 RVA: 0x00226284 File Offset: 0x00224484
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

	// Token: 0x06005959 RID: 22873 RVA: 0x002262E8 File Offset: 0x002244E8
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

	// Token: 0x0600595A RID: 22874 RVA: 0x00226334 File Offset: 0x00224534
	public string GetNameForId(int _id)
	{
		string text;
		lock (this)
		{
			text = this.idsToNames[_id];
		}
		return text;
	}

	// Token: 0x0600595B RID: 22875 RVA: 0x00226374 File Offset: 0x00224574
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

	// Token: 0x0600595C RID: 22876 RVA: 0x00226414 File Offset: 0x00224614
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

	// Token: 0x0600595D RID: 22877 RVA: 0x002264D4 File Offset: 0x002246D4
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

	// Token: 0x0600595E RID: 22878 RVA: 0x00226500 File Offset: 0x00224700
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

	// Token: 0x0600595F RID: 22879 RVA: 0x002265E0 File Offset: 0x002247E0
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

	// Token: 0x06005960 RID: 22880 RVA: 0x00226650 File Offset: 0x00224850
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

	// Token: 0x06005961 RID: 22881 RVA: 0x0022670C File Offset: 0x0022490C
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

	// Token: 0x06005962 RID: 22882 RVA: 0x002267D8 File Offset: 0x002249D8
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

	// Token: 0x06005963 RID: 22883 RVA: 0x0022686C File Offset: 0x00224A6C
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

	// Token: 0x06005964 RID: 22884 RVA: 0x0022690C File Offset: 0x00224B0C
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

	// Token: 0x06005965 RID: 22885 RVA: 0x000027FC File Offset: 0x000009FC
	public void Cleanup()
	{
	}

	// Token: 0x06005966 RID: 22886 RVA: 0x0022695D File Offset: 0x00224B5D
	[PublicizedFrom(EAccessModifier.Private)]
	public void System.IDisposable.Dispose()
	{
		MemoryPools.poolNameIdMapping.FreeSync(this);
	}

	// Token: 0x04004590 RID: 17808
	[PublicizedFrom(EAccessModifier.Private)]
	public const int FILE_VERSION = 1;

	// Token: 0x04004591 RID: 17809
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<string, int> namesToIds;

	// Token: 0x04004592 RID: 17810
	[PublicizedFrom(EAccessModifier.Private)]
	public string[] idsToNames;

	// Token: 0x04004593 RID: 17811
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isDirty;

	// Token: 0x04004594 RID: 17812
	[PublicizedFrom(EAccessModifier.Private)]
	public string path;

	// Token: 0x04004595 RID: 17813
	[PublicizedFrom(EAccessModifier.Private)]
	public string filename;

	// Token: 0x02000B96 RID: 2966
	// (Invoke) Token: 0x06005969 RID: 22889
	public delegate int MissingEntryCallbackDelegate(string _entryName, int _sourceId);
}
