using System;
using UnityEngine;

// Token: 0x020001D6 RID: 470
public class BuffEntityUINotification : EntityUINotification
{
	// Token: 0x06000E7D RID: 3709 RVA: 0x0005F9AC File Offset: 0x0005DBAC
	public BuffEntityUINotification(EntityAlive _owner, BuffValue _buff)
	{
		this.owner = _owner;
		this.buff = _buff;
	}

	// Token: 0x1700010E RID: 270
	// (get) Token: 0x06000E7E RID: 3710 RVA: 0x0005F9C2 File Offset: 0x0005DBC2
	public BuffValue Buff
	{
		get
		{
			return this.buff;
		}
	}

	// Token: 0x1700010F RID: 271
	// (get) Token: 0x06000E7F RID: 3711 RVA: 0x0005F9CA File Offset: 0x0005DBCA
	public string Icon
	{
		get
		{
			return this.buff.BuffClass.Icon;
		}
	}

	// Token: 0x06000E80 RID: 3712 RVA: 0x0005F9DC File Offset: 0x0005DBDC
	public Color GetColor()
	{
		return this.Buff.BuffClass.IconColor;
	}

	// Token: 0x17000110 RID: 272
	// (get) Token: 0x06000E81 RID: 3713 RVA: 0x0005F9F0 File Offset: 0x0005DBF0
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
	// (get) Token: 0x06000E82 RID: 3714 RVA: 0x0005FA4C File Offset: 0x0005DC4C
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
	// (get) Token: 0x06000E83 RID: 3715 RVA: 0x0005FAF4 File Offset: 0x0005DCF4
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
	// (get) Token: 0x06000E84 RID: 3716 RVA: 0x00010E62 File Offset: 0x0000F062
	public EnumEntityUINotificationSubject Subject
	{
		get
		{
			return EnumEntityUINotificationSubject.Buff;
		}
	}

	// Token: 0x04000C5B RID: 3163
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityAlive owner;

	// Token: 0x04000C5C RID: 3164
	[PublicizedFrom(EAccessModifier.Private)]
	public BuffValue buff;
}
