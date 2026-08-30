using System;
using System.IO;

// Token: 0x020012FE RID: 4862
public class Waypoint
{
	// Token: 0x17001233 RID: 4659
	// (get) Token: 0x060099DD RID: 39389 RVA: 0x003A41F5 File Offset: 0x003A23F5
	// (set) Token: 0x060099DE RID: 39390 RVA: 0x003A4200 File Offset: 0x003A2400
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

	// Token: 0x17001234 RID: 4660
	// (get) Token: 0x060099DF RID: 39391 RVA: 0x003A4241 File Offset: 0x003A2441
	// (set) Token: 0x060099E0 RID: 39392 RVA: 0x003A4249 File Offset: 0x003A2449
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

	// Token: 0x060099E1 RID: 39393 RVA: 0x003A4263 File Offset: 0x003A2463
	public Waypoint()
	{
		this.name = new AuthoredText();
	}

	// Token: 0x060099E2 RID: 39394 RVA: 0x003A4284 File Offset: 0x003A2484
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

	// Token: 0x060099E3 RID: 39395 RVA: 0x003A434C File Offset: 0x003A254C
	public void Read(BinaryReader _br, int version = 7)
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

	// Token: 0x060099E4 RID: 39396 RVA: 0x003A4414 File Offset: 0x003A2614
	public void Write(BinaryWriter _bw)
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

	// Token: 0x060099E5 RID: 39397 RVA: 0x003A44C8 File Offset: 0x003A26C8
	public override bool Equals(object obj)
	{
		Waypoint waypoint = obj as Waypoint;
		return waypoint != null && (waypoint.pos.Equals(this.pos) && waypoint.icon == this.icon && waypoint.name.Text == this.name.Text && object.Equals(waypoint.ownerId, this.ownerId)) && waypoint.lastKnownPositionEntityId == this.lastKnownPositionEntityId;
	}

	// Token: 0x060099E6 RID: 39398 RVA: 0x003A4548 File Offset: 0x003A2748
	public override int GetHashCode()
	{
		int num = (((17 * 31 + this.pos.GetHashCode()) * 31 + this.icon.GetHashCode()) * 31 + this.name.Text.GetHashCode()) * 31;
		PlatformUserIdentifierAbs platformUserIdentifierAbs = this.ownerId;
		return (num + ((platformUserIdentifierAbs != null) ? platformUserIdentifierAbs.GetHashCode() : 0)) * 31 + this.lastKnownPositionEntityId.GetHashCode();
	}

	// Token: 0x060099E7 RID: 39399 RVA: 0x003A45B4 File Offset: 0x003A27B4
	public bool CanBeViewedBy(PlatformUserIdentifierAbs _userIdentifier)
	{
		return this.lastKnownPositionEntityId == -1 || (_userIdentifier != null && _userIdentifier.Equals(this.ownerId));
	}

	// Token: 0x060099E8 RID: 39400 RVA: 0x003A45D4 File Offset: 0x003A27D4
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

	// Token: 0x04007408 RID: 29704
	public Vector3i pos;

	// Token: 0x04007409 RID: 29705
	public string icon;

	// Token: 0x0400740A RID: 29706
	public AuthoredText name;

	// Token: 0x0400740B RID: 29707
	public bool bTracked;

	// Token: 0x0400740C RID: 29708
	public PlatformUserIdentifierAbs ownerId;

	// Token: 0x0400740D RID: 29709
	public int lastKnownPositionEntityId = -1;

	// Token: 0x0400740E RID: 29710
	public eLastKnownPositionEntityType lastKnownPositionEntityType;

	// Token: 0x0400740F RID: 29711
	public long MapObjectKey;

	// Token: 0x04007410 RID: 29712
	public bool hiddenOnCompass;

	// Token: 0x04007411 RID: 29713
	public NavObject navObject;

	// Token: 0x04007412 RID: 29714
	public bool bIsAutoWaypoint;

	// Token: 0x04007413 RID: 29715
	public bool bUsingLocalizationId;

	// Token: 0x04007414 RID: 29716
	public bool IsSaved = true;

	// Token: 0x04007415 RID: 29717
	[PublicizedFrom(EAccessModifier.Private)]
	public int inviterEntityId;

	// Token: 0x04007416 RID: 29718
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hiddenOnMap;
}
