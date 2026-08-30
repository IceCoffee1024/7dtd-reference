using System;
using System.Collections.Generic;
using System.IO;

// Token: 0x0200147A RID: 5242
public class PList<T> : List<T>
{
	// Token: 0x0600A470 RID: 42096 RVA: 0x003D479E File Offset: 0x003D299E
	public PList()
		: this(1U)
	{
	}

	// Token: 0x0600A471 RID: 42097 RVA: 0x003D47A7 File Offset: 0x003D29A7
	public PList(uint _saveVersion)
	{
		this.saveVersion = _saveVersion;
	}

	// Token: 0x0600A472 RID: 42098 RVA: 0x003D47C4 File Offset: 0x003D29C4
	public void Write(BinaryWriter _bw)
	{
		_bw.Write((ushort)base.Count);
		_bw.Write((byte)this.saveVersion);
		foreach (T t in this)
		{
			this.writeElement(_bw, t);
		}
	}

	// Token: 0x0600A473 RID: 42099 RVA: 0x003D4834 File Offset: 0x003D2A34
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

	// Token: 0x0600A474 RID: 42100 RVA: 0x003D4876 File Offset: 0x003D2A76
	public void MarkToRemove(T _v)
	{
		this.toRemove.Add(_v);
	}

	// Token: 0x0600A475 RID: 42101 RVA: 0x003D4884 File Offset: 0x003D2A84
	public void RemoveAllMarked()
	{
		foreach (T t in this.toRemove)
		{
			base.Remove(t);
		}
		this.toRemove.Clear();
	}

	// Token: 0x04007ADE RID: 31454
	public Action<BinaryWriter, T> writeElement;

	// Token: 0x04007ADF RID: 31455
	public Func<BinaryReader, uint, T> readElement;

	// Token: 0x04007AE0 RID: 31456
	[PublicizedFrom(EAccessModifier.Protected)]
	public uint saveVersion;

	// Token: 0x04007AE1 RID: 31457
	[PublicizedFrom(EAccessModifier.Protected)]
	public List<T> toRemove = new List<T>();
}
