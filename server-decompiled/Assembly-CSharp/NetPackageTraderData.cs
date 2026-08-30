using System;
using UnityEngine.Scripting;

// Token: 0x0200085A RID: 2138
[Preserve]
public class NetPackageTraderData : NetPackage
{
	// Token: 0x1700067E RID: 1662
	// (get) Token: 0x06003E82 RID: 16002 RVA: 0x00020021 File Offset: 0x0001E221
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003E83 RID: 16003 RVA: 0x0018AA78 File Offset: 0x00188C78
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

	// Token: 0x06003E84 RID: 16004 RVA: 0x0018AAC3 File Offset: 0x00188CC3
	public NetPackageTraderData Setup(EntityTrader _entityTrader)
	{
		this.entityId = _entityTrader.entityId;
		this.tePosition = Vector3i.zero;
		TraderData traderData = _entityTrader.TraderData;
		this.traderData = ((traderData != null) ? traderData.Clone() : null);
		return this;
	}

	// Token: 0x06003E85 RID: 16005 RVA: 0x0018AAF5 File Offset: 0x00188CF5
	public NetPackageTraderData Setup(TileEntityVendingMachine _vendingMachine)
	{
		this.entityId = -1;
		this.tePosition = _vendingMachine.ToWorldPos();
		TraderData traderData = _vendingMachine.TraderData;
		this.traderData = ((traderData != null) ? traderData.Clone() : null);
		return this;
	}

	// Token: 0x06003E86 RID: 16006 RVA: 0x0018AB24 File Offset: 0x00188D24
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

	// Token: 0x06003E87 RID: 16007 RVA: 0x0018AB94 File Offset: 0x00188D94
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

	// Token: 0x06003E88 RID: 16008 RVA: 0x0018ABFC File Offset: 0x00188DFC
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

	// Token: 0x06003E89 RID: 16009 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLength()
	{
		return 0;
	}

	// Token: 0x0400329A RID: 12954
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityId = -1;

	// Token: 0x0400329B RID: 12955
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i tePosition;

	// Token: 0x0400329C RID: 12956
	[PublicizedFrom(EAccessModifier.Private)]
	public TraderData traderData;
}
