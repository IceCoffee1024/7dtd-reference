using System;
using Twitch;
using UnityEngine.Scripting;

// Token: 0x02000861 RID: 2145
[Preserve]
public class NetPackageTwitchVoteScheduling : NetPackage
{
	// Token: 0x06003E40 RID: 15936 RVA: 0x0012AC85 File Offset: 0x00128E85
	public NetPackageTwitchVoteScheduling Setup()
	{
		return this;
	}

	// Token: 0x06003E41 RID: 15937 RVA: 0x000027FC File Offset: 0x000009FC
	public override void read(PooledBinaryReader _br)
	{
	}

	// Token: 0x06003E42 RID: 15938 RVA: 0x0017C2CF File Offset: 0x0017A4CF
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
	}

	// Token: 0x06003E43 RID: 15939 RVA: 0x0018B054 File Offset: 0x00189254
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
