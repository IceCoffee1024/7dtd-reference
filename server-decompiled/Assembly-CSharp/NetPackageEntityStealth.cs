using System;
using UnityEngine.Scripting;

// Token: 0x020007E1 RID: 2017
[Preserve]
public class NetPackageEntityStealth : NetPackage
{
	// Token: 0x1700062E RID: 1582
	// (get) Token: 0x06003B8B RID: 15243 RVA: 0x00010E62 File Offset: 0x0000F062
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.Both;
		}
	}

	// Token: 0x06003B8C RID: 15244 RVA: 0x0017E3E8 File Offset: 0x0017C5E8
	public NetPackageEntityStealth Setup(EntityPlayer player, bool _isCrouching)
	{
		this.id = player.entityId;
		this.data = (_isCrouching ? 1 : 0);
		return this;
	}

	// Token: 0x06003B8D RID: 15245 RVA: 0x0017E404 File Offset: 0x0017C604
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

	// Token: 0x06003B8E RID: 15246 RVA: 0x0017E45B File Offset: 0x0017C65B
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

	// Token: 0x06003B8F RID: 15247 RVA: 0x0017E491 File Offset: 0x0017C691
	public override void read(PooledBinaryReader _br)
	{
		this.id = _br.ReadInt32();
		this.data = _br.ReadUInt16();
	}

	// Token: 0x06003B90 RID: 15248 RVA: 0x0017E4AB File Offset: 0x0017C6AB
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.id);
		_bw.Write(this.data);
	}

	// Token: 0x06003B91 RID: 15249 RVA: 0x0017E4CC File Offset: 0x0017C6CC
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

	// Token: 0x06003B92 RID: 15250 RVA: 0x000B8CB1 File Offset: 0x000B6EB1
	public override int GetLength()
	{
		return 20;
	}

	// Token: 0x0400303C RID: 12348
	[PublicizedFrom(EAccessModifier.Private)]
	public int id;

	// Token: 0x0400303D RID: 12349
	[PublicizedFrom(EAccessModifier.Private)]
	public const ushort cFIsCrouching = 1;

	// Token: 0x0400303E RID: 12350
	[PublicizedFrom(EAccessModifier.Private)]
	public const ushort cFIsSmellData = 2;

	// Token: 0x0400303F RID: 12351
	[PublicizedFrom(EAccessModifier.Private)]
	public const ushort cFIsEating = 4;

	// Token: 0x04003040 RID: 12352
	[PublicizedFrom(EAccessModifier.Private)]
	public const ushort cFIsSheltered = 8;

	// Token: 0x04003041 RID: 12353
	[PublicizedFrom(EAccessModifier.Private)]
	public const ushort cFIsAlert = 32768;

	// Token: 0x04003042 RID: 12354
	[PublicizedFrom(EAccessModifier.Private)]
	public ushort data;

	// Token: 0x04003043 RID: 12355
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cSmellRadiusMin = -1;
}
