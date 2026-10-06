using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Token: 0x020013E1 RID: 5089
public abstract class DatabaseWithFixedDS<KEY, DATA> where KEY : struct where DATA : class
{
	// Token: 0x06009FBF RID: 40895 RVA: 0x003C4238 File Offset: 0x003C2438
	public DatabaseWithFixedDS(int _magicBytes, int _sizeofKey, int _maxCountOfDataSets, int _sizeOfDataSet, KEY _invalidKeyValue, int _oldMaxCountOfDataSets = -1)
	{
		this.magicBytes = _magicBytes;
		this.sizeofKey = _sizeofKey;
		this.maxCountOfDataSets = _maxCountOfDataSets;
		this.oldMaxCountOfDataSets = _oldMaxCountOfDataSets;
		this.sizeOfDataSet = _sizeOfDataSet;
		this.invalidKeyValue = _invalidKeyValue;
	}

	// Token: 0x06009FC0 RID: 40896 RVA: 0x003C429C File Offset: 0x003C249C
	public List<KEY> GetAllKeys()
	{
		DictionaryKeyList<KEY, int> dictionaryKeyList = this.catalog;
		List<KEY> list;
		lock (dictionaryKeyList)
		{
			list = this.catalog.list;
		}
		return list;
	}

	// Token: 0x06009FC1 RID: 40897 RVA: 0x003C42E4 File Offset: 0x003C24E4
	public DATA GetDS(KEY _key)
	{
		DictionaryKeyList<KEY, int> dictionaryKeyList = this.catalog;
		DATA data;
		lock (dictionaryKeyList)
		{
			if (!this.catalog.dict.ContainsKey(_key))
			{
				data = default(DATA);
				data = data;
			}
			else
			{
				data = this.database[_key];
			}
		}
		return data;
	}

	// Token: 0x06009FC2 RID: 40898 RVA: 0x003C434C File Offset: 0x003C254C
	public void SetDS(KEY _key, DATA _data)
	{
		DictionaryKeyList<KEY, int> dictionaryKeyList = this.catalog;
		lock (dictionaryKeyList)
		{
			if (this.catalog.dict.Count >= this.maxCountOfDataSets)
			{
				int num = this.maxCountOfDataSets / 10;
				int num2 = 0;
				while (this.catalog.list.Count > 0 && num2 < num)
				{
					KEY key = this.catalog.list[0];
					this.catalog.Remove(key);
					this.database.Remove(key);
					num2++;
				}
				this.dirty.Clear();
				for (int i = 0; i < this.catalog.list.Count; i++)
				{
					this.dirty[this.catalog.list[i]] = true;
				}
			}
			if (!this.catalog.dict.ContainsKey(_key))
			{
				this.catalog.Add(_key, this.catalog.list.Count);
			}
			this.database[_key] = _data;
			this.dirty[_key] = true;
		}
	}

	// Token: 0x06009FC3 RID: 40899 RVA: 0x003C4494 File Offset: 0x003C2694
	public bool ContainsDS(KEY _key)
	{
		DictionaryKeyList<KEY, int> dictionaryKeyList = this.catalog;
		bool flag2;
		lock (dictionaryKeyList)
		{
			flag2 = this.catalog.dict.ContainsKey(_key);
		}
		return flag2;
	}

	// Token: 0x06009FC4 RID: 40900 RVA: 0x003C44E4 File Offset: 0x003C26E4
	public int CountDS()
	{
		DictionaryKeyList<KEY, int> dictionaryKeyList = this.catalog;
		int count;
		lock (dictionaryKeyList)
		{
			count = this.catalog.list.Count;
		}
		return count;
	}

	// Token: 0x06009FC5 RID: 40901 RVA: 0x003C4530 File Offset: 0x003C2730
	public virtual void Clear()
	{
		DictionaryKeyList<KEY, int> dictionaryKeyList = this.catalog;
		lock (dictionaryKeyList)
		{
			this.catalog.Clear();
			this.database.Clear();
			this.dirty.Clear();
		}
	}

