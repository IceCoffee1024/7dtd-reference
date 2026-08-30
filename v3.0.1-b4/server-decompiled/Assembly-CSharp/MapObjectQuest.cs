using System;
using UnityEngine;

// Token: 0x0200062C RID: 1580
public class MapObjectQuest : MapObject
{
	// Token: 0x0600333E RID: 13118 RVA: 0x0014B129 File Offset: 0x00149329
	public MapObjectQuest(Vector3 _position, string newIcon = "ui_game_symbol_quest")
		: base(EnumMapObjectType.Quest, _position, (long)(++MapObjectQuest.newID), null, false)
	{
		this.icon = newIcon;
	}

	// Token: 0x0600333F RID: 13119 RVA: 0x0014B14B File Offset: 0x0014934B
	public override string GetMapIcon()
	{
		return this.icon;
	}

	// Token: 0x06003340 RID: 13120 RVA: 0x0014B14B File Offset: 0x0014934B
	public override string GetCompassIcon()
	{
		return this.icon;
	}

	// Token: 0x06003341 RID: 13121 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool IsOnCompass()
	{
		return true;
	}

	// Token: 0x06003342 RID: 13122 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool IsCompassIconClamped()
	{
		return true;
	}

	// Token: 0x06003343 RID: 13123 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool NearbyCompassBlink()
	{
		return true;
	}

	// Token: 0x06003344 RID: 13124 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLayerForMapIcon()
	{
		return 0;
	}

	// Token: 0x06003345 RID: 13125 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool IsMapIconEnabled()
	{
		return true;
	}

	// Token: 0x06003346 RID: 13126 RVA: 0x0014B153 File Offset: 0x00149353
	public override Color GetMapIconColor()
	{
		if (this.IsSelected)
		{
			return new Color32(byte.MaxValue, 180, 0, byte.MaxValue);
		}
		return Color.white;
	}

	// Token: 0x04002894 RID: 10388
	public bool IsSelected;

	// Token: 0x04002895 RID: 10389
	[PublicizedFrom(EAccessModifier.Private)]
	public string icon;

	// Token: 0x04002896 RID: 10390
	[PublicizedFrom(EAccessModifier.Private)]
	public static int newID;
}
