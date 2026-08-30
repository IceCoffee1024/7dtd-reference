using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000F8C RID: 3980
[Preserve]
public class XUiC_PartyEntryList : XUiController
{
	// Token: 0x06007D85 RID: 32133 RVA: 0x002FAD40 File Offset: 0x002F8F40
	public override void Init()
	{
		base.Init();
		XUiC_PartyEntry[] childrenByType = base.GetChildrenByType<XUiC_PartyEntry>(null);
		for (int i = 0; i < childrenByType.Length; i++)
		{
			this.entryList.Add(childrenByType[i]);
		}
	}

	// Token: 0x06007D86 RID: 32134 RVA: 0x002FAD78 File Offset: 0x002F8F78
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

	// Token: 0x06007D87 RID: 32135 RVA: 0x002FAE25 File Offset: 0x002F9025
	[PublicizedFrom(EAccessModifier.Private)]
	public void Party_PartyLeaderChanged(Party _affectedParty, EntityPlayer _player)
	{
		this.RefreshPartyList();
	}

	// Token: 0x06007D88 RID: 32136 RVA: 0x002FAE25 File Offset: 0x002F9025
	[PublicizedFrom(EAccessModifier.Private)]
	public void Party_PartyMemberChanged(EntityPlayer _player)
	{
		this.RefreshPartyList();
	}

	// Token: 0x06007D89 RID: 32137 RVA: 0x002FAE30 File Offset: 0x002F9030
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

	// Token: 0x06007D8A RID: 32138 RVA: 0x002FAEEC File Offset: 0x002F90EC
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

	// Token: 0x06007D8B RID: 32139 RVA: 0x002FAF94 File Offset: 0x002F9194
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

	// Token: 0x04005D00 RID: 23808
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<XUiC_PartyEntry> entryList = new List<XUiC_PartyEntry>();
}
