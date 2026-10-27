using System;

// Token: 0x02001330 RID: 4912
public class Waypoint
{
	// Token: 0x17001284 RID: 4740
	// (get) Token: 0x06009AE1 RID: 39649 RVA: 0x003AB1A9 File Offset: 0x003A93A9
	// (set) Token: 0x06009AE2 RID: 39650 RVA: 0x003AB1B4 File Offset: 0x003A93B4
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

	// Token: 0x17001285 RID: 4741
	// (get) Token: 0x06009AE3 RID: 39651 RVA: 0x003AB1F5 File Offset: 0x003A93F5
	// (set) Token: 0x06009AE4 RID: 39652 RVA: 0x003AB1FD File Offset: 0x003A93FD
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

	// Token: 0x06009AE5 RID: 39653 RVA: 0x003AB217 File Offset: 0x003A9417
	public Waypoint()
	{
		this.name = new AuthoredText();
	}

	// Token: 0x06009AE6 RID: 39654 RVA: 0x003AB238 File Offset: 0x003A9438
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

	// Token: 0x06009AE7 RID: 39655 RVA: 0x003AB300 File Offset: 0x003A9500
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

	// Token: 0x06009AE8 RID: 39656 RVA: 0x003AB3C8 File Offset: 0x003A95C8
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

	// Token: 0x06009AE9 RID: 39657 RVA: 0x003AB47C File Offset: 0x003A967C
	public override bool Equals(object obj)
	{
		Waypoint waypoint = obj as Waypoint;
		return waypoint != null && (waypoint.pos.Equals(this.pos) && waypoint.icon == this.icon && waypoint.name.Text == this.name.Text && object.Equals(waypoint.ownerId, this.ownerId)) && waypoint.lastKnownPositionEntityId == this.lastKnownPositionEntityId;
	}

	// Token: 0x06009AEA RID: 39658 RVA: 0x003AB4FC File Offset: 0x003A96FC
	public override int GetHashCode()
	{
		int num = (((17 * 31 + this.pos.GetHashCode()) * 31 + this.icon.GetHashCode()) * 31 + this.name.Text.GetHashCode()) * 31;
		PlatformUserIdentifierAbs platformUserIdentifierAbs = this.ownerId;
		return (num + ((platformUserIdentifierAbs != null) ? platformUserIdentifierAbs.GetHashCode() : 0)) * 31 + this.lastKnownPositionEntityId.GetHashCode();
	}

	// Token: 0x06009AEB RID: 39659 RVA: 0x003AB568 File Offset: 0x003A9768
	public bool CanBeViewedBy(PlatformUserIdentifierAbs _userIdentifier)
	{
		return this.lastKnownPositionEntityId == -1 || (_userIdentifier != null && _userIdentifier.Equals(this.ownerId));
	}

	// Token: 0x06009AEC RID: 39660 RVA: 0x003AB588 File Offset: 0x003A9788
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

	// Token: 0x040074C4 RID: 29892
	public Vector3i pos;

	// Token: 0x040074C5 RID: 29893
	public string icon;

	// Token: 0x040074C6 RID: 29894
	public AuthoredText name;

	// Token: 0x040074C7 RID: 29895
	public bool bTracked;

	// Token: 0x040074C8 RID: 29896
	public PlatformUserIdentifierAbs ownerId;

	// Token: 0x040074C9 RID: 29897
	public int lastKnownPositionEntityId = -1;

	// Token: 0x040074CA RID: 29898
	public eLastKnownPositionEntityType lastKnownPositionEntityType;

	// Token: 0x040074CB RID: 29899
	public long MapObjectKey;

	// Token: 0x040074CC RID: 29900
	public bool hiddenOnCompass;

	// Token: 0x040074CD RID: 29901
	public NavObject navObject;

	// Token: 0x040074CE RID: 29902
	public bool bIsAutoWaypoint;

	// Token: 0x040074CF RID: 29903
	public bool bUsingLocalizationId;

	// Token: 0x040074D0 RID: 29904
	public bool IsSaved = true;

	// Token: 0x040074D1 RID: 29905
	[PublicizedFrom(EAccessModifier.Private)]
	public int inviterEntityId;

	// Token: 0x040074D2 RID: 29906
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hiddenOnMap;
}
