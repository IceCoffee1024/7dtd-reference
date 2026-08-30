using System;
using UnityEngine;

// Token: 0x02000637 RID: 1591
public class MapObjectSleeperVolume : MapObject
{
	// Token: 0x060033A6 RID: 13222 RVA: 0x0014D2DE File Offset: 0x0014B4DE
	public MapObjectSleeperVolume(Vector3 _position)
		: base(EnumMapObjectType.SleeperVolume, _position, (long)(++MapObjectSleeperVolume.newID), null, false)
	{
	}

	// Token: 0x060033A7 RID: 13223 RVA: 0x0014D300 File Offset: 0x0014B500
	public override string GetMapIcon()
	{
		return "ui_game_symbol_enemy_dot";
	}

	// Token: 0x060033A8 RID: 13224 RVA: 0x0014D300 File Offset: 0x0014B500
	public override string GetCompassIcon()
	{
		return "ui_game_symbol_enemy_dot";
	}

	// Token: 0x060033A9 RID: 13225 RVA: 0x0014D307 File Offset: 0x0014B507
	public override string GetCompassDownIcon()
	{
		return "ui_game_symbol_enemy_dot_down";
	}

	// Token: 0x060033AA RID: 13226 RVA: 0x0014D30E File Offset: 0x0014B50E
	public override string GetCompassUpIcon()
	{
		return "ui_game_symbol_enemy_dot_up";
	}

	// Token: 0x060033AB RID: 13227 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool UseUpDownCompassIcons()
	{
		return true;
	}

	// Token: 0x060033AC RID: 13228 RVA: 0x0014D315 File Offset: 0x0014B515
	public override bool IsOnCompass()
	{
		return this.IsShowing;
	}

	// Token: 0x060033AD RID: 13229 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool IsCompassIconClamped()
	{
		return true;
	}

	// Token: 0x060033AE RID: 13230 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool IsMapIconEnabled()
	{
		return false;
	}

	// Token: 0x060033AF RID: 13231 RVA: 0x00040F84 File Offset: 0x0003F184
	public override float GetMaxCompassIconScale()
	{
		return 1f;
	}

	// Token: 0x060033B0 RID: 13232 RVA: 0x0014CCAD File Offset: 0x0014AEAD
	public override float GetMinCompassIconScale()
	{
		return 0.6f;
	}

	// Token: 0x060033B1 RID: 13233 RVA: 0x0014D31D File Offset: 0x0014B51D
	public override float GetMaxCompassDistance()
	{
		return 32f;
	}

	// Token: 0x060033B2 RID: 13234 RVA: 0x0014D324 File Offset: 0x0014B524
	public override Color GetMapIconColor()
	{
		return new Color32(byte.MaxValue, 180, 0, byte.MaxValue);
	}

	// Token: 0x0400290C RID: 10508
	public bool IsShowing = true;

	// Token: 0x0400290D RID: 10509
	[PublicizedFrom(EAccessModifier.Private)]
	public static int newID;
}
