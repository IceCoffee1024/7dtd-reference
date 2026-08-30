using System;

// Token: 0x0200096A RID: 2410
public abstract class BaseObjectiveModifier
{
	// Token: 0x17000768 RID: 1896
	// (get) Token: 0x06004670 RID: 18032 RVA: 0x001BCB9F File Offset: 0x001BAD9F
	// (set) Token: 0x06004671 RID: 18033 RVA: 0x001BCBA7 File Offset: 0x001BADA7
	public BaseObjective OwnerObjective { get; set; }

	// Token: 0x06004672 RID: 18034 RVA: 0x0000640C File Offset: 0x0000460C
	public BaseObjectiveModifier()
	{
	}

	// Token: 0x06004673 RID: 18035 RVA: 0x001BCBB0 File Offset: 0x001BADB0
	public void HandleAddHooks()
	{
		this.AddHooks();
	}

	// Token: 0x06004674 RID: 18036 RVA: 0x001BCBB8 File Offset: 0x001BADB8
	public void HandleRemoveHooks()
	{
		this.RemoveHooks();
	}

	// Token: 0x06004675 RID: 18037 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void AddHooks()
	{
	}

	// Token: 0x06004676 RID: 18038 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void RemoveHooks()
	{
	}

	// Token: 0x06004677 RID: 18039 RVA: 0x0001FF1E File Offset: 0x0001E11E
	public virtual BaseObjectiveModifier Clone()
	{
		return null;
	}

	// Token: 0x06004678 RID: 18040 RVA: 0x001BCBC0 File Offset: 0x001BADC0
	public virtual void ParseProperties(DynamicProperties properties)
	{
		this.Properties = properties;
		this.OwnerObjective.OwnerQuestClass.HandleVariablesForProperties(properties);
	}

	// Token: 0x0400386E RID: 14446
	public DynamicProperties Properties;
}
