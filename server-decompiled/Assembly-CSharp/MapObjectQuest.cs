using System;
using UnityEngine;

// Token: 0x02000637 RID: 1591
public class MapObjectQuest : MapObject
{
	// Token: 0x060033D4 RID: 13268 RVA: 0x0014DBB5 File Offset: 0x0014BDB5
	public MapObjectQuest(Vector3 _position, string newIcon = "ui_game_symbol_quest")
		: base(EnumMapObjectType.Quest, _position, (long)(++MapObjectQuest.newID), null, false)
	{
		this.icon = newIcon;
	}

	// Token: 0x060033D5 RID: 13269 RVA: 0x0014DBD7 File Offset: 0x0014BDD7
	public override string GetMapIcon()
	{
		return this.icon;
	}

	// Token: 0x060033D6 RID: 13270 RVA: 0x0014DBD7 File Offset: 0x0014BDD7
	public override string GetCompassIcon()
	{
		return this.icon;
	}

	// Token: 0x060033D7 RID: 13271 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsOnCompass()
	{
		return true;
	}

	// Token: 0x060033D8 RID: 13272 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsCompassIconClamped()
	{
		return true;
	}

	// Token: 0x060033D9 RID: 13273 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool NearbyCompassBlink()
	{
		return true;
	}

	// Token: 0x060033DA RID: 13274 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLayerForMapIcon()
	{
		return 0;
	}

	// Token: 0x060033DB RID: 13275 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsMapIconEnabled()
	{
		return true;
	}

	// Token: 0x060033DC RID: 13276 RVA: 0x0014DBDF File Offset: 0x0014BDDF
	public override Color GetMapIconColor()
	{
		if (this.IsSelected)
		{
			return new Color32(byte.MaxValue, 180, 0, byte.MaxValue);
		}
		return Color.white;
	}

	// Token: 0x04002900 RID: 10496
	public bool IsSelected;

	// Token: 0x04002901 RID: 10497
	[PublicizedFrom(EAccessModifier.Private)]
	public string icon;

	// Token: 0x04002902 RID: 10498
	[PublicizedFrom(EAccessModifier.Private)]
	public static int newID;
}
