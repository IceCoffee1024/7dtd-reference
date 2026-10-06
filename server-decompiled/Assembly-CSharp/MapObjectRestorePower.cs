using System;
using UnityEngine;

// Token: 0x02000638 RID: 1592
public class MapObjectRestorePower : MapObject
{
	// Token: 0x060033DD RID: 13277 RVA: 0x0014DC09 File Offset: 0x0014BE09
	public MapObjectRestorePower(Vector3 _position)
		: base(EnumMapObjectType.RestorePower, _position, (long)(++MapObjectRestorePower.newID), null, false)
	{
	}

	// Token: 0x060033DE RID: 13278 RVA: 0x0014D775 File Offset: 0x0014B975
	public override string GetMapIcon()
	{
		return "ui_game_symbol_fetch_loot";
	}

	// Token: 0x060033DF RID: 13279 RVA: 0x0014D775 File Offset: 0x0014B975
	public override string GetCompassIcon()
	{
		return "ui_game_symbol_fetch_loot";
	}

	// Token: 0x060033E0 RID: 13280 RVA: 0x0014D77C File Offset: 0x0014B97C
	public override string GetCompassDownIcon()
	{
		return "ui_game_symbol_fetch_loot_down";
	}

	// Token: 0x060033E1 RID: 13281 RVA: 0x0014D783 File Offset: 0x0014B983
	public override string GetCompassUpIcon()
	{
		return "ui_game_symbol_fetch_loot_up";
	}

	// Token: 0x060033E2 RID: 13282 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool UseUpDownCompassIcons()
	{
		return true;
	}

	// Token: 0x060033E3 RID: 13283 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsOnCompass()
	{
		return true;
	}

	// Token: 0x060033E4 RID: 13284 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsCompassIconClamped()
	{
		return true;
	}

	// Token: 0x060033E5 RID: 13285 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool NearbyCompassBlink()
	{
		return true;
	}

	// Token: 0x060033E6 RID: 13286 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLayerForMapIcon()
	{
		return 0;
	}

	// Token: 0x060033E7 RID: 13287 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool IsMapIconEnabled()
	{
		return false;
	}

	// Token: 0x060033E8 RID: 13288 RVA: 0x0014DC24 File Offset: 0x0014BE24
	public override Color GetMapIconColor()
	{
		if (this.IsSelected)
		{
			return new Color32(byte.MaxValue, 180, 0, byte.MaxValue);
		}
		return Color.yellow;
	}

	// Token: 0x04002903 RID: 10499
	public bool IsSelected;

	// Token: 0x04002904 RID: 10500
	[PublicizedFrom(EAccessModifier.Private)]
	public static int newID;
}
