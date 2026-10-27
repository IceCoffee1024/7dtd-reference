using System;
using UnityEngine.Scripting;

// Token: 0x020007F9 RID: 2041
[Preserve]
public class NetPackageEntityStealth : NetPackage
{
	// Token: 0x17000664 RID: 1636
	// (get) Token: 0x06003C14 RID: 15380 RVA: 0x00010E62 File Offset: 0x0000F062
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.Both;
		}
	}

	// Token: 0x06003C15 RID: 15381 RVA: 0x00181DFC File Offset: 0x0017FFFC
	public NetPackageEntityStealth Setup(EntityPlayer player, bool _isCrouching)
	{
		this.id = player.entityId;
		this.data = (_isCrouching ? 1 : 0);
		return this;
	}

	// Token: 0x06003C16 RID: 15382 RVA: 0x00181E18 File Offset: 0x00180018
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

	// Token: 0x06003C17 RID: 15383 RVA: 0x00181E6F File Offset: 0x0018006F
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

	// Token: 0x06003C18 RID: 15384 RVA: 0x00181EA5 File Offset: 0x001800A5
	public override void read(PooledBinaryReader _br)
	{
		this.id = _br.ReadInt32();
		this.data = _br.ReadUInt16();
	}

	// Token: 0x06003C19 RID: 15385 RVA: 0x00181EBF File Offset: 0x001800BF
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.id);
		_bw.Write(this.data);
	}

	// Token: 0x06003C1A RID: 15386 RVA: 0x00181EE0 File Offset: 0x001800E0
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

	// Token: 0x040030D4 RID: 12500
	[PublicizedFrom(EAccessModifier.Private)]
	public int id;

	// Token: 0x040030D5 RID: 12501
	[PublicizedFrom(EAccessModifier.Private)]
	public const ushort cFIsCrouching = 1;

	// Token: 0x040030D6 RID: 12502
	[PublicizedFrom(EAccessModifier.Private)]
	public const ushort cFIsSmellData = 2;

	// Token: 0x040030D7 RID: 12503
	[PublicizedFrom(EAccessModifier.Private)]
	public const ushort cFIsEating = 4;

	// Token: 0x040030D8 RID: 12504
	[PublicizedFrom(EAccessModifier.Private)]
	public const ushort cFIsSheltered = 8;

	// Token: 0x040030D9 RID: 12505
	[PublicizedFrom(EAccessModifier.Private)]
	public const ushort cFIsAlert = 32768;

	// Token: 0x040030DA RID: 12506
	[PublicizedFrom(EAccessModifier.Private)]
	public ushort data;

	// Token: 0x040030DB RID: 12507
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cSmellRadiusMin = -1;
}
