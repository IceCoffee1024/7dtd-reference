using System;
using UnityEngine;

// Token: 0x020001D3 RID: 467
public class BuffEntityUINotification : EntityUINotification
{
	// Token: 0x06000E7F RID: 3711 RVA: 0x0005F473 File Offset: 0x0005D673
	public BuffEntityUINotification(EntityAlive _owner, BuffValue _buff)
	{
		this.owner = _owner;
		this.buff = _buff;
	}

	// Token: 0x17000116 RID: 278
	// (get) Token: 0x06000E80 RID: 3712 RVA: 0x0005F489 File Offset: 0x0005D689
	public BuffValue Buff
	{
		get
		{
			return this.buff;
		}
	}

	// Token: 0x17000117 RID: 279
	// (get) Token: 0x06000E81 RID: 3713 RVA: 0x0005F491 File Offset: 0x0005D691
	public string Icon
	{
		get
		{
			return this.buff.BuffClass.Icon;
		}
	}

	// Token: 0x06000E82 RID: 3714 RVA: 0x0005F4A3 File Offset: 0x0005D6A3
	public Color GetColor()
	{
		return this.Buff.BuffClass.IconColor;
	}

	// Token: 0x17000118 RID: 280
	// (get) Token: 0x06000E83 RID: 3715 RVA: 0x0005F4B8 File Offset: 0x0005D6B8
	public float CurrentValue
	{
		get
		{
			if (this.buff != null && this.buff.BuffClass != null && this.buff.BuffClass.DisplayValueCVar != null)
			{
				return this.owner.Buffs.GetCustomVar(this.buff.BuffClass.DisplayValueCVar);
			}
			return 0f;
		}
	}

	// Token: 0x17000119 RID: 281
	// (get) Token: 0x06000E84 RID: 3716 RVA: 0x0005F514 File Offset: 0x0005D714
	public string Units
	{
		get
		{
			if (this.buff == null || this.buff.BuffClass == null || this.buff.BuffClass.DisplayValueCVar == null)
			{
				return "";
			}
			if (this.buff.BuffClass.DisplayValueCVar.StartsWith("$") || this.buff.BuffClass.DisplayValueCVar.StartsWith(".") || this.buff.BuffClass.DisplayValueCVar.StartsWith("_"))
			{
				return "cvar";
			}
			return this.buff.BuffClass.DisplayValueCVar;
		}
	}

	// Token: 0x1700011A RID: 282
	// (get) Token: 0x06000E85 RID: 3717 RVA: 0x0005F5BC File Offset: 0x0005D7BC
	public EnumEntityUINotificationDisplayMode DisplayMode
	{
		get
		{
			EnumEntityUINotificationDisplayMode enumEntityUINotificationDisplayMode = ((this.buff != null && this.buff.BuffClass != null) ? this.buff.BuffClass.DisplayType : EnumEntityUINotificationDisplayMode.IconOnly);
			if (this.buff.BuffClass.DisplayValueCVar != null && this.buff.BuffClass.DisplayValueCVar != "")
			{
				enumEntityUINotificationDisplayMode = EnumEntityUINotificationDisplayMode.IconPlusCurrentValue;
			}
			return enumEntityUINotificationDisplayMode;
		}
	}

	// Token: 0x1700011B RID: 283
	// (get) Token: 0x06000E86 RID: 3718 RVA: 0x00010E62 File Offset: 0x0000F062
	public EnumEntityUINotificationSubject Subject
	{
		get
		{
			return EnumEntityUINotificationSubject.Buff;
		}
	}

	// Token: 0x04000C55 RID: 3157
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityAlive owner;

	// Token: 0x04000C56 RID: 3158
	[PublicizedFrom(EAccessModifier.Private)]
	public BuffValue buff;
}
