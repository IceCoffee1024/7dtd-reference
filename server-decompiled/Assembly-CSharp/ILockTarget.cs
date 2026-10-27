using System;

// Token: 0x0200058E RID: 1422
public interface ILockTarget : IHasWorldAddress
{
	// Token: 0x06002EA1 RID: 11937 RVA: 0x00010E62 File Offset: 0x0000F062
	bool IsSharedLock(ushort _channel)
	{
		return false;
	}

	// Token: 0x06002EA2 RID: 11938 RVA: 0x000027FC File Offset: 0x000009FC
	void OnLockRequestLocal(PooledBinaryWriter _bwLocalContext, ushort _channel)
	{
	}

	// Token: 0x06002EA3 RID: 11939 RVA: 0x00020119 File Offset: 0x0001E319
	bool OnLockRequestServer(int _lockingPlayerID, PooledBinaryReader _brLocalContext, ushort _channel)
	{
		return true;
	}

	// Token: 0x06002EA4 RID: 11940 RVA: 0x000027FC File Offset: 0x000009FC
	void OnLockResponseServer(int _lockingPlayerID, PooledBinaryWriter _bwServerContext, ushort _channel)
	{
	}

	// Token: 0x06002EA5 RID: 11941 RVA: 0x00045D74 File Offset: 0x00043F74
	bool OnLockResponseLocal(bool _success, PooledBinaryReader _brServerContext, ushort _channel)
	{
		return LocalPlayerUI.GetUIForPrimaryPlayer() != null;
	}

	// Token: 0x06002EA6 RID: 11942 RVA: 0x000027FC File Offset: 0x000009FC
	void OnUnlockedServer(int _unlockingPlayerId, ushort _channel)
	{
	}
}
