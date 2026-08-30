using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x02001869 RID: 6249
	public class TwitchVoteScheduler
	{
		// Token: 0x170017A1 RID: 6049
		// (get) Token: 0x0600C0F7 RID: 49399 RVA: 0x004770E4 File Offset: 0x004752E4
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

		// Token: 0x170017A2 RID: 6050
		// (get) Token: 0x0600C0F8 RID: 49400 RVA: 0x004770FC File Offset: 0x004752FC
		public static bool HasInstance
		{
			get
			{
				return TwitchVoteScheduler.instance != null;
			}
		}

		// Token: 0x0600C0F9 RID: 49401 RVA: 0x00477106 File Offset: 0x00475306
		[PublicizedFrom(EAccessModifier.Private)]
		public TwitchVoteScheduler()
		{
		}

		// Token: 0x0600C0FA RID: 49402 RVA: 0x00477119 File Offset: 0x00475319
		public void Cleanup()
		{
			this.ClearParticipants();
			TwitchVoteScheduler.instance = null;
		}

		// Token: 0x0600C0FB RID: 49403 RVA: 0x00477127 File Offset: 0x00475327
		public void AddParticipant(int entityID)
		{
			if (!this.votingParticipants.Contains(entityID))
			{
				this.votingParticipants.Add(entityID);
			}
		}

		// Token: 0x0600C0FC RID: 49404 RVA: 0x00477143 File Offset: 0x00475343
		public void ClearParticipants()
		{
			this.votingParticipants.Clear();
		}

		// Token: 0x0600C0FD RID: 49405 RVA: 0x000027FC File Offset: 0x000009FC
		public void Init()
		{
		}

		// Token: 0x0600C0FE RID: 49406 RVA: 0x00477150 File Offset: 0x00475350
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

		// Token: 0x040091CE RID: 37326
		[PublicizedFrom(EAccessModifier.Private)]
		public static TwitchVoteScheduler instance;

		// Token: 0x040091CF RID: 37327
		[PublicizedFrom(EAccessModifier.Private)]
		public List<int> votingParticipants = new List<int>();

		// Token: 0x040091D0 RID: 37328
		[PublicizedFrom(EAccessModifier.Private)]
		public float nextVoteTime;
	}
}
