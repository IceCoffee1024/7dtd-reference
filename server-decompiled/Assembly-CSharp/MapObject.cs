using System;
using UnityEngine;

// Token: 0x0200062E RID: 1582
public class MapObject
{
	// Token: 0x06003369 RID: 13161 RVA: 0x0014D07C File Offset: 0x0014B27C
	public MapObject(EnumMapObjectType _type, Vector3 _position, long _key, Entity _entity, bool _bSelectable)
	{
		this.type = _type;
		this.position = _position;
		this.key = _key;
		this.bSelectable = _bSelectable;
		this.entity = _entity;
	}

	// Token: 0x0600336A RID: 13162 RVA: 0x0014D0AC File Offset: 0x0014B2AC
	public MapObject(MapObject _other)
	{
		this.type = _other.type;
		this.position = _other.position;
		this.key = _other.key;
		this.bSelectable = _other.bSelectable;
		this.entity = _other.entity;
	}

	// Token: 0x0600336B RID: 13163 RVA: 0x0014D0FB File Offset: 0x0014B2FB
	public virtual Vector3 GetPosition()
	{
		if (this.type == EnumMapObjectType.Entity && this.entity != null)
		{
			return this.entity.GetPosition();
		}
		return this.position;
	}

	// Token: 0x0600336C RID: 13164 RVA: 0x0014D125 File Offset: 0x0014B325
	public virtual void SetPosition(Vector3 _pos)
	{
		if (this.type == EnumMapObjectType.Entity)
		{
			throw new Exception("Setting of position not allowed!");
		}
		this.position = _pos;
	}

	// Token: 0x0600336D RID: 13165 RVA: 0x0014D144 File Offset: 0x0014B344
	public virtual Vector3 GetRotation()
	{
		if (this.type != EnumMapObjectType.Entity || !(this.entity != null))
		{
			return Vector3.zero;
		}
		if (this.entity.AttachedToEntity != null)
		{
			return this.entity.AttachedToEntity.rotation;
		}
		return this.entity.rotation;
	}

	// Token: 0x0600336E RID: 13166 RVA: 0x00020119 File Offset: 0x0001E319
	public virtual bool IsTracked()
	{
		return true;
	}

	// Token: 0x0600336F RID: 13167 RVA: 0x0014D19C File Offset: 0x0014B39C
	public virtual bool IsMapIconEnabled()
	{
		return this.type != EnumMapObjectType.Entity || !(this.entity != null) || this.entity.IsDrawMapIcon();
	}

	// Token: 0x06003370 RID: 13168 RVA: 0x0014D1C1 File Offset: 0x0014B3C1
	public virtual float GetMaxCompassDistance()
	{
		return 1024f;
	}

	// Token: 0x06003371 RID: 13169 RVA: 0x0003D3B2 File Offset: 0x0003B5B2
	public virtual float GetMinCompassDistance()
	{
		return 0f;
	}

	// Token: 0x06003372 RID: 13170 RVA: 0x0014D1C8 File Offset: 0x0014B3C8
	public virtual float GetMaxCompassIconScale()
	{
		return 1.25f;
	}

	// Token: 0x06003373 RID: 13171 RVA: 0x0014D1CF File Offset: 0x0014B3CF
	public virtual float GetMinCompassIconScale()
	{
		return 0.5f;
	}

	// Token: 0x06003374 RID: 13172 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool IsCompassIconClamped()
	{
		return false;
	}

	// Token: 0x06003375 RID: 13173 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool NearbyCompassBlink()
	{
		return false;
	}

	// Token: 0x06003376 RID: 13174 RVA: 0x0014D1D6 File Offset: 0x0014B3D6
	public virtual Vector3 GetMapIconScale()
	{
		if (this.type == EnumMapObjectType.Entity && this.entity != null)
		{
			return this.entity.GetMapIconScale();
		}
		return Vector3.one;
	}

	// Token: 0x06003377 RID: 13175 RVA: 0x0014D1FF File Offset: 0x0014B3FF
	public virtual string GetMapIcon()
	{
		if (this.type == EnumMapObjectType.Entity && this.entity != null)
		{
			return this.entity.GetMapIcon();
		}
		return "";
	}

	// Token: 0x06003378 RID: 13176 RVA: 0x0014D228 File Offset: 0x0014B428
	public virtual string GetCompassIcon()
	{
		if (this.type == EnumMapObjectType.Entity && this.entity != null)
		{
			return this.entity.GetCompassIcon();
		}
		return null;
	}

	// Token: 0x06003379 RID: 13177 RVA: 0x0014D24D File Offset: 0x0014B44D
	public virtual string GetCompassUpIcon()
	{
		if (this.type == EnumMapObjectType.Entity && this.entity != null)
		{
			return this.entity.GetCompassUpIcon();
		}
		return "";
	}

