using System;
using UnityEngine;

// Token: 0x0200062F RID: 1583
public class MapObjectSleepingBag : MapObject
{
	// Token: 0x06003360 RID: 13152 RVA: 0x0014B224 File Offset: 0x00149424
	public MapObjectSleepingBag(Vector3 _position, Entity _entity)
		: base(EnumMapObjectType.SleepingBag, _position, (long)_entity.entityId, _entity, false)
	{
	}

	// Token: 0x06003361 RID: 13153 RVA: 0x0014B237 File Offset: 0x00149437
	public override string GetMapIcon()
	{
		return "ui_game_symbol_map_bed";
	}

	// Token: 0x06003362 RID: 13154 RVA: 0x0014B237 File Offset: 0x00149437
	public override string GetCompassIcon()
	{
		return "ui_game_symbol_map_bed";
	}

	// Token: 0x06003363 RID: 13155 RVA: 0x0014B23E File Offset: 0x0014943E
	public override bool IsOnCompass()
	{
		return this.IsMapIconEnabled() && this.entity is EntityPlayerLocal;
	}

	// Token: 0x06003364 RID: 13156 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLayerForMapIcon()
	{
		return 0;
	}

	// Token: 0x06003365 RID: 13157 RVA: 0x0014B258 File Offset: 0x00149458
	public override bool IsMapIconEnabled()
	{
		EntityPlayer entityPlayer = this.entity as EntityPlayer;
		return entityPlayer == null || entityPlayer is EntityPlayerLocal || entityPlayer.IsFriendOfLocalPlayer;
	}

	// Token: 0x06003366 RID: 13158 RVA: 0x0014B288 File Offset: 0x00149488
	public override Color GetMapIconColor()
	{
		EntityPlayer entityPlayer = this.entity as EntityPlayer;
		if (entityPlayer != null && !(entityPlayer is EntityPlayerLocal))
		{
			bool isFriendOfLocalPlayer = entityPlayer.IsFriendOfLocalPlayer;
			return Color.green * 0.75f;
		}
		return Color.white;
	}
}
