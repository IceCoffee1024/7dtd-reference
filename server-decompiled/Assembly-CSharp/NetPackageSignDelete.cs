using System;
using UnityEngine.Scripting;

// Token: 0x0200085D RID: 2141
[Preserve]
public class NetPackageSignDelete : NetPackage
{
	// Token: 0x06003E1A RID: 15898 RVA: 0x0018ADF2 File Offset: 0x00188FF2
	public NetPackageSignDelete Setup(GlobalSignId _signId)
	{
		this.signId = _signId;
		return this;
	}

	// Token: 0x06003E1B RID: 15899 RVA: 0x0018ADFC File Offset: 0x00188FFC
	public override void read(PooledBinaryReader _br)
	{
		this.signId = GlobalSignId.FromStream(_br);
	}

	// Token: 0x06003E1C RID: 15900 RVA: 0x0018AE0A File Offset: 0x0018900A
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		this.signId.ToStream(_bw);
	}

	// Token: 0x06003E1D RID: 15901 RVA: 0x0018AE20 File Offset: 0x00189020
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

	// Token: 0x0400328C RID: 12940
	[PublicizedFrom(EAccessModifier.Private)]
	public GlobalSignId signId;
}
