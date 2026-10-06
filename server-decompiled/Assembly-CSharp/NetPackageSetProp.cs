using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200084A RID: 2122
[Preserve]
public class NetPackageSetProp : NetPackage
{
	// Token: 0x06003DC9 RID: 15817 RVA: 0x00188F87 File Offset: 0x00187187
	public NetPackageSetProp Setup(PersistentPlayerData persistentPlayerData, List<PropChangeInfo> propChanges, int localPlayerThatChanged)
	{
		this.m_persistentPlayerId = ((persistentPlayerData != null) ? persistentPlayerData.PrimaryId : null);
		this.m_propChanges = propChanges;
		this.m_localPlayerThatChanged = localPlayerThatChanged;
		return this;
	}

	// Token: 0x06003DCA RID: 15818 RVA: 0x00188FAC File Offset: 0x001871AC
	public override void read(PooledBinaryReader _br)
	{
		this.m_persistentPlayerId = PlatformUserIdentifierAbs.FromStream(_br, false, false);
		int num = (int)_br.ReadInt16();
		this.m_propChanges = new List<PropChangeInfo>();
		for (int i = 0; i < num; i++)
		{
			this.m_propChanges.Add(PropChangeInfo.Read(_br));
		}
		this.m_localPlayerThatChanged = _br.ReadInt32();
	}

	// Token: 0x06003DCB RID: 15819 RVA: 0x00189004 File Offset: 0x00187204
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		this.m_persistentPlayerId.ToStream(_bw, false);
		int count = this.m_propChanges.Count;
		_bw.Write((short)count);
		for (int i = 0; i < count; i++)
		{
			this.m_propChanges[i].Write(_bw);
		}
		_bw.Write(this.m_localPlayerThatChanged);
	}

	// Token: 0x06003DCC RID: 15820 RVA: 0x00189064 File Offset: 0x00187264
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (!base.ValidUserIdForSender(this.m_persistentPlayerId))
		{
			return;
		}
		if (!base.ValidEntityIdForSender(this.m_localPlayerThatChanged, false))
		{
			return;
		}
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			_callbacks.SetPropsOnClients(this.m_localPlayerThatChanged, this);
		}
		if (((_world != null) ? _world.ChunkCache : null) == null)
		{
			return;
		}
		_callbacks.ChangeProps(this.m_persistentPlayerId, this.m_propChanges);
	}

	// Token: 0x0400324E RID: 12878
	[PublicizedFrom(EAccessModifier.Private)]
	public List<PropChangeInfo> m_propChanges;

	// Token: 0x0400324F RID: 12879
	[PublicizedFrom(EAccessModifier.Private)]
	public PlatformUserIdentifierAbs m_persistentPlayerId;

	// Token: 0x04003250 RID: 12880
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_localPlayerThatChanged;
}
