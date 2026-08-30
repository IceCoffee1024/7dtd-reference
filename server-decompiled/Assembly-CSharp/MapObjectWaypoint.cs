using System;
using UnityEngine;

// Token: 0x0200063D RID: 1597
public class MapObjectWaypoint : MapObject
{
	// Token: 0x060033D7 RID: 13271 RVA: 0x0014D4A8 File Offset: 0x0014B6A8
	public MapObjectWaypoint(Waypoint _w)
		: base(EnumMapObjectType.MapMarker, _w.pos.ToVector3(), (long)MapObjectWaypoint.MapObjectWaypointKeys, null, false)
	{
		this.waypoint = _w;
		_w.MapObjectKey = (long)MapObjectWaypoint.MapObjectWaypointKeys++;
		this.name = _w.name.Text;
		this.iconName = _w.icon;
	}

	// Token: 0x060033D8 RID: 13272 RVA: 0x0014D507 File Offset: 0x0014B707
	public override string GetMapIcon()
	{
		return this.iconName;
	}

	// Token: 0x060033D9 RID: 13273 RVA: 0x0014D507 File Offset: 0x0014B707
	public override string GetCompassIcon()
	{
		return this.iconName;
	}

	// Token: 0x060033DA RID: 13274 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool IsOnCompass()
	{
		return true;
	}

	// Token: 0x060033DB RID: 13275 RVA: 0x0014D50F File Offset: 0x0014B70F
	public override bool IsTracked()
	{
		return this.waypoint.bTracked;
	}

	// Token: 0x060033DC RID: 13276 RVA: 0x0014D51C File Offset: 0x0014B71C
	public override float GetMaxCompassDistance()
	{
		return (float)(this.waypoint.bTracked ? 1000 : 1000);
	}

	// Token: 0x060033DD RID: 13277 RVA: 0x0014D538 File Offset: 0x0014B738
	public override float GetMinCompassDistance()
	{
		return (float)(this.waypoint.bTracked ? 250 : 0);
	}

	// Token: 0x060033DE RID: 13278 RVA: 0x0014D550 File Offset: 0x0014B750
	public override float GetMaxCompassIconScale()
	{
		return base.GetMaxCompassIconScale();
	}

	// Token: 0x060033DF RID: 13279 RVA: 0x0014D558 File Offset: 0x0014B758
	public override float GetMinCompassIconScale()
	{
		return base.GetMinCompassIconScale();
	}

	// Token: 0x060033E0 RID: 13280 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLayerForMapIcon()
	{
		return 0;
	}

	// Token: 0x060033E1 RID: 13281 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool IsMapIconEnabled()
	{
		return true;
	}

	// Token: 0x060033E2 RID: 13282 RVA: 0x0014CCDE File Offset: 0x0014AEDE
	public override void SetPosition(Vector3 _pos)
	{
		this.position = _pos;
	}

	// Token: 0x060033E3 RID: 13283 RVA: 0x0014D560 File Offset: 0x0014B760
	public override string GetName()
	{
		return this.waypoint.name.Text;
	}

	// Token: 0x060033E4 RID: 13284 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool IsShowName()
	{
		return false;
	}

	// Token: 0x04002910 RID: 10512
	[PublicizedFrom(EAccessModifier.Private)]
	public string name;

	// Token: 0x04002911 RID: 10513
	[PublicizedFrom(EAccessModifier.Private)]
	public string iconName;

	// Token: 0x04002912 RID: 10514
	public Waypoint waypoint;

	// Token: 0x04002913 RID: 10515
	[PublicizedFrom(EAccessModifier.Private)]
	public static int MapObjectWaypointKeys;
}
