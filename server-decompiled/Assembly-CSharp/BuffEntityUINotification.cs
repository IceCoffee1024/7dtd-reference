using System;
using UnityEngine;

// Token: 0x020001D8 RID: 472
public class BuffEntityUINotification : EntityUINotification
{
	// Token: 0x06000E81 RID: 3713 RVA: 0x0005FC70 File Offset: 0x0005DE70
	public BuffEntityUINotification(EntityAlive _owner, BuffValue _buff)
	{
		this.owner = _owner;
		this.buff = _buff;
	}

	// Token: 0x1700010E RID: 270
	// (get) Token: 0x06000E82 RID: 3714 RVA: 0x0005FC86 File Offset: 0x0005DE86
	public BuffValue Buff
	{
		get
		{
			return this.buff;
		}
	}

	// Token: 0x1700010F RID: 271
	// (get) Token: 0x06000E83 RID: 3715 RVA: 0x0005FC8E File Offset: 0x0005DE8E
	public string Icon
	{
		get
		{
			return this.buff.BuffClass.Icon;
		}
	}

	// Token: 0x06000E84 RID: 3716 RVA: 0x0005FCA0 File Offset: 0x0005DEA0
	public Color GetColor()
	{
		return this.Buff.BuffClass.IconColor;
	}

	// Token: 0x17000110 RID: 272
	// (get) Token: 0x06000E85 RID: 3717 RVA: 0x0005FCB4 File Offset: 0x0005DEB4
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

	// Token: 0x17000111 RID: 273
	// (get) Token: 0x06000E86 RID: 3718 RVA: 0x0005FD10 File Offset: 0x0005DF10
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

	// Token: 0x17000112 RID: 274
	// (get) Token: 0x06000E87 RID: 3719 RVA: 0x0005FDB8 File Offset: 0x0005DFB8
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

	// Token: 0x17000113 RID: 275
	// (get) Token: 0x06000E88 RID: 3720 RVA: 0x00010E62 File Offset: 0x0000F062
	public EnumEntityUINotificationSubject Subject
	{
		get
		{
			return EnumEntityUINotificationSubject.Buff;
		}
	}

	// Token: 0x04000C5D RID: 3165
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityAlive owner;

	// Token: 0x04000C5E RID: 3166
	[PublicizedFrom(EAccessModifier.Private)]
	public BuffValue buff;
}
