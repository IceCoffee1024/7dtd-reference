using System;
using Twitch;
using UnityEngine.Scripting;

// Token: 0x02000832 RID: 2098
[Preserve]
public class NetPackagePlayerTwitchStats : NetPackageEntityTargeted
{
	// Token: 0x06003D51 RID: 15697 RVA: 0x001863C4 File Offset: 0x001845C4
	public NetPackagePlayerTwitchStats Setup(EntityAlive _entity)
	{
		base.Setup(_entity.entityId);
		EntityPlayer entityPlayer = _entity as EntityPlayer;
		if (entityPlayer)
		{
			this.twitchEnabled = entityPlayer.TwitchEnabled;
			this.twitchSafe = entityPlayer.TwitchSafe;
			this.twitchVoteLock = entityPlayer.TwitchVoteLock;
			this.twitchVisionDisabled = entityPlayer.TwitchVisionDisabled;
			this.twitchActionsEnabled = entityPlayer.TwitchActionsEnabled;
		}
		return this;
	}

	// Token: 0x06003D52 RID: 15698 RVA: 0x0018642C File Offset: 0x0018462C
	public override void read(PooledBinaryReader _reader)
	{
		base.read(_reader);
		this.twitchEnabled = _reader.ReadBoolean();
		this.twitchSafe = _reader.ReadBoolean();
		this.twitchVoteLock = (TwitchVoteLockTypes)_reader.ReadByte();
		this.twitchVisionDisabled = _reader.ReadBoolean();
		this.twitchActionsEnabled = (EntityPlayer.TwitchActionsStates)_reader.ReadByte();
	}

	// Token: 0x06003D53 RID: 15699 RVA: 0x0018647C File Offset: 0x0018467C
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.twitchEnabled);
		_writer.Write(this.twitchSafe);
		_writer.Write((byte)this.twitchVoteLock);
		_writer.Write(this.twitchVisionDisabled);
		_writer.Write((byte)this.twitchActionsEnabled);
	}

	// Token: 0x06003D54 RID: 15700 RVA: 0x001864D0 File Offset: 0x001846D0
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		EntityPlayer entityPlayer = _world.GetEntity(this.entityId) as EntityPlayer;
		if (entityPlayer)
		{
			entityPlayer.TwitchEnabled = this.twitchEnabled;
			entityPlayer.TwitchSafe = this.twitchSafe;
			entityPlayer.TwitchVoteLock = this.twitchVoteLock;
			entityPlayer.TwitchVisionDisabled = this.twitchVisionDisabled;
			entityPlayer.TwitchActionsEnabled = this.twitchActionsEnabled;
		}
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackagePlayerTwitchStats>().Setup(entityPlayer), false, -1, base.Sender.entityId, -1, null, 192, false);
		}
	}

	// Token: 0x040031C5 RID: 12741
	[PublicizedFrom(EAccessModifier.Private)]
	public bool twitchEnabled;

	// Token: 0x040031C6 RID: 12742
	[PublicizedFrom(EAccessModifier.Private)]
	public bool twitchSafe;

	// Token: 0x040031C7 RID: 12743
	[PublicizedFrom(EAccessModifier.Private)]
	public TwitchVoteLockTypes twitchVoteLock;

	// Token: 0x040031C8 RID: 12744
	[PublicizedFrom(EAccessModifier.Private)]
	public bool twitchVisionDisabled;

	// Token: 0x040031C9 RID: 12745
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayer.TwitchActionsStates twitchActionsEnabled;
}
