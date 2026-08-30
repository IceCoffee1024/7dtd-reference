using System;
using UnityEngine;

// Token: 0x02000634 RID: 1588
public class MapObjectWaypoint : MapObject
{
	// Token: 0x06003384 RID: 13188 RVA: 0x0014B38C File Offset: 0x0014958C
	public MapObjectWaypoint(Waypoint _w)
		: base(EnumMapObjectType.MapMarker, _w.pos.ToVector3(), (long)MapObjectWaypoint.MapObjectWaypointKeys, null, false)
	{
		this.waypoint = _w;
		_w.MapObjectKey = (long)MapObjectWaypoint.MapObjectWaypointKeys++;
		this.name = _w.name.Text;
		this.iconName = _w.icon;
	}

	// Token: 0x06003385 RID: 13189 RVA: 0x0014B3EB File Offset: 0x001495EB
	public override string GetMapIcon()
	{
		return this.iconName;
	}

	// Token: 0x06003386 RID: 13190 RVA: 0x0014B3EB File Offset: 0x001495EB
	public override string GetCompassIcon()
	{
		return this.iconName;
	}

	// Token: 0x06003387 RID: 13191 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool IsOnCompass()
	{
		return true;
	}

	// Token: 0x06003388 RID: 13192 RVA: 0x0014B3F3 File Offset: 0x001495F3
	public override bool IsTracked()
	{
		return this.waypoint.bTracked;
	}

	// Token: 0x06003389 RID: 13193 RVA: 0x0014B400 File Offset: 0x00149600
	public override float GetMaxCompassDistance()
	{
		return (float)(this.waypoint.bTracked ? 1000 : 1000);
	}

	// Token: 0x0600338A RID: 13194 RVA: 0x0014B41C File Offset: 0x0014961C
	public override float GetMinCompassDistance()
	{
		return (float)(this.waypoint.bTracked ? 250 : 0);
	}

	// Token: 0x0600338B RID: 13195 RVA: 0x0014B434 File Offset: 0x00149634
	public override float GetMaxCompassIconScale()
	{
		return base.GetMaxCompassIconScale();
	}

	// Token: 0x0600338C RID: 13196 RVA: 0x0014B43C File Offset: 0x0014963C
	public override float GetMinCompassIconScale()
	{
		return base.GetMinCompassIconScale();
	}

	// Token: 0x0600338D RID: 13197 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLayerForMapIcon()
	{
		return 0;
	}

	// Token: 0x0600338E RID: 13198 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool IsMapIconEnabled()
	{
		return true;
	}

	// Token: 0x0600338F RID: 13199 RVA: 0x0014ABC2 File Offset: 0x00148DC2
	public override void SetPosition(Vector3 _pos)
	{
		this.position = _pos;
	}

	// Token: 0x06003390 RID: 13200 RVA: 0x0014B444 File Offset: 0x00149644
	public override string GetName()
	{
		return this.waypoint.name.Text;
	}

	// Token: 0x06003391 RID: 13201 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool IsShowName()
	{
		return false;
	}

	// Token: 0x0400289D RID: 10397
	[PublicizedFrom(EAccessModifier.Private)]
	public string name;

	// Token: 0x0400289E RID: 10398
	[PublicizedFrom(EAccessModifier.Private)]
	public string iconName;

	// Token: 0x0400289F RID: 10399
	public Waypoint waypoint;

	// Token: 0x040028A0 RID: 10400
	[PublicizedFrom(EAccessModifier.Private)]
	public static int MapObjectWaypointKeys;
}
