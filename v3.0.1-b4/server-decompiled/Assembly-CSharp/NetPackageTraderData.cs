using System;
using UnityEngine.Scripting;

// Token: 0x0200084E RID: 2126
[Preserve]
public class NetPackageTraderData : NetPackage
{
	// Token: 0x17000672 RID: 1650
	// (get) Token: 0x06003E1D RID: 15901 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003E1E RID: 15902 RVA: 0x001889A8 File Offset: 0x00186BA8
	public NetPackageTraderData Setup(ITrader _trader)
	{
		EntityTrader entityTrader = _trader as EntityTrader;
		if (entityTrader != null)
		{
			return this.Setup(entityTrader);
		}
		TileEntityVendingMachine tileEntityVendingMachine = _trader as TileEntityVendingMachine;
		if (tileEntityVendingMachine != null)
		{
			return this.Setup(tileEntityVendingMachine);
		}
		this.entityId = -1;
		this.tePosition = Vector3i.zero;
		this.traderData = null;
		return this;
	}

	// Token: 0x06003E1F RID: 15903 RVA: 0x001889F3 File Offset: 0x00186BF3
	public NetPackageTraderData Setup(EntityTrader _entityTrader)
	{
		this.entityId = _entityTrader.entityId;
		this.tePosition = Vector3i.zero;
		TraderData traderData = _entityTrader.TraderData;
		this.traderData = ((traderData != null) ? traderData.Clone() : null);
		return this;
	}

	// Token: 0x06003E20 RID: 15904 RVA: 0x00188A25 File Offset: 0x00186C25
	public NetPackageTraderData Setup(TileEntityVendingMachine _vendingMachine)
	{
		this.entityId = -1;
		this.tePosition = _vendingMachine.ToWorldPos();
		TraderData traderData = _vendingMachine.TraderData;
		this.traderData = ((traderData != null) ? traderData.Clone() : null);
		return this;
	}

	// Token: 0x06003E21 RID: 15905 RVA: 0x00188A54 File Offset: 0x00186C54
	public override void read(PooledBinaryReader _br)
	{
		bool flag = _br.ReadBoolean();
		this.entityId = -1;
		this.tePosition = Vector3i.zero;
		if (flag)
		{
			this.entityId = _br.ReadInt32();
		}
		else
		{
			this.tePosition = StreamUtils.ReadVector3i(_br);
		}
		if (_br.ReadBoolean())
		{
			if (this.traderData == null)
			{
				this.traderData = new TraderData();
			}
			this.traderData.Read(_br);
			return;
		}
		this.traderData = null;
	}

	// Token: 0x06003E22 RID: 15906 RVA: 0x00188AC4 File Offset: 0x00186CC4
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		bool flag = this.entityId != -1;
		_bw.Write(flag);
		if (flag)
		{
			_bw.Write(this.entityId);
		}
		else
		{
			StreamUtils.Write(_bw, this.tePosition);
		}
		_bw.Write(this.traderData != null);
		if (this.traderData != null)
		{
			this.traderData.Write(_bw);
		}
	}

	// Token: 0x06003E23 RID: 15907 RVA: 0x00188B2C File Offset: 0x00186D2C
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null || this.traderData == null || !SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			return;
		}
		if (this.entityId != -1)
		{
			EntityTrader entityTrader = _world.GetEntity(this.entityId) as EntityTrader;
			if (entityTrader == null)
			{
				return;
			}
			entityTrader.TraderData.CopyFrom(this.traderData);
			return;
		}
		else
		{
			TileEntityVendingMachine tileEntityVendingMachine = _world.GetTileEntity(this.tePosition) as TileEntityVendingMachine;
			if (tileEntityVendingMachine == null)
			{
				return;
			}
			tileEntityVendingMachine.TraderData.CopyFrom(this.traderData);
			tileEntityVendingMachine.NotifyListeners();
			return;
		}
	}

	// Token: 0x06003E24 RID: 15908 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLength()
	{
		return 0;
	}

	// Token: 0x04003219 RID: 12825
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityId = -1;

	// Token: 0x0400321A RID: 12826
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i tePosition;

	// Token: 0x0400321B RID: 12827
	[PublicizedFrom(EAccessModifier.Private)]
	public TraderData traderData;
}
