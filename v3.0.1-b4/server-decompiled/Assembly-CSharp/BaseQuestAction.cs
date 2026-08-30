using System;
using System.Collections;
using System.Globalization;
using UnityEngine;

// Token: 0x02000955 RID: 2389
public abstract class BaseQuestAction
{
	// Token: 0x17000759 RID: 1881
	// (get) Token: 0x060045EE RID: 17902 RVA: 0x001BB72B File Offset: 0x001B992B
	// (set) Token: 0x060045EF RID: 17903 RVA: 0x001BB733 File Offset: 0x001B9933
	public Quest OwnerQuest { get; set; }

	// Token: 0x1700075A RID: 1882
	// (get) Token: 0x060045F0 RID: 17904 RVA: 0x001BB73C File Offset: 0x001B993C
	// (set) Token: 0x060045F1 RID: 17905 RVA: 0x001BB744 File Offset: 0x001B9944
	public QuestClass Owner { get; set; }

	// Token: 0x1700075B RID: 1883
	// (get) Token: 0x060045F2 RID: 17906 RVA: 0x001BB74D File Offset: 0x001B994D
	// (set) Token: 0x060045F3 RID: 17907 RVA: 0x001BB755 File Offset: 0x001B9955
	public int Phase { get; set; }

	// Token: 0x1700075C RID: 1884
	// (get) Token: 0x060045F4 RID: 17908 RVA: 0x001BB75E File Offset: 0x001B995E
	// (set) Token: 0x060045F5 RID: 17909 RVA: 0x001BB766 File Offset: 0x001B9966
	public float Delay { get; set; }

	// Token: 0x1700075D RID: 1885
	// (get) Token: 0x060045F6 RID: 17910 RVA: 0x001BB76F File Offset: 0x001B996F
	// (set) Token: 0x060045F7 RID: 17911 RVA: 0x001BB777 File Offset: 0x001B9977
	public bool OnComplete { get; set; }

	// Token: 0x060045F8 RID: 17912 RVA: 0x001BB780 File Offset: 0x001B9980
	public BaseQuestAction()
	{
		this.Phase = 1;
	}

	// Token: 0x060045F9 RID: 17913 RVA: 0x001BB78F File Offset: 0x001B998F
	[PublicizedFrom(EAccessModifier.Protected)]
	public void CopyValues(BaseQuestAction action)
	{
		action.ID = this.ID;
		action.Value = this.Value;
		action.Phase = this.Phase;
		action.Delay = this.Delay;
		action.OnComplete = this.OnComplete;
	}

	// Token: 0x060045FA RID: 17914 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void SetupAction()
	{
	}

	// Token: 0x060045FB RID: 17915 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void PerformAction(Quest ownerQuest)
	{
	}

	// Token: 0x060045FC RID: 17916 RVA: 0x001BB7CD File Offset: 0x001B99CD
	public void HandlePerformAction()
	{
		if (this.Delay == 0f)
		{
			this.PerformAction(this.OwnerQuest);
			return;
		}
		GameManager.Instance.StartCoroutine(this.PerformActionLater());
	}

	// Token: 0x060045FD RID: 17917 RVA: 0x001BB7FA File Offset: 0x001B99FA
	[PublicizedFrom(EAccessModifier.Protected)]
	public IEnumerator PerformActionLater()
	{
		yield return new WaitForSeconds(this.Delay);
		if (XUi.IsGameRunning())
		{
			this.PerformAction(this.OwnerQuest);
		}
		yield break;
	}

	// Token: 0x060045FE RID: 17918 RVA: 0x001BB809 File Offset: 0x001B9A09
	public virtual void HandleVariables()
	{
		this.ID = this.OwnerQuest.ParseVariable(this.ID);
		this.Value = this.OwnerQuest.ParseVariable(this.Value);
	}

	// Token: 0x060045FF RID: 17919 RVA: 0x0001FF1E File Offset: 0x0001E11E
	public virtual BaseQuestAction Clone()
	{
		return null;
	}

	// Token: 0x06004600 RID: 17920 RVA: 0x001BB83C File Offset: 0x001B9A3C
	public virtual void ParseProperties(DynamicProperties properties)
	{
		this.Properties = properties;
		this.Owner.HandleVariablesForProperties(properties);
		if (properties.Values.ContainsKey(BaseQuestAction.PropID))
		{
			this.ID = properties.Values[BaseQuestAction.PropID];
		}
		if (properties.Values.ContainsKey(BaseQuestAction.PropValue))
		{
			this.Value = properties.Values[BaseQuestAction.PropValue];
		}
		if (properties.Values.ContainsKey(BaseQuestAction.PropPhase))
		{
			this.Phase = (int)Convert.ToByte(properties.Values[BaseQuestAction.PropPhase]);
		}
		if (properties.Values.ContainsKey(BaseQuestAction.PropPhase))
		{
			this.Phase = (int)Convert.ToByte(properties.Values[BaseQuestAction.PropPhase]);
		}
		if (properties.Values.ContainsKey(BaseQuestAction.PropDelay))
		{
			this.Delay = StringParsers.ParseFloat(properties.Values[BaseQuestAction.PropDelay], 0, -1, NumberStyles.Any);
		}
		if (properties.Values.ContainsKey(BaseQuestAction.PropOnComplete))
		{
			this.OnComplete = StringParsers.ParseBool(properties.Values[BaseQuestAction.PropOnComplete], 0, -1, true);
		}
	}

	// Token: 0x0400382D RID: 14381
	public static string PropID = "id";

	// Token: 0x0400382E RID: 14382
	public static string PropValue = "value";

	// Token: 0x0400382F RID: 14383
	public static string PropPhase = "phase";

	// Token: 0x04003830 RID: 14384
	public static string PropDelay = "delay";

	// Token: 0x04003831 RID: 14385
	public static string PropOnComplete = "on_complete";

	// Token: 0x04003832 RID: 14386
	public string ID;

	// Token: 0x04003833 RID: 14387
	public string Value;

	// Token: 0x04003839 RID: 14393
	public DynamicProperties Properties;
}
