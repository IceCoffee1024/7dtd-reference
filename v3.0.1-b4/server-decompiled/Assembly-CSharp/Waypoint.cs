using System;
using System.IO;

// Token: 0x020012E4 RID: 4836
public class Waypoint
{
	// Token: 0x170011FA RID: 4602
	// (get) Token: 0x060098E2 RID: 39138 RVA: 0x0039EF5D File Offset: 0x0039D15D
	// (set) Token: 0x060098E3 RID: 39139 RVA: 0x0039EF68 File Offset: 0x0039D168
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

	// Token: 0x170011FB RID: 4603
	// (get) Token: 0x060098E4 RID: 39140 RVA: 0x0039EFA9 File Offset: 0x0039D1A9
	// (set) Token: 0x060098E5 RID: 39141 RVA: 0x0039EFB1 File Offset: 0x0039D1B1
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

	// Token: 0x060098E6 RID: 39142 RVA: 0x0039EFCB File Offset: 0x0039D1CB
	public Waypoint()
	{
		this.name = new AuthoredText();
	}

	// Token: 0x060098E7 RID: 39143 RVA: 0x0039EFEC File Offset: 0x0039D1EC
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

	// Token: 0x060098E8 RID: 39144 RVA: 0x0039F0B4 File Offset: 0x0039D2B4
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

	// Token: 0x060098E9 RID: 39145 RVA: 0x0039F17C File Offset: 0x0039D37C
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

	// Token: 0x060098EA RID: 39146 RVA: 0x0039F230 File Offset: 0x0039D430
	public override bool Equals(object obj)
	{
		Waypoint waypoint = obj as Waypoint;
		return waypoint != null && (waypoint.pos.Equals(this.pos) && waypoint.icon == this.icon && waypoint.name.Text == this.name.Text && object.Equals(waypoint.ownerId, this.ownerId)) && waypoint.lastKnownPositionEntityId == this.lastKnownPositionEntityId;
	}

	// Token: 0x060098EB RID: 39147 RVA: 0x0039F2B0 File Offset: 0x0039D4B0
	public override int GetHashCode()
	{
		int num = (((17 * 31 + this.pos.GetHashCode()) * 31 + this.icon.GetHashCode()) * 31 + this.name.Text.GetHashCode()) * 31;
		PlatformUserIdentifierAbs platformUserIdentifierAbs = this.ownerId;
		return (num + ((platformUserIdentifierAbs != null) ? platformUserIdentifierAbs.GetHashCode() : 0)) * 31 + this.lastKnownPositionEntityId.GetHashCode();
	}

	// Token: 0x060098EC RID: 39148 RVA: 0x0039F31C File Offset: 0x0039D51C
	public bool CanBeViewedBy(PlatformUserIdentifierAbs _userIdentifier)
	{
		return this.lastKnownPositionEntityId == -1 || (_userIdentifier != null && _userIdentifier.Equals(this.ownerId));
	}

	// Token: 0x060098ED RID: 39149 RVA: 0x0039F33C File Offset: 0x0039D53C
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

	// Token: 0x04007330 RID: 29488
	public Vector3i pos;

	// Token: 0x04007331 RID: 29489
	public string icon;

	// Token: 0x04007332 RID: 29490
	public AuthoredText name;

	// Token: 0x04007333 RID: 29491
	public bool bTracked;

	// Token: 0x04007334 RID: 29492
	public PlatformUserIdentifierAbs ownerId;

	// Token: 0x04007335 RID: 29493
	public int lastKnownPositionEntityId = -1;

	// Token: 0x04007336 RID: 29494
	public eLastKnownPositionEntityType lastKnownPositionEntityType;

	// Token: 0x04007337 RID: 29495
	public long MapObjectKey;

	// Token: 0x04007338 RID: 29496
	public bool hiddenOnCompass;

	// Token: 0x04007339 RID: 29497
	public NavObject navObject;

	// Token: 0x0400733A RID: 29498
	public bool bIsAutoWaypoint;

	// Token: 0x0400733B RID: 29499
	public bool bUsingLocalizationId;

	// Token: 0x0400733C RID: 29500
	public bool IsSaved = true;

	// Token: 0x0400733D RID: 29501
	[PublicizedFrom(EAccessModifier.Private)]
	public int inviterEntityId;

	// Token: 0x0400733E RID: 29502
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hiddenOnMap;
}
