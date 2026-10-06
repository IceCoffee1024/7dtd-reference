using System;
using System.IO;

// Token: 0x02000B31 RID: 2865
public class ChunkCustomData
{
	// Token: 0x06005629 RID: 22057 RVA: 0x0000640C File Offset: 0x0000460C
	public ChunkCustomData()
	{
	}

	// Token: 0x0600562A RID: 22058 RVA: 0x00211821 File Offset: 0x0020FA21
	public ChunkCustomData(string _key, ulong _expiresInWorldTime, bool _isSavedToNetwork)
	{
		this.key = _key;
		this.expiresInWorldTime = _expiresInWorldTime;
		this.isSavedToNetwork = _isSavedToNetwork;
	}

	// Token: 0x0600562B RID: 22059 RVA: 0x00211840 File Offset: 0x0020FA40
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

	// Token: 0x0600562C RID: 22060 RVA: 0x00211894 File Offset: 0x0020FA94
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

	// Token: 0x0600562D RID: 22061 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OnRemove(Chunk chunk)
	{
	}

	// Token: 0x040042C5 RID: 17093
	public string key;

	// Token: 0x040042C6 RID: 17094
	public ulong expiresInWorldTime;

	// Token: 0x040042C7 RID: 17095
	public bool isSavedToNetwork;

	// Token: 0x040042C8 RID: 17096
	public byte[] data;

	// Token: 0x040042C9 RID: 17097
	public ChunkCustomData.TriggerWriteData TriggerWriteDataDelegate;

	// Token: 0x02000B32 RID: 2866
	// (Invoke) Token: 0x0600562F RID: 22063
	public delegate void TriggerWriteData();
}
