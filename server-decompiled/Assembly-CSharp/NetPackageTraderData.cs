using System;
using UnityEngine.Scripting;

// Token: 0x0200085D RID: 2141
[Preserve]
public class NetPackageTraderData : NetPackage
{
	// Token: 0x1700069D RID: 1693
	// (get) Token: 0x06003E27 RID: 15911 RVA: 0x00020119 File Offset: 0x0001E319
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003E28 RID: 15912 RVA: 0x0018A914 File Offset: 0x00188B14
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

	// Token: 0x06003E29 RID: 15913 RVA: 0x0018A95F File Offset: 0x00188B5F
	public NetPackageTraderData Setup(EntityTrader _entityTrader)
	{
		this.entityId = _entityTrader.entityId;
		this.tePosition = Vector3i.zero;
		TraderData traderData = _entityTrader.TraderData;
		this.traderData = ((traderData != null) ? traderData.Clone() : null);
		return this;
	}

	// Token: 0x06003E2A RID: 15914 RVA: 0x0018A991 File Offset: 0x00188B91
	public NetPackageTraderData Setup(TileEntityVendingMachine _vendingMachine)
	{
		this.entityId = -1;
		this.tePosition = _vendingMachine.ToWorldPos();
		TraderData traderData = _vendingMachine.TraderData;
		this.traderData = ((traderData != null) ? traderData.Clone() : null);
		return this;
	}

	// Token: 0x06003E2B RID: 15915 RVA: 0x0018A9C0 File Offset: 0x00188BC0
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

	// Token: 0x06003E2C RID: 15916 RVA: 0x0018AA30 File Offset: 0x00188C30
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

	// Token: 0x06003E2D RID: 15917 RVA: 0x0018AA98 File Offset: 0x00188C98
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

	// Token: 0x04003285 RID: 12933
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityId = -1;

	// Token: 0x04003286 RID: 12934
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i tePosition;

	// Token: 0x04003287 RID: 12935
	[PublicizedFrom(EAccessModifier.Private)]
	public TraderData traderData;
}
