using System;
using UnityEngine;

// Token: 0x02000640 RID: 1600
public class MapObjectSleeperVolume : MapObject
{
	// Token: 0x060033F7 RID: 13303 RVA: 0x0014E842 File Offset: 0x0014CA42
	public MapObjectSleeperVolume(Vector3 _position)
		: base(EnumMapObjectType.SleeperVolume, _position, (long)(++MapObjectSleeperVolume.newID), null, false)
	{
	}

	// Token: 0x060033F8 RID: 13304 RVA: 0x0014E864 File Offset: 0x0014CA64
	public override string GetMapIcon()
	{
		return "ui_game_symbol_enemy_dot";
	}

	// Token: 0x060033F9 RID: 13305 RVA: 0x0014E864 File Offset: 0x0014CA64
	public override string GetCompassIcon()
	{
		return "ui_game_symbol_enemy_dot";
	}

	// Token: 0x060033FA RID: 13306 RVA: 0x0014E86B File Offset: 0x0014CA6B
	public override string GetCompassDownIcon()
	{
		return "ui_game_symbol_enemy_dot_down";
	}

	// Token: 0x060033FB RID: 13307 RVA: 0x0014E872 File Offset: 0x0014CA72
	public override string GetCompassUpIcon()
	{
		return "ui_game_symbol_enemy_dot_up";
	}

	// Token: 0x060033FC RID: 13308 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool UseUpDownCompassIcons()
	{
		return true;
	}

	// Token: 0x060033FD RID: 13309 RVA: 0x0014E879 File Offset: 0x0014CA79
	public override bool IsOnCompass()
	{
		return this.IsShowing;
	}

	// Token: 0x060033FE RID: 13310 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsCompassIconClamped()
	{
		return true;
	}

	// Token: 0x060033FF RID: 13311 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool IsMapIconEnabled()
	{
		return false;
	}

	// Token: 0x06003400 RID: 13312 RVA: 0x00041024 File Offset: 0x0003F224
	public override float GetMaxCompassIconScale()
	{
		return 1f;
	}

	// Token: 0x06003401 RID: 13313 RVA: 0x0014E211 File Offset: 0x0014C411
	public override float GetMinCompassIconScale()
	{
		return 0.6f;
	}

	// Token: 0x06003402 RID: 13314 RVA: 0x0014E881 File Offset: 0x0014CA81
	public override float GetMaxCompassDistance()
	{
		return 32f;
	}

	// Token: 0x06003403 RID: 13315 RVA: 0x0014E888 File Offset: 0x0014CA88
	public override Color GetMapIconColor()
	{
		return new Color32(byte.MaxValue, 180, 0, byte.MaxValue);
	}

	// Token: 0x04002920 RID: 10528
	public bool IsShowing = true;

	// Token: 0x04002921 RID: 10529
	[PublicizedFrom(EAccessModifier.Private)]
	public static int newID;
}
