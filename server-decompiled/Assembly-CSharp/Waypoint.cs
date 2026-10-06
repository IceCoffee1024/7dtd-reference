using System;

// Token: 0x0200131E RID: 4894
public class Waypoint
{
	// Token: 0x17001278 RID: 4728
	// (get) Token: 0x06009A89 RID: 39561 RVA: 0x003A8DA5 File Offset: 0x003A6FA5
	// (set) Token: 0x06009A8A RID: 39562 RVA: 0x003A8DB0 File Offset: 0x003A6FB0
	public int InviterEntityId
	{
		get
		{
			return this.inviterEntityId;
		}
		set
		{
			this.inviterEntityId = value;
			PlatformUserIdentifierAbs primaryId = GameManager.Instance.persistentPlayers.GetPlayerDataFromEntityID(value).PrimaryId;
			this.name.Update(this.name.Text, primaryId);
		}
	}

	// Token: 0x17001279 RID: 4729
	// (get) Token: 0x06009A8B RID: 39563 RVA: 0x003A8DF1 File Offset: 0x003A6FF1
	// (set) Token: 0x06009A8C RID: 39564 RVA: 0x003A8DF9 File Offset: 0x003A6FF9
	public bool HiddenOnMap
	{
		get
		{
			return this.hiddenOnMap;
		}
		set
		{
			this.hiddenOnMap = value;
			this.navObject.hiddenOnMap = this.hiddenOnMap;
		}
	}

	// Token: 0x06009A8D RID: 39565 RVA: 0x003A8E13 File Offset: 0x003A7013
	public Waypoint()
	{
		this.name = new AuthoredText();
	}

	// Token: 0x06009A8E RID: 39566 RVA: 0x003A8E34 File Offset: 0x003A7034
	public Waypoint Clone()
	{
		Waypoint waypoint = new Waypoint();
		waypoint.pos = this.pos;
		waypoint.icon = this.icon;
		AuthoredText authoredText = this.name;
		waypoint.name = ((authoredText != null) ? authoredText.Clone() : null);
		waypoint.bTracked = this.bTracked;
		waypoint.ownerId = this.ownerId;
		waypoint.lastKnownPositionEntityId = this.lastKnownPositionEntityId;
		waypoint.lastKnownPositionEntityType = this.lastKnownPositionEntityType;
		waypoint.navObject = this.navObject;
		waypoint.hiddenOnCompass = this.hiddenOnCompass;
		waypoint.bIsAutoWaypoint = this.bIsAutoWaypoint;
		waypoint.bUsingLocalizationId = this.bUsingLocalizationId;
		waypoint.IsSaved = this.IsSaved;
		waypoint.inviterEntityId = this.inviterEntityId;
		waypoint.hiddenOnMap = this.hiddenOnMap;
		return waypoint;
	}

	// Token: 0x06009A8F RID: 39567 RVA: 0x003A8EFC File Offset: 0x003A70FC
	public void Read(PooledBinaryReader _br, int version = 7)
	{
		this.pos = StreamUtils.ReadVector3i(_br);
		this.icon = _br.ReadString();
		this.name = AuthoredText.FromStream(_br);
		this.bTracked = _br.ReadBoolean();
		if (version > 2)
		{
			this.hiddenOnCompass = _br.ReadBoolean();
		}
		if (version > 1)
		{
			this.ownerId = PlatformUserIdentifierAbs.FromStream(_br, false, false);
			this.lastKnownPositionEntityId = _br.ReadInt32();
		}
		if (version > 3)
		{
			this.bIsAutoWaypoint = _br.ReadBoolean();
			this.bUsingLocalizationId = _br.ReadBoolean();
		}
		if (version > 4)
		{
			this.inviterEntityId = _br.ReadInt32();
		}
		if (version > 5)
		{
			this.hiddenOnMap = _br.ReadBoolean();
		}
		if (version > 6)
		{
			this.lastKnownPositionEntityType = (eLastKnownPositionEntityType)_br.ReadInt32();
			return;
		}
		if (this.lastKnownPositionEntityId > -1)
		{
			this.lastKnownPositionEntityType = eLastKnownPositionEntityType.Vehicle;
		}
	}

