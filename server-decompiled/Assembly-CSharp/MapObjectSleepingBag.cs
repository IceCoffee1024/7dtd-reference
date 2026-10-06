using System;
using UnityEngine;

// Token: 0x0200063A RID: 1594
public class MapObjectSleepingBag : MapObject
{
	// Token: 0x060033F6 RID: 13302 RVA: 0x0014DCB0 File Offset: 0x0014BEB0
	public MapObjectSleepingBag(Vector3 _position, Entity _entity)
		: base(EnumMapObjectType.SleepingBag, _position, (long)_entity.entityId, _entity, false)
	{
	}

	// Token: 0x060033F7 RID: 13303 RVA: 0x0014DCC3 File Offset: 0x0014BEC3
	public override string GetMapIcon()
	{
		return "ui_game_symbol_map_bed";
	}

	// Token: 0x060033F8 RID: 13304 RVA: 0x0014DCC3 File Offset: 0x0014BEC3
	public override string GetCompassIcon()
	{
		return "ui_game_symbol_map_bed";
	}

	// Token: 0x060033F9 RID: 13305 RVA: 0x0014DCCA File Offset: 0x0014BECA
	public override bool IsOnCompass()
	{
		return this.IsMapIconEnabled() && this.entity is EntityPlayerLocal;
	}

	// Token: 0x060033FA RID: 13306 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLayerForMapIcon()
	{
		return 0;
	}

	// Token: 0x060033FB RID: 13307 RVA: 0x0014DCE4 File Offset: 0x0014BEE4
	public override bool IsMapIconEnabled()
	{
		EntityPlayer entityPlayer = this.entity as EntityPlayer;
		return entityPlayer == null || entityPlayer is EntityPlayerLocal || entityPlayer.IsFriendOfLocalPlayer;
	}

	// Token: 0x060033FC RID: 13308 RVA: 0x0014DD14 File Offset: 0x0014BF14
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
