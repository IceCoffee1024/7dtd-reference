using System;
using UnityEngine;

// Token: 0x02000635 RID: 1589
public class MapObject
{
	// Token: 0x06003377 RID: 13175 RVA: 0x0014DC70 File Offset: 0x0014BE70
	public MapObject(EnumMapObjectType _type, Vector3 _position, long _key, Entity _entity, bool _bSelectable)
	{
		this.type = _type;
		this.position = _position;
		this.key = _key;
		this.bSelectable = _bSelectable;
		this.entity = _entity;
	}

	// Token: 0x06003378 RID: 13176 RVA: 0x0014DCA0 File Offset: 0x0014BEA0
	public MapObject(MapObject _other)
	{
		this.type = _other.type;
		this.position = _other.position;
		this.key = _other.key;
		this.bSelectable = _other.bSelectable;
		this.entity = _other.entity;
	}

	// Token: 0x06003379 RID: 13177 RVA: 0x0014DCEF File Offset: 0x0014BEEF
	public virtual Vector3 GetPosition()
	{
		if (this.type == EnumMapObjectType.Entity && this.entity != null)
		{
			return this.entity.GetPosition();
		}
		return this.position;
	}

	// Token: 0x0600337A RID: 13178 RVA: 0x0014DD19 File Offset: 0x0014BF19
	public virtual void SetPosition(Vector3 _pos)
	{
		if (this.type == EnumMapObjectType.Entity)
		{
			throw new Exception("Setting of position not allowed!");
		}
		this.position = _pos;
	}

	// Token: 0x0600337B RID: 13179 RVA: 0x0014DD38 File Offset: 0x0014BF38
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

	// Token: 0x0600337C RID: 13180 RVA: 0x00020119 File Offset: 0x0001E319
	public virtual bool IsTracked()
	{
		return true;
	}

	// Token: 0x0600337D RID: 13181 RVA: 0x0014DD90 File Offset: 0x0014BF90
	public virtual bool IsMapIconEnabled()
	{
		return this.type != EnumMapObjectType.Entity || !(this.entity != null) || this.entity.IsDrawMapIcon();
	}

	// Token: 0x0600337E RID: 13182 RVA: 0x0014DDB5 File Offset: 0x0014BFB5
	public virtual float GetMaxCompassDistance()
	{
		return 1024f;
	}

	// Token: 0x0600337F RID: 13183 RVA: 0x0003D372 File Offset: 0x0003B572
	public virtual float GetMinCompassDistance()
	{
		return 0f;
	}

	// Token: 0x06003380 RID: 13184 RVA: 0x0014DDBC File Offset: 0x0014BFBC
	public virtual float GetMaxCompassIconScale()
	{
		return 1.25f;
	}

	// Token: 0x06003381 RID: 13185 RVA: 0x0014DDC3 File Offset: 0x0014BFC3
	public virtual float GetMinCompassIconScale()
	{
		return 0.5f;
	}

	// Token: 0x06003382 RID: 13186 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool IsCompassIconClamped()
	{
		return false;
	}

	// Token: 0x06003383 RID: 13187 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool NearbyCompassBlink()
	{
		return false;
	}

	// Token: 0x06003384 RID: 13188 RVA: 0x0014DDCA File Offset: 0x0014BFCA
	public virtual Vector3 GetMapIconScale()
	{
		if (this.type == EnumMapObjectType.Entity && this.entity != null)
		{
			return this.entity.GetMapIconScale();
		}
		return Vector3.one;
	}

	// Token: 0x06003385 RID: 13189 RVA: 0x0014DDF3 File Offset: 0x0014BFF3
	public virtual string GetMapIcon()
	{
		if (this.type == EnumMapObjectType.Entity && this.entity != null)
		{
			return this.entity.GetMapIcon();
		}
		return "";
	}

	// Token: 0x06003386 RID: 13190 RVA: 0x0014DE1C File Offset: 0x0014C01C
	public virtual string GetCompassIcon()
	{
		if (this.type == EnumMapObjectType.Entity && this.entity != null)
		{
			return this.entity.GetCompassIcon();
		}
		return null;
	}

	// Token: 0x06003387 RID: 13191 RVA: 0x0014DE41 File Offset: 0x0014C041
	public virtual string GetCompassUpIcon()
	{
		if (this.type == EnumMapObjectType.Entity && this.entity != null)
		{
			return this.entity.GetCompassUpIcon();
		}
		return "";
	}

	// Token: 0x06003388 RID: 13192 RVA: 0x0014DE6A File Offset: 0x0014C06A
	public virtual string GetCompassDownIcon()
	{
		if (this.type == EnumMapObjectType.Entity && this.entity != null)
		{
			return this.entity.GetCompassDownIcon();
		}
		return "";
	}

	// Token: 0x06003389 RID: 13193 RVA: 0x0014DE93 File Offset: 0x0014C093
	public virtual bool UseUpDownCompassIcons()
	{
		return this.type == EnumMapObjectType.Entity && this.entity != null && this.entity.GetCompassDownIcon() != null;
	}

	// Token: 0x0600338A RID: 13194 RVA: 0x0014DEBB File Offset: 0x0014C0BB
	public virtual bool IsMapIconBlinking()
	{
		return this.type == EnumMapObjectType.Entity && this.entity != null && this.entity.IsMapIconBlinking();
	}

	// Token: 0x0600338B RID: 13195 RVA: 0x0014DEE0 File Offset: 0x0014C0E0
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

	// Token: 0x0600338C RID: 13196 RVA: 0x0014DF86 File Offset: 0x0014C186
	public virtual bool CanMapIconBeSelected()
	{
		return this.type == EnumMapObjectType.Entity && this.entity != null && this.entity.CanMapIconBeSelected();
	}

	// Token: 0x0600338D RID: 13197 RVA: 0x0014DFAC File Offset: 0x0014C1AC
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

	// Token: 0x0600338E RID: 13198 RVA: 0x0014E01C File Offset: 0x0014C21C
	public virtual int GetLayerForMapIcon()
	{
		if (this.type == EnumMapObjectType.Entity && this.entity != null)
		{
			return this.entity.GetLayerForMapIcon();
		}
		return 2;
	}

	// Token: 0x0600338F RID: 13199 RVA: 0x0014E044 File Offset: 0x0014C244
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

	// Token: 0x06003390 RID: 13200 RVA: 0x00020119 File Offset: 0x0001E319
	public virtual bool IsShowName()
	{
		return true;
	}

	// Token: 0x06003391 RID: 13201 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool IsCenterOnLeftBottomCorner()
	{
		return false;
	}

	// Token: 0x06003392 RID: 13202 RVA: 0x0014E0D0 File Offset: 0x0014C2D0
	public virtual float GetCompassIconScale(float _distance)
	{
		float num = 1f - _distance / this.GetMaxCompassDistance();
		return Mathf.Lerp(this.GetMinCompassIconScale(), this.GetMaxCompassIconScale(), num);
	}

	// Token: 0x06003393 RID: 13203 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void RefreshData()
	{
	}

	// Token: 0x0400290A RID: 10506
	public EnumMapObjectType type;

	// Token: 0x0400290B RID: 10507
	public long key;

	// Token: 0x0400290C RID: 10508
	[PublicizedFrom(EAccessModifier.Protected)]
	public Vector3 position;

	// Token: 0x0400290D RID: 10509
	public bool bSelectable;

	// Token: 0x0400290E RID: 10510
	[PublicizedFrom(EAccessModifier.Protected)]
	public Entity entity;
}
