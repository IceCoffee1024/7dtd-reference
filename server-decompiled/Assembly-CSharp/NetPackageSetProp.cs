using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000849 RID: 2121
[Preserve]
public class NetPackageSetProp : NetPackage
{
	// Token: 0x06003E20 RID: 15904 RVA: 0x0018945F File Offset: 0x0018765F
	public NetPackageSetProp Setup(PersistentPlayerData persistentPlayerData, List<PropChangeInfo> propChanges, int localPlayerThatChanged)
	{
		this.m_persistentPlayerId = ((persistentPlayerData != null) ? persistentPlayerData.PrimaryId : null);
		this.m_propChanges = propChanges;
		this.m_localPlayerThatChanged = localPlayerThatChanged;
		return this;
	}

	// Token: 0x06003E21 RID: 15905 RVA: 0x00189484 File Offset: 0x00187684
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

	// Token: 0x06003E22 RID: 15906 RVA: 0x001894DC File Offset: 0x001876DC
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

	// Token: 0x06003E23 RID: 15907 RVA: 0x0018953C File Offset: 0x0018773C
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

	// Token: 0x06003E24 RID: 15908 RVA: 0x001895A2 File Offset: 0x001877A2
	public override int GetLength()
	{
		return this.m_propChanges.Count * 16;
	}

	// Token: 0x04003267 RID: 12903
	[PublicizedFrom(EAccessModifier.Private)]
	public List<PropChangeInfo> m_propChanges;

	// Token: 0x04003268 RID: 12904
	[PublicizedFrom(EAccessModifier.Private)]
	public PlatformUserIdentifierAbs m_persistentPlayerId;

	// Token: 0x04003269 RID: 12905
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_localPlayerThatChanged;
}
