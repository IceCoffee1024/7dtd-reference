using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000FC4 RID: 4036
[Preserve]
public class XUiC_PartyEntryList : XUiController
{
	// Token: 0x06007ED5 RID: 32469 RVA: 0x00304008 File Offset: 0x00302208
	public override void Init()
	{
		base.Init();
		XUiC_PartyEntry[] childrenByType = base.GetChildrenByType<XUiC_PartyEntry>(null);
		for (int i = 0; i < childrenByType.Length; i++)
		{
			this.entryList.Add(childrenByType[i]);
		}
	}

	// Token: 0x06007ED6 RID: 32470 RVA: 0x00304040 File Offset: 0x00302240
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

	// Token: 0x06007ED7 RID: 32471 RVA: 0x003040ED File Offset: 0x003022ED
	[PublicizedFrom(EAccessModifier.Private)]
	public void Party_PartyLeaderChanged(Party _affectedParty, EntityPlayer _player)
	{
		this.RefreshPartyList();
	}

	// Token: 0x06007ED8 RID: 32472 RVA: 0x003040ED File Offset: 0x003022ED
	[PublicizedFrom(EAccessModifier.Private)]
	public void Party_PartyMemberChanged(EntityPlayer _player)
	{
		this.RefreshPartyList();
	}

	// Token: 0x06007ED9 RID: 32473 RVA: 0x003040F8 File Offset: 0x003022F8
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

	// Token: 0x06007EDA RID: 32474 RVA: 0x003041B4 File Offset: 0x003023B4
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

	// Token: 0x06007EDB RID: 32475 RVA: 0x0030425C File Offset: 0x0030245C
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

	// Token: 0x04005E2B RID: 24107
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<XUiC_PartyEntry> entryList = new List<XUiC_PartyEntry>();
}
