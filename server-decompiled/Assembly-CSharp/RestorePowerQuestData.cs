using System;
using UnityEngine.Scripting;

// Token: 0x0200097E RID: 2430
[Preserve]
public class RestorePowerQuestData : BaseQuestData
{
	// Token: 0x17000796 RID: 1942
	// (get) Token: 0x06004680 RID: 18048 RVA: 0x001BF4F1 File Offset: 0x001BD6F1
	// (set) Token: 0x06004681 RID: 18049 RVA: 0x001BF4F9 File Offset: 0x001BD6F9
	public Vector3i PrefabPosition
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x06004682 RID: 18050 RVA: 0x001BF502 File Offset: 0x001BD702
	public RestorePowerQuestData(int _questCode, int _entityID, Vector3i _position, string _completeEvent)
	{
		this.CompleteEvent = _completeEvent;
		this.questCode = _questCode;
		this.entityList.Add(_entityID);
		this.PrefabPosition = _position;
	}

	// Token: 0x06004683 RID: 18051 RVA: 0x001BF537 File Offset: 0x001BD737
	public void UpdatePosition(Vector3i _pos)
	{
		this.PrefabPosition = _pos;
	}

	// Token: 0x06004684 RID: 18052 RVA: 0x001BF540 File Offset: 0x001BD740
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void RemoveFromDictionary()
	{
		QuestEventManager.Current.BlockActivateQuestDictionary.Remove(this.questCode);
	}

	// Token: 0x06004685 RID: 18053 RVA: 0x001BF558 File Offset: 0x001BD758
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnRemove(EntityPlayer player)
	{
		if (this.CompleteEvent != "")
		{
			GameEventManager.Current.HandleAction(this.CompleteEvent, null, player, false, this.PrefabPosition, "", "", false, true, "", null, null);
		}
	}

	// Token: 0x040038EE RID: 14574
	public string CompleteEvent = "";
}
