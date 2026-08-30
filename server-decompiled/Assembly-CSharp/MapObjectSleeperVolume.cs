using System;
using UnityEngine;

// Token: 0x0200062E RID: 1582
public class MapObjectSleeperVolume : MapObject
{
	// Token: 0x06003353 RID: 13139 RVA: 0x0014B1C2 File Offset: 0x001493C2
	public MapObjectSleeperVolume(Vector3 _position)
		: base(EnumMapObjectType.SleeperVolume, _position, (long)(++MapObjectSleeperVolume.newID), null, false)
	{
	}

	// Token: 0x06003354 RID: 13140 RVA: 0x0014B1E4 File Offset: 0x001493E4
	public override string GetMapIcon()
	{
		return "ui_game_symbol_enemy_dot";
	}

	// Token: 0x06003355 RID: 13141 RVA: 0x0014B1E4 File Offset: 0x001493E4
	public override string GetCompassIcon()
	{
		return "ui_game_symbol_enemy_dot";
	}

	// Token: 0x06003356 RID: 13142 RVA: 0x0014B1EB File Offset: 0x001493EB
	public override string GetCompassDownIcon()
	{
		return "ui_game_symbol_enemy_dot_down";
	}

	// Token: 0x06003357 RID: 13143 RVA: 0x0014B1F2 File Offset: 0x001493F2
	public override string GetCompassUpIcon()
	{
		return "ui_game_symbol_enemy_dot_up";
	}

	// Token: 0x06003358 RID: 13144 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool UseUpDownCompassIcons()
	{
		return true;
	}

	// Token: 0x06003359 RID: 13145 RVA: 0x0014B1F9 File Offset: 0x001493F9
	public override bool IsOnCompass()
	{
		return this.IsShowing;
	}

	// Token: 0x0600335A RID: 13146 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool IsCompassIconClamped()
	{
		return true;
	}

	// Token: 0x0600335B RID: 13147 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool IsMapIconEnabled()
	{
		return false;
	}

	// Token: 0x0600335C RID: 13148 RVA: 0x00040D24 File Offset: 0x0003EF24
	public override float GetMaxCompassIconScale()
	{
		return 1f;
	}

	// Token: 0x0600335D RID: 13149 RVA: 0x0014AB91 File Offset: 0x00148D91
	public override float GetMinCompassIconScale()
	{
		return 0.6f;
	}

	// Token: 0x0600335E RID: 13150 RVA: 0x0014B201 File Offset: 0x00149401
	public override float GetMaxCompassDistance()
	{
		return 32f;
	}

	// Token: 0x0600335F RID: 13151 RVA: 0x0014B208 File Offset: 0x00149408
	public override Color GetMapIconColor()
	{
		return new Color32(byte.MaxValue, 180, 0, byte.MaxValue);
	}

	// Token: 0x04002899 RID: 10393
	public bool IsShowing = true;

	// Token: 0x0400289A RID: 10394
	[PublicizedFrom(EAccessModifier.Private)]
	public static int newID;
}
