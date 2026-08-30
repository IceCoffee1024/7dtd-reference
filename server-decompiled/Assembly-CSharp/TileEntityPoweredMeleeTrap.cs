using System;

// Token: 0x02000C74 RID: 3188
public class TileEntityPoweredMeleeTrap : TileEntityPoweredBlock
{
	// Token: 0x06006175 RID: 24949 RVA: 0x002678FB File Offset: 0x00265AFB
	public TileEntityPoweredMeleeTrap(Chunk _chunk)
		: base(_chunk)
	{
	}

	// Token: 0x170009EF RID: 2543
	// (get) Token: 0x06006176 RID: 24950 RVA: 0x0026790B File Offset: 0x00265B0B
	// (set) Token: 0x06006177 RID: 24951 RVA: 0x00267922 File Offset: 0x00265B22
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

	// Token: 0x06006178 RID: 24952 RVA: 0x0005F529 File Offset: 0x0005D729
	public override TileEntityType GetTileEntityType()
	{
		return TileEntityType.PowerMeleeTrap;
	}

	// Token: 0x06006179 RID: 24953 RVA: 0x0026792B File Offset: 0x00265B2B
	public bool IsOwner(PlatformUserIdentifierAbs _userIdentifier)
	{
		return _userIdentifier != null && _userIdentifier.Equals(this.ownerID);
	}

	// Token: 0x0600617A RID: 24954 RVA: 0x0026793E File Offset: 0x00265B3E
	public PlatformUserIdentifierAbs GetOwner()
	{
		return this.ownerID;
	}

	// Token: 0x0600617B RID: 24955 RVA: 0x00267946 File Offset: 0x00265B46
	public void SetOwner(PlatformUserIdentifierAbs _userIdentifier)
	{
		this.ownerID = _userIdentifier;
		this.SetOwnerEntityID();
		this.setModified();
	}

	// Token: 0x0600617C RID: 24956 RVA: 0x000027FC File Offset: 0x000009FC
	public override void OnSetLocalChunkPosition()
	{
	}

	// Token: 0x0600617D RID: 24957 RVA: 0x0026795C File Offset: 0x00265B5C
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

	// Token: 0x0600617E RID: 24958 RVA: 0x0026799C File Offset: 0x00265B9C
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

	// Token: 0x0600617F RID: 24959 RVA: 0x002679D5 File Offset: 0x00265BD5
	public override void write(PooledBinaryWriter _bw, TileEntity.StreamModeWrite _eStreamMode)
	{
		base.write(_bw, _eStreamMode);
		if (_eStreamMode == TileEntity.StreamModeWrite.Persistency)
		{
			_bw.Write(18);
		}
		this.ownerID.ToStream(_bw, false);
	}

	// Token: 0x04004C31 RID: 19505
	[PublicizedFrom(EAccessModifier.Private)]
	public new const int Version = 18;

	// Token: 0x04004C32 RID: 19506
	[PublicizedFrom(EAccessModifier.Private)]
	public PlatformUserIdentifierAbs ownerID;

	// Token: 0x04004C33 RID: 19507
	[PublicizedFrom(EAccessModifier.Private)]
	public int ownerEntityID = -1;
}
