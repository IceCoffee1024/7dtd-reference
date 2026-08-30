using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200097A RID: 2426
[Preserve]
public class QuestEvent
{
	// Token: 0x060046F1 RID: 18161 RVA: 0x001BF202 File Offset: 0x001BD402
	public QuestEvent(string type)
	{
		this.EventType = type;
	}

	// Token: 0x060046F2 RID: 18162 RVA: 0x001BF228 File Offset: 0x001BD428
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

	// Token: 0x060046F3 RID: 18163 RVA: 0x001BF28E File Offset: 0x001BD48E
	public virtual void ParseProperties(DynamicProperties properties)
	{
		this.Properties = properties;
		this.Owner.HandleVariablesForProperties(properties);
		properties.ParseFloat(QuestEvent.PropChance, ref this.Chance);
		properties.ParseBool(QuestEvent.PropServerOnly, ref this.IsServerOnly);
	}

	// Token: 0x060046F4 RID: 18164 RVA: 0x001BF2C8 File Offset: 0x001BD4C8
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

	// Token: 0x040038F6 RID: 14582
	public string EventType;

	// Token: 0x040038F7 RID: 14583
	public float Chance = 1f;

	// Token: 0x040038F8 RID: 14584
	public List<BaseQuestAction> Actions = new List<BaseQuestAction>();

	// Token: 0x040038F9 RID: 14585
	public bool IsServerOnly;

	// Token: 0x040038FA RID: 14586
	public static string PropChance = "chance";

	// Token: 0x040038FB RID: 14587
	public static string PropServerOnly = "server_only";

	// Token: 0x040038FC RID: 14588
	public QuestClass Owner;

	// Token: 0x040038FD RID: 14589
	public DynamicProperties Properties;
}