	// Token: 0x06009FC6 RID: 40902 RVA: 0x003C458C File Offset: 0x003C278C
	public void Load(string _dir, string _filename)
	{
		try
		{
			if (SdFile.Exists(_dir + "/" + _filename))
			{
				using (Stream stream = SdFile.OpenRead(_dir + "/" + _filename))
				{
					using (PooledBinaryReader pooledBinaryReader = MemoryPools.poolBinaryReader.AllocSync(false))
					{
						pooledBinaryReader.SetBaseStream(stream);
						int num = pooledBinaryReader.ReadInt32();
						if (num != this.magicBytes)
						{
							Log.Error(string.Format("Map file has invalid magic bytes: 0x{0:X8}", num));
						}
						else
						{
							uint num2 = (uint)pooledBinaryReader.ReadByte();
							if (this.arrayReadBuf == null)
							{
								this.arrayReadBuf = new byte[this.sizeOfDataSet];
							}
							pooledBinaryReader.Read(this.arrayReadBuf, 0, 3);
							this.read(pooledBinaryReader, num2);
						}
					}
				}
			}
		}
		catch (Exception ex)
		{
			Log.Error(string.Concat(new string[] { "Could not load file '", _dir, "/", _filename, "': ", ex.Message }));
			Log.Exception(ex);
		}
	}

	// Token: 0x06009FC7 RID: 40903 RVA: 0x003C46C4 File Offset: 0x003C28C4
	[PublicizedFrom(EAccessModifier.Private)]
	public void read(BinaryReader _br, uint _version)
	{
		DictionaryKeyList<KEY, int> dictionaryKeyList = this.catalog;
		lock (dictionaryKeyList)
		{
			int num = this.oldMaxCountOfDataSets;
			if (_version <= 2U && this.oldMaxCountOfDataSets < 0)
			{
				throw new Exception("Can not load old dataset file, unknown size of key list");
			}
			if (_version > 2U)
			{
				num = _br.ReadInt32();
			}
			this.catalog.Clear();
			this.dirty.Clear();
			uint num2 = _br.ReadUInt32();
			for (uint num3 = 0U; num3 < num2; num3 += 1U)
			{
				KEY key = this.readKey(_br);
				this.catalog.Add(key, (int)num3);
			}
			uint num4 = num2;
			while ((ulong)num4 < (ulong)((long)num))
			{
				this.readKey(_br);
				num4 += 1U;
			}
			this.database.Clear();
			for (int i = 0; i < this.catalog.list.Count; i++)
			{
				_br.Read(this.arrayReadBuf, 0, this.arrayReadBuf.Length);
				DATA data = this.allocateDataStorage();
				this.copyFromRead(this.arrayReadBuf, data);
				this.database[this.catalog.list[i]] = data;
			}
			if (num != this.maxCountOfDataSets)
			{
				if (num > this.maxCountOfDataSets)
				{
					int num5 = num - this.maxCountOfDataSets;
					int num6 = 0;
					while (this.catalog.list.Count > 0 && num6 < num5)
					{
						KEY key2 = this.catalog.list[0];
						this.catalog.Remove(key2);
						this.database.Remove(key2);
						num6++;
					}
					this.dirty.Clear();
					for (int j = 0; j < this.catalog.list.Count; j++)
					{
						this.dirty[this.catalog.list[j]] = true;
					}
				}
				else
				{
					this.dirty.Clear();
					for (int k = 0; k < this.catalog.list.Count; k++)
					{
						this.dirty[this.catalog.list[k]] = true;
					}
				}
			}
		}
	}

	// Token: 0x06009FC8 RID: 40904 RVA: 0x003C4910 File Offset: 0x003C2B10
	public void Save(string _dir, string _filename)
	{
		try
		{
			if (!SdDirectory.Exists(_dir))
			{
				SdDirectory.CreateDirectory(_dir);
			}
			lock (this)
			{
				using (Stream stream = SdFile.Open(_dir + "/" + _filename, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read))
				{
					using (PooledBinaryWriter pooledBinaryWriter = MemoryPools.poolBinaryWriter.AllocSync(false))
					{
						pooledBinaryWriter.SetBaseStream(stream);
						pooledBinaryWriter.BaseStream.Seek(0L, SeekOrigin.Begin);
						pooledBinaryWriter.Write(this.magicBytes);
						pooledBinaryWriter.Write(3);
						pooledBinaryWriter.Write(0);
						pooledBinaryWriter.Write(0);
						pooledBinaryWriter.Write(0);
						this.write(pooledBinaryWriter);
					}
				}
			}
		}
		catch (Exception ex)
		{
			Log.Error(string.Concat(new string[] { "Could not save file '", _dir, "/", _filename, "': ", ex.Message }));
			Log.Exception(ex);
		}
	}