	// Token: 0x0600337A RID: 13178 RVA: 0x0014D276 File Offset: 0x0014B476
	public virtual string GetCompassDownIcon()
	{
		if (this.type == EnumMapObjectType.Entity && this.entity != null)
		{
			return this.entity.GetCompassDownIcon();
		}
		return "";
	}

	// Token: 0x0600337B RID: 13179 RVA: 0x0014D29F File Offset: 0x0014B49F
	public virtual bool UseUpDownCompassIcons()
	{
		return this.type == EnumMapObjectType.Entity && this.entity != null && this.entity.GetCompassDownIcon() != null;
	}

	// Token: 0x0600337C RID: 13180 RVA: 0x0014D2C7 File Offset: 0x0014B4C7
	public virtual bool IsMapIconBlinking()
	{
		return this.type == EnumMapObjectType.Entity && this.entity != null && this.entity.IsMapIconBlinking();
	}

	// Token: 0x0600337D RID: 13181 RVA: 0x0014D2EC File Offset: 0x0014B4EC
	public virtual Color GetMapIconColor()
	{
		if (this.type != EnumMapObjectType.Entity || !(this.entity != null))
		{
			return Color.white;
		}
		EntityPlayerLocal primaryPlayer = this.entity.world.GetPrimaryPlayer();
		if (primaryPlayer != null && primaryPlayer.Party != null && primaryPlayer.Party.MemberList.Contains(this.entity as EntityPlayer))
		{
			int num = primaryPlayer.Party.MemberList.IndexOf(this.entity as EntityPlayer);
			return Constants.TrackedFriendColors[num % Constants.TrackedFriendColors.Length];
		}
		return this.entity.GetMapIconColor();
	}

	// Token: 0x0600337E RID: 13182 RVA: 0x0014D392 File Offset: 0x0014B592
	public virtual bool CanMapIconBeSelected()
	{
		return this.type == EnumMapObjectType.Entity && this.entity != null && this.entity.CanMapIconBeSelected();
	}

	// Token: 0x0600337F RID: 13183 RVA: 0x0014D3B8 File Offset: 0x0014B5B8
	public virtual bool IsOnCompass()
	{
		if (this.type == EnumMapObjectType.Entity && this.entity != null)
		{
			EntityPlayerLocal primaryPlayer = this.entity.world.GetPrimaryPlayer();
			if (primaryPlayer != null && primaryPlayer.Party != null && this.entity != primaryPlayer)
			{
				return primaryPlayer.Party.MemberList.Contains(this.entity as EntityPlayer);
			}
		}
		return false;
	}

	// Token: 0x06003380 RID: 13184 RVA: 0x0014D428 File Offset: 0x0014B628
	public virtual int GetLayerForMapIcon()
	{
		if (this.type == EnumMapObjectType.Entity && this.entity != null)
		{
			return this.entity.GetLayerForMapIcon();
		}
		return 2;
	}

	// Token: 0x06003381 RID: 13185 RVA: 0x0014D450 File Offset: 0x0014B650
	public virtual string GetName()
	{
		if (this.type == EnumMapObjectType.Entity && this.entity is EntityAlive)
		{
			bool flag = !SingletonMonoBehaviour<ConnectionManager>.Instance.IsSinglePlayer;
			EntityPlayerLocal primaryPlayer = this.entity.world.GetPrimaryPlayer();
			if (!(this.entity is EntityPlayerLocal) && !(this.entity is EntityVehicle))
			{
				return ((EntityAlive)this.entity).EntityName;
			}
			if (primaryPlayer == this.entity && flag)
			{
				return Localization.Get("xuiMapSelfLabel", false, null);
			}
		}
		return null;
	}

	// Token: 0x06003382 RID: 13186 RVA: 0x00020119 File Offset: 0x0001E319
	public virtual bool IsShowName()
	{
		return true;
	}

	// Token: 0x06003383 RID: 13187 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool IsCenterOnLeftBottomCorner()
	{
		return false;
	}

	// Token: 0x06003384 RID: 13188 RVA: 0x0014D4DC File Offset: 0x0014B6DC
	public virtual float GetCompassIconScale(float _distance)
	{
		float num = 1f - _distance / this.GetMaxCompassDistance();
		return Mathf.Lerp(this.GetMinCompassIconScale(), this.GetMaxCompassIconScale(), num);
	}

	// Token: 0x06003385 RID: 13189 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void RefreshData()
	{
	}

	// Token: 0x040028EF RID: 10479
	public EnumMapObjectType type;

	// Token: 0x040028F0 RID: 10480
	public long key;

	// Token: 0x040028F1 RID: 10481
	[PublicizedFrom(EAccessModifier.Protected)]
	public Vector3 position;

	// Token: 0x040028F2 RID: 10482
	public bool bSelectable;

	// Token: 0x040028F3 RID: 10483
	[PublicizedFrom(EAccessModifier.Protected)]
	public Entity entity;
}
