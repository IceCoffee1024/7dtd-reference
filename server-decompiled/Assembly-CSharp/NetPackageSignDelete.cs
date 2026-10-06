using System;
using UnityEngine.Scripting;

// Token: 0x02000853 RID: 2131
[Preserve]
public class NetPackageSignDelete : NetPackage
{
	// Token: 0x06003DF7 RID: 15863 RVA: 0x00189D4A File Offset: 0x00187F4A
	public NetPackageSignDelete Setup(GlobalSignId _signId)
	{
		this.signId = _signId;
		return this;
	}

	// Token: 0x06003DF8 RID: 15864 RVA: 0x00189D54 File Offset: 0x00187F54
	public override void read(PooledBinaryReader _br)
	{
		this.signId = GlobalSignId.FromStream(_br);
	}

	// Token: 0x06003DF9 RID: 15865 RVA: 0x00189D62 File Offset: 0x00187F62
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		this.signId.ToStream(_bw);
	}

	// Token: 0x06003DFA RID: 15866 RVA: 0x00189D78 File Offset: 0x00187F78
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (!this.signId.IsValid)
		{
			Log.Warning(string.Format("[NetPackageSignDelete] Received invalid sign id '{0}'; ignoring.", this.signId));
			return;
		}
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			if (this.signId.libraryId == "[D]")
			{
				Log.Warning(string.Format("[NetPackageSignDelete] Client '{0}' ({1}) requested deletion of sign '{2}' from the default library; ignoring.", base.Sender.playerName, base.Sender.InternalId, this.signId));
				return;
			}
			if (SignDataManager.localLibraryIds.Contains(this.signId.libraryId))
			{
				Log.Warning(string.Format("[NetPackageSignDelete] Client '{0}' ({1}) requested deletion of sign '{2}' from a local library; ignoring.", base.Sender.playerName, base.Sender.InternalId, this.signId));
				return;
			}
			if (SignDataManager.IsPlayerLibraryId(this.signId.libraryId))
			{
				string libraryId = this.signId.libraryId;
				PlatformUserIdentifierAbs internalId = base.Sender.InternalId;
				if (libraryId != ((internalId != null) ? internalId.CombinedString : null))
				{
					Log.Warning(string.Format("[NetPackageSignDelete] Client '{0}' ({1}) requested deletion of sign '{2}', which they don't own; ignoring.", base.Sender.playerName, base.Sender.InternalId, this.signId));
					return;
				}
			}
			else if (!PrefabEditModeManager.Instance.IsActive())
			{
				Log.Warning(string.Format("[NetPackageSignDelete] Client '{0}' ({1}) requested deletion of sign '{2}' from prefab library whilst not in prefab editor; ignoring.", base.Sender.playerName, base.Sender.InternalId, this.signId));
				return;
			}
		}
		SignDataManager.Instance.ProcessSignDeleteReceived(this.signId);
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(this, false, -1, base.Sender.entityId, -1, null, 192, false);
		}
	}

	// Token: 0x0400326C RID: 12908
	[PublicizedFrom(EAccessModifier.Private)]
	public GlobalSignId signId;
}
