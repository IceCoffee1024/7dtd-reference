using System;
using System.Collections.Generic;
using System.IO;

// Token: 0x0200145F RID: 5215
public class PList<T> : List<T>
{
	// Token: 0x0600A373 RID: 41843 RVA: 0x003CF45A File Offset: 0x003CD65A
	public PList()
		: this(1U)
	{
	}

	// Token: 0x0600A374 RID: 41844 RVA: 0x003CF463 File Offset: 0x003CD663
	public PList(uint _saveVersion)
	{
		this.saveVersion = _saveVersion;
	}

	// Token: 0x0600A375 RID: 41845 RVA: 0x003CF480 File Offset: 0x003CD680
	public void Write(BinaryWriter _bw)
	{
		_bw.Write((ushort)base.Count);
		_bw.Write((byte)this.saveVersion);
		foreach (T t in this)
		{
			this.writeElement(_bw, t);
		}
	}

	// Token: 0x0600A376 RID: 41846 RVA: 0x003CF4F0 File Offset: 0x003CD6F0
	public void Read(BinaryReader _br)
	{
		base.Clear();
		int num = (int)_br.ReadUInt16();
		uint num2 = (uint)_br.ReadByte();
		for (int i = 0; i < num; i++)
		{
			T t = this.readElement(_br, num2);
			base.Add(t);
		}
	}

	// Token: 0x0600A377 RID: 41847 RVA: 0x003CF532 File Offset: 0x003CD732
	public void MarkToRemove(T _v)
	{
		this.toRemove.Add(_v);
	}

	// Token: 0x0600A378 RID: 41848 RVA: 0x003CF540 File Offset: 0x003CD740
	public void RemoveAllMarked()
	{
		foreach (T t in this.toRemove)
		{
			base.Remove(t);
		}
		this.toRemove.Clear();
	}

	// Token: 0x04007A05 RID: 31237
	public Action<BinaryWriter, T> writeElement;

	// Token: 0x04007A06 RID: 31238
	public Func<BinaryReader, uint, T> readElement;

	// Token: 0x04007A07 RID: 31239
	[PublicizedFrom(EAccessModifier.Protected)]
	public uint saveVersion;

	// Token: 0x04007A08 RID: 31240
	[PublicizedFrom(EAccessModifier.Protected)]
	public List<T> toRemove = new List<T>();
}
