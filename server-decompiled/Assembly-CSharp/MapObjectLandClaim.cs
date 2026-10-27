using System;
using UnityEngine;

// Token: 0x0200063A RID: 1594
public class MapObjectLandClaim : MapObject
{
	// Token: 0x060033C2 RID: 13250 RVA: 0x0014E3ED File Offset: 0x0014C5ED
	public MapObjectLandClaim(Vector3 _position, Entity _entity)
		: base(EnumMapObjectType.LandClaim, _position, (long)MapObjectLandClaim.MapObjectLandCLaimKeys++, _entity, false)
	{
	}

	// Token: 0x060033C3 RID: 13251 RVA: 0x0014E408 File Offset: 0x0014C608
	public override string GetMapIcon()
	{
		return "ui_game_symbol_brick";
	}

	// Token: 0x060033C4 RID: 13252 RVA: 0x0014E408 File Offset: 0x0014C608
	public override string GetCompassIcon()
	{
		return "ui_game_symbol_brick";
	}

	// Token: 0x060033C5 RID: 13253 RVA: 0x0014E40F File Offset: 0x0014C60F
	public override bool IsOnCompass()
	{
		return this.IsMapIconEnabled();
	}

	// Token: 0x060033C6 RID: 13254 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLayerForMapIcon()
	{
		return 0;
	}

	// Token: 0x060033C7 RID: 13255 RVA: 0x0014E417 File Offset: 0x0014C617
	public override bool IsMapIconEnabled()
	{
		return this.entity != null && this.entity is EntityPlayerLocal;
	}

	// Token: 0x060033C8 RID: 13256 RVA: 0x000BDB9B File Offset: 0x000BBD9B
	public override Color GetMapIconColor()
	{
		return Color.white;
	}

	// Token: 0x04002917 RID: 10519
	[PublicizedFrom(EAccessModifier.Private)]
	public static int MapObjectLandCLaimKeys;
}
