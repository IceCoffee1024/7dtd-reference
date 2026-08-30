using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x02001843 RID: 6211
	public class TwitchVoteScheduler
	{
		// Token: 0x1700172F RID: 5935
		// (get) Token: 0x0600BF72 RID: 49010 RVA: 0x00471348 File Offset: 0x0046F548
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

		// Token: 0x17001730 RID: 5936
		// (get) Token: 0x0600BF73 RID: 49011 RVA: 0x00471360 File Offset: 0x0046F560
		public static bool HasInstance
		{
			get
			{
				return TwitchVoteScheduler.instance != null;
			}
		}

		// Token: 0x0600BF74 RID: 49012 RVA: 0x0047136A File Offset: 0x0046F56A
		[PublicizedFrom(EAccessModifier.Private)]
		public TwitchVoteScheduler()
		{
		}

		// Token: 0x0600BF75 RID: 49013 RVA: 0x0047137D File Offset: 0x0046F57D
		public void Cleanup()
		{
			this.ClearParticipants();
			TwitchVoteScheduler.instance = null;
		}

		// Token: 0x0600BF76 RID: 49014 RVA: 0x0047138B File Offset: 0x0046F58B
		public void AddParticipant(int entityID)
		{
			if (!this.votingParticipants.Contains(entityID))
			{
				this.votingParticipants.Add(entityID);
			}
		}

		// Token: 0x0600BF77 RID: 49015 RVA: 0x004713A7 File Offset: 0x0046F5A7
		public void ClearParticipants()
		{
			this.votingParticipants.Clear();
		}

		// Token: 0x0600BF78 RID: 49016 RVA: 0x000027FC File Offset: 0x000009FC
		public void Init()
		{
		}

		// Token: 0x0600BF79 RID: 49017 RVA: 0x004713B4 File Offset: 0x0046F5B4
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

		// Token: 0x040090B2 RID: 37042
		[PublicizedFrom(EAccessModifier.Private)]
		public static TwitchVoteScheduler instance;

		// Token: 0x040090B3 RID: 37043
		[PublicizedFrom(EAccessModifier.Private)]
		public List<int> votingParticipants = new List<int>();

		// Token: 0x040090B4 RID: 37044
		[PublicizedFrom(EAccessModifier.Private)]
		public float nextVoteTime;
	}
}
