using System;
using Twitch;
using UnityEngine.Scripting;

// Token: 0x02000852 RID: 2130
[Preserve]
public class NetPackageTwitchVoteScheduling : NetPackage
{
	// Token: 0x06003E3A RID: 15930 RVA: 0x001290F5 File Offset: 0x001272F5
	public NetPackageTwitchVoteScheduling Setup()
	{
		return this;
	}

	// Token: 0x06003E3B RID: 15931 RVA: 0x000027FC File Offset: 0x000009FC
	public override void read(PooledBinaryReader _br)
	{
	}

	// Token: 0x06003E3C RID: 15932 RVA: 0x001799B3 File Offset: 0x00177BB3
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
	}

	// Token: 0x06003E3D RID: 15933 RVA: 0x001890E8 File Offset: 0x001872E8
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

	// Token: 0x06003E3E RID: 15934 RVA: 0x00104CB5 File Offset: 0x00102EB5
	public override int GetLength()
	{
		return 30;
	}
}
