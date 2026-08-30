using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Token: 0x020013C1 RID: 5057
public abstract class DatabaseWithFixedDS<KEY, DATA> where KEY : struct where DATA : class
{
	// Token: 0x06009F14 RID: 40724 RVA: 0x003BF69C File Offset: 0x003BD89C
	public DatabaseWithFixedDS(int _magicBytes, int _sizeofKey, int _maxCountOfDataSets, int _sizeOfDataSet, KEY _invalidKeyValue, int _oldMaxCountOfDataSets = -1)
	{
		this.magicBytes = _magicBytes;
		this.sizeofKey = _sizeofKey;
		this.maxCountOfDataSets = _maxCountOfDataSets;
		this.oldMaxCountOfDataSets = _oldMaxCountOfDataSets;
		this.sizeOfDataSet = _sizeOfDataSet;
		this.invalidKeyValue = _invalidKeyValue;
	}

	// Token: 0x06009F15 RID: 40725 RVA: 0x003BF700 File Offset: 0x003BD900
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

	// Token: 0x06009F16 RID: 40726 RVA: 0x003BF748 File Offset: 0x003BD948
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

	// Token: 0x06009F17 RID: 40727 RVA: 0x003BF7B0 File Offset: 0x003BD9B0
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

	// Token: 0x06009F18 RID: 40728 RVA: 0x003BF8F8 File Offset: 0x003BDAF8
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

	// Token: 0x06009F19 RID: 40729 RVA: 0x003BF948 File Offset: 0x003BDB48
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

	// Token: 0x06009F1A RID: 40730 RVA: 0x003BF994 File Offset: 0x003BDB94
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

	// Token: 0x06009F1B RID: 40731 RVA: 0x003BF9F0 File Offset: 0x003BDBF0
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

	// Token: 0x06009F1C RID: 40732 RVA: 0x003BFB28 File Offset: 0x003BDD28
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

	// Token: 0x06009F1D RID: 40733 RVA: 0x003BFD74 File Offset: 0x003BDF74
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

	// Token: 0x06009F1E RID: 40734 RVA: 0x003BFEA4 File Offset: 0x003BE0A4
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

	// Token: 0x06009F1F RID: 40735
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract KEY readKey(BinaryReader _br);

	// Token: 0x06009F20 RID: 40736
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract void writeKey(BinaryWriter _bw, KEY _key);

	// Token: 0x06009F21 RID: 40737
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract void copyFromRead(byte[] _dataRead, DATA _data);

	// Token: 0x06009F22 RID: 40738
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract void copyToWrite(DATA _data, byte[] _dataWrite);

	// Token: 0x06009F23 RID: 40739
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract DATA allocateDataStorage();

	// Token: 0x04007883 RID: 30851
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cHeaderSize = 16;

	// Token: 0x04007884 RID: 30852
	[PublicizedFrom(EAccessModifier.Private)]
	public const int VERSION = 3;

	// Token: 0x04007885 RID: 30853
	[PublicizedFrom(EAccessModifier.Private)]
	public int magicBytes;

	// Token: 0x04007886 RID: 30854
	[PublicizedFrom(EAccessModifier.Private)]
	public int sizeofKey;

	// Token: 0x04007887 RID: 30855
	[PublicizedFrom(EAccessModifier.Private)]
	public int maxCountOfDataSets;

	// Token: 0x04007888 RID: 30856
	[PublicizedFrom(EAccessModifier.Private)]
	public int oldMaxCountOfDataSets;

	// Token: 0x04007889 RID: 30857
	[PublicizedFrom(EAccessModifier.Private)]
	public int sizeOfDataSet;

	// Token: 0x0400788A RID: 30858
	[PublicizedFrom(EAccessModifier.Private)]
	public KEY invalidKeyValue;

	// Token: 0x0400788B RID: 30859
	[PublicizedFrom(EAccessModifier.Private)]
	public DictionaryKeyList<KEY, int> catalog = new DictionaryKeyList<KEY, int>();

	// Token: 0x0400788C RID: 30860
	[PublicizedFrom(EAccessModifier.Private)]
	public DictionarySave<KEY, DATA> database = new DictionarySave<KEY, DATA>();

	// Token: 0x0400788D RID: 30861
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<KEY, bool> dirty = new Dictionary<KEY, bool>();

	// Token: 0x0400788E RID: 30862
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] arrayReadBuf;

	// Token: 0x0400788F RID: 30863
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] arrayWriteBuf;
}