	// Token: 0x06009A90 RID: 39568 RVA: 0x003A8FC4 File Offset: 0x003A71C4
	public void Write(PooledBinaryWriter _bw)
	{
		StreamUtils.Write(_bw, this.pos);
		if (this.icon == null)
		{
			_bw.Write("");
		}
		else
		{
			_bw.Write(this.icon);
		}
		AuthoredText.ToStream(this.name, _bw);
		_bw.Write(this.bTracked);
		_bw.Write(this.hiddenOnCompass);
		this.ownerId.ToStream(_bw, false);
		_bw.Write(this.lastKnownPositionEntityId);
		_bw.Write(this.bIsAutoWaypoint);
		_bw.Write(this.bUsingLocalizationId);
		_bw.Write(this.inviterEntityId);
		_bw.Write(this.hiddenOnMap);
		_bw.Write((int)this.lastKnownPositionEntityType);
	}

	// Token: 0x06009A91 RID: 39569 RVA: 0x003A9078 File Offset: 0x003A7278
	public override bool Equals(object obj)
	{
		Waypoint waypoint = obj as Waypoint;
		return waypoint != null && (waypoint.pos.Equals(this.pos) && waypoint.icon == this.icon && waypoint.name.Text == this.name.Text && object.Equals(waypoint.ownerId, this.ownerId)) && waypoint.lastKnownPositionEntityId == this.lastKnownPositionEntityId;
	}

	// Token: 0x06009A92 RID: 39570 RVA: 0x003A90F8 File Offset: 0x003A72F8
	public override int GetHashCode()
	{
		int num = (((17 * 31 + this.pos.GetHashCode()) * 31 + this.icon.GetHashCode()) * 31 + this.name.Text.GetHashCode()) * 31;
		PlatformUserIdentifierAbs platformUserIdentifierAbs = this.ownerId;
		return (num + ((platformUserIdentifierAbs != null) ? platformUserIdentifierAbs.GetHashCode() : 0)) * 31 + this.lastKnownPositionEntityId.GetHashCode();
	}

	// Token: 0x06009A93 RID: 39571 RVA: 0x003A9164 File Offset: 0x003A7364
	public bool CanBeViewedBy(PlatformUserIdentifierAbs _userIdentifier)
	{
		return this.lastKnownPositionEntityId == -1 || (_userIdentifier != null && _userIdentifier.Equals(this.ownerId));
	}

	// Token: 0x06009A94 RID: 39572 RVA: 0x003A9184 File Offset: 0x003A7384
	public override string ToString()
	{
		string[] array = new string[6];
		array[0] = "Waypoint name:";
		int num = 1;
		AuthoredText authoredText = this.name;
		array[num] = ((authoredText != null) ? authoredText.ToString() : null);
		array[2] = " icon:";
		array[3] = this.icon;
		array[4] = " Entity ID:";
		array[5] = this.lastKnownPositionEntityId.ToString();
		return string.Concat(array);
	}

	// Token: 0x04007483 RID: 29827
	public Vector3i pos;

	// Token: 0x04007484 RID: 29828
	public string icon;

	// Token: 0x04007485 RID: 29829
	public AuthoredText name;

	// Token: 0x04007486 RID: 29830
	public bool bTracked;

	// Token: 0x04007487 RID: 29831
	public PlatformUserIdentifierAbs ownerId;

	// Token: 0x04007488 RID: 29832
	public int lastKnownPositionEntityId = -1;

	// Token: 0x04007489 RID: 29833
	public eLastKnownPositionEntityType lastKnownPositionEntityType;

	// Token: 0x0400748A RID: 29834
	public long MapObjectKey;

	// Token: 0x0400748B RID: 29835
	public bool hiddenOnCompass;

	// Token: 0x0400748C RID: 29836
	public NavObject navObject;

	// Token: 0x0400748D RID: 29837
	public bool bIsAutoWaypoint;

	// Token: 0x0400748E RID: 29838
	public bool bUsingLocalizationId;

	// Token: 0x0400748F RID: 29839
	public bool IsSaved = true;

	// Token: 0x04007490 RID: 29840
	[PublicizedFrom(EAccessModifier.Private)]
	public int inviterEntityId;

	// Token: 0x04007491 RID: 29841
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hiddenOnMap;
}
