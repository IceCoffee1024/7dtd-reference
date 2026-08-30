using System;
using UnityEngine.Scripting;

// Token: 0x020007EC RID: 2028
[Preserve]
public class NetPackageEntityStealth : NetPackage
{
	// Token: 0x17000639 RID: 1593
	// (get) Token: 0x06003BEA RID: 15338 RVA: 0x00010E62 File Offset: 0x0000F062
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.Both;
		}
	}

	// Token: 0x06003BEB RID: 15339 RVA: 0x001807C0 File Offset: 0x0017E9C0
	public NetPackageEntityStealth Setup(EntityPlayer player, bool _isCrouching)
	{
		this.id = player.entityId;
		this.data = (_isCrouching ? 1 : 0);
		return this;
	}

	// Token: 0x06003BEC RID: 15340 RVA: 0x001807DC File Offset: 0x0017E9DC
	public NetPackageEntityStealth Setup(EntityPlayerLocal player, int _smellRadius, bool _eating, bool _sheltered)
	{
		this.id = player.entityId;
		this.data = (ushort)(2 | (Utils.FastMin(_smellRadius - -1, 255) << 8));
		if (_eating)
		{
			this.data |= 4;
		}
		if (_sheltered)
		{
			this.data |= 8;
		}
		return this;
	}

	// Token: 0x06003BED RID: 15341 RVA: 0x00180833 File Offset: 0x0017EA33
	public NetPackageEntityStealth Setup(EntityPlayer player, int _lightLevel, int _noiseVolume, bool _isAlert)
	{
		this.id = player.entityId;
		this.data = (ushort)((int)((byte)_lightLevel) | ((_noiseVolume & 127) << 8));
		if (_isAlert)
		{
			this.data |= 32768;
		}
		return this;
	}

	// Token: 0x06003BEE RID: 15342 RVA: 0x00180869 File Offset: 0x0017EA69
	public override void read(PooledBinaryReader _br)
	{
		this.id = _br.ReadInt32();
		this.data = _br.ReadUInt16();
	}

	// Token: 0x06003BEF RID: 15343 RVA: 0x00180883 File Offset: 0x0017EA83
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.id);
		_bw.Write(this.data);
	}

	// Token: 0x06003BF0 RID: 15344 RVA: 0x001808A4 File Offset: 0x0017EAA4
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		if (!base.ValidEntityIdForSender(this.id, false))
		{
			return;
		}
		EntityPlayer entityPlayer = _world.GetEntity(this.id) as EntityPlayer;
		if (entityPlayer == null)
		{
			Log.Out("Discarding " + base.GetType().Name);
			return;
		}
		if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			float num = (float)((byte)this.data);
			float num2 = (float)((this.data >> 8) & 127);
			entityPlayer.Stealth.SetClientLevels(num, num2, (this.data & 32768) > 0);
			return;
		}
		if ((this.data & 2) > 0)
		{
			entityPlayer.Stealth.SetSmellRadiusTarget((this.data >> 8) + -1, (this.data & 4) > 0, (this.data & 8) > 0);
			return;
		}
		entityPlayer.Crouching = (this.data & 1) > 0;
	}

	// Token: 0x06003BF1 RID: 15345 RVA: 0x000B93F5 File Offset: 0x000B75F5
	public override int GetLength()
	{
		return 20;
	}

	// Token: 0x040030BB RID: 12475
	[PublicizedFrom(EAccessModifier.Private)]
	public int id;

	// Token: 0x040030BC RID: 12476
	[PublicizedFrom(EAccessModifier.Private)]
	public const ushort cFIsCrouching = 1;

	// Token: 0x040030BD RID: 12477
	[PublicizedFrom(EAccessModifier.Private)]
	public const ushort cFIsSmellData = 2;

	// Token: 0x040030BE RID: 12478
	[PublicizedFrom(EAccessModifier.Private)]
	public const ushort cFIsEating = 4;

	// Token: 0x040030BF RID: 12479
	[PublicizedFrom(EAccessModifier.Private)]
	public const ushort cFIsSheltered = 8;

	// Token: 0x040030C0 RID: 12480
	[PublicizedFrom(EAccessModifier.Private)]
	public const ushort cFIsAlert = 32768;

	// Token: 0x040030C1 RID: 12481
	[PublicizedFrom(EAccessModifier.Private)]
	public ushort data;

	// Token: 0x040030C2 RID: 12482
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cSmellRadiusMin = -1;
}
