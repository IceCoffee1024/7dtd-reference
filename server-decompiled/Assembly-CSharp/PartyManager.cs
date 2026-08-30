using System;
using System.Collections.Generic;

// Token: 0x020008CD RID: 2253
public class PartyManager
{
	// Token: 0x170006CE RID: 1742
	// (get) Token: 0x0600414A RID: 16714 RVA: 0x00197DE4 File Offset: 0x00195FE4
	public static PartyManager Current
	{
		get
		{
			PartyManager partyManager;
			if ((partyManager = PartyManager.instance) == null)
			{
				partyManager = (PartyManager.instance = new PartyManager());
			}
			return partyManager;
		}
	}

	// Token: 0x170006CF RID: 1743
	// (get) Token: 0x0600414B RID: 16715 RVA: 0x00197DFA File Offset: 0x00195FFA
	public static bool HasInstance
	{
		get
		{
			return PartyManager.instance != null;
		}
	}

	// Token: 0x0600414C RID: 16716 RVA: 0x00197E04 File Offset: 0x00196004
	[PublicizedFrom(EAccessModifier.Private)]
	public PartyManager()
	{
		this.voice = PartyVoice.Instance;
	}

	// Token: 0x0600414D RID: 16717 RVA: 0x00197E24 File Offset: 0x00196024
	public Party CreateParty()
	{
		if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			return null;
		}
		Party party = new Party();
		int num = this.nextPartyID + 1;
		this.nextPartyID = num;
		party.PartyID = num;
		Party party2 = party;
		this.partyList.Add(party2);
		return party2;
	}

	// Token: 0x0600414E RID: 16718 RVA: 0x00197E6C File Offset: 0x0019606C
	public Party CreateClientParty(World world, int partyID, int leaderIndex, int[] partyMembers, string voiceLobbyId)
	{
		Party party = new Party
		{
			PartyID = partyID,
			LeaderIndex = leaderIndex,
			VoiceLobbyId = voiceLobbyId
		};
		this.partyList.Add(party);
		party.UpdateMemberList(world, partyMembers);
		return party;
	}

	// Token: 0x0600414F RID: 16719 RVA: 0x00197EAB File Offset: 0x001960AB
	public void RemoveParty(Party party)
	{
		if (this.partyList.Contains(party))
		{
			this.partyList.Remove(party);
		}
	}

	// Token: 0x06004150 RID: 16720 RVA: 0x00197EC8 File Offset: 0x001960C8
	public Party GetParty(int partyID)
	{
		for (int i = 0; i < this.partyList.Count; i++)
		{
			if (this.partyList[i].PartyID == partyID)
			{
				return this.partyList[i];
			}
		}
		return null;
	}

	// Token: 0x06004151 RID: 16721 RVA: 0x00197F0D File Offset: 0x0019610D
	public void Cleanup()
	{
		this.partyList.Clear();
		this.nextPartyID = 0;
	}

	// Token: 0x06004152 RID: 16722 RVA: 0x00197F21 File Offset: 0x00196121
	public void Update()
	{
		this.voice.Update();
	}

	// Token: 0x040034F2 RID: 13554
	[PublicizedFrom(EAccessModifier.Private)]
	public static PartyManager instance;

	// Token: 0x040034F3 RID: 13555
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly PartyVoice voice;

	// Token: 0x040034F4 RID: 13556
	[PublicizedFrom(EAccessModifier.Private)]
	public int nextPartyID;

	// Token: 0x040034F5 RID: 13557
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<Party> partyList = new List<Party>();
}
