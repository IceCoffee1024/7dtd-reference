using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000969 RID: 2409
[Preserve]
public class QuestEvent
{
	// Token: 0x0600466B RID: 18027 RVA: 0x001BCA0E File Offset: 0x001BAC0E
	public QuestEvent(string type)
	{
		this.EventType = type;
	}

	// Token: 0x0600466C RID: 18028 RVA: 0x001BCA34 File Offset: 0x001BAC34
	public void HandleEvent(Quest quest)
	{
		if (this.IsServerOnly && !SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			return;
		}
		if (GameManager.Instance.World.GetGameRandom().RandomFloat < this.Chance)
		{
			for (int i = 0; i < this.Actions.Count; i++)
			{
				this.Actions[i].PerformAction(quest);
			}
		}
	}

	// Token: 0x0600466D RID: 18029 RVA: 0x001BCA9A File Offset: 0x001BAC9A
	public virtual void ParseProperties(DynamicProperties properties)
	{
		this.Properties = properties;
		this.Owner.HandleVariablesForProperties(properties);
		properties.ParseFloat(QuestEvent.PropChance, ref this.Chance);
		properties.ParseBool(QuestEvent.PropServerOnly, ref this.IsServerOnly);
	}

	// Token: 0x0600466E RID: 18030 RVA: 0x001BCAD4 File Offset: 0x001BACD4
	public QuestEvent Clone()
	{
		QuestEvent questEvent = new QuestEvent(this.EventType);
		questEvent.Chance = this.Chance;
		questEvent.IsServerOnly = this.IsServerOnly;
		if (this.Actions != null)
		{
			for (int i = 0; i < this.Actions.Count; i++)
			{
				BaseQuestAction baseQuestAction = this.Actions[i].Clone();
				baseQuestAction.Properties = new DynamicProperties();
				baseQuestAction.Owner = this.Owner;
				if (this.Actions[i].Properties != null)
				{
					baseQuestAction.Properties.CopyFrom(this.Actions[i].Properties, null);
				}
				questEvent.Actions.Add(baseQuestAction);
			}
		}
		return questEvent;
	}

	// Token: 0x04003865 RID: 14437
	public string EventType;

	// Token: 0x04003866 RID: 14438
	public float Chance = 1f;

	// Token: 0x04003867 RID: 14439
	public List<BaseQuestAction> Actions = new List<BaseQuestAction>();

	// Token: 0x04003868 RID: 14440
	public bool IsServerOnly;

	// Token: 0x04003869 RID: 14441
	public static string PropChance = "chance";

	// Token: 0x0400386A RID: 14442
	public static string PropServerOnly = "server_only";

	// Token: 0x0400386B RID: 14443
	public QuestClass Owner;

	// Token: 0x0400386C RID: 14444
	public DynamicProperties Properties;
}
