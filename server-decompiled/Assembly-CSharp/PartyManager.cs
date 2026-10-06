using System;
using System.Collections.Generic;

// Token: 0x020008D2 RID: 2258
public class PartyManager
{
	// Token: 0x170006ED RID: 1773
	// (get) Token: 0x060040E0 RID: 16608 RVA: 0x00197EE8 File Offset: 0x001960E8
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

	// Token: 0x170006EE RID: 1774
	// (get) Token: 0x060040E1 RID: 16609 RVA: 0x00197EFE File Offset: 0x001960FE
	public static bool HasInstance
	{
		get
		{
			return PartyManager.instance != null;
		}
	}

	// Token: 0x060040E2 RID: 16610 RVA: 0x00197F08 File Offset: 0x00196108
	[PublicizedFrom(EAccessModifier.Private)]
	public PartyManager()
	{
		this.voice = PartyVoice.Instance;
	}

	// Token: 0x060040E3 RID: 16611 RVA: 0x00197F28 File Offset: 0x00196128
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

	// Token: 0x060040E4 RID: 16612 RVA: 0x00197F70 File Offset: 0x00196170
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

	// Token: 0x060040E5 RID: 16613 RVA: 0x00197FAF File Offset: 0x001961AF
	public void RemoveParty(Party party)
	{
		if (this.partyList.Contains(party))
		{
			this.partyList.Remove(party);
		}
	}

	// Token: 0x060040E6 RID: 16614 RVA: 0x00197FCC File Offset: 0x001961CC
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

	// Token: 0x060040E7 RID: 16615 RVA: 0x00198011 File Offset: 0x00196211
	public void Cleanup()
	{
		this.partyList.Clear();
		this.nextPartyID = 0;
	}

	// Token: 0x060040E8 RID: 16616 RVA: 0x00198025 File Offset: 0x00196225
	public void Update()
	{
		this.voice.Update();
	}

	// Token: 0x040034E6 RID: 13542
	[PublicizedFrom(EAccessModifier.Private)]
	public static PartyManager instance;

	// Token: 0x040034E7 RID: 13543
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly PartyVoice voice;

	// Token: 0x040034E8 RID: 13544
	[PublicizedFrom(EAccessModifier.Private)]
	public int nextPartyID;

	// Token: 0x040034E9 RID: 13545
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<Party> partyList = new List<Party>();
}
