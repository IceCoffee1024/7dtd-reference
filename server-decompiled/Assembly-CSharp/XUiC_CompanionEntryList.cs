using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000F18 RID: 3864
[Preserve]
public class XUiC_CompanionEntryList : XUiController
{
	// Token: 0x0600787B RID: 30843 RVA: 0x002DAB2C File Offset: 0x002D8D2C
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

	// Token: 0x0600787C RID: 30844 RVA: 0x002DAB84 File Offset: 0x002D8D84
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

	// Token: 0x0600787D RID: 30845 RVA: 0x002DAC58 File Offset: 0x002D8E58
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

	// Token: 0x0600787E RID: 30846 RVA: 0x002DAD26 File Offset: 0x002D8F26
	[PublicizedFrom(EAccessModifier.Private)]
	public void Party_Changed(Party _affectedParty, EntityPlayer _player)
	{
		this.RefreshPartyList();
	}

	// Token: 0x0600787F RID: 30847 RVA: 0x002DAD26 File Offset: 0x002D8F26
	[PublicizedFrom(EAccessModifier.Private)]
	public void Party_PartyMemberChanged(EntityPlayer player)
	{
		this.RefreshPartyList();
	}

	// Token: 0x06007880 RID: 30848 RVA: 0x002DAD30 File Offset: 0x002D8F30
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

	// Token: 0x04005966 RID: 22886
	[PublicizedFrom(EAccessModifier.Private)]
	public List<XUiC_CompanionEntry> entryList = new List<XUiC_CompanionEntry>();

	// Token: 0x04005967 RID: 22887
	[PublicizedFrom(EAccessModifier.Private)]
	public float yOffset;
}
