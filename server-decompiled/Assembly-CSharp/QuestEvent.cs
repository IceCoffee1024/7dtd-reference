using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000980 RID: 2432
[Preserve]
public class QuestEvent
{
	// Token: 0x06004692 RID: 18066 RVA: 0x001BF70A File Offset: 0x001BD90A
	public QuestEvent(string type)
	{
		this.EventType = type;
	}

	// Token: 0x06004693 RID: 18067 RVA: 0x001BF730 File Offset: 0x001BD930
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

	// Token: 0x06004694 RID: 18068 RVA: 0x001BF796 File Offset: 0x001BD996
	public virtual void ParseProperties(DynamicProperties properties)
	{
		this.Properties = properties;
		this.Owner.HandleVariablesForProperties(properties);
		properties.ParseFloat(QuestEvent.PropChance, ref this.Chance);
		properties.ParseBool(QuestEvent.PropServerOnly, ref this.IsServerOnly);
	}

	// Token: 0x06004695 RID: 18069 RVA: 0x001BF7D0 File Offset: 0x001BD9D0
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

	// Token: 0x040038F2 RID: 14578
	public string EventType;

	// Token: 0x040038F3 RID: 14579
	public float Chance = 1f;

	// Token: 0x040038F4 RID: 14580
	public List<BaseQuestAction> Actions = new List<BaseQuestAction>();

	// Token: 0x040038F5 RID: 14581
	public bool IsServerOnly;

	// Token: 0x040038F6 RID: 14582
	public static string PropChance = "chance";

	// Token: 0x040038F7 RID: 14583
	public static string PropServerOnly = "server_only";

	// Token: 0x040038F8 RID: 14584
	public QuestClass Owner;

	// Token: 0x040038F9 RID: 14585
	public DynamicProperties Properties;
}
