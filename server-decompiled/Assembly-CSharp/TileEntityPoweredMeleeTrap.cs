using System;

// Token: 0x02000C62 RID: 3170
public class TileEntityPoweredMeleeTrap : TileEntityPoweredBlock
{
	// Token: 0x060060EA RID: 24810 RVA: 0x00264A13 File Offset: 0x00262C13
	public TileEntityPoweredMeleeTrap(Chunk _chunk)
		: base(_chunk)
	{
	}

	// Token: 0x170009DC RID: 2524
	// (get) Token: 0x060060EB RID: 24811 RVA: 0x00264A23 File Offset: 0x00262C23
	// (set) Token: 0x060060EC RID: 24812 RVA: 0x00264A3A File Offset: 0x00262C3A
	public int OwnerEntityID
	{
		get
		{
			if (this.ownerEntityID == -1)
			{
				this.SetOwnerEntityID();
			}
			return this.ownerEntityID;
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			this.ownerEntityID = value;
		}
	}

	// Token: 0x060060ED RID: 24813 RVA: 0x0005EFE1 File Offset: 0x0005D1E1
	public override TileEntityType GetTileEntityType()
	{
		return TileEntityType.PowerMeleeTrap;
	}

	// Token: 0x060060EE RID: 24814 RVA: 0x00264A43 File Offset: 0x00262C43
	public bool IsOwner(PlatformUserIdentifierAbs _userIdentifier)
	{
		return _userIdentifier != null && _userIdentifier.Equals(this.ownerID);
	}

	// Token: 0x060060EF RID: 24815 RVA: 0x00264A56 File Offset: 0x00262C56
	public PlatformUserIdentifierAbs GetOwner()
	{
		return this.ownerID;
	}

	// Token: 0x060060F0 RID: 24816 RVA: 0x00264A5E File Offset: 0x00262C5E
	public void SetOwner(PlatformUserIdentifierAbs _userIdentifier)
	{
		this.ownerID = _userIdentifier;
		this.SetOwnerEntityID();
		this.setModified();
	}

	// Token: 0x060060F1 RID: 24817 RVA: 0x000027FC File Offset: 0x000009FC
	public override void OnSetLocalChunkPosition()
	{
	}

	// Token: 0x060060F2 RID: 24818 RVA: 0x00264A74 File Offset: 0x00262C74
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetOwnerEntityID()
	{
		this.ownerEntityID = -1;
		PersistentPlayerList persistentPlayerList = GameManager.Instance.GetPersistentPlayerList();
		PersistentPlayerData persistentPlayerData = ((persistentPlayerList != null) ? persistentPlayerList.GetPlayerData(this.ownerID) : null);
		if (persistentPlayerData != null)
		{
			this.ownerEntityID = persistentPlayerData.EntityId;
		}
	}

	// Token: 0x060060F3 RID: 24819 RVA: 0x00264AB4 File Offset: 0x00262CB4
	public override void read(PooledBinaryReader _br, TileEntity.StreamModeRead _eStreamMode)
	{
		base.read(_br, _eStreamMode);
		if (_eStreamMode == TileEntity.StreamModeRead.Persistency)
		{
			if (base.UseLocalVersioning())
			{
				_br.ReadUInt16();
			}
			else
			{
				base.GetLegacyForkVersion();
			}
		}
		this.ownerID = PlatformUserIdentifierAbs.FromStream(_br, false, false);
		this.SetOwnerEntityID();
	}

	// Token: 0x060060F4 RID: 24820 RVA: 0x00264AED File Offset: 0x00262CED
	public override void write(PooledBinaryWriter _bw, TileEntity.StreamModeWrite _eStreamMode)
	{
		base.write(_bw, _eStreamMode);
		if (_eStreamMode == TileEntity.StreamModeWrite.Persistency)
		{
			_bw.Write(18);
		}
		this.ownerID.ToStream(_bw, false);
	}

	// Token: 0x04004B94 RID: 19348
	[PublicizedFrom(EAccessModifier.Private)]
	public new const int Version = 18;

	// Token: 0x04004B95 RID: 19349
	[PublicizedFrom(EAccessModifier.Private)]
	public PlatformUserIdentifierAbs ownerID;

	// Token: 0x04004B96 RID: 19350
	[PublicizedFrom(EAccessModifier.Private)]
	public int ownerEntityID = -1;
}
