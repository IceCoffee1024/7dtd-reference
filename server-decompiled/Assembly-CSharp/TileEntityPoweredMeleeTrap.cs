using System;

// Token: 0x02000C8E RID: 3214
public class TileEntityPoweredMeleeTrap : TileEntityPoweredBlock
{
	// Token: 0x06006196 RID: 24982 RVA: 0x0026C2C3 File Offset: 0x0026A4C3
	public TileEntityPoweredMeleeTrap(Chunk _chunk)
		: base(_chunk)
	{
	}

	// Token: 0x17000A0B RID: 2571
	// (get) Token: 0x06006197 RID: 24983 RVA: 0x0026C2D3 File Offset: 0x0026A4D3
	// (set) Token: 0x06006198 RID: 24984 RVA: 0x0026C2EA File Offset: 0x0026A4EA
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

	// Token: 0x06006199 RID: 24985 RVA: 0x0026C2F3 File Offset: 0x0026A4F3
	public override TileEntityType GetTileEntityType()
	{
		return TileEntityType.PowerMeleeTrap;
	}

	// Token: 0x0600619A RID: 24986 RVA: 0x0026C2F7 File Offset: 0x0026A4F7
	public bool IsOwner(PlatformUserIdentifierAbs _userIdentifier)
	{
		return _userIdentifier != null && _userIdentifier.Equals(this.ownerID);
	}

	// Token: 0x0600619B RID: 24987 RVA: 0x0026C30A File Offset: 0x0026A50A
	public PlatformUserIdentifierAbs GetOwner()
	{
		return this.ownerID;
	}

	// Token: 0x0600619C RID: 24988 RVA: 0x0026C312 File Offset: 0x0026A512
	public void SetOwner(PlatformUserIdentifierAbs _userIdentifier)
	{
		this.ownerID = _userIdentifier;
		this.SetOwnerEntityID();
		this.setModified();
	}

	// Token: 0x0600619D RID: 24989 RVA: 0x000027FC File Offset: 0x000009FC
	public override void OnSetLocalChunkPosition()
	{
	}

	// Token: 0x0600619E RID: 24990 RVA: 0x0026C328 File Offset: 0x0026A528
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

	// Token: 0x0600619F RID: 24991 RVA: 0x0026C368 File Offset: 0x0026A568
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

	// Token: 0x060061A0 RID: 24992 RVA: 0x0026C3A1 File Offset: 0x0026A5A1
	public override void write(PooledBinaryWriter _bw, StreamModeWrite _eStreamMode)
	{
		base.write(_bw, _eStreamMode);
		if (_eStreamMode == StreamModeWrite.Persistency)
		{
			_bw.Write(18);
		}
		this.ownerID.ToStream(_bw, false);
	}

	// Token: 0x04004C8D RID: 19597
	[PublicizedFrom(EAccessModifier.Private)]
	public new const int Version = 18;

	// Token: 0x04004C8E RID: 19598
	[PublicizedFrom(EAccessModifier.Private)]
	public PlatformUserIdentifierAbs ownerID;

	// Token: 0x04004C8F RID: 19599
	[PublicizedFrom(EAccessModifier.Private)]
	public int ownerEntityID = -1;
}
