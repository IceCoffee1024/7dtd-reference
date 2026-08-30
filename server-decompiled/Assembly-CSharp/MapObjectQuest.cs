using System;
using UnityEngine;

// Token: 0x02000635 RID: 1589
public class MapObjectQuest : MapObject
{
	// Token: 0x06003391 RID: 13201 RVA: 0x0014D245 File Offset: 0x0014B445
	public MapObjectQuest(Vector3 _position, string newIcon = "ui_game_symbol_quest")
		: base(EnumMapObjectType.Quest, _position, (long)(++MapObjectQuest.newID), null, false)
	{
		this.icon = newIcon;
	}

	// Token: 0x06003392 RID: 13202 RVA: 0x0014D267 File Offset: 0x0014B467
	public override string GetMapIcon()
	{
		return this.icon;
	}

	// Token: 0x06003393 RID: 13203 RVA: 0x0014D267 File Offset: 0x0014B467
	public override string GetCompassIcon()
	{
		return this.icon;
	}

	// Token: 0x06003394 RID: 13204 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool IsOnCompass()
	{
		return true;
	}

	// Token: 0x06003395 RID: 13205 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool IsCompassIconClamped()
	{
		return true;
	}

	// Token: 0x06003396 RID: 13206 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool NearbyCompassBlink()
	{
		return true;
	}

	// Token: 0x06003397 RID: 13207 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLayerForMapIcon()
	{
		return 0;
	}

	// Token: 0x06003398 RID: 13208 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool IsMapIconEnabled()
	{
		return true;
	}

	// Token: 0x06003399 RID: 13209 RVA: 0x0014D26F File Offset: 0x0014B46F
	public override Color GetMapIconColor()
	{
		if (this.IsSelected)
		{
			return new Color32(byte.MaxValue, 180, 0, byte.MaxValue);
		}
		return Color.white;
	}

	// Token: 0x04002907 RID: 10503
	public bool IsSelected;

	// Token: 0x04002908 RID: 10504
	[PublicizedFrom(EAccessModifier.Private)]
	public string icon;

	// Token: 0x04002909 RID: 10505
	[PublicizedFrom(EAccessModifier.Private)]
	public static int newID;
}
