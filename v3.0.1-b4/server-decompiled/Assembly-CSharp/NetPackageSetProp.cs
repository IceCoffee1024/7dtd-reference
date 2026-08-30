using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200083D RID: 2109
[Preserve]
public class NetPackageSetProp : NetPackage
{
	// Token: 0x06003DBB RID: 15803 RVA: 0x0018743F File Offset: 0x0018563F
	public NetPackageSetProp Setup(PersistentPlayerData persistentPlayerData, List<PropChangeInfo> propChanges, int localPlayerThatChanged)
	{
		this.m_persistentPlayerId = ((persistentPlayerData != null) ? persistentPlayerData.PrimaryId : null);
		this.m_propChanges = propChanges;
		this.m_localPlayerThatChanged = localPlayerThatChanged;
		return this;
	}

	// Token: 0x06003DBC RID: 15804 RVA: 0x00187464 File Offset: 0x00185664
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

	// Token: 0x06003DBD RID: 15805 RVA: 0x001874BC File Offset: 0x001856BC
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

	// Token: 0x06003DBE RID: 15806 RVA: 0x0018751C File Offset: 0x0018571C
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

	// Token: 0x06003DBF RID: 15807 RVA: 0x00187582 File Offset: 0x00185782
	public override int GetLength()
	{
		return this.m_propChanges.Count * 16;
	}

	// Token: 0x040031E8 RID: 12776
	[PublicizedFrom(EAccessModifier.Private)]
	public List<PropChangeInfo> m_propChanges;

	// Token: 0x040031E9 RID: 12777
	[PublicizedFrom(EAccessModifier.Private)]
	public PlatformUserIdentifierAbs m_persistentPlayerId;

	// Token: 0x040031EA RID: 12778
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_localPlayerThatChanged;
}
