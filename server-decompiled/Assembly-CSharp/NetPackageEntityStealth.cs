using System;
using UnityEngine.Scripting;

// Token: 0x020007EF RID: 2031
[Preserve]
public class NetPackageEntityStealth : NetPackage
{
	// Token: 0x1700065E RID: 1630
	// (get) Token: 0x06003BF1 RID: 15345 RVA: 0x00010E62 File Offset: 0x0000F062
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.Both;
		}
	}

	// Token: 0x06003BF2 RID: 15346 RVA: 0x00180CF8 File Offset: 0x0017EEF8
	public NetPackageEntityStealth Setup(EntityPlayer player, bool _isCrouching)
	{
		this.id = player.entityId;
		this.data = (_isCrouching ? 1 : 0);
		return this;
	}

	// Token: 0x06003BF3 RID: 15347 RVA: 0x00180D14 File Offset: 0x0017EF14
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

	// Token: 0x06003BF4 RID: 15348 RVA: 0x00180D6B File Offset: 0x0017EF6B
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

	// Token: 0x06003BF5 RID: 15349 RVA: 0x00180DA1 File Offset: 0x0017EFA1
	public override void read(PooledBinaryReader _br)
	{
		this.id = _br.ReadInt32();
		this.data = _br.ReadUInt16();
	}

	// Token: 0x06003BF6 RID: 15350 RVA: 0x00180DBB File Offset: 0x0017EFBB
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.id);
		_bw.Write(this.data);
	}

	// Token: 0x06003BF7 RID: 15351 RVA: 0x00180DDC File Offset: 0x0017EFDC
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

	// Token: 0x040030B4 RID: 12468
	[PublicizedFrom(EAccessModifier.Private)]
	public int id;

	// Token: 0x040030B5 RID: 12469
	[PublicizedFrom(EAccessModifier.Private)]
	public const ushort cFIsCrouching = 1;

	// Token: 0x040030B6 RID: 12470
	[PublicizedFrom(EAccessModifier.Private)]
	public const ushort cFIsSmellData = 2;

	// Token: 0x040030B7 RID: 12471
	[PublicizedFrom(EAccessModifier.Private)]
	public const ushort cFIsEating = 4;

	// Token: 0x040030B8 RID: 12472
	[PublicizedFrom(EAccessModifier.Private)]
	public const ushort cFIsSheltered = 8;

	// Token: 0x040030B9 RID: 12473
	[PublicizedFrom(EAccessModifier.Private)]
	public const ushort cFIsAlert = 32768;

	// Token: 0x040030BA RID: 12474
	[PublicizedFrom(EAccessModifier.Private)]
	public ushort data;

	// Token: 0x040030BB RID: 12475
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cSmellRadiusMin = -1;
}
