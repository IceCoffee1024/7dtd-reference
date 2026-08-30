using System;
using UnityEngine;

// Token: 0x02000625 RID: 1573
public class MapObjectBackpack : MapObject
{
	// Token: 0x060032FD RID: 13053 RVA: 0x0014AB9F File Offset: 0x00148D9F
	public MapObjectBackpack(EntityPlayerLocal _epl, Vector3 _position, int _key)
		: base(EnumMapObjectType.Backpack, _position, (long)_key, null, false)
	{
		this.owningLocalPlayer = _epl;
	}

	// Token: 0x060032FE RID: 13054 RVA: 0x0014ABB4 File Offset: 0x00148DB4
	public override string GetMapIcon()
	{
		return "ui_game_symbol_backpack";
	}

	// Token: 0x060032FF RID: 13055 RVA: 0x0014ABB4 File Offset: 0x00148DB4
	public override string GetCompassIcon()
	{
		return "ui_game_symbol_backpack";
	}

	// Token: 0x06003300 RID: 13056 RVA: 0x0014ABBB File Offset: 0x00148DBB
	public override Color GetMapIconColor()
	{
		return Color.cyan;
	}

	// Token: 0x06003301 RID: 13057 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool IsOnCompass()
	{
		return true;
	}

	// Token: 0x06003302 RID: 13058 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLayerForMapIcon()
	{
		return 0;
	}

	// Token: 0x06003303 RID: 13059 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool IsMapIconEnabled()
	{
		return true;
	}

	// Token: 0x06003304 RID: 13060 RVA: 0x0014ABC2 File Offset: 0x00148DC2
	public override void SetPosition(Vector3 _pos)
	{
		this.position = _pos;
	}

	// Token: 0x06003305 RID: 13061 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool IsCenterOnLeftBottomCorner()
	{
		return true;
	}

	// Token: 0x06003306 RID: 13062 RVA: 0x0014ABCC File Offset: 0x00148DCC
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

	// Token: 0x04002889 RID: 10377
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayerLocal owningLocalPlayer;

	// Token: 0x0400288A RID: 10378
	[PublicizedFrom(EAccessModifier.Private)]
	public Entity myBackpack;

	// Token: 0x0400288B RID: 10379
	[PublicizedFrom(EAccessModifier.Private)]
	public float lastTimeBackpackChecked;
}
