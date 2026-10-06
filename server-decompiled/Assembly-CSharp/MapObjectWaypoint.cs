using System;
using UnityEngine;

// Token: 0x0200063F RID: 1599
public class MapObjectWaypoint : MapObject
{
	// Token: 0x0600341A RID: 13338 RVA: 0x0014DE18 File Offset: 0x0014C018
	public MapObjectWaypoint(Waypoint _w)
		: base(EnumMapObjectType.MapMarker, _w.pos.ToVector3(), (long)MapObjectWaypoint.MapObjectWaypointKeys, null, false)
	{
		this.waypoint = _w;
		_w.MapObjectKey = (long)MapObjectWaypoint.MapObjectWaypointKeys++;
		this.name = _w.name.Text;
		this.iconName = _w.icon;
	}

	// Token: 0x0600341B RID: 13339 RVA: 0x0014DE77 File Offset: 0x0014C077
	public override string GetMapIcon()
	{
		return this.iconName;
	}

	// Token: 0x0600341C RID: 13340 RVA: 0x0014DE77 File Offset: 0x0014C077
	public override string GetCompassIcon()
	{
		return this.iconName;
	}

	// Token: 0x0600341D RID: 13341 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsOnCompass()
	{
		return true;
	}

	// Token: 0x0600341E RID: 13342 RVA: 0x0014DE7F File Offset: 0x0014C07F
	public override bool IsTracked()
	{
		return this.waypoint.bTracked;
	}

	// Token: 0x0600341F RID: 13343 RVA: 0x0014DE8C File Offset: 0x0014C08C
	public override float GetMaxCompassDistance()
	{
		return (float)(this.waypoint.bTracked ? 1000 : 1000);
	}

	// Token: 0x06003420 RID: 13344 RVA: 0x0014DEA8 File Offset: 0x0014C0A8
	public override float GetMinCompassDistance()
	{
		return (float)(this.waypoint.bTracked ? 250 : 0);
	}

	// Token: 0x06003421 RID: 13345 RVA: 0x0014DEC0 File Offset: 0x0014C0C0
	public override float GetMaxCompassIconScale()
	{
		return base.GetMaxCompassIconScale();
	}

	// Token: 0x06003422 RID: 13346 RVA: 0x0014DEC8 File Offset: 0x0014C0C8
	public override float GetMinCompassIconScale()
	{
		return base.GetMinCompassIconScale();
	}

	// Token: 0x06003423 RID: 13347 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLayerForMapIcon()
	{
		return 0;
	}

	// Token: 0x06003424 RID: 13348 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsMapIconEnabled()
	{
		return true;
	}

	// Token: 0x06003425 RID: 13349 RVA: 0x0014D64E File Offset: 0x0014B84E
	public override void SetPosition(Vector3 _pos)
	{
		this.position = _pos;
	}

	// Token: 0x06003426 RID: 13350 RVA: 0x0014DED0 File Offset: 0x0014C0D0
	public override string GetName()
	{
		return this.waypoint.name.Text;
	}

	// Token: 0x06003427 RID: 13351 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool IsShowName()
	{
		return false;
	}

	// Token: 0x04002909 RID: 10505
	[PublicizedFrom(EAccessModifier.Private)]
	public string name;

	// Token: 0x0400290A RID: 10506
	[PublicizedFrom(EAccessModifier.Private)]
	public string iconName;

	// Token: 0x0400290B RID: 10507
	public Waypoint waypoint;

	// Token: 0x0400290C RID: 10508
	[PublicizedFrom(EAccessModifier.Private)]
	public static int MapObjectWaypointKeys;
}
