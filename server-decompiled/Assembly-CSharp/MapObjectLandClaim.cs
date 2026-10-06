using System;
using UnityEngine;

// Token: 0x02000633 RID: 1587
public class MapObjectLandClaim : MapObject
{
	// Token: 0x060033B4 RID: 13236 RVA: 0x0014D7F9 File Offset: 0x0014B9F9
	public MapObjectLandClaim(Vector3 _position, Entity _entity)
		: base(EnumMapObjectType.LandClaim, _position, (long)MapObjectLandClaim.MapObjectLandCLaimKeys++, _entity, false)
	{
	}

	// Token: 0x060033B5 RID: 13237 RVA: 0x0014D814 File Offset: 0x0014BA14
	public override string GetMapIcon()
	{
		return "ui_game_symbol_brick";
	}

	// Token: 0x060033B6 RID: 13238 RVA: 0x0014D814 File Offset: 0x0014BA14
	public override string GetCompassIcon()
	{
		return "ui_game_symbol_brick";
	}

	// Token: 0x060033B7 RID: 13239 RVA: 0x0014D81B File Offset: 0x0014BA1B
	public override bool IsOnCompass()
	{
		return this.IsMapIconEnabled();
	}

	// Token: 0x060033B8 RID: 13240 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLayerForMapIcon()
	{
		return 0;
	}

	// Token: 0x060033B9 RID: 13241 RVA: 0x0014D823 File Offset: 0x0014BA23
	public override bool IsMapIconEnabled()
	{
		return this.entity != null && this.entity is EntityPlayerLocal;
	}

	// Token: 0x060033BA RID: 13242 RVA: 0x000BD7EF File Offset: 0x000BB9EF
	public override Color GetMapIconColor()
	{
		return Color.white;
	}

	// Token: 0x040028FC RID: 10492
	[PublicizedFrom(EAccessModifier.Private)]
	public static int MapObjectLandCLaimKeys;
}
