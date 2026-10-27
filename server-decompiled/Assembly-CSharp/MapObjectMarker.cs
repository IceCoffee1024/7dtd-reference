using System;
using UnityEngine;

// Token: 0x0200063D RID: 1597
public class MapObjectMarker : MapObject
{
	// Token: 0x060033D9 RID: 13273 RVA: 0x0014E78E File Offset: 0x0014C98E
	public MapObjectMarker(Vector3 _position, long _key)
		: base(EnumMapObjectType.MapQuickMarker, _position, _key, null, false)
	{
	}

	// Token: 0x060033DA RID: 13274 RVA: 0x0014E79B File Offset: 0x0014C99B
	public override string GetMapIcon()
	{
		return "ui_game_symbol_map_waypoint_set";
	}

	// Token: 0x060033DB RID: 13275 RVA: 0x0014E79B File Offset: 0x0014C99B
	public override string GetCompassIcon()
	{
		return "ui_game_symbol_map_waypoint_set";
	}

	// Token: 0x060033DC RID: 13276 RVA: 0x0014E7A2 File Offset: 0x0014C9A2
	public override Color GetMapIconColor()
	{
		return Color.red;
	}

	// Token: 0x060033DD RID: 13277 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsOnCompass()
	{
		return true;
	}

	// Token: 0x060033DE RID: 13278 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLayerForMapIcon()
	{
		return 0;
	}

	// Token: 0x060033DF RID: 13279 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsMapIconEnabled()
	{
		return true;
	}

	// Token: 0x060033E0 RID: 13280 RVA: 0x0014E242 File Offset: 0x0014C442
	public override void SetPosition(Vector3 _pos)
	{
		this.position = _pos;
	}

	// Token: 0x060033E1 RID: 13281 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsCenterOnLeftBottomCorner()
	{
		return true;
	}
}
