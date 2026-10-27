using System;
using Twitch;
using UnityEngine.Scripting;

// Token: 0x0200086B RID: 2155
[Preserve]
public class NetPackageTwitchVoteScheduling : NetPackage
{
	// Token: 0x06003E63 RID: 15971 RVA: 0x0012B879 File Offset: 0x00129A79
	public NetPackageTwitchVoteScheduling Setup()
	{
		return this;
	}

	// Token: 0x06003E64 RID: 15972 RVA: 0x000027FC File Offset: 0x000009FC
	public override void read(PooledBinaryReader _br)
	{
	}

	// Token: 0x06003E65 RID: 15973 RVA: 0x0017D267 File Offset: 0x0017B467
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
	}

	// Token: 0x06003E66 RID: 15974 RVA: 0x0018C0FC File Offset: 0x0018A2FC
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			TwitchVoteScheduler.Current.AddParticipant(base.Sender.entityId);
			return;
		}
		TwitchManager.Current.VotingManager.RequestApprovedToStart();
	}
}
