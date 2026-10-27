using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x020018C6 RID: 6342
	public class TwitchVoteScheduler
	{
		// Token: 0x1700188B RID: 6283
		// (get) Token: 0x0600C363 RID: 50019 RVA: 0x0047EFCC File Offset: 0x0047D1CC
		public static TwitchVoteScheduler Current
		{
			get
			{
				if (TwitchVoteScheduler.instance == null)
				{
					TwitchVoteScheduler.instance = new TwitchVoteScheduler();
				}
				return TwitchVoteScheduler.instance;
			}
		}

		// Token: 0x1700188C RID: 6284
		// (get) Token: 0x0600C364 RID: 50020 RVA: 0x0047EFE4 File Offset: 0x0047D1E4
		public static bool HasInstance
		{
			get
			{
				return TwitchVoteScheduler.instance != null;
			}
		}

		// Token: 0x0600C365 RID: 50021 RVA: 0x0047EFEE File Offset: 0x0047D1EE
		[PublicizedFrom(EAccessModifier.Private)]
		public TwitchVoteScheduler()
		{
		}

		// Token: 0x0600C366 RID: 50022 RVA: 0x0047F001 File Offset: 0x0047D201
		public void Cleanup()
		{
			this.ClearParticipants();
			TwitchVoteScheduler.instance = null;
		}

		// Token: 0x0600C367 RID: 50023 RVA: 0x0047F00F File Offset: 0x0047D20F
		public void AddParticipant(int entityID)
		{
			if (!this.votingParticipants.Contains(entityID))
			{
				this.votingParticipants.Add(entityID);
			}
		}

		// Token: 0x0600C368 RID: 50024 RVA: 0x0047F02B File Offset: 0x0047D22B
		public void ClearParticipants()
		{
			this.votingParticipants.Clear();
		}

		// Token: 0x0600C369 RID: 50025 RVA: 0x000027FC File Offset: 0x000009FC
		public void Init()
		{
		}

		// Token: 0x0600C36A RID: 50026 RVA: 0x0047F038 File Offset: 0x0047D238
		public void Update(float deltaTime)
		{
			if (GameManager.Instance.World == null || GameManager.Instance.World.Players == null || GameManager.Instance.World.Players.Count == 0)
			{
				return;
			}
			if (this.nextVoteTime > 0f)
			{
				this.nextVoteTime -= deltaTime;
			}
			if (this.votingParticipants.Count == 0)
			{
				return;
			}
			if (this.nextVoteTime <= 0f)
			{
				if (GameManager.Instance.World.GetPrimaryPlayerId() == this.votingParticipants[0])
				{
					TwitchManager.Current.VotingManager.RequestApprovedToStart();
				}
				else
				{
					SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageTwitchVoteScheduling>().Setup(), false, this.votingParticipants[0], -1, -1, null, 192, false);
				}
				this.votingParticipants.RemoveAt(0);
				this.nextVoteTime = 3f;
			}
		}

		// Token: 0x0400937D RID: 37757
		[PublicizedFrom(EAccessModifier.Private)]
		public static TwitchVoteScheduler instance;

		// Token: 0x0400937E RID: 37758
		[PublicizedFrom(EAccessModifier.Private)]
		public List<int> votingParticipants = new List<int>();

		// Token: 0x0400937F RID: 37759
		[PublicizedFrom(EAccessModifier.Private)]
		public float nextVoteTime;
	}
}
