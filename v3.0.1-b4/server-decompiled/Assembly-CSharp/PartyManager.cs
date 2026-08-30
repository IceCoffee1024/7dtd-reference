using System;
using System.Collections.Generic;

// Token: 0x020008C1 RID: 2241
public class PartyManager
{
	// Token: 0x170006C2 RID: 1730
	// (get) Token: 0x060040E5 RID: 16613 RVA: 0x00195D0C File Offset: 0x00193F0C
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

	// Token: 0x170006C3 RID: 1731
	// (get) Token: 0x060040E6 RID: 16614 RVA: 0x00195D22 File Offset: 0x00193F22
	public static bool HasInstance
	{
		get
		{
			return PartyManager.instance != null;
		}
	}

	// Token: 0x060040E7 RID: 16615 RVA: 0x00195D2C File Offset: 0x00193F2C
	[PublicizedFrom(EAccessModifier.Private)]
	public PartyManager()
	{
		this.voice = PartyVoice.Instance;
	}

	// Token: 0x060040E8 RID: 16616 RVA: 0x00195D4C File Offset: 0x00193F4C
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

	// Token: 0x060040E9 RID: 16617 RVA: 0x00195D94 File Offset: 0x00193F94
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

	// Token: 0x060040EA RID: 16618 RVA: 0x00195DD3 File Offset: 0x00193FD3
	public void RemoveParty(Party party)
	{
		if (this.partyList.Contains(party))
		{
			this.partyList.Remove(party);
		}
	}

	// Token: 0x060040EB RID: 16619 RVA: 0x00195DF0 File Offset: 0x00193FF0
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

	// Token: 0x060040EC RID: 16620 RVA: 0x00195E35 File Offset: 0x00194035
	public void Cleanup()
	{
		this.partyList.Clear();
		this.nextPartyID = 0;
	}

	// Token: 0x060040ED RID: 16621 RVA: 0x00195E49 File Offset: 0x00194049
	public void Update()
	{
		this.voice.Update();
	}

	// Token: 0x04003471 RID: 13425
	[PublicizedFrom(EAccessModifier.Private)]
	public static PartyManager instance;

	// Token: 0x04003472 RID: 13426
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly PartyVoice voice;

	// Token: 0x04003473 RID: 13427
	[PublicizedFrom(EAccessModifier.Private)]
	public int nextPartyID;

	// Token: 0x04003474 RID: 13428
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<Party> partyList = new List<Party>();
}
