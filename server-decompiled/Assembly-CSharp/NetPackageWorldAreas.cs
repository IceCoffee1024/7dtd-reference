using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000863 RID: 2147
[Preserve]
public class NetPackageWorldAreas : NetPackage
{
	// Token: 0x06003E9A RID: 16026 RVA: 0x0018A799 File Offset: 0x00188999
	public NetPackageWorldAreas Setup(List<TraderArea> _list)
	{
		this.traders = _list;
		return this;
	}

	// Token: 0x06003E9B RID: 16027 RVA: 0x0018A7A4 File Offset: 0x001889A4
	public override void read(PooledBinaryReader _reader)
	{
		_reader.ReadByte();
		int num = (int)_reader.ReadInt16();
		this.traders = new List<TraderArea>();
		for (int i = 0; i < num; i++)
		{
			TraderArea traderArea = TraderArea.Read(_reader);
			this.traders.Add(traderArea);
		}
	}

	// Token: 0x06003E9C RID: 16028 RVA: 0x0018A7EC File Offset: 0x001889EC
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(1);
		_writer.Write((short)this.traders.Count);
		for (int i = 0; i < this.traders.Count; i++)
		{
			this.traders[i].Write(_writer);
		}
	}

	// Token: 0x06003E9D RID: 16029 RVA: 0x0018A841 File Offset: 0x00188A41
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		World.SetWorldAreas(this.traders);
	}

	// Token: 0x06003E9E RID: 16030 RVA: 0x0018A850 File Offset: 0x00188A50
	public override int GetLength()
	{
		int num = 2;
		for (int i = 0; i < this.traders.Count; i++)
		{
			num += this.traders[i].GetReadWriteSize();
		}
		return num;
	}

	// Token: 0x17000678 RID: 1656
	// (get) Token: 0x06003E9F RID: 16031 RVA: 0x0002EEAC File Offset: 0x0002D0AC
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x0400325A RID: 12890
	[PublicizedFrom(EAccessModifier.Private)]
	public const byte cVersion = 1;

	// Token: 0x0400325B RID: 12891
	[PublicizedFrom(EAccessModifier.Private)]
	public List<TraderArea> traders;
}
