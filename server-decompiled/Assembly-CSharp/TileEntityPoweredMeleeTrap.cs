using System;

// Token: 0x02000C83 RID: 3203
public class TileEntityPoweredMeleeTrap : TileEntityPoweredBlock
{
	// Token: 0x06006171 RID: 24945 RVA: 0x0026AD9B File Offset: 0x00268F9B
	public TileEntityPoweredMeleeTrap(Chunk _chunk)
		: base(_chunk)
	{
	}

	// Token: 0x17000A05 RID: 2565
	// (get) Token: 0x06006172 RID: 24946 RVA: 0x0026ADAB File Offset: 0x00268FAB
	// (set) Token: 0x06006173 RID: 24947 RVA: 0x0026ADC2 File Offset: 0x00268FC2
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

	// Token: 0x06006174 RID: 24948 RVA: 0x0026ADCB File Offset: 0x00268FCB
	public override TileEntityType GetTileEntityType()
	{
		return TileEntityType.PowerMeleeTrap;
	}

	// Token: 0x06006175 RID: 24949 RVA: 0x0026ADCF File Offset: 0x00268FCF
	public bool IsOwner(PlatformUserIdentifierAbs _userIdentifier)
	{
		return _userIdentifier != null && _userIdentifier.Equals(this.ownerID);
	}

	// Token: 0x06006176 RID: 24950 RVA: 0x0026ADE2 File Offset: 0x00268FE2
	public PlatformUserIdentifierAbs GetOwner()
	{
		return this.ownerID;
	}

	// Token: 0x06006177 RID: 24951 RVA: 0x0026ADEA File Offset: 0x00268FEA
	public void SetOwner(PlatformUserIdentifierAbs _userIdentifier)
	{
		this.ownerID = _userIdentifier;
		this.SetOwnerEntityID();
		this.setModified();
	}

	// Token: 0x06006178 RID: 24952 RVA: 0x000027FC File Offset: 0x000009FC
	public override void OnSetLocalChunkPosition()
	{
	}

	// Token: 0x06006179 RID: 24953 RVA: 0x0026AE00 File Offset: 0x00269000
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

	// Token: 0x0600617A RID: 24954 RVA: 0x0026AE40 File Offset: 0x00269040
	public override void read(PooledBinaryReader _br, StreamModeRead _eStreamMode)
	{
		base.read(_br, _eStreamMode);
		if (_eStreamMode == StreamModeRead.Persistency)
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

	// Token: 0x0600617B RID: 24955 RVA: 0x0026AE79 File Offset: 0x00269079
	public override void write(PooledBinaryWriter _bw, StreamModeWrite _eStreamMode)
	{
		base.write(_bw, _eStreamMode);
		if (_eStreamMode == StreamModeWrite.Persistency)
		{
			_bw.Write(18);
		}
		this.ownerID.ToStream(_bw, false);
	}

	// Token: 0x04004C68 RID: 19560
	[PublicizedFrom(EAccessModifier.Private)]
	public new const int Version = 18;

	// Token: 0x04004C69 RID: 19561
	[PublicizedFrom(EAccessModifier.Private)]
	public PlatformUserIdentifierAbs ownerID;

	// Token: 0x04004C6A RID: 19562
	[PublicizedFrom(EAccessModifier.Private)]
	public int ownerEntityID = -1;
}
