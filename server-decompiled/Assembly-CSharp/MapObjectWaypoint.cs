using System;
using UnityEngine;

// Token: 0x02000646 RID: 1606
public class MapObjectWaypoint : MapObject
{
	// Token: 0x06003428 RID: 13352 RVA: 0x0014EA0C File Offset: 0x0014CC0C
	public MapObjectWaypoint(Waypoint _w)
		: base(EnumMapObjectType.MapMarker, _w.pos.ToVector3(), (long)MapObjectWaypoint.MapObjectWaypointKeys, null, false)
	{
		this.waypoint = _w;
		_w.MapObjectKey = (long)MapObjectWaypoint.MapObjectWaypointKeys++;
		this.name = _w.name.Text;
		this.iconName = _w.icon;
	}

	// Token: 0x06003429 RID: 13353 RVA: 0x0014EA6B File Offset: 0x0014CC6B
	public override string GetMapIcon()
	{
		return this.iconName;
	}

	// Token: 0x0600342A RID: 13354 RVA: 0x0014EA6B File Offset: 0x0014CC6B
	public override string GetCompassIcon()
	{
		return this.iconName;
	}

	// Token: 0x0600342B RID: 13355 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsOnCompass()
	{
		return true;
	}

	// Token: 0x0600342C RID: 13356 RVA: 0x0014EA73 File Offset: 0x0014CC73
	public override bool IsTracked()
	{
		return this.waypoint.bTracked;
	}

	// Token: 0x0600342D RID: 13357 RVA: 0x0014EA80 File Offset: 0x0014CC80
	public override float GetMaxCompassDistance()
	{
		return (float)(this.waypoint.bTracked ? 1000 : 1000);
	}

	// Token: 0x0600342E RID: 13358 RVA: 0x0014EA9C File Offset: 0x0014CC9C
	public override float GetMinCompassDistance()
	{
		return (float)(this.waypoint.bTracked ? 250 : 0);
	}

	// Token: 0x0600342F RID: 13359 RVA: 0x0014EAB4 File Offset: 0x0014CCB4
	public override float GetMaxCompassIconScale()
	{
		return base.GetMaxCompassIconScale();
	}

	// Token: 0x06003430 RID: 13360 RVA: 0x0014EABC File Offset: 0x0014CCBC
	public override float GetMinCompassIconScale()
	{
		return base.GetMinCompassIconScale();
	}

	// Token: 0x06003431 RID: 13361 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLayerForMapIcon()
	{
		return 0;
	}

	// Token: 0x06003432 RID: 13362 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsMapIconEnabled()
	{
		return true;
	}

	// Token: 0x06003433 RID: 13363 RVA: 0x0014E242 File Offset: 0x0014C442
	public override void SetPosition(Vector3 _pos)
	{
		this.position = _pos;
	}

	// Token: 0x06003434 RID: 13364 RVA: 0x0014EAC4 File Offset: 0x0014CCC4
	public override string GetName()
	{
		return this.waypoint.name.Text;
	}

	// Token: 0x06003435 RID: 13365 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool IsShowName()
	{
		return false;
	}

	// Token: 0x04002924 RID: 10532
	[PublicizedFrom(EAccessModifier.Private)]
	public string name;

	// Token: 0x04002925 RID: 10533
	[PublicizedFrom(EAccessModifier.Private)]
	public string iconName;

	// Token: 0x04002926 RID: 10534
	public Waypoint waypoint;

	// Token: 0x04002927 RID: 10535
	[PublicizedFrom(EAccessModifier.Private)]
	public static int MapObjectWaypointKeys;
}
