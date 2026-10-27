using System;
using UnityEngine;

// Token: 0x0200063E RID: 1598
public class MapObjectQuest : MapObject
{
	// Token: 0x060033E2 RID: 13282 RVA: 0x0014E7A9 File Offset: 0x0014C9A9
	public MapObjectQuest(Vector3 _position, string newIcon = "ui_game_symbol_quest")
		: base(EnumMapObjectType.Quest, _position, (long)(++MapObjectQuest.newID), null, false)
	{
		this.icon = newIcon;
	}

	// Token: 0x060033E3 RID: 13283 RVA: 0x0014E7CB File Offset: 0x0014C9CB
	public override string GetMapIcon()
	{
		return this.icon;
	}

	// Token: 0x060033E4 RID: 13284 RVA: 0x0014E7CB File Offset: 0x0014C9CB
	public override string GetCompassIcon()
	{
		return this.icon;
	}

	// Token: 0x060033E5 RID: 13285 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsOnCompass()
	{
		return true;
	}

	// Token: 0x060033E6 RID: 13286 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsCompassIconClamped()
	{
		return true;
	}

	// Token: 0x060033E7 RID: 13287 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool NearbyCompassBlink()
	{
		return true;
	}

	// Token: 0x060033E8 RID: 13288 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLayerForMapIcon()
	{
		return 0;
	}

	// Token: 0x060033E9 RID: 13289 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsMapIconEnabled()
	{
		return true;
	}

	// Token: 0x060033EA RID: 13290 RVA: 0x0014E7D3 File Offset: 0x0014C9D3
	public override Color GetMapIconColor()
	{
		if (this.IsSelected)
		{
			return new Color32(byte.MaxValue, 180, 0, byte.MaxValue);
		}
		return Color.white;
	}

	// Token: 0x0400291B RID: 10523
	public bool IsSelected;

	// Token: 0x0400291C RID: 10524
	[PublicizedFrom(EAccessModifier.Private)]
	public string icon;

	// Token: 0x0400291D RID: 10525
	[PublicizedFrom(EAccessModifier.Private)]
	public static int newID;
}
