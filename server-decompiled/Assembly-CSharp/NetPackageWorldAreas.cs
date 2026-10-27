using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200087C RID: 2172
[Preserve]
public class NetPackageWorldAreas : NetPackage
{
	// Token: 0x06003EB7 RID: 16055 RVA: 0x0018D789 File Offset: 0x0018B989
	public NetPackageWorldAreas Setup(List<TraderArea> _list)
	{
		this.traders = _list;
		return this;
	}

	// Token: 0x06003EB8 RID: 16056 RVA: 0x0018D794 File Offset: 0x0018B994
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

	// Token: 0x06003EB9 RID: 16057 RVA: 0x0018D7DC File Offset: 0x0018B9DC
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

	// Token: 0x06003EBA RID: 16058 RVA: 0x0018D831 File Offset: 0x0018BA31
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		World.SetWorldAreas(this.traders);
	}

	// Token: 0x170006A9 RID: 1705
	// (get) Token: 0x06003EBB RID: 16059 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x040032E6 RID: 13030
	[PublicizedFrom(EAccessModifier.Private)]
	public const byte cVersion = 1;

	// Token: 0x040032E7 RID: 13031
	[PublicizedFrom(EAccessModifier.Private)]
	public List<TraderArea> traders;
}
