using System;
using UnityEngine.Scripting;

// Token: 0x02000967 RID: 2407
[Preserve]
public class RestorePowerQuestData : BaseQuestData
{
	// Token: 0x17000764 RID: 1892
	// (get) Token: 0x06004659 RID: 18009 RVA: 0x001BC7F5 File Offset: 0x001BA9F5
	// (set) Token: 0x0600465A RID: 18010 RVA: 0x001BC7FD File Offset: 0x001BA9FD
	public Vector3i PrefabPosition
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x0600465B RID: 18011 RVA: 0x001BC806 File Offset: 0x001BAA06
	public RestorePowerQuestData(int _questCode, int _entityID, Vector3i _position, string _completeEvent)
	{
		this.CompleteEvent = _completeEvent;
		this.questCode = _questCode;
		this.entityList.Add(_entityID);
		this.PrefabPosition = _position;
	}

	// Token: 0x0600465C RID: 18012 RVA: 0x001BC83B File Offset: 0x001BAA3B
	public void UpdatePosition(Vector3i _pos)
	{
		this.PrefabPosition = _pos;
	}

	// Token: 0x0600465D RID: 18013 RVA: 0x001BC844 File Offset: 0x001BAA44
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void RemoveFromDictionary()
	{
		QuestEventManager.Current.BlockActivateQuestDictionary.Remove(this.questCode);
	}

	// Token: 0x0600465E RID: 18014 RVA: 0x001BC85C File Offset: 0x001BAA5C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnRemove(EntityPlayer player)
	{
		if (this.CompleteEvent != "")
		{
			GameEventManager.Current.HandleAction(this.CompleteEvent, null, player, false, this.PrefabPosition, "", "", false, true, "", null, null);
		}
	}

	// Token: 0x04003861 RID: 14433
	public string CompleteEvent = "";
}
