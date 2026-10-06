using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000872 RID: 2162
[Preserve]
public class NetPackageWorldAreas : NetPackage
{
	// Token: 0x06003E94 RID: 16020 RVA: 0x0018C6E1 File Offset: 0x0018A8E1
	public NetPackageWorldAreas Setup(List<TraderArea> _list)
	{
		this.traders = _list;
		return this;
	}

	// Token: 0x06003E95 RID: 16021 RVA: 0x0018C6EC File Offset: 0x0018A8EC
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

	// Token: 0x06003E96 RID: 16022 RVA: 0x0018C734 File Offset: 0x0018A934
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

	// Token: 0x06003E97 RID: 16023 RVA: 0x0018C789 File Offset: 0x0018A989
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		World.SetWorldAreas(this.traders);
	}

	// Token: 0x170006A3 RID: 1699
	// (get) Token: 0x06003E98 RID: 16024 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x040032C6 RID: 12998
	[PublicizedFrom(EAccessModifier.Private)]
	public const byte cVersion = 1;

	// Token: 0x040032C7 RID: 12999
	[PublicizedFrom(EAccessModifier.Private)]
	public List<TraderArea> traders;
}
