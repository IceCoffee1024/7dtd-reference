using System;
using UnityEngine;

// Token: 0x02000641 RID: 1601
public class MapObjectSleepingBag : MapObject
{
	// Token: 0x06003404 RID: 13316 RVA: 0x0014E8A4 File Offset: 0x0014CAA4
	public MapObjectSleepingBag(Vector3 _position, Entity _entity)
		: base(EnumMapObjectType.SleepingBag, _position, (long)_entity.entityId, _entity, false)
	{
	}

	// Token: 0x06003405 RID: 13317 RVA: 0x0014E8B7 File Offset: 0x0014CAB7
	public override string GetMapIcon()
	{
		return "ui_game_symbol_map_bed";
	}

	// Token: 0x06003406 RID: 13318 RVA: 0x0014E8B7 File Offset: 0x0014CAB7
	public override string GetCompassIcon()
	{
		return "ui_game_symbol_map_bed";
	}

	// Token: 0x06003407 RID: 13319 RVA: 0x0014E8BE File Offset: 0x0014CABE
	public override bool IsOnCompass()
	{
		return this.IsMapIconEnabled() && this.entity is EntityPlayerLocal;
	}

	// Token: 0x06003408 RID: 13320 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLayerForMapIcon()
	{
		return 0;
	}

	// Token: 0x06003409 RID: 13321 RVA: 0x0014E8D8 File Offset: 0x0014CAD8
	public override bool IsMapIconEnabled()
	{
		EntityPlayer entityPlayer = this.entity as EntityPlayer;
		return entityPlayer == null || entityPlayer is EntityPlayerLocal || entityPlayer.IsFriendOfLocalPlayer;
	}

	// Token: 0x0600340A RID: 13322 RVA: 0x0014E908 File Offset: 0x0014CB08
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
