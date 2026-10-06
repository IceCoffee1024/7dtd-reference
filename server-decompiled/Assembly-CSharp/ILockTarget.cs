using System;

// Token: 0x02000587 RID: 1415
public interface ILockTarget : IHasWorldAddress
{
	// Token: 0x06002E93 RID: 11923 RVA: 0x00010E62 File Offset: 0x0000F062
	bool IsSharedLock(ushort _channel)
	{
		return false;
	}

	// Token: 0x06002E94 RID: 11924 RVA: 0x000027FC File Offset: 0x000009FC
	void OnLockRequestLocal(PooledBinaryWriter _bwLocalContext, ushort _channel)
	{
	}

	// Token: 0x06002E95 RID: 11925 RVA: 0x00020119 File Offset: 0x0001E319
	bool OnLockRequestServer(int _lockingPlayerID, PooledBinaryReader _brLocalContext, ushort _channel)
	{
		return true;
	}

	// Token: 0x06002E96 RID: 11926 RVA: 0x000027FC File Offset: 0x000009FC
	void OnLockResponseServer(int _lockingPlayerID, PooledBinaryWriter _bwServerContext, ushort _channel)
	{
	}

	// Token: 0x06002E97 RID: 11927 RVA: 0x00045DB4 File Offset: 0x00043FB4
	bool OnLockResponseLocal(bool _success, PooledBinaryReader _brServerContext, ushort _channel)
	{
		return LocalPlayerUI.GetUIForPrimaryPlayer() != null;
	}

	// Token: 0x06002E98 RID: 11928 RVA: 0x000027FC File Offset: 0x000009FC
	void OnUnlockedServer(int _unlockingPlayerId, ushort _channel)
	{
	}
}
