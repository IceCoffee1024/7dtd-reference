using System;
using UnityEngine;

// Token: 0x02000636 RID: 1590
public class MapObjectMarker : MapObject
{
	// Token: 0x060033CB RID: 13259 RVA: 0x0014DB9A File Offset: 0x0014BD9A
	public MapObjectMarker(Vector3 _position, long _key)
		: base(EnumMapObjectType.MapQuickMarker, _position, _key, null, false)
	{
	}

	// Token: 0x060033CC RID: 13260 RVA: 0x0014DBA7 File Offset: 0x0014BDA7
	public override string GetMapIcon()
	{
		return "ui_game_symbol_map_waypoint_set";
	}

	// Token: 0x060033CD RID: 13261 RVA: 0x0014DBA7 File Offset: 0x0014BDA7
	public override string GetCompassIcon()
	{
		return "ui_game_symbol_map_waypoint_set";
	}

	// Token: 0x060033CE RID: 13262 RVA: 0x0014DBAE File Offset: 0x0014BDAE
	public override Color GetMapIconColor()
	{
		return Color.red;
	}

	// Token: 0x060033CF RID: 13263 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsOnCompass()
	{
		return true;
	}

	// Token: 0x060033D0 RID: 13264 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLayerForMapIcon()
	{
		return 0;
	}

	// Token: 0x060033D1 RID: 13265 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsMapIconEnabled()
	{
		return true;
	}

	// Token: 0x060033D2 RID: 13266 RVA: 0x0014D64E File Offset: 0x0014B84E
	public override void SetPosition(Vector3 _pos)
	{
		this.position = _pos;
	}

	// Token: 0x060033D3 RID: 13267 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsCenterOnLeftBottomCorner()
	{
		return true;
	}
}
