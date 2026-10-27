using System;
using System.Collections.Generic;

// Token: 0x020008DC RID: 2268
public class PartyManager
{
	// Token: 0x170006F3 RID: 1779
	// (get) Token: 0x06004103 RID: 16643 RVA: 0x00198F90 File Offset: 0x00197190
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

	// Token: 0x170006F4 RID: 1780
	// (get) Token: 0x06004104 RID: 16644 RVA: 0x00198FA6 File Offset: 0x001971A6
	public static bool HasInstance
	{
		get
		{
			return PartyManager.instance != null;
		}
	}

	// Token: 0x06004105 RID: 16645 RVA: 0x00198FB0 File Offset: 0x001971B0
	[PublicizedFrom(EAccessModifier.Private)]
	public PartyManager()
	{
		this.voice = PartyVoice.Instance;
	}

	// Token: 0x06004106 RID: 16646 RVA: 0x00198FD0 File Offset: 0x001971D0
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

	// Token: 0x06004107 RID: 16647 RVA: 0x00199018 File Offset: 0x00197218
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

	// Token: 0x06004108 RID: 16648 RVA: 0x00199057 File Offset: 0x00197257
	public void RemoveParty(Party party)
	{
		if (this.partyList.Contains(party))
		{
			this.partyList.Remove(party);
		}
	}

	// Token: 0x06004109 RID: 16649 RVA: 0x00199074 File Offset: 0x00197274
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

	// Token: 0x0600410A RID: 16650 RVA: 0x001990B9 File Offset: 0x001972B9
	public void Cleanup()
	{
		this.partyList.Clear();
		this.nextPartyID = 0;
	}

	// Token: 0x0600410B RID: 16651 RVA: 0x001990CD File Offset: 0x001972CD
	public void Update()
	{
		this.voice.Update();
	}

	// Token: 0x04003506 RID: 13574
	[PublicizedFrom(EAccessModifier.Private)]
	public static PartyManager instance;

	// Token: 0x04003507 RID: 13575
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly PartyVoice voice;

	// Token: 0x04003508 RID: 13576
	[PublicizedFrom(EAccessModifier.Private)]
	public int nextPartyID;

	// Token: 0x04003509 RID: 13577
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<Party> partyList = new List<Party>();
}
