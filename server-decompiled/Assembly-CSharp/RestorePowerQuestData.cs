using System;
using UnityEngine.Scripting;

// Token: 0x02000978 RID: 2424
[Preserve]
public class RestorePowerQuestData : BaseQuestData
{
	// Token: 0x17000776 RID: 1910
	// (get) Token: 0x060046DF RID: 18143 RVA: 0x001BEFE9 File Offset: 0x001BD1E9
	// (set) Token: 0x060046E0 RID: 18144 RVA: 0x001BEFF1 File Offset: 0x001BD1F1
	public Vector3i PrefabPosition
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x060046E1 RID: 18145 RVA: 0x001BEFFA File Offset: 0x001BD1FA
	public RestorePowerQuestData(int _questCode, int _entityID, Vector3i _position, string _completeEvent)
	{
		this.CompleteEvent = _completeEvent;
		this.questCode = _questCode;
		this.entityList.Add(_entityID);
		this.PrefabPosition = _position;
	}

	// Token: 0x060046E2 RID: 18146 RVA: 0x001BF02F File Offset: 0x001BD22F
	public void UpdatePosition(Vector3i _pos)
	{
		this.PrefabPosition = _pos;
	}

	// Token: 0x060046E3 RID: 18147 RVA: 0x001BF038 File Offset: 0x001BD238
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void RemoveFromDictionary()
	{
		QuestEventManager.Current.BlockActivateQuestDictionary.Remove(this.questCode);
	}

	// Token: 0x060046E4 RID: 18148 RVA: 0x001BF050 File Offset: 0x001BD250
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnRemove(EntityPlayer player)
	{
		if (this.CompleteEvent != "")
		{
			GameEventManager.Current.HandleAction(this.CompleteEvent, null, player, false, this.PrefabPosition, "", "", false, true, "", null, null);
		}
	}

	// Token: 0x040038F2 RID: 14578
	public string CompleteEvent = "";
}
