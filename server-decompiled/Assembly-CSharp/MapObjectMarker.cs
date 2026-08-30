using System;
using UnityEngine;

// Token: 0x02000634 RID: 1588
public class MapObjectMarker : MapObject
{
	// Token: 0x06003388 RID: 13192 RVA: 0x0014D22A File Offset: 0x0014B42A
	public MapObjectMarker(Vector3 _position, long _key)
		: base(EnumMapObjectType.MapQuickMarker, _position, _key, null, false)
	{
	}

	// Token: 0x06003389 RID: 13193 RVA: 0x0014D237 File Offset: 0x0014B437
	public override string GetMapIcon()
	{
		return "ui_game_symbol_map_waypoint_set";
	}

	// Token: 0x0600338A RID: 13194 RVA: 0x0014D237 File Offset: 0x0014B437
	public override string GetCompassIcon()
	{
		return "ui_game_symbol_map_waypoint_set";
	}

	// Token: 0x0600338B RID: 13195 RVA: 0x0014D23E File Offset: 0x0014B43E
	public override Color GetMapIconColor()
	{
		return Color.red;
	}

	// Token: 0x0600338C RID: 13196 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool IsOnCompass()
	{
		return true;
	}

	// Token: 0x0600338D RID: 13197 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLayerForMapIcon()
	{
		return 0;
	}

	// Token: 0x0600338E RID: 13198 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool IsMapIconEnabled()
	{
		return true;
	}

	// Token: 0x0600338F RID: 13199 RVA: 0x0014CCDE File Offset: 0x0014AEDE
	public override void SetPosition(Vector3 _pos)
	{
		this.position = _pos;
	}

	// Token: 0x06003390 RID: 13200 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool IsCenterOnLeftBottomCorner()
	{
		return true;
	}
}
