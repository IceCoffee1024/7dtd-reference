using System;
using Twitch;
using UnityEngine.Scripting;

// Token: 0x0200083C RID: 2108
[Preserve]
public class NetPackagePlayerTwitchStats : NetPackageEntityTargeted
{
	// Token: 0x06003D74 RID: 15732 RVA: 0x001874C8 File Offset: 0x001856C8
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

	// Token: 0x06003D75 RID: 15733 RVA: 0x00187530 File Offset: 0x00185730
	public override void read(PooledBinaryReader _reader)
	{
		base.read(_reader);
		this.twitchEnabled = _reader.ReadBoolean();
		this.twitchSafe = _reader.ReadBoolean();
		this.twitchVoteLock = (TwitchVoteLockTypes)_reader.ReadByte();
		this.twitchVisionDisabled = _reader.ReadBoolean();
		this.twitchActionsEnabled = (EntityPlayer.TwitchActionsStates)_reader.ReadByte();
	}

	// Token: 0x06003D76 RID: 15734 RVA: 0x00187580 File Offset: 0x00185780
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.twitchEnabled);
		_writer.Write(this.twitchSafe);
		_writer.Write((byte)this.twitchVoteLock);
		_writer.Write(this.twitchVisionDisabled);
		_writer.Write((byte)this.twitchActionsEnabled);
	}

	// Token: 0x06003D77 RID: 15735 RVA: 0x001875D4 File Offset: 0x001857D4
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

	// Token: 0x040031E5 RID: 12773
	[PublicizedFrom(EAccessModifier.Private)]
	public bool twitchEnabled;

	// Token: 0x040031E6 RID: 12774
	[PublicizedFrom(EAccessModifier.Private)]
	public bool twitchSafe;

	// Token: 0x040031E7 RID: 12775
	[PublicizedFrom(EAccessModifier.Private)]
	public TwitchVoteLockTypes twitchVoteLock;

	// Token: 0x040031E8 RID: 12776
	[PublicizedFrom(EAccessModifier.Private)]
	public bool twitchVisionDisabled;

	// Token: 0x040031E9 RID: 12777
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayer.TwitchActionsStates twitchActionsEnabled;
}
