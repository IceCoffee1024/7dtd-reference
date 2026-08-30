using System;
using UnityEngine;

// Token: 0x0200062E RID: 1582
public class MapObjectBackpack : MapObject
{
	// Token: 0x06003350 RID: 13136 RVA: 0x0014CCBB File Offset: 0x0014AEBB
	public MapObjectBackpack(EntityPlayerLocal _epl, Vector3 _position, int _key)
		: base(EnumMapObjectType.Backpack, _position, (long)_key, null, false)
	{
		this.owningLocalPlayer = _epl;
	}

	// Token: 0x06003351 RID: 13137 RVA: 0x0014CCD0 File Offset: 0x0014AED0
	public override string GetMapIcon()
	{
		return "ui_game_symbol_backpack";
	}

	// Token: 0x06003352 RID: 13138 RVA: 0x0014CCD0 File Offset: 0x0014AED0
	public override string GetCompassIcon()
	{
		return "ui_game_symbol_backpack";
	}

	// Token: 0x06003353 RID: 13139 RVA: 0x0014CCD7 File Offset: 0x0014AED7
	public override Color GetMapIconColor()
	{
		return Color.cyan;
	}

	// Token: 0x06003354 RID: 13140 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool IsOnCompass()
	{
		return true;
	}

	// Token: 0x06003355 RID: 13141 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLayerForMapIcon()
	{
		return 0;
	}

	// Token: 0x06003356 RID: 13142 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool IsMapIconEnabled()
	{
		return true;
	}

	// Token: 0x06003357 RID: 13143 RVA: 0x0014CCDE File Offset: 0x0014AEDE
	public override void SetPosition(Vector3 _pos)
	{
		this.position = _pos;
	}

	// Token: 0x06003358 RID: 13144 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool IsCenterOnLeftBottomCorner()
	{
		return true;
	}

	// Token: 0x06003359 RID: 13145 RVA: 0x0014CCE8 File Offset: 0x0014AEE8
	public override Vector3 GetPosition()
	{
		if (this.myBackpack == null && Time.time - this.lastTimeBackpackChecked > 5f)
		{
			this.lastTimeBackpackChecked = Time.time;
			World world = GameManager.Instance.World;
			for (int i = world.Entities.list.Count - 1; i >= 0; i--)
			{
				if (world.Entities.list[i] is EntityBackpack && ((EntityBackpack)world.Entities.list[i]).RefPlayerId == this.owningLocalPlayer.entityId)
				{
					this.myBackpack = world.Entities.list[i];
					break;
				}
			}
		}
		if (this.myBackpack != null && this.myBackpack.IsMarkedForUnload())
		{
			this.myBackpack = null;
		}
		if (!(this.myBackpack != null))
		{
			return base.GetPosition();
		}
		return this.myBackpack.position;
	}

	// Token: 0x040028FC RID: 10492
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayerLocal owningLocalPlayer;

	// Token: 0x040028FD RID: 10493
	[PublicizedFrom(EAccessModifier.Private)]
	public Entity myBackpack;

	// Token: 0x040028FE RID: 10494
	[PublicizedFrom(EAccessModifier.Private)]
	public float lastTimeBackpackChecked;
}
