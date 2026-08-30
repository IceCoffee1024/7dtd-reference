using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Token: 0x020013A6 RID: 5030
public abstract class DatabaseWithFixedDS<KEY, DATA> where KEY : struct where DATA : class
{
	// Token: 0x06009E17 RID: 40471 RVA: 0x003BA358 File Offset: 0x003B8558
	public DatabaseWithFixedDS(int _magicBytes, int _sizeofKey, int _maxCountOfDataSets, int _sizeOfDataSet, KEY _invalidKeyValue, int _oldMaxCountOfDataSets = -1)
	{
		this.magicBytes = _magicBytes;
		this.sizeofKey = _sizeofKey;
		this.maxCountOfDataSets = _maxCountOfDataSets;
		this.oldMaxCountOfDataSets = _oldMaxCountOfDataSets;
		this.sizeOfDataSet = _sizeOfDataSet;
		this.invalidKeyValue = _invalidKeyValue;
	}

	// Token: 0x06009E18 RID: 40472 RVA: 0x003BA3BC File Offset: 0x003B85BC
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

	// Token: 0x06009E19 RID: 40473 RVA: 0x003BA404 File Offset: 0x003B8604
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

	// Token: 0x06009E1A RID: 40474 RVA: 0x003BA46C File Offset: 0x003B866C
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

	// Token: 0x06009E1B RID: 40475 RVA: 0x003BA5B4 File Offset: 0x003B87B4
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

	// Token: 0x06009E1C RID: 40476 RVA: 0x003BA604 File Offset: 0x003B8804
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

	// Token: 0x06009E1D RID: 40477 RVA: 0x003BA650 File Offset: 0x003B8850
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

	// Token: 0x06009E1E RID: 40478 RVA: 0x003BA6AC File Offset: 0x003B88AC
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

	// Token: 0x06009E1F RID: 40479 RVA: 0x003BA7E4 File Offset: 0x003B89E4
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

	// Token: 0x06009E20 RID: 40480 RVA: 0x003BAA30 File Offset: 0x003B8C30
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

	// Token: 0x06009E21 RID: 40481 RVA: 0x003BAB60 File Offset: 0x003B8D60
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

	// Token: 0x06009E22 RID: 40482
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract KEY readKey(BinaryReader _br);

	// Token: 0x06009E23 RID: 40483
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract void writeKey(BinaryWriter _bw, KEY _key);

	// Token: 0x06009E24 RID: 40484
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract void copyFromRead(byte[] _dataRead, DATA _data);

	// Token: 0x06009E25 RID: 40485
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract void copyToWrite(DATA _data, byte[] _dataWrite);

	// Token: 0x06009E26 RID: 40486
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract DATA allocateDataStorage();

	// Token: 0x040077AA RID: 30634
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cHeaderSize = 16;

	// Token: 0x040077AB RID: 30635
	[PublicizedFrom(EAccessModifier.Private)]
	public const int VERSION = 3;

	// Token: 0x040077AC RID: 30636
	[PublicizedFrom(EAccessModifier.Private)]
	public int magicBytes;

	// Token: 0x040077AD RID: 30637
	[PublicizedFrom(EAccessModifier.Private)]
	public int sizeofKey;

	// Token: 0x040077AE RID: 30638
	[PublicizedFrom(EAccessModifier.Private)]
	public int maxCountOfDataSets;

	// Token: 0x040077AF RID: 30639
	[PublicizedFrom(EAccessModifier.Private)]
	public int oldMaxCountOfDataSets;

	// Token: 0x040077B0 RID: 30640
	[PublicizedFrom(EAccessModifier.Private)]
	public int sizeOfDataSet;

	// Token: 0x040077B1 RID: 30641
	[PublicizedFrom(EAccessModifier.Private)]
	public KEY invalidKeyValue;

	// Token: 0x040077B2 RID: 30642
	[PublicizedFrom(EAccessModifier.Private)]
	public DictionaryKeyList<KEY, int> catalog = new DictionaryKeyList<KEY, int>();

	// Token: 0x040077B3 RID: 30643
	[PublicizedFrom(EAccessModifier.Private)]
	public DictionarySave<KEY, DATA> database = new DictionarySave<KEY, DATA>();

	// Token: 0x040077B4 RID: 30644
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<KEY, bool> dirty = new Dictionary<KEY, bool>();

	// Token: 0x040077B5 RID: 30645
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] arrayReadBuf;

	// Token: 0x040077B6 RID: 30646
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] arrayWriteBuf;
}
