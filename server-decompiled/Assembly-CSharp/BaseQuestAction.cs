using System;
using System.Collections;
using System.Globalization;
using UnityEngine;

// Token: 0x02000966 RID: 2406
public abstract class BaseQuestAction
{
	// Token: 0x1700076B RID: 1899
	// (get) Token: 0x06004674 RID: 18036 RVA: 0x001BDF1F File Offset: 0x001BC11F
	// (set) Token: 0x06004675 RID: 18037 RVA: 0x001BDF27 File Offset: 0x001BC127
	public Quest OwnerQuest { get; set; }

	// Token: 0x1700076C RID: 1900
	// (get) Token: 0x06004676 RID: 18038 RVA: 0x001BDF30 File Offset: 0x001BC130
	// (set) Token: 0x06004677 RID: 18039 RVA: 0x001BDF38 File Offset: 0x001BC138
	public QuestClass Owner { get; set; }

	// Token: 0x1700076D RID: 1901
	// (get) Token: 0x06004678 RID: 18040 RVA: 0x001BDF41 File Offset: 0x001BC141
	// (set) Token: 0x06004679 RID: 18041 RVA: 0x001BDF49 File Offset: 0x001BC149
	public int Phase { get; set; }

	// Token: 0x1700076E RID: 1902
	// (get) Token: 0x0600467A RID: 18042 RVA: 0x001BDF52 File Offset: 0x001BC152
	// (set) Token: 0x0600467B RID: 18043 RVA: 0x001BDF5A File Offset: 0x001BC15A
	public float Delay { get; set; }

	// Token: 0x1700076F RID: 1903
	// (get) Token: 0x0600467C RID: 18044 RVA: 0x001BDF63 File Offset: 0x001BC163
	// (set) Token: 0x0600467D RID: 18045 RVA: 0x001BDF6B File Offset: 0x001BC16B
	public bool OnComplete { get; set; }

	// Token: 0x0600467E RID: 18046 RVA: 0x001BDF74 File Offset: 0x001BC174
	public BaseQuestAction()
	{
		this.Phase = 1;
	}

	// Token: 0x0600467F RID: 18047 RVA: 0x001BDF83 File Offset: 0x001BC183
	[PublicizedFrom(EAccessModifier.Protected)]
	public void CopyValues(BaseQuestAction action)
	{
		action.ID = this.ID;
		action.Value = this.Value;
		action.Phase = this.Phase;
		action.Delay = this.Delay;
		action.OnComplete = this.OnComplete;
	}

	// Token: 0x06004680 RID: 18048 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void SetupAction()
	{
	}

	// Token: 0x06004681 RID: 18049 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void PerformAction(Quest ownerQuest)
	{
	}

	// Token: 0x06004682 RID: 18050 RVA: 0x001BDFC1 File Offset: 0x001BC1C1
	public void HandlePerformAction()
	{
		if (this.Delay == 0f)
		{
			this.PerformAction(this.OwnerQuest);
			return;
		}
		GameManager.Instance.StartCoroutine(this.PerformActionLater());
	}

	// Token: 0x06004683 RID: 18051 RVA: 0x001BDFEE File Offset: 0x001BC1EE
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

	// Token: 0x06004684 RID: 18052 RVA: 0x001BDFFD File Offset: 0x001BC1FD
	public virtual void HandleVariables()
	{
		this.ID = this.OwnerQuest.ParseVariable(this.ID);
		this.Value = this.OwnerQuest.ParseVariable(this.Value);
	}

	// Token: 0x06004685 RID: 18053 RVA: 0x0001FFE2 File Offset: 0x0001E1E2
	public virtual BaseQuestAction Clone()
	{
		return null;
	}

	// Token: 0x06004686 RID: 18054 RVA: 0x001BE030 File Offset: 0x001BC230
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

	// Token: 0x040038BE RID: 14526
	public static string PropID = "id";

	// Token: 0x040038BF RID: 14527
	public static string PropValue = "value";

	// Token: 0x040038C0 RID: 14528
	public static string PropPhase = "phase";

	// Token: 0x040038C1 RID: 14529
	public static string PropDelay = "delay";

	// Token: 0x040038C2 RID: 14530
	public static string PropOnComplete = "on_complete";

	// Token: 0x040038C3 RID: 14531
	public string ID;

	// Token: 0x040038C4 RID: 14532
	public string Value;

	// Token: 0x040038CA RID: 14538
	public DynamicProperties Properties;
}
