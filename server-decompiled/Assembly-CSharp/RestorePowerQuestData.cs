using System;
using UnityEngine.Scripting;

// Token: 0x02000988 RID: 2440
[Preserve]
public class RestorePowerQuestData : BaseQuestData
{
	// Token: 0x1700079C RID: 1948
	// (get) Token: 0x060046A3 RID: 18083 RVA: 0x001C0575 File Offset: 0x001BE775
	// (set) Token: 0x060046A4 RID: 18084 RVA: 0x001C057D File Offset: 0x001BE77D
	public Vector3i PrefabPosition
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x060046A5 RID: 18085 RVA: 0x001C0586 File Offset: 0x001BE786
	public RestorePowerQuestData(int _questCode, int _entityID, Vector3i _position, string _completeEvent)
	{
		this.CompleteEvent = _completeEvent;
		this.questCode = _questCode;
		this.entityList.Add(_entityID);
		this.PrefabPosition = _position;
	}

	// Token: 0x060046A6 RID: 18086 RVA: 0x001C05BB File Offset: 0x001BE7BB
	public void UpdatePosition(Vector3i _pos)
	{
		this.PrefabPosition = _pos;
	}

	// Token: 0x060046A7 RID: 18087 RVA: 0x001C05C4 File Offset: 0x001BE7C4
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void RemoveFromDictionary()
	{
		QuestEventManager.Current.BlockActivateQuestDictionary.Remove(this.questCode);
	}

	// Token: 0x060046A8 RID: 18088 RVA: 0x001C05DC File Offset: 0x001BE7DC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnRemove(EntityPlayer player)
	{
		if (this.CompleteEvent != "")
		{
			GameEventManager.Current.HandleAction(this.CompleteEvent, null, player, false, this.PrefabPosition, "", "", false, true, "", null, null);
		}
	}

	// Token: 0x0400390E RID: 14606
	public string CompleteEvent = "";
}
