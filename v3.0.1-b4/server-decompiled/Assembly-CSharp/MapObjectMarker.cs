using System;
using UnityEngine;

// Token: 0x0200062B RID: 1579
public class MapObjectMarker : MapObject
{
	// Token: 0x06003335 RID: 13109 RVA: 0x0014B10E File Offset: 0x0014930E
	public MapObjectMarker(Vector3 _position, long _key)
		: base(EnumMapObjectType.MapQuickMarker, _position, _key, null, false)
	{
	}

	// Token: 0x06003336 RID: 13110 RVA: 0x0014B11B File Offset: 0x0014931B
	public override string GetMapIcon()
	{
		return "ui_game_symbol_map_waypoint_set";
	}

	// Token: 0x06003337 RID: 13111 RVA: 0x0014B11B File Offset: 0x0014931B
	public override string GetCompassIcon()
	{
		return "ui_game_symbol_map_waypoint_set";
	}

	// Token: 0x06003338 RID: 13112 RVA: 0x0014B122 File Offset: 0x00149322
	public override Color GetMapIconColor()
	{
		return Color.red;
	}

	// Token: 0x06003339 RID: 13113 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool IsOnCompass()
	{
		return true;
	}

	// Token: 0x0600333A RID: 13114 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLayerForMapIcon()
	{
		return 0;
	}

	// Token: 0x0600333B RID: 13115 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool IsMapIconEnabled()
	{
		return true;
	}

	// Token: 0x0600333C RID: 13116 RVA: 0x0014ABC2 File Offset: 0x00148DC2
	public override void SetPosition(Vector3 _pos)
	{
		this.position = _pos;
	}

	// Token: 0x0600333D RID: 13117 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool IsCenterOnLeftBottomCorner()
	{
		return true;
	}
}
