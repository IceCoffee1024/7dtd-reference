using System;

// Token: 0x0200098B RID: 2443
public abstract class BaseObjectiveModifier
{
	// Token: 0x170007A0 RID: 1952
	// (get) Token: 0x060046BA RID: 18106 RVA: 0x001C091F File Offset: 0x001BEB1F
	// (set) Token: 0x060046BB RID: 18107 RVA: 0x001C0927 File Offset: 0x001BEB27
	public BaseObjective OwnerObjective { get; set; }

	// Token: 0x060046BC RID: 18108 RVA: 0x0000640C File Offset: 0x0000460C
	public BaseObjectiveModifier()
	{
	}

	// Token: 0x060046BD RID: 18109 RVA: 0x001C0930 File Offset: 0x001BEB30
	public void HandleAddHooks()
	{
		this.AddHooks();
	}

	// Token: 0x060046BE RID: 18110 RVA: 0x001C0938 File Offset: 0x001BEB38
	public void HandleRemoveHooks()
	{
		this.RemoveHooks();
	}

	// Token: 0x060046BF RID: 18111 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void AddHooks()
	{
	}

	// Token: 0x060046C0 RID: 18112 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void RemoveHooks()
	{
	}

	// Token: 0x060046C1 RID: 18113 RVA: 0x000200DA File Offset: 0x0001E2DA
	public virtual BaseObjectiveModifier Clone()
	{
		return null;
	}

	// Token: 0x060046C2 RID: 18114 RVA: 0x001C0940 File Offset: 0x001BEB40
	public virtual void ParseProperties(DynamicProperties properties)
	{
		this.Properties = properties;
		this.OwnerObjective.OwnerQuestClass.HandleVariablesForProperties(properties);
	}

	// Token: 0x0400391B RID: 14619
	public DynamicProperties Properties;
}
