using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x02001895 RID: 6293
	public class TwitchVoteScheduler
	{
		// Token: 0x17001843 RID: 6211
		// (get) Token: 0x0600C24F RID: 49743 RVA: 0x0047B620 File Offset: 0x00479820
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

		// Token: 0x17001844 RID: 6212
		// (get) Token: 0x0600C250 RID: 49744 RVA: 0x0047B638 File Offset: 0x00479838
		public static bool HasInstance
		{
			get
			{
				return TwitchVoteScheduler.instance != null;
			}
		}

		// Token: 0x0600C251 RID: 49745 RVA: 0x0047B642 File Offset: 0x00479842
		[PublicizedFrom(EAccessModifier.Private)]
		public TwitchVoteScheduler()
		{
		}

		// Token: 0x0600C252 RID: 49746 RVA: 0x0047B655 File Offset: 0x00479855
		public void Cleanup()
		{
			this.ClearParticipants();
			TwitchVoteScheduler.instance = null;
		}

		// Token: 0x0600C253 RID: 49747 RVA: 0x0047B663 File Offset: 0x00479863
		public void AddParticipant(int entityID)
		{
			if (!this.votingParticipants.Contains(entityID))
			{
				this.votingParticipants.Add(entityID);
			}
		}

		// Token: 0x0600C254 RID: 49748 RVA: 0x0047B67F File Offset: 0x0047987F
		public void ClearParticipants()
		{
			this.votingParticipants.Clear();
		}

		// Token: 0x0600C255 RID: 49749 RVA: 0x000027FC File Offset: 0x000009FC
		public void Init()
		{
		}

		// Token: 0x0600C256 RID: 49750 RVA: 0x0047B68C File Offset: 0x0047988C
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

		// Token: 0x04009294 RID: 37524
		[PublicizedFrom(EAccessModifier.Private)]
		public static TwitchVoteScheduler instance;

		// Token: 0x04009295 RID: 37525
		[PublicizedFrom(EAccessModifier.Private)]
		public List<int> votingParticipants = new List<int>();

		// Token: 0x04009296 RID: 37526
		[PublicizedFrom(EAccessModifier.Private)]
		public float nextVoteTime;
	}
}
