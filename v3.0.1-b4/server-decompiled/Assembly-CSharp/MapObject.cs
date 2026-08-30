using System;
using UnityEngine;

// Token: 0x02000623 RID: 1571
public class MapObject
{
	// Token: 0x060032D3 RID: 13011 RVA: 0x0014A5F0 File Offset: 0x001487F0
	public MapObject(EnumMapObjectType _type, Vector3 _position, long _key, Entity _entity, bool _bSelectable)
	{
		this.type = _type;
		this.position = _position;
		this.key = _key;
		this.bSelectable = _bSelectable;
		this.entity = _entity;
	}

	// Token: 0x060032D4 RID: 13012 RVA: 0x0014A620 File Offset: 0x00148820
	public MapObject(MapObject _other)
	{
		this.type = _other.type;
		this.position = _other.position;
		this.key = _other.key;
		this.bSelectable = _other.bSelectable;
		this.entity = _other.entity;
	}

	// Token: 0x060032D5 RID: 13013 RVA: 0x0014A66F File Offset: 0x0014886F
	public virtual Vector3 GetPosition()
	{
		if (this.type == EnumMapObjectType.Entity && this.entity != null)
		{
			return this.entity.GetPosition();
		}
		return this.position;
	}

	// Token: 0x060032D6 RID: 13014 RVA: 0x0014A699 File Offset: 0x00148899
	public virtual void SetPosition(Vector3 _pos)
	{
		if (this.type == EnumMapObjectType.Entity)
		{
			throw new Exception("Setting of position not allowed!");
		}
		this.position = _pos;
	}

	// Token: 0x060032D7 RID: 13015 RVA: 0x0014A6B8 File Offset: 0x001488B8
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

	// Token: 0x060032D8 RID: 13016 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public virtual bool IsTracked()
	{
		return true;
	}

	// Token: 0x060032D9 RID: 13017 RVA: 0x0014A710 File Offset: 0x00148910
	public virtual bool IsMapIconEnabled()
	{
		return this.type != EnumMapObjectType.Entity || !(this.entity != null) || this.entity.IsDrawMapIcon();
	}

	// Token: 0x060032DA RID: 13018 RVA: 0x0014A735 File Offset: 0x00148935
	public virtual float GetMaxCompassDistance()
	{
		return 1024f;
	}

	// Token: 0x060032DB RID: 13019 RVA: 0x0003D066 File Offset: 0x0003B266
	public virtual float GetMinCompassDistance()
	{
		return 0f;
	}

	// Token: 0x060032DC RID: 13020 RVA: 0x0014A73C File Offset: 0x0014893C
	public virtual float GetMaxCompassIconScale()
	{
		return 1.25f;
	}

	// Token: 0x060032DD RID: 13021 RVA: 0x0014A743 File Offset: 0x00148943
	public virtual float GetMinCompassIconScale()
	{
		return 0.5f;
	}

	// Token: 0x060032DE RID: 13022 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool IsCompassIconClamped()
	{
		return false;
	}

	// Token: 0x060032DF RID: 13023 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool NearbyCompassBlink()
	{
		return false;
	}

	// Token: 0x060032E0 RID: 13024 RVA: 0x0014A74A File Offset: 0x0014894A
	public virtual Vector3 GetMapIconScale()
	{
		if (this.type == EnumMapObjectType.Entity && this.entity != null)
		{
			return this.entity.GetMapIconScale();
		}
		return Vector3.one;
	}

	// Token: 0x060032E1 RID: 13025 RVA: 0x0014A773 File Offset: 0x00148973
	public virtual string GetMapIcon()
	{
		if (this.type == EnumMapObjectType.Entity && this.entity != null)
		{
			return this.entity.GetMapIcon();
		}
		return "";
	}

	// Token: 0x060032E2 RID: 13026 RVA: 0x0014A79C File Offset: 0x0014899C
	public virtual string GetCompassIcon()
	{
		if (this.type == EnumMapObjectType.Entity && this.entity != null)
		{
			return this.entity.GetCompassIcon();
		}
		return null;
	}

	// Token: 0x060032E3 RID: 13027 RVA: 0x0014A7C1 File Offset: 0x001489C1
	public virtual string GetCompassUpIcon()
	{
		if (this.type == EnumMapObjectType.Entity && this.entity != null)
		{
			return this.entity.GetCompassUpIcon();
		}
		return "";
	}

	// Token: 0x060032E4 RID: 13028 RVA: 0x0014A7EA File Offset: 0x001489EA
	public virtual string GetCompassDownIcon()
	{
		if (this.type == EnumMapObjectType.Entity && this.entity != null)
		{
			return this.entity.GetCompassDownIcon();
		}
		return "";
	}

	// Token: 0x060032E5 RID: 13029 RVA: 0x0014A813 File Offset: 0x00148A13
	public virtual bool UseUpDownCompassIcons()
	{
		return this.type == EnumMapObjectType.Entity && this.entity != null && this.entity.GetCompassDownIcon() != null;
	}

	// Token: 0x060032E6 RID: 13030 RVA: 0x0014A83B File Offset: 0x00148A3B
	public virtual bool IsMapIconBlinking()
	{
		return this.type == EnumMapObjectType.Entity && this.entity != null && this.entity.IsMapIconBlinking();
	}

	// Token: 0x060032E7 RID: 13031 RVA: 0x0014A860 File Offset: 0x00148A60
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

	// Token: 0x060032E8 RID: 13032 RVA: 0x0014A906 File Offset: 0x00148B06
	public virtual bool CanMapIconBeSelected()
	{
		return this.type == EnumMapObjectType.Entity && this.entity != null && this.entity.CanMapIconBeSelected();
	}

	// Token: 0x060032E9 RID: 13033 RVA: 0x0014A92C File Offset: 0x00148B2C
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

	// Token: 0x060032EA RID: 13034 RVA: 0x0014A99C File Offset: 0x00148B9C
	public virtual int GetLayerForMapIcon()
	{
		if (this.type == EnumMapObjectType.Entity && this.entity != null)
		{
			return this.entity.GetLayerForMapIcon();
		}
		return 2;
	}

	// Token: 0x060032EB RID: 13035 RVA: 0x0014A9C4 File Offset: 0x00148BC4
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

	// Token: 0x060032EC RID: 13036 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public virtual bool IsShowName()
	{
		return true;
	}

	// Token: 0x060032ED RID: 13037 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool IsCenterOnLeftBottomCorner()
	{
		return false;
	}

	// Token: 0x060032EE RID: 13038 RVA: 0x0014AA50 File Offset: 0x00148C50
	public virtual float GetCompassIconScale(float _distance)
	{
		float num = 1f - _distance / this.GetMaxCompassDistance();
		return Mathf.Lerp(this.GetMinCompassIconScale(), this.GetMaxCompassIconScale(), num);
	}

	// Token: 0x060032EF RID: 13039 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void RefreshData()
	{
	}

	// Token: 0x04002883 RID: 10371
	public EnumMapObjectType type;

	// Token: 0x04002884 RID: 10372
	public long key;

	// Token: 0x04002885 RID: 10373
	[PublicizedFrom(EAccessModifier.Protected)]
	public Vector3 position;

	// Token: 0x04002886 RID: 10374
	public bool bSelectable;

	// Token: 0x04002887 RID: 10375
	[PublicizedFrom(EAccessModifier.Protected)]
	public Entity entity;
}
