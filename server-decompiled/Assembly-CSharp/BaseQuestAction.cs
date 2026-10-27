using System;
using System.Collections;
using System.Globalization;
using UnityEngine;

// Token: 0x02000976 RID: 2422
public abstract class BaseQuestAction
{
	// Token: 0x17000791 RID: 1937
	// (get) Token: 0x06004638 RID: 17976 RVA: 0x001BF4AB File Offset: 0x001BD6AB
	// (set) Token: 0x06004639 RID: 17977 RVA: 0x001BF4B3 File Offset: 0x001BD6B3
	public Quest OwnerQuest { get; set; }

	// Token: 0x17000792 RID: 1938
	// (get) Token: 0x0600463A RID: 17978 RVA: 0x001BF4BC File Offset: 0x001BD6BC
	// (set) Token: 0x0600463B RID: 17979 RVA: 0x001BF4C4 File Offset: 0x001BD6C4
	public QuestClass Owner { get; set; }

	// Token: 0x17000793 RID: 1939
	// (get) Token: 0x0600463C RID: 17980 RVA: 0x001BF4CD File Offset: 0x001BD6CD
	// (set) Token: 0x0600463D RID: 17981 RVA: 0x001BF4D5 File Offset: 0x001BD6D5
	public int Phase { get; set; }

	// Token: 0x17000794 RID: 1940
	// (get) Token: 0x0600463E RID: 17982 RVA: 0x001BF4DE File Offset: 0x001BD6DE
	// (set) Token: 0x0600463F RID: 17983 RVA: 0x001BF4E6 File Offset: 0x001BD6E6
	public float Delay { get; set; }

	// Token: 0x17000795 RID: 1941
	// (get) Token: 0x06004640 RID: 17984 RVA: 0x001BF4EF File Offset: 0x001BD6EF
	// (set) Token: 0x06004641 RID: 17985 RVA: 0x001BF4F7 File Offset: 0x001BD6F7
	public bool OnComplete { get; set; }

	// Token: 0x06004642 RID: 17986 RVA: 0x001BF500 File Offset: 0x001BD700
	public BaseQuestAction()
	{
		this.Phase = 1;
	}

	// Token: 0x06004643 RID: 17987 RVA: 0x001BF50F File Offset: 0x001BD70F
	[PublicizedFrom(EAccessModifier.Protected)]
	public void CopyValues(BaseQuestAction action)
	{
		action.ID = this.ID;
		action.Value = this.Value;
		action.Phase = this.Phase;
		action.Delay = this.Delay;
		action.OnComplete = this.OnComplete;
	}

	// Token: 0x06004644 RID: 17988 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void SetupAction()
	{
	}

	// Token: 0x06004645 RID: 17989 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void PerformAction(Quest ownerQuest)
	{
	}

	// Token: 0x06004646 RID: 17990 RVA: 0x001BF54D File Offset: 0x001BD74D
	public void HandlePerformAction()
	{
		if (this.Delay == 0f)
		{
			this.PerformAction(this.OwnerQuest);
			return;
		}
		GameManager.Instance.StartCoroutine(this.PerformActionLater());
	}

	// Token: 0x06004647 RID: 17991 RVA: 0x001BF57A File Offset: 0x001BD77A
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

	// Token: 0x06004648 RID: 17992 RVA: 0x001BF589 File Offset: 0x001BD789
	public virtual void HandleVariables()
	{
		this.ID = this.OwnerQuest.ParseVariable(this.ID);
		this.Value = this.OwnerQuest.ParseVariable(this.Value);
	}

	// Token: 0x06004649 RID: 17993 RVA: 0x000200DA File Offset: 0x0001E2DA
	public virtual BaseQuestAction Clone()
	{
		return null;
	}

	// Token: 0x0600464A RID: 17994 RVA: 0x001BF5BC File Offset: 0x001BD7BC
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

	// Token: 0x040038DA RID: 14554
	public static string PropID = "id";

	// Token: 0x040038DB RID: 14555
	public static string PropValue = "value";

	// Token: 0x040038DC RID: 14556
	public static string PropPhase = "phase";

	// Token: 0x040038DD RID: 14557
	public static string PropDelay = "delay";

	// Token: 0x040038DE RID: 14558
	public static string PropOnComplete = "on_complete";

	// Token: 0x040038DF RID: 14559
	public string ID;

	// Token: 0x040038E0 RID: 14560
	public string Value;

	// Token: 0x040038E6 RID: 14566
	public DynamicProperties Properties;
}
