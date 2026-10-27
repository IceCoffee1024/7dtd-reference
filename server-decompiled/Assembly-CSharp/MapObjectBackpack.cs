using System;
using UnityEngine;

// Token: 0x02000637 RID: 1591
public class MapObjectBackpack : MapObject
{
	// Token: 0x060033A1 RID: 13217 RVA: 0x0014E21F File Offset: 0x0014C41F
	public MapObjectBackpack(EntityPlayerLocal _epl, Vector3 _position, int _key)
		: base(EnumMapObjectType.Backpack, _position, (long)_key, null, false)
	{
		this.owningLocalPlayer = _epl;
	}

	// Token: 0x060033A2 RID: 13218 RVA: 0x0014E234 File Offset: 0x0014C434
	public override string GetMapIcon()
	{
		return "ui_game_symbol_backpack";
	}

	// Token: 0x060033A3 RID: 13219 RVA: 0x0014E234 File Offset: 0x0014C434
	public override string GetCompassIcon()
	{
		return "ui_game_symbol_backpack";
	}

	// Token: 0x060033A4 RID: 13220 RVA: 0x0014E23B File Offset: 0x0014C43B
	public override Color GetMapIconColor()
	{
		return Color.cyan;
	}

	// Token: 0x060033A5 RID: 13221 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsOnCompass()
	{
		return true;
	}

	// Token: 0x060033A6 RID: 13222 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLayerForMapIcon()
	{
		return 0;
	}

	// Token: 0x060033A7 RID: 13223 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsMapIconEnabled()
	{
		return true;
	}

	// Token: 0x060033A8 RID: 13224 RVA: 0x0014E242 File Offset: 0x0014C442
	public override void SetPosition(Vector3 _pos)
	{
		this.position = _pos;
	}

	// Token: 0x060033A9 RID: 13225 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsCenterOnLeftBottomCorner()
	{
		return true;
	}

	// Token: 0x060033AA RID: 13226 RVA: 0x0014E24C File Offset: 0x0014C44C
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

	// Token: 0x04002910 RID: 10512
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayerLocal owningLocalPlayer;

	// Token: 0x04002911 RID: 10513
	[PublicizedFrom(EAccessModifier.Private)]
	public Entity myBackpack;

	// Token: 0x04002912 RID: 10514
	[PublicizedFrom(EAccessModifier.Private)]
	public float lastTimeBackpackChecked;
}
