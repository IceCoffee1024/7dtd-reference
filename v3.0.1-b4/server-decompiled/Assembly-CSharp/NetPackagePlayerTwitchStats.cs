using System;
using Twitch;
using UnityEngine.Scripting;

// Token: 0x02000826 RID: 2086
[Preserve]
public class NetPackagePlayerTwitchStats : NetPackageEntityTargeted
{
	// Token: 0x06003D37 RID: 15671 RVA: 0x001844B4 File Offset: 0x001826B4
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

	// Token: 0x06003D38 RID: 15672 RVA: 0x0018451C File Offset: 0x0018271C
	public override void read(PooledBinaryReader _reader)
	{
		base.read(_reader);
		this.twitchEnabled = _reader.ReadBoolean();
		this.twitchSafe = _reader.ReadBoolean();
		this.twitchVoteLock = (TwitchVoteLockTypes)_reader.ReadByte();
		this.twitchVisionDisabled = _reader.ReadBoolean();
		this.twitchActionsEnabled = (EntityPlayer.TwitchActionsStates)_reader.ReadByte();
	}

	// Token: 0x06003D39 RID: 15673 RVA: 0x0018456C File Offset: 0x0018276C
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.twitchEnabled);
		_writer.Write(this.twitchSafe);
		_writer.Write((byte)this.twitchVoteLock);
		_writer.Write(this.twitchVisionDisabled);
		_writer.Write((byte)this.twitchActionsEnabled);
	}

	// Token: 0x06003D3A RID: 15674 RVA: 0x001845C0 File Offset: 0x001827C0
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

	// Token: 0x06003D3B RID: 15675 RVA: 0x00175819 File Offset: 0x00173A19
	public override int GetLength()
	{
		return 60;
	}

	// Token: 0x0400315F RID: 12639
	[PublicizedFrom(EAccessModifier.Private)]
	public bool twitchEnabled;

	// Token: 0x04003160 RID: 12640
	[PublicizedFrom(EAccessModifier.Private)]
	public bool twitchSafe;

	// Token: 0x04003161 RID: 12641
	[PublicizedFrom(EAccessModifier.Private)]
	public TwitchVoteLockTypes twitchVoteLock;

	// Token: 0x04003162 RID: 12642
	[PublicizedFrom(EAccessModifier.Private)]
	public bool twitchVisionDisabled;

	// Token: 0x04003163 RID: 12643
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayer.TwitchActionsStates twitchActionsEnabled;
}
