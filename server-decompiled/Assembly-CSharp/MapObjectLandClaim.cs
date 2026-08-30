using System;
using UnityEngine;

// Token: 0x02000631 RID: 1585
public class MapObjectLandClaim : MapObject
{
	// Token: 0x06003371 RID: 13169 RVA: 0x0014CE89 File Offset: 0x0014B089
	public MapObjectLandClaim(Vector3 _position, Entity _entity)
		: base(EnumMapObjectType.LandClaim, _position, (long)MapObjectLandClaim.MapObjectLandCLaimKeys++, _entity, false)
	{
	}

	// Token: 0x06003372 RID: 13170 RVA: 0x0014CEA4 File Offset: 0x0014B0A4
	public override string GetMapIcon()
	{
		return "ui_game_symbol_brick";
	}

	// Token: 0x06003373 RID: 13171 RVA: 0x0014CEA4 File Offset: 0x0014B0A4
	public override string GetCompassIcon()
	{
		return "ui_game_symbol_brick";
	}

	// Token: 0x06003374 RID: 13172 RVA: 0x0014CEAB File Offset: 0x0014B0AB
	public override bool IsOnCompass()
	{
		return this.IsMapIconEnabled();
	}

	// Token: 0x06003375 RID: 13173 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLayerForMapIcon()
	{
		return 0;
	}

	// Token: 0x06003376 RID: 13174 RVA: 0x0014CEB3 File Offset: 0x0014B0B3
	public override bool IsMapIconEnabled()
	{
		return this.entity != null && this.entity is EntityPlayerLocal;
	}

	// Token: 0x06003377 RID: 13175 RVA: 0x000BD617 File Offset: 0x000BB817
	public override Color GetMapIconColor()
	{
		return Color.white;
	}

	// Token: 0x04002903 RID: 10499
	[PublicizedFrom(EAccessModifier.Private)]
	public static int MapObjectLandCLaimKeys;
}
