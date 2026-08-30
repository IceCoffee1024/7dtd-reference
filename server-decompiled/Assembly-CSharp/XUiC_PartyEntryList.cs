using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000FA5 RID: 4005
[Preserve]
public class XUiC_PartyEntryList : XUiController
{
	// Token: 0x06007E77 RID: 32375 RVA: 0x002FF514 File Offset: 0x002FD714
	public override void Init()
	{
		base.Init();
		XUiC_PartyEntry[] childrenByType = base.GetChildrenByType<XUiC_PartyEntry>(null);
		for (int i = 0; i < childrenByType.Length; i++)
		{
			this.entryList.Add(childrenByType[i]);
		}
	}

	// Token: 0x06007E78 RID: 32376 RVA: 0x002FF54C File Offset: 0x002FD74C
	public override void OnOpen()
	{
		base.OnOpen();
		this.RefreshPartyList();
		EntityPlayer entityPlayer = this.xui.playerUI.entityPlayer;
		entityPlayer.PartyJoined += this.EntityPlayer_PartyJoined;
		entityPlayer.PartyChanged += this.EntityPlayer_PartyJoined;
		entityPlayer.PartyLeave += this.EntityPlayer_PartyJoined;
		if (entityPlayer.Party != null)
		{
			entityPlayer.Party.PartyMemberAdded += this.Party_PartyMemberChanged;
			entityPlayer.Party.PartyMemberRemoved += this.Party_PartyMemberChanged;
			entityPlayer.Party.PartyLeaderChanged += this.Party_PartyLeaderChanged;
		}
	}

	// Token: 0x06007E79 RID: 32377 RVA: 0x002FF5F9 File Offset: 0x002FD7F9
	[PublicizedFrom(EAccessModifier.Private)]
	public void Party_PartyLeaderChanged(Party _affectedParty, EntityPlayer _player)
	{
		this.RefreshPartyList();
	}

	// Token: 0x06007E7A RID: 32378 RVA: 0x002FF5F9 File Offset: 0x002FD7F9
	[PublicizedFrom(EAccessModifier.Private)]
	public void Party_PartyMemberChanged(EntityPlayer _player)
	{
		this.RefreshPartyList();
	}

	// Token: 0x06007E7B RID: 32379 RVA: 0x002FF604 File Offset: 0x002FD804
	[PublicizedFrom(EAccessModifier.Private)]
	public void EntityPlayer_PartyJoined(Party _affectedParty, EntityPlayer _player)
	{
		this.RefreshPartyList();
		EntityPlayer entityPlayer = this.xui.playerUI.entityPlayer;
		if (entityPlayer.Party != null)
		{
			entityPlayer.Party.PartyMemberAdded -= this.Party_PartyMemberChanged;
			entityPlayer.Party.PartyMemberRemoved -= this.Party_PartyMemberChanged;
			entityPlayer.Party.PartyLeaderChanged -= this.Party_PartyLeaderChanged;
			entityPlayer.Party.PartyMemberAdded += this.Party_PartyMemberChanged;
			entityPlayer.Party.PartyMemberRemoved += this.Party_PartyMemberChanged;
			entityPlayer.Party.PartyLeaderChanged += this.Party_PartyLeaderChanged;
		}
	}

	// Token: 0x06007E7C RID: 32380 RVA: 0x002FF6C0 File Offset: 0x002FD8C0
	public override void OnClose()
	{
		base.OnClose();
		EntityPlayer entityPlayer = this.xui.playerUI.entityPlayer;
		entityPlayer.PartyJoined -= this.EntityPlayer_PartyJoined;
		entityPlayer.PartyChanged -= this.EntityPlayer_PartyJoined;
		entityPlayer.PartyLeave -= this.EntityPlayer_PartyJoined;
		if (entityPlayer.Party != null)
		{
			entityPlayer.Party.PartyMemberAdded -= this.Party_PartyMemberChanged;
			entityPlayer.Party.PartyMemberRemoved -= this.Party_PartyMemberChanged;
			entityPlayer.Party.PartyLeaderChanged -= this.Party_PartyLeaderChanged;
		}
	}

	// Token: 0x06007E7D RID: 32381 RVA: 0x002FF768 File Offset: 0x002FD968
	public void RefreshPartyList()
	{
		EntityPlayer entityPlayer = this.xui.playerUI.entityPlayer;
		int i = 0;
		if (entityPlayer.Party != null)
		{
			for (int j = 0; j < entityPlayer.Party.MemberList.Count; j++)
			{
				EntityPlayer entityPlayer2 = entityPlayer.Party.MemberList[j];
				if (i >= this.entryList.Count)
				{
					IL_0090:
					while (i < this.entryList.Count)
					{
						this.entryList[i].SetPlayer(null);
						i++;
					}
					return;
				}
				if (entityPlayer2 != entityPlayer)
				{
					this.entryList[i++].SetPlayer(entityPlayer2);
				}
			}
			goto IL_0090;
		}
		for (int k = 0; k < this.entryList.Count; k++)
		{
			this.entryList[k].SetPlayer(null);
		}
	}

	// Token: 0x04005DC7 RID: 24007
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<XUiC_PartyEntry> entryList = new List<XUiC_PartyEntry>();
}
