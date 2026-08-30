using System;
using UnityEngine;

// Token: 0x02000638 RID: 1592
public class MapObjectSleepingBag : MapObject
{
	// Token: 0x060033B3 RID: 13235 RVA: 0x0014D340 File Offset: 0x0014B540
	public MapObjectSleepingBag(Vector3 _position, Entity _entity)
		: base(EnumMapObjectType.SleepingBag, _position, (long)_entity.entityId, _entity, false)
	{
	}

	// Token: 0x060033B4 RID: 13236 RVA: 0x0014D353 File Offset: 0x0014B553
	public override string GetMapIcon()
	{
		return "ui_game_symbol_map_bed";
	}

	// Token: 0x060033B5 RID: 13237 RVA: 0x0014D353 File Offset: 0x0014B553
	public override string GetCompassIcon()
	{
		return "ui_game_symbol_map_bed";
	}

	// Token: 0x060033B6 RID: 13238 RVA: 0x0014D35A File Offset: 0x0014B55A
	public override bool IsOnCompass()
	{
		return this.IsMapIconEnabled() && this.entity is EntityPlayerLocal;
	}

	// Token: 0x060033B7 RID: 13239 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLayerForMapIcon()
	{
		return 0;
	}

	// Token: 0x060033B8 RID: 13240 RVA: 0x0014D374 File Offset: 0x0014B574
	public override bool IsMapIconEnabled()
	{
		EntityPlayer entityPlayer = this.entity as EntityPlayer;
		return entityPlayer == null || entityPlayer is EntityPlayerLocal || entityPlayer.IsFriendOfLocalPlayer;
	}

	// Token: 0x060033B9 RID: 13241 RVA: 0x0014D3A4 File Offset: 0x0014B5A4
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
