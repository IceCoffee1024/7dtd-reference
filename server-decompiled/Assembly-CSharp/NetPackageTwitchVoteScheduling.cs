using System;
using Twitch;
using UnityEngine.Scripting;

// Token: 0x0200085E RID: 2142
[Preserve]
public class NetPackageTwitchVoteScheduling : NetPackage
{
	// Token: 0x06003E9F RID: 16031 RVA: 0x0012A165 File Offset: 0x00128365
	public NetPackageTwitchVoteScheduling Setup()
	{
		return this;
	}

	// Token: 0x06003EA0 RID: 16032 RVA: 0x000027FC File Offset: 0x000009FC
	public override void read(PooledBinaryReader _br)
	{
	}

	// Token: 0x06003EA1 RID: 16033 RVA: 0x0017BC33 File Offset: 0x00179E33
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
	}

	// Token: 0x06003EA2 RID: 16034 RVA: 0x0018B1B8 File Offset: 0x001893B8
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

	// Token: 0x06003EA3 RID: 16035 RVA: 0x00105CC1 File Offset: 0x00103EC1
	public override int GetLength()
	{
		return 30;
	}
}
