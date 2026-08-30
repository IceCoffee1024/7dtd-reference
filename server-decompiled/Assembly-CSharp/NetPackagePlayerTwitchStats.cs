using System;
using Twitch;
using UnityEngine.Scripting;

// Token: 0x02000831 RID: 2097
[Preserve]
public class NetPackagePlayerTwitchStats : NetPackageEntityTargeted
{
	// Token: 0x06003D96 RID: 15766 RVA: 0x0018688C File Offset: 0x00184A8C
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

	// Token: 0x06003D97 RID: 15767 RVA: 0x001868F4 File Offset: 0x00184AF4
	public override void read(PooledBinaryReader _reader)
	{
		base.read(_reader);
		this.twitchEnabled = _reader.ReadBoolean();
		this.twitchSafe = _reader.ReadBoolean();
		this.twitchVoteLock = (TwitchVoteLockTypes)_reader.ReadByte();
		this.twitchVisionDisabled = _reader.ReadBoolean();
		this.twitchActionsEnabled = (EntityPlayer.TwitchActionsStates)_reader.ReadByte();
	}

	// Token: 0x06003D98 RID: 15768 RVA: 0x00186944 File Offset: 0x00184B44
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.twitchEnabled);
		_writer.Write(this.twitchSafe);
		_writer.Write((byte)this.twitchVoteLock);
		_writer.Write(this.twitchVisionDisabled);
		_writer.Write((byte)this.twitchActionsEnabled);
	}

	// Token: 0x06003D99 RID: 15769 RVA: 0x00186998 File Offset: 0x00184B98
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

	// Token: 0x06003D9A RID: 15770 RVA: 0x00177A99 File Offset: 0x00175C99
	public override int GetLength()
	{
		return 60;
	}

	// Token: 0x040031DE RID: 12766
	[PublicizedFrom(EAccessModifier.Private)]
	public bool twitchEnabled;

	// Token: 0x040031DF RID: 12767
	[PublicizedFrom(EAccessModifier.Private)]
	public bool twitchSafe;

	// Token: 0x040031E0 RID: 12768
	[PublicizedFrom(EAccessModifier.Private)]
	public TwitchVoteLockTypes twitchVoteLock;

	// Token: 0x040031E1 RID: 12769
	[PublicizedFrom(EAccessModifier.Private)]
	public bool twitchVisionDisabled;

	// Token: 0x040031E2 RID: 12770
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayer.TwitchActionsStates twitchActionsEnabled;
}
