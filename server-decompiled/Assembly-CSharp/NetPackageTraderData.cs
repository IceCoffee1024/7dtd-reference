using System;
using UnityEngine.Scripting;

// Token: 0x02000867 RID: 2151
[Preserve]
public class NetPackageTraderData : NetPackage
{
	// Token: 0x170006A3 RID: 1699
	// (get) Token: 0x06003E4A RID: 15946 RVA: 0x00020119 File Offset: 0x0001E319
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003E4B RID: 15947 RVA: 0x0018B9BC File Offset: 0x00189BBC
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

	// Token: 0x06003E4C RID: 15948 RVA: 0x0018BA07 File Offset: 0x00189C07
	public NetPackageTraderData Setup(EntityTrader _entityTrader)
	{
		this.entityId = _entityTrader.entityId;
		this.tePosition = Vector3i.zero;
		TraderData traderData = _entityTrader.TraderData;
		this.traderData = ((traderData != null) ? traderData.Clone() : null);
		return this;
	}

	// Token: 0x06003E4D RID: 15949 RVA: 0x0018BA39 File Offset: 0x00189C39
	public NetPackageTraderData Setup(TileEntityVendingMachine _vendingMachine)
	{
		this.entityId = -1;
		this.tePosition = _vendingMachine.ToWorldPos();
		TraderData traderData = _vendingMachine.TraderData;
		this.traderData = ((traderData != null) ? traderData.Clone() : null);
		return this;
	}

	// Token: 0x06003E4E RID: 15950 RVA: 0x0018BA68 File Offset: 0x00189C68
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

	// Token: 0x06003E4F RID: 15951 RVA: 0x0018BAD8 File Offset: 0x00189CD8
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

	// Token: 0x06003E50 RID: 15952 RVA: 0x0018BB40 File Offset: 0x00189D40
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

	// Token: 0x040032A5 RID: 12965
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityId = -1;

	// Token: 0x040032A6 RID: 12966
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i tePosition;

	// Token: 0x040032A7 RID: 12967
	[PublicizedFrom(EAccessModifier.Private)]
	public TraderData traderData;
}
