using System;
using System.Collections;
using System.Globalization;
using UnityEngine;

// Token: 0x0200096C RID: 2412
public abstract class BaseQuestAction
{
	// Token: 0x1700078B RID: 1931
	// (get) Token: 0x06004615 RID: 17941 RVA: 0x001BE427 File Offset: 0x001BC627
	// (set) Token: 0x06004616 RID: 17942 RVA: 0x001BE42F File Offset: 0x001BC62F
	public Quest OwnerQuest { get; set; }

	// Token: 0x1700078C RID: 1932
	// (get) Token: 0x06004617 RID: 17943 RVA: 0x001BE438 File Offset: 0x001BC638
	// (set) Token: 0x06004618 RID: 17944 RVA: 0x001BE440 File Offset: 0x001BC640
	public QuestClass Owner { get; set; }

	// Token: 0x1700078D RID: 1933
	// (get) Token: 0x06004619 RID: 17945 RVA: 0x001BE449 File Offset: 0x001BC649
	// (set) Token: 0x0600461A RID: 17946 RVA: 0x001BE451 File Offset: 0x001BC651
	public int Phase { get; set; }

	// Token: 0x1700078E RID: 1934
	// (get) Token: 0x0600461B RID: 17947 RVA: 0x001BE45A File Offset: 0x001BC65A
	// (set) Token: 0x0600461C RID: 17948 RVA: 0x001BE462 File Offset: 0x001BC662
	public float Delay { get; set; }

	// Token: 0x1700078F RID: 1935
	// (get) Token: 0x0600461D RID: 17949 RVA: 0x001BE46B File Offset: 0x001BC66B
	// (set) Token: 0x0600461E RID: 17950 RVA: 0x001BE473 File Offset: 0x001BC673
	public bool OnComplete { get; set; }

	// Token: 0x0600461F RID: 17951 RVA: 0x001BE47C File Offset: 0x001BC67C
	public BaseQuestAction()
	{
		this.Phase = 1;
	}

	// Token: 0x06004620 RID: 17952 RVA: 0x001BE48B File Offset: 0x001BC68B
	[PublicizedFrom(EAccessModifier.Protected)]
	public void CopyValues(BaseQuestAction action)
	{
		action.ID = this.ID;
		action.Value = this.Value;
		action.Phase = this.Phase;
		action.Delay = this.Delay;
		action.OnComplete = this.OnComplete;
	}

	// Token: 0x06004621 RID: 17953 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void SetupAction()
	{
	}

	// Token: 0x06004622 RID: 17954 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void PerformAction(Quest ownerQuest)
	{
	}

	// Token: 0x06004623 RID: 17955 RVA: 0x001BE4C9 File Offset: 0x001BC6C9
	public void HandlePerformAction()
	{
		if (this.Delay == 0f)
		{
			this.PerformAction(this.OwnerQuest);
			return;
		}
		GameManager.Instance.StartCoroutine(this.PerformActionLater());
	}

	// Token: 0x06004624 RID: 17956 RVA: 0x001BE4F6 File Offset: 0x001BC6F6
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

	// Token: 0x06004625 RID: 17957 RVA: 0x001BE505 File Offset: 0x001BC705
	public virtual void HandleVariables()
	{
		this.ID = this.OwnerQuest.ParseVariable(this.ID);
		this.Value = this.OwnerQuest.ParseVariable(this.Value);
	}

	// Token: 0x06004626 RID: 17958 RVA: 0x000200DA File Offset: 0x0001E2DA
	public virtual BaseQuestAction Clone()
	{
		return null;
	}

	// Token: 0x06004627 RID: 17959 RVA: 0x001BE538 File Offset: 0x001BC738
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

	// Token: 0x040038BA RID: 14522
	public static string PropID = "id";

	// Token: 0x040038BB RID: 14523
	public static string PropValue = "value";

	// Token: 0x040038BC RID: 14524
	public static string PropPhase = "phase";

	// Token: 0x040038BD RID: 14525
	public static string PropDelay = "delay";

	// Token: 0x040038BE RID: 14526
	public static string PropOnComplete = "on_complete";

	// Token: 0x040038BF RID: 14527
	public string ID;

	// Token: 0x040038C0 RID: 14528
	public string Value;

	// Token: 0x040038C6 RID: 14534
	public DynamicProperties Properties;
}
