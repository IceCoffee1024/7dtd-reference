using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200086F RID: 2159
[Preserve]
public class NetPackageWorldAreas : NetPackage
{
	// Token: 0x06003EFF RID: 16127 RVA: 0x0018C869 File Offset: 0x0018AA69
	public NetPackageWorldAreas Setup(List<TraderArea> _list)
	{
		this.traders = _list;
		return this;
	}

	// Token: 0x06003F00 RID: 16128 RVA: 0x0018C874 File Offset: 0x0018AA74
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

	// Token: 0x06003F01 RID: 16129 RVA: 0x0018C8BC File Offset: 0x0018AABC
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

	// Token: 0x06003F02 RID: 16130 RVA: 0x0018C911 File Offset: 0x0018AB11
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		World.SetWorldAreas(this.traders);
	}

	// Token: 0x06003F03 RID: 16131 RVA: 0x0018C920 File Offset: 0x0018AB20
	public override int GetLength()
	{
		int num = 2;
		for (int i = 0; i < this.traders.Count; i++)
		{
			num += this.traders[i].GetReadWriteSize();
		}
		return num;
	}

	// Token: 0x17000684 RID: 1668
	// (get) Token: 0x06003F04 RID: 16132 RVA: 0x0002F168 File Offset: 0x0002D368
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x040032DB RID: 13019
	[PublicizedFrom(EAccessModifier.Private)]
	public const byte cVersion = 1;

	// Token: 0x040032DC RID: 13020
	[PublicizedFrom(EAccessModifier.Private)]
	public List<TraderArea> traders;
}
