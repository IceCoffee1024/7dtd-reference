using System;
using UnityEngine;

// Token: 0x02000639 RID: 1593
public class MapObjectSleeperVolume : MapObject
{
	// Token: 0x060033E9 RID: 13289 RVA: 0x0014DC4E File Offset: 0x0014BE4E
	public MapObjectSleeperVolume(Vector3 _position)
		: base(EnumMapObjectType.SleeperVolume, _position, (long)(++MapObjectSleeperVolume.newID), null, false)
	{
	}

	// Token: 0x060033EA RID: 13290 RVA: 0x0014DC70 File Offset: 0x0014BE70
	public override string GetMapIcon()
	{
		return "ui_game_symbol_enemy_dot";
	}

	// Token: 0x060033EB RID: 13291 RVA: 0x0014DC70 File Offset: 0x0014BE70
	public override string GetCompassIcon()
	{
		return "ui_game_symbol_enemy_dot";
	}

	// Token: 0x060033EC RID: 13292 RVA: 0x0014DC77 File Offset: 0x0014BE77
	public override string GetCompassDownIcon()
	{
		return "ui_game_symbol_enemy_dot_down";
	}

	// Token: 0x060033ED RID: 13293 RVA: 0x0014DC7E File Offset: 0x0014BE7E
	public override string GetCompassUpIcon()
	{
		return "ui_game_symbol_enemy_dot_up";
	}

	// Token: 0x060033EE RID: 13294 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool UseUpDownCompassIcons()
	{
		return true;
	}

	// Token: 0x060033EF RID: 13295 RVA: 0x0014DC85 File Offset: 0x0014BE85
	public override bool IsOnCompass()
	{
		return this.IsShowing;
	}

	// Token: 0x060033F0 RID: 13296 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsCompassIconClamped()
	{
		return true;
	}

	// Token: 0x060033F1 RID: 13297 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool IsMapIconEnabled()
	{
		return false;
	}

	// Token: 0x060033F2 RID: 13298 RVA: 0x00041064 File Offset: 0x0003F264
	public override float GetMaxCompassIconScale()
	{
		return 1f;
	}

	// Token: 0x060033F3 RID: 13299 RVA: 0x0014D61D File Offset: 0x0014B81D
	public override float GetMinCompassIconScale()
	{
		return 0.6f;
	}

	// Token: 0x060033F4 RID: 13300 RVA: 0x0014DC8D File Offset: 0x0014BE8D
	public override float GetMaxCompassDistance()
	{
		return 32f;
	}

	// Token: 0x060033F5 RID: 13301 RVA: 0x0014DC94 File Offset: 0x0014BE94
	public override Color GetMapIconColor()
	{
		return new Color32(byte.MaxValue, 180, 0, byte.MaxValue);
	}

	// Token: 0x04002905 RID: 10501
	public bool IsShowing = true;

	// Token: 0x04002906 RID: 10502
	[PublicizedFrom(EAccessModifier.Private)]
	public static int newID;
}
