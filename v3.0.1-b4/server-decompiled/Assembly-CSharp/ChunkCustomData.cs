using System;
using System.IO;

// Token: 0x02000B0D RID: 2829
public class ChunkCustomData
{
	// Token: 0x06005588 RID: 21896 RVA: 0x0000640C File Offset: 0x0000460C
	public ChunkCustomData()
	{
	}

	// Token: 0x06005589 RID: 21897 RVA: 0x0020B895 File Offset: 0x00209A95
	public ChunkCustomData(string _key, ulong _expiresInWorldTime, bool _isSavedToNetwork)
	{
		this.key = _key;
		this.expiresInWorldTime = _expiresInWorldTime;
		this.isSavedToNetwork = _isSavedToNetwork;
	}

	// Token: 0x0600558A RID: 21898 RVA: 0x0020B8B4 File Offset: 0x00209AB4
	public void Read(BinaryReader _br)
	{
		this.key = _br.ReadString();
		this.expiresInWorldTime = _br.ReadUInt64();
		this.isSavedToNetwork = _br.ReadBoolean();
		int num = (int)_br.ReadUInt16();
		if (num > 0)
		{
			this.data = _br.ReadBytes(num);
			return;
		}
		this.data = null;
	}

	// Token: 0x0600558B RID: 21899 RVA: 0x0020B908 File Offset: 0x00209B08
	public void Write(BinaryWriter _bw)
	{
		_bw.Write(this.key);
		_bw.Write(this.expiresInWorldTime);
		_bw.Write(this.isSavedToNetwork);
		if (this.TriggerWriteDataDelegate != null)
		{
			this.TriggerWriteDataDelegate();
		}
		_bw.Write((ushort)((this.data != null) ? this.data.Length : 0));
		if (this.data != null && this.data.Length != 0)
		{
			_bw.Write(this.data);
		}
	}

	// Token: 0x0600558C RID: 21900 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OnRemove(Chunk chunk)
	{
	}

	// Token: 0x040041ED RID: 16877
	public string key;

	// Token: 0x040041EE RID: 16878
	public ulong expiresInWorldTime;

	// Token: 0x040041EF RID: 16879
	public bool isSavedToNetwork;

	// Token: 0x040041F0 RID: 16880
	public byte[] data;

	// Token: 0x040041F1 RID: 16881
	public ChunkCustomData.TriggerWriteData TriggerWriteDataDelegate;

	// Token: 0x02000B0E RID: 2830
	// (Invoke) Token: 0x0600558E RID: 21902
	public delegate void TriggerWriteData();
}
