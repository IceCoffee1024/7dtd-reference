using System;

// Token: 0x02000981 RID: 2433
public abstract class BaseObjectiveModifier
{
	// Token: 0x1700079A RID: 1946
	// (get) Token: 0x06004697 RID: 18071 RVA: 0x001BF89B File Offset: 0x001BDA9B
	// (set) Token: 0x06004698 RID: 18072 RVA: 0x001BF8A3 File Offset: 0x001BDAA3
	public BaseObjective OwnerObjective { get; set; }

	// Token: 0x06004699 RID: 18073 RVA: 0x0000640C File Offset: 0x0000460C
	public BaseObjectiveModifier()
	{
	}

	// Token: 0x0600469A RID: 18074 RVA: 0x001BF8AC File Offset: 0x001BDAAC
	public void HandleAddHooks()
	{
		this.AddHooks();
	}

	// Token: 0x0600469B RID: 18075 RVA: 0x001BF8B4 File Offset: 0x001BDAB4
	public void HandleRemoveHooks()
	{
		this.RemoveHooks();
	}

	// Token: 0x0600469C RID: 18076 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void AddHooks()
	{
	}

	// Token: 0x0600469D RID: 18077 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void RemoveHooks()
	{
	}

	// Token: 0x0600469E RID: 18078 RVA: 0x000200DA File Offset: 0x0001E2DA
	public virtual BaseObjectiveModifier Clone()
	{
		return null;
	}

	// Token: 0x0600469F RID: 18079 RVA: 0x001BF8BC File Offset: 0x001BDABC
	public virtual void ParseProperties(DynamicProperties properties)
	{
		this.Properties = properties;
		this.OwnerObjective.OwnerQuestClass.HandleVariablesForProperties(properties);
	}

	// Token: 0x040038FB RID: 14587
	public DynamicProperties Properties;
}
