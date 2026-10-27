using System;
using System.IO;

// Token: 0x02000B3C RID: 2876
public class ChunkCustomData
{
	// Token: 0x0600564E RID: 22094 RVA: 0x0000640C File Offset: 0x0000460C
	public ChunkCustomData()
	{
	}

	// Token: 0x0600564F RID: 22095 RVA: 0x00212D9D File Offset: 0x00210F9D
	public ChunkCustomData(string _key, ulong _expiresInWorldTime, bool _isSavedToNetwork)
	{
		this.key = _key;
		this.expiresInWorldTime = _expiresInWorldTime;
		this.isSavedToNetwork = _isSavedToNetwork;
	}

	// Token: 0x06005650 RID: 22096 RVA: 0x00212DBC File Offset: 0x00210FBC
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

	// Token: 0x06005651 RID: 22097 RVA: 0x00212E10 File Offset: 0x00211010
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

	// Token: 0x06005652 RID: 22098 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OnRemove(Chunk chunk)
	{
	}

	// Token: 0x040042EA RID: 17130
	public string key;

	// Token: 0x040042EB RID: 17131
	public ulong expiresInWorldTime;

	// Token: 0x040042EC RID: 17132
	public bool isSavedToNetwork;

	// Token: 0x040042ED RID: 17133
	public byte[] data;

	// Token: 0x040042EE RID: 17134
	public ChunkCustomData.TriggerWriteData TriggerWriteDataDelegate;

	// Token: 0x02000B3D RID: 2877
	// (Invoke) Token: 0x06005654 RID: 22100
	public delegate void TriggerWriteData();
}
