using System;
using UnityEngine;

// Token: 0x0200062C RID: 1580
public class MapObject
{
	// Token: 0x06003326 RID: 13094 RVA: 0x0014C70C File Offset: 0x0014A90C
	public MapObject(EnumMapObjectType _type, Vector3 _position, long _key, Entity _entity, bool _bSelectable)
	{
		this.type = _type;
		this.position = _position;
		this.key = _key;
		this.bSelectable = _bSelectable;
		this.entity = _entity;
	}

	// Token: 0x06003327 RID: 13095 RVA: 0x0014C73C File Offset: 0x0014A93C
	public MapObject(MapObject _other)
	{
		this.type = _other.type;
		this.position = _other.position;
		this.key = _other.key;
		this.bSelectable = _other.bSelectable;
		this.entity = _other.entity;
	}

	// Token: 0x06003328 RID: 13096 RVA: 0x0014C78B File Offset: 0x0014A98B
	public virtual Vector3 GetPosition()
	{
		if (this.type == EnumMapObjectType.Entity && this.entity != null)
		{
			return this.entity.GetPosition();
		}
		return this.position;
	}

	// Token: 0x06003329 RID: 13097 RVA: 0x0014C7B5 File Offset: 0x0014A9B5
	public virtual void SetPosition(Vector3 _pos)
	{
		if (this.type == EnumMapObjectType.Entity)
		{
			throw new Exception("Setting of position not allowed!");
		}
		this.position = _pos;
	}

	// Token: 0x0600332A RID: 13098 RVA: 0x0014C7D4 File Offset: 0x0014A9D4
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

	// Token: 0x0600332B RID: 13099 RVA: 0x00020021 File Offset: 0x0001E221
	public virtual bool IsTracked()
	{
		return true;
	}

	// Token: 0x0600332C RID: 13100 RVA: 0x0014C82C File Offset: 0x0014AA2C
	public virtual bool IsMapIconEnabled()
	{
		return this.type != EnumMapObjectType.Entity || !(this.entity != null) || this.entity.IsDrawMapIcon();
	}

	// Token: 0x0600332D RID: 13101 RVA: 0x0014C851 File Offset: 0x0014AA51
	public virtual float GetMaxCompassDistance()
	{
		return 1024f;
	}

	// Token: 0x0600332E RID: 13102 RVA: 0x0003D2C6 File Offset: 0x0003B4C6
	public virtual float GetMinCompassDistance()
	{
		return 0f;
	}

	// Token: 0x0600332F RID: 13103 RVA: 0x0014C858 File Offset: 0x0014AA58
	public virtual float GetMaxCompassIconScale()
	{
		return 1.25f;
	}

	// Token: 0x06003330 RID: 13104 RVA: 0x0014C85F File Offset: 0x0014AA5F
	public virtual float GetMinCompassIconScale()
	{
		return 0.5f;
	}

	// Token: 0x06003331 RID: 13105 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool IsCompassIconClamped()
	{
		return false;
	}

	// Token: 0x06003332 RID: 13106 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool NearbyCompassBlink()
	{
		return false;
	}

	// Token: 0x06003333 RID: 13107 RVA: 0x0014C866 File Offset: 0x0014AA66
	public virtual Vector3 GetMapIconScale()
	{
		if (this.type == EnumMapObjectType.Entity && this.entity != null)
		{
			return this.entity.GetMapIconScale();
		}
		return Vector3.one;
	}

	// Token: 0x06003334 RID: 13108 RVA: 0x0014C88F File Offset: 0x0014AA8F
	public virtual string GetMapIcon()
	{
		if (this.type == EnumMapObjectType.Entity && this.entity != null)
		{
			return this.entity.GetMapIcon();
		}
		return "";
	}

	// Token: 0x06003335 RID: 13109 RVA: 0x0014C8B8 File Offset: 0x0014AAB8
	public virtual string GetCompassIcon()
	{
		if (this.type == EnumMapObjectType.Entity && this.entity != null)
		{
			return this.entity.GetCompassIcon();
		}
		return null;
	}

	// Token: 0x06003336 RID: 13110 RVA: 0x0014C8DD File Offset: 0x0014AADD
	public virtual string GetCompassUpIcon()
	{
		if (this.type == EnumMapObjectType.Entity && this.entity != null)
		{
			return this.entity.GetCompassUpIcon();
		}
		return "";
	}

	// Token: 0x06003337 RID: 13111 RVA: 0x0014C906 File Offset: 0x0014AB06
	public virtual string GetCompassDownIcon()
	{
		if (this.type == EnumMapObjectType.Entity && this.entity != null)
		{
			return this.entity.GetCompassDownIcon();
		}
		return "";
	}

	// Token: 0x06003338 RID: 13112 RVA: 0x0014C92F File Offset: 0x0014AB2F
	public virtual bool UseUpDownCompassIcons()
	{
		return this.type == EnumMapObjectType.Entity && this.entity != null && this.entity.GetCompassDownIcon() != null;
	}

	// Token: 0x06003339 RID: 13113 RVA: 0x0014C957 File Offset: 0x0014AB57
	public virtual bool IsMapIconBlinking()
	{
		return this.type == EnumMapObjectType.Entity && this.entity != null && this.entity.IsMapIconBlinking();
	}

	// Token: 0x0600333A RID: 13114 RVA: 0x0014C97C File Offset: 0x0014AB7C
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

	// Token: 0x0600333B RID: 13115 RVA: 0x0014CA22 File Offset: 0x0014AC22
	public virtual bool CanMapIconBeSelected()
	{
		return this.type == EnumMapObjectType.Entity && this.entity != null && this.entity.CanMapIconBeSelected();
	}

	// Token: 0x0600333C RID: 13116 RVA: 0x0014CA48 File Offset: 0x0014AC48
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

	// Token: 0x0600333D RID: 13117 RVA: 0x0014CAB8 File Offset: 0x0014ACB8
	public virtual int GetLayerForMapIcon()
	{
		if (this.type == EnumMapObjectType.Entity && this.entity != null)
		{
			return this.entity.GetLayerForMapIcon();
		}
		return 2;
	}

	// Token: 0x0600333E RID: 13118 RVA: 0x0014CAE0 File Offset: 0x0014ACE0
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

	// Token: 0x0600333F RID: 13119 RVA: 0x00020021 File Offset: 0x0001E221
	public virtual bool IsShowName()
	{
		return true;
	}

	// Token: 0x06003340 RID: 13120 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool IsCenterOnLeftBottomCorner()
	{
		return false;
	}

	// Token: 0x06003341 RID: 13121 RVA: 0x0014CB6C File Offset: 0x0014AD6C
	public virtual float GetCompassIconScale(float _distance)
	{
		float num = 1f - _distance / this.GetMaxCompassDistance();
		return Mathf.Lerp(this.GetMinCompassIconScale(), this.GetMaxCompassIconScale(), num);
	}

	// Token: 0x06003342 RID: 13122 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void RefreshData()
	{
	}

	// Token: 0x040028F6 RID: 10486
	public EnumMapObjectType type;

	// Token: 0x040028F7 RID: 10487
	public long key;

	// Token: 0x040028F8 RID: 10488
	[PublicizedFrom(EAccessModifier.Protected)]
	public Vector3 position;

	// Token: 0x040028F9 RID: 10489
	public bool bSelectable;

	// Token: 0x040028FA RID: 10490
	[PublicizedFrom(EAccessModifier.Protected)]
	public Entity entity;
}
