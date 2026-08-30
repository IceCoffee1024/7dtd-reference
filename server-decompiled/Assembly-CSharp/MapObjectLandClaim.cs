using System;
using UnityEngine;

// Token: 0x02000628 RID: 1576
public class MapObjectLandClaim : MapObject
{
	// Token: 0x0600331E RID: 13086 RVA: 0x0014AD6D File Offset: 0x00148F6D
	public MapObjectLandClaim(Vector3 _position, Entity _entity)
		: base(EnumMapObjectType.LandClaim, _position, (long)MapObjectLandClaim.MapObjectLandCLaimKeys++, _entity, false)
	{
	}

	// Token: 0x0600331F RID: 13087 RVA: 0x0014AD88 File Offset: 0x00148F88
	public override string GetMapIcon()
	{
		return "ui_game_symbol_brick";
	}

	// Token: 0x06003320 RID: 13088 RVA: 0x0014AD88 File Offset: 0x00148F88
	public override string GetCompassIcon()
	{
		return "ui_game_symbol_brick";
	}

	// Token: 0x06003321 RID: 13089 RVA: 0x0014AD8F File Offset: 0x00148F8F
	public override bool IsOnCompass()
	{
		return this.IsMapIconEnabled();
	}

	// Token: 0x06003322 RID: 13090 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLayerForMapIcon()
	{
		return 0;
	}

	// Token: 0x06003323 RID: 13091 RVA: 0x0014AD97 File Offset: 0x00148F97
	public override bool IsMapIconEnabled()
	{
		return this.entity != null && this.entity is EntityPlayerLocal;
	}

	// Token: 0x06003324 RID: 13092 RVA: 0x000BCED3 File Offset: 0x000BB0D3
	public override Color GetMapIconColor()
	{
		return Color.white;
	}

	// Token: 0x04002890 RID: 10384
	[PublicizedFrom(EAccessModifier.Private)]
	public static int MapObjectLandCLaimKeys;
}
