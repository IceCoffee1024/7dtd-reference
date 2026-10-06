using System;
using System.Collections.Generic;
using System.IO;

// Token: 0x0200149A RID: 5274
public class PList<T> : List<T>
{
	// Token: 0x0600A51D RID: 42269 RVA: 0x003D93DA File Offset: 0x003D75DA
	public PList()
		: this(1U)
	{
	}

	// Token: 0x0600A51E RID: 42270 RVA: 0x003D93E3 File Offset: 0x003D75E3
	public PList(uint _saveVersion)
	{
		this.saveVersion = _saveVersion;
	}

	// Token: 0x0600A51F RID: 42271 RVA: 0x003D9400 File Offset: 0x003D7600
	public void Write(BinaryWriter _bw)
	{
		_bw.Write((ushort)base.Count);
		_bw.Write((byte)this.saveVersion);
		foreach (T t in this)
		{
			this.writeElement(_bw, t);
		}
	}

	// Token: 0x0600A520 RID: 42272 RVA: 0x003D9470 File Offset: 0x003D7670
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

	// Token: 0x0600A521 RID: 42273 RVA: 0x003D94B2 File Offset: 0x003D76B2
	public void MarkToRemove(T _v)
	{
		this.toRemove.Add(_v);
	}

	// Token: 0x0600A522 RID: 42274 RVA: 0x003D94C0 File Offset: 0x003D76C0
	public void RemoveAllMarked()
	{
		foreach (T t in this.toRemove)
		{
			base.Remove(t);
		}
		this.toRemove.Clear();
	}

	// Token: 0x04007B59 RID: 31577
	public Action<BinaryWriter, T> writeElement;

	// Token: 0x04007B5A RID: 31578
	public Func<BinaryReader, uint, T> readElement;

	// Token: 0x04007B5B RID: 31579
	[PublicizedFrom(EAccessModifier.Protected)]
	public uint saveVersion;

	// Token: 0x04007B5C RID: 31580
	[PublicizedFrom(EAccessModifier.Protected)]
	public List<T> toRemove = new List<T>();
}
