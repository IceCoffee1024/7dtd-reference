using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000854 RID: 2132
[Preserve]
public class NetPackageSetProp : NetPackage
{
	// Token: 0x06003DEC RID: 15852 RVA: 0x0018A02F File Offset: 0x0018822F
	public NetPackageSetProp Setup(PersistentPlayerData persistentPlayerData, List<PropChangeInfo> propChanges, int localPlayerThatChanged)
	{
		this.m_persistentPlayerId = ((persistentPlayerData != null) ? persistentPlayerData.PrimaryId : null);
		this.m_propChanges = propChanges;
		this.m_localPlayerThatChanged = localPlayerThatChanged;
		return this;
	}

	// Token: 0x06003DED RID: 15853 RVA: 0x0018A054 File Offset: 0x00188254
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

	// Token: 0x06003DEE RID: 15854 RVA: 0x0018A0AC File Offset: 0x001882AC
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

	// Token: 0x06003DEF RID: 15855 RVA: 0x0018A10C File Offset: 0x0018830C
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

	// Token: 0x0400326E RID: 12910
	[PublicizedFrom(EAccessModifier.Private)]
	public List<PropChangeInfo> m_propChanges;

	// Token: 0x0400326F RID: 12911
	[PublicizedFrom(EAccessModifier.Private)]
	public PlatformUserIdentifierAbs m_persistentPlayerId;

	// Token: 0x04003270 RID: 12912
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_localPlayerThatChanged;
}
