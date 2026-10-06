using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000F0D RID: 3853
[Preserve]
public class XUiC_CompanionEntryList : XUiController
{
	// Token: 0x06007855 RID: 30805 RVA: 0x002D95D4 File Offset: 0x002D77D4
	public override void Init()
	{
		base.Init();
		XUiController[] childrenByType = base.GetChildrenByType<XUiC_CompanionEntry>(null);
		XUiController[] array = childrenByType;
		for (int i = 0; i < array.Length; i++)
		{
			this.entryList.Add((XUiC_CompanionEntry)array[i]);
		}
		this.yOffset = (float)this.viewComponent.Position.y;
	}

	// Token: 0x06007856 RID: 30806 RVA: 0x002D962C File Offset: 0x002D782C
	public override void OnOpen()
	{
		base.OnOpen();
		this.RefreshPartyList();
		EntityPlayer entityPlayer = this.xui.playerUI.entityPlayer;
		CompanionGroup companions = entityPlayer.Companions;
		companions.OnGroupChanged = (OnCompanionGroupChanged)Delegate.Combine(companions.OnGroupChanged, new OnCompanionGroupChanged(this.RefreshPartyList));
		entityPlayer.PartyJoined += this.Party_Changed;
		entityPlayer.PartyChanged += this.Party_Changed;
		entityPlayer.PartyLeave += this.Party_Changed;
		if (entityPlayer.Party != null)
		{
			entityPlayer.Party.PartyMemberAdded += this.Party_PartyMemberChanged;
			entityPlayer.Party.PartyMemberRemoved += this.Party_PartyMemberChanged;
			entityPlayer.Party.PartyLeaderChanged += this.Party_Changed;
		}
	}

	// Token: 0x06007857 RID: 30807 RVA: 0x002D9700 File Offset: 0x002D7900
	public override void OnClose()
	{
		base.OnClose();
		EntityPlayer entityPlayer = this.xui.playerUI.entityPlayer;
		CompanionGroup companions = entityPlayer.Companions;
		companions.OnGroupChanged = (OnCompanionGroupChanged)Delegate.Remove(companions.OnGroupChanged, new OnCompanionGroupChanged(this.RefreshPartyList));
		entityPlayer.PartyJoined -= this.Party_Changed;
		entityPlayer.PartyChanged -= this.Party_Changed;
		entityPlayer.PartyLeave -= this.Party_Changed;
		if (entityPlayer.Party != null)
		{
			entityPlayer.Party.PartyMemberAdded -= this.Party_PartyMemberChanged;
			entityPlayer.Party.PartyMemberRemoved -= this.Party_PartyMemberChanged;
			entityPlayer.Party.PartyLeaderChanged -= this.Party_Changed;
		}
	}

	// Token: 0x06007858 RID: 30808 RVA: 0x002D97CE File Offset: 0x002D79CE
	[PublicizedFrom(EAccessModifier.Private)]
	public void Party_Changed(Party _affectedParty, EntityPlayer _player)
	{
		this.RefreshPartyList();
	}

	// Token: 0x06007859 RID: 30809 RVA: 0x002D97CE File Offset: 0x002D79CE
	[PublicizedFrom(EAccessModifier.Private)]
	public void Party_PartyMemberChanged(EntityPlayer player)
	{
		this.RefreshPartyList();
	}

	// Token: 0x0600785A RID: 30810 RVA: 0x002D97D8 File Offset: 0x002D79D8
	public void RefreshPartyList()
	{
		int i = 0;
		EntityPlayer entityPlayer = this.xui.playerUI.entityPlayer;
		int num = 0;
		if (entityPlayer.Party != null)
		{
			num = (entityPlayer.Party.MemberList.Count - 1) * 40;
		}
		if (entityPlayer.Companions != null)
		{
			num += (entityPlayer.Companions.Count - 1) * 40;
		}
		this.viewComponent.Position = new Vector2i(this.viewComponent.Position.x, (int)this.yOffset - num);
		this.viewComponent.UiTransform.localPosition = new Vector3((float)this.viewComponent.Position.x, (float)this.viewComponent.Position.y);
		if (entityPlayer.Companions != null)
		{
			for (int j = 0; j < entityPlayer.Companions.Count; j++)
			{
				EntityAlive entityAlive = entityPlayer.Companions[j];
				if (i >= this.entryList.Count)
				{
					IL_0117:
					while (i < this.entryList.Count)
					{
						this.entryList[i].SetCompanion(null);
						i++;
					}
					return;
				}
				this.entryList[i++].SetCompanion(entityAlive);
			}
			goto IL_0117;
		}
		for (int k = 0; k < this.entryList.Count; k++)
		{
			this.entryList[k].SetCompanion(null);
		}
	}

	// Token: 0x04005941 RID: 22849
	[PublicizedFrom(EAccessModifier.Private)]
	public List<XUiC_CompanionEntry> entryList = new List<XUiC_CompanionEntry>();

	// Token: 0x04005942 RID: 22850
	[PublicizedFrom(EAccessModifier.Private)]
	public float yOffset;
}
