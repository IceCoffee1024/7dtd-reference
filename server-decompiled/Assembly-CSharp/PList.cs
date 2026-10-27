using System;
using System.Collections.Generic;
using System.IO;

// Token: 0x020014AC RID: 5292
public class PList<T> : List<T>
{
	// Token: 0x0600A575 RID: 42357 RVA: 0x003DB7DE File Offset: 0x003D99DE
	public PList()
		: this(1U)
	{
	}

	// Token: 0x0600A576 RID: 42358 RVA: 0x003DB7E7 File Offset: 0x003D99E7
	public PList(uint _saveVersion)
	{
		this.saveVersion = _saveVersion;
	}

	// Token: 0x0600A577 RID: 42359 RVA: 0x003DB804 File Offset: 0x003D9A04
	public void Write(BinaryWriter _bw)
	{
		_bw.Write((ushort)base.Count);
		_bw.Write((byte)this.saveVersion);
		foreach (T t in this)
		{
			this.writeElement(_bw, t);
		}
	}

	// Token: 0x0600A578 RID: 42360 RVA: 0x003DB874 File Offset: 0x003D9A74
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

	// Token: 0x0600A579 RID: 42361 RVA: 0x003DB8B6 File Offset: 0x003D9AB6
	public void MarkToRemove(T _v)
	{
		this.toRemove.Add(_v);
	}

	// Token: 0x0600A57A RID: 42362 RVA: 0x003DB8C4 File Offset: 0x003D9AC4
	public void RemoveAllMarked()
	{
		foreach (T t in this.toRemove)
		{
			base.Remove(t);
		}
		this.toRemove.Clear();
	}

	// Token: 0x04007B9A RID: 31642
	public Action<BinaryWriter, T> writeElement;

	// Token: 0x04007B9B RID: 31643
	public Func<BinaryReader, uint, T> readElement;

	// Token: 0x04007B9C RID: 31644
	[PublicizedFrom(EAccessModifier.Protected)]
	public uint saveVersion;

	// Token: 0x04007B9D RID: 31645
	[PublicizedFrom(EAccessModifier.Protected)]
	public List<T> toRemove = new List<T>();
}
