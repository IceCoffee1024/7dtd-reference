using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

// Token: 0x02000BA0 RID: 2976
public class NameIdMapping : IMemoryPoolableObject, IDisposable
{
	// Token: 0x17000991 RID: 2449
	// (get) Token: 0x06005979 RID: 22905 RVA: 0x0022776C File Offset: 0x0022596C
	public Dictionary<string, int>.Enumerator NamesToIdsIterator
	{
		get
		{
			return this.namesToIds.GetEnumerator();
		}
	}

	// Token: 0x0600597A RID: 22906 RVA: 0x0000640C File Offset: 0x0000460C
	public NameIdMapping()
	{
	}

	// Token: 0x0600597B RID: 22907 RVA: 0x00227779 File Offset: 0x00225979
	public NameIdMapping(string _filename, int _maxIds)
	{
		this.InitMapping(_filename, _maxIds);
	}

	// Token: 0x0600597C RID: 22908 RVA: 0x0022778C File Offset: 0x0022598C
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

	// Token: 0x0600597D RID: 22909 RVA: 0x002277E0 File Offset: 0x002259E0
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

	// Token: 0x0600597E RID: 22910 RVA: 0x00227844 File Offset: 0x00225A44
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

	// Token: 0x0600597F RID: 22911 RVA: 0x00227890 File Offset: 0x00225A90
	public string GetNameForId(int _id)
	{
		string text;
		lock (this)
		{
			text = this.idsToNames[_id];
		}
		return text;
	}

	// Token: 0x06005980 RID: 22912 RVA: 0x002278D0 File Offset: 0x00225AD0
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

	// Token: 0x06005981 RID: 22913 RVA: 0x00227970 File Offset: 0x00225B70
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

	// Token: 0x06005982 RID: 22914 RVA: 0x00227A30 File Offset: 0x00225C30
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

	// Token: 0x06005983 RID: 22915 RVA: 0x00227A5C File Offset: 0x00225C5C
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

	// Token: 0x06005984 RID: 22916 RVA: 0x00227B3C File Offset: 0x00225D3C
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

	// Token: 0x06005985 RID: 22917 RVA: 0x00227BAC File Offset: 0x00225DAC
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

	// Token: 0x06005986 RID: 22918 RVA: 0x00227C68 File Offset: 0x00225E68
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

	// Token: 0x06005987 RID: 22919 RVA: 0x00227D34 File Offset: 0x00225F34
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

	// Token: 0x06005988 RID: 22920 RVA: 0x00227DC8 File Offset: 0x00225FC8
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

	// Token: 0x06005989 RID: 22921 RVA: 0x00227E68 File Offset: 0x00226068
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

	// Token: 0x0600598A RID: 22922 RVA: 0x000027FC File Offset: 0x000009FC
	public void Cleanup()
	{
	}

	// Token: 0x0600598B RID: 22923 RVA: 0x00227EB9 File Offset: 0x002260B9
	[PublicizedFrom(EAccessModifier.Private)]
	public void System.IDisposable.Dispose()
	{
		MemoryPools.poolNameIdMapping.FreeSync(this);
	}

	// Token: 0x040045B5 RID: 17845
	[PublicizedFrom(EAccessModifier.Private)]
	public const int FILE_VERSION = 1;

	// Token: 0x040045B6 RID: 17846
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<string, int> namesToIds;

	// Token: 0x040045B7 RID: 17847
	[PublicizedFrom(EAccessModifier.Private)]
	public string[] idsToNames;

	// Token: 0x040045B8 RID: 17848
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isDirty;

	// Token: 0x040045B9 RID: 17849
	[PublicizedFrom(EAccessModifier.Private)]
	public string path;

	// Token: 0x040045BA RID: 17850
	[PublicizedFrom(EAccessModifier.Private)]
	public string filename;

	// Token: 0x02000BA1 RID: 2977
	// (Invoke) Token: 0x0600598E RID: 22926
	public delegate int MissingEntryCallbackDelegate(string _entryName, int _sourceId);
}
