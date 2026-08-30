using System;
using UnityEngine;

// Token: 0x020001D5 RID: 469
public class BuffEntityUINotification : EntityUINotification
{
	// Token: 0x06000E8A RID: 3722 RVA: 0x0005F9BC File Offset: 0x0005DBBC
	public BuffEntityUINotification(EntityAlive _owner, BuffValue _buff)
	{
		this.owner = _owner;
		this.buff = _buff;
	}

	// Token: 0x17000116 RID: 278
	// (get) Token: 0x06000E8B RID: 3723 RVA: 0x0005F9D2 File Offset: 0x0005DBD2
	public BuffValue Buff
	{
		get
		{
			return this.buff;
		}
	}

	// Token: 0x17000117 RID: 279
	// (get) Token: 0x06000E8C RID: 3724 RVA: 0x0005F9DA File Offset: 0x0005DBDA
	public string Icon
	{
		get
		{
			return this.buff.BuffClass.Icon;
		}
	}

	// Token: 0x06000E8D RID: 3725 RVA: 0x0005F9EC File Offset: 0x0005DBEC
	public Color GetColor()
	{
		return this.Buff.BuffClass.IconColor;
	}

	// Token: 0x17000118 RID: 280
	// (get) Token: 0x06000E8E RID: 3726 RVA: 0x0005FA00 File Offset: 0x0005DC00
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
	// (get) Token: 0x06000E8F RID: 3727 RVA: 0x0005FA5C File Offset: 0x0005DC5C
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
	// (get) Token: 0x06000E90 RID: 3728 RVA: 0x0005FB04 File Offset: 0x0005DD04
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
	// (get) Token: 0x06000E91 RID: 3729 RVA: 0x00010E62 File Offset: 0x0000F062
	public EnumEntityUINotificationSubject Subject
	{
		get
		{
			return EnumEntityUINotificationSubject.Buff;
		}
	}

	// Token: 0x04000C62 RID: 3170
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityAlive owner;

	// Token: 0x04000C63 RID: 3171
	[PublicizedFrom(EAccessModifier.Private)]
	public BuffValue buff;
}
