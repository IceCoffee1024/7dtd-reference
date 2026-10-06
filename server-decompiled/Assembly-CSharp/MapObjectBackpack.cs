using System;
using UnityEngine;

// Token: 0x02000630 RID: 1584
public class MapObjectBackpack : MapObject
{
	// Token: 0x06003393 RID: 13203 RVA: 0x0014D62B File Offset: 0x0014B82B
	public MapObjectBackpack(EntityPlayerLocal _epl, Vector3 _position, int _key)
		: base(EnumMapObjectType.Backpack, _position, (long)_key, null, false)
	{
		this.owningLocalPlayer = _epl;
	}

	// Token: 0x06003394 RID: 13204 RVA: 0x0014D640 File Offset: 0x0014B840
	public override string GetMapIcon()
	{
		return "ui_game_symbol_backpack";
	}

	// Token: 0x06003395 RID: 13205 RVA: 0x0014D640 File Offset: 0x0014B840
	public override string GetCompassIcon()
	{
		return "ui_game_symbol_backpack";
	}

	// Token: 0x06003396 RID: 13206 RVA: 0x0014D647 File Offset: 0x0014B847
	public override Color GetMapIconColor()
	{
		return Color.cyan;
	}

	// Token: 0x06003397 RID: 13207 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsOnCompass()
	{
		return true;
	}

	// Token: 0x06003398 RID: 13208 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLayerForMapIcon()
	{
		return 0;
	}

	// Token: 0x06003399 RID: 13209 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsMapIconEnabled()
	{
		return true;
	}

	// Token: 0x0600339A RID: 13210 RVA: 0x0014D64E File Offset: 0x0014B84E
	public override void SetPosition(Vector3 _pos)
	{
		this.position = _pos;
	}

	// Token: 0x0600339B RID: 13211 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsCenterOnLeftBottomCorner()
	{
		return true;
	}

	// Token: 0x0600339C RID: 13212 RVA: 0x0014D658 File Offset: 0x0014B858
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

	// Token: 0x040028F5 RID: 10485
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayerLocal owningLocalPlayer;

	// Token: 0x040028F6 RID: 10486
	[PublicizedFrom(EAccessModifier.Private)]
	public Entity myBackpack;

	// Token: 0x040028F7 RID: 10487
	[PublicizedFrom(EAccessModifier.Private)]
	public float lastTimeBackpackChecked;
}