	// Token: 0x06009FC9 RID: 40905 RVA: 0x003C4A40 File Offset: 0x003C2C40
	[PublicizedFrom(EAccessModifier.Private)]
	public void write(BinaryWriter _bw)
	{
		DictionaryKeyList<KEY, int> dictionaryKeyList = this.catalog;
		lock (dictionaryKeyList)
		{
			_bw.Write((uint)this.maxCountOfDataSets);
			_bw.Write((uint)this.catalog.list.Count);
			for (int i = 0; i < this.catalog.list.Count; i++)
			{
				this.writeKey(_bw, this.catalog.list[i]);
			}
			for (int j = this.catalog.list.Count; j < this.maxCountOfDataSets; j++)
			{
				this.writeKey(_bw, this.invalidKeyValue);
			}
			int num = 0;
			if (this.arrayWriteBuf == null)
			{
				this.arrayWriteBuf = new byte[this.sizeOfDataSet];
			}
			int count = this.catalog.list.Count;
			for (int k = 0; k < count; k++)
			{
				if (!this.dirty.ContainsKey(this.catalog.list[k]) || !this.dirty[this.catalog.list[k]])
				{
					_bw.Seek(this.sizeOfDataSet, SeekOrigin.Current);
				}
				else
				{
					DATA data = this.database[this.catalog.list[k]];
					this.copyToWrite(data, this.arrayWriteBuf);
					_bw.Write(this.arrayWriteBuf, 0, this.arrayWriteBuf.Length);
					num++;
				}
			}
			this.dirty.Clear();
			Array.Clear(this.arrayWriteBuf, 0, this.arrayWriteBuf.Length);
			int num2 = (int)Mathf.Pow(2f, Mathf.Ceil(Mathf.Log((float)count) / Mathf.Log(2f)));
			int num3 = 16 + this.maxCountOfDataSets * this.sizeofKey + num2 * this.sizeOfDataSet;
			if (_bw.BaseStream.Length < (long)num3)
			{
				for (int l = count; l < num2; l++)
				{
					_bw.Write(this.arrayWriteBuf, 0, this.arrayWriteBuf.Length);
				}
			}
			else if (_bw.BaseStream.Length > (long)num3)
			{
				_bw.BaseStream.SetLength((long)num3);
			}
		}
	}

	// Token: 0x06009FCA RID: 40906
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract KEY readKey(BinaryReader _br);

	// Token: 0x06009FCB RID: 40907
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract void writeKey(BinaryWriter _bw, KEY _key);

	// Token: 0x06009FCC RID: 40908
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract void copyFromRead(byte[] _dataRead, DATA _data);

	// Token: 0x06009FCD RID: 40909
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract void copyToWrite(DATA _data, byte[] _dataWrite);

	// Token: 0x06009FCE RID: 40910
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract DATA allocateDataStorage();

	// Token: 0x040078FE RID: 30974
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cHeaderSize = 16;

	// Token: 0x040078FF RID: 30975
	[PublicizedFrom(EAccessModifier.Private)]
	public const int VERSION = 3;

	// Token: 0x04007900 RID: 30976
	[PublicizedFrom(EAccessModifier.Private)]
	public int magicBytes;

	// Token: 0x04007901 RID: 30977
	[PublicizedFrom(EAccessModifier.Private)]
	public int sizeofKey;

	// Token: 0x04007902 RID: 30978
	[PublicizedFrom(EAccessModifier.Private)]
	public int maxCountOfDataSets;

	// Token: 0x04007903 RID: 30979
	[PublicizedFrom(EAccessModifier.Private)]
	public int oldMaxCountOfDataSets;

	// Token: 0x04007904 RID: 30980
	[PublicizedFrom(EAccessModifier.Private)]
	public int sizeOfDataSet;

	// Token: 0x04007905 RID: 30981
	[PublicizedFrom(EAccessModifier.Private)]
	public KEY invalidKeyValue;

	// Token: 0x04007906 RID: 30982
	[PublicizedFrom(EAccessModifier.Private)]
	public DictionaryKeyList<KEY, int> catalog = new DictionaryKeyList<KEY, int>();

	// Token: 0x04007907 RID: 30983
	[PublicizedFrom(EAccessModifier.Private)]
	public DictionarySave<KEY, DATA> database = new DictionarySave<KEY, DATA>();

	// Token: 0x04007908 RID: 30984
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<KEY, bool> dirty = new Dictionary<KEY, bool>();

	// Token: 0x04007909 RID: 30985
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] arrayReadBuf;

	// Token: 0x0400790A RID: 30986
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] arrayWriteBuf;
}
