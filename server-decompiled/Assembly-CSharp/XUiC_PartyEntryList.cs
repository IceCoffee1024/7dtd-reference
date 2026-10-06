using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000FB4 RID: 4020
[Preserve]
public class XUiC_PartyEntryList : XUiController
{
	// Token: 0x06007E86 RID: 32390 RVA: 0x003024EC File Offset: 0x003006EC
	public override void Init()
	{
		base.Init();
		XUiC_PartyEntry[] childrenByType = base.GetChildrenByType<XUiC_PartyEntry>(null);
		for (int i = 0; i < childrenByType.Length; i++)
		{
			this.entryList.Add(childrenByType[i]);
		}
	}

	// Token: 0x06007E87 RID: 32391 RVA: 0x00302524 File Offset: 0x00300724
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

	// Token: 0x06007E88 RID: 32392 RVA: 0x003025D1 File Offset: 0x003007D1
	[PublicizedFrom(EAccessModifier.Private)]
	public void Party_PartyLeaderChanged(Party _affectedParty, EntityPlayer _player)
	{
		this.RefreshPartyList();
	}

	// Token: 0x06007E89 RID: 32393 RVA: 0x003025D1 File Offset: 0x003007D1
	[PublicizedFrom(EAccessModifier.Private)]
	public void Party_PartyMemberChanged(EntityPlayer _player)
	{
		this.RefreshPartyList();
	}

	// Token: 0x06007E8A RID: 32394 RVA: 0x003025DC File Offset: 0x003007DC
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

	// Token: 0x06007E8B RID: 32395 RVA: 0x00302698 File Offset: 0x00300898
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

	// Token: 0x06007E8C RID: 32396 RVA: 0x00302740 File Offset: 0x00300940
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

	// Token: 0x04005DF0 RID: 24048
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<XUiC_PartyEntry> entryList = new List<XUiC_PartyEntry>();
}
