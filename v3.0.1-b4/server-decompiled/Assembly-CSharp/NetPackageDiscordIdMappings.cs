using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000334 RID: 820
[Preserve]
public class NetPackageDiscordIdMappings : NetPackage
{
	// Token: 0x060017CD RID: 6093 RVA: 0x00088A7A File Offset: 0x00086C7A
	public NetPackageDiscordIdMappings Setup(int _entityId, bool _remove, ulong _discordId)
	{
		this.entityId = _entityId;
		this.remove = _remove;
		this.discordId = _discordId;
		List<int> list = this.entityIds;
		if (list != null)
		{
			list.Clear();
		}
		List<ulong> list2 = this.discordIds;
		if (list2 != null)
		{
			list2.Clear();
		}
		return this;
	}

	// Token: 0x060017CE RID: 6094 RVA: 0x00088AB4 File Offset: 0x00086CB4
	public NetPackageDiscordIdMappings Setup(List<int> _entityIds, List<ulong> _discordIds)
	{
		this.entityId = 0;
		this.remove = false;
		this.discordId = 0UL;
		this.entityIds = _entityIds;
		this.discordIds = _discordIds;
		return this;
	}

	// Token: 0x060017CF RID: 6095 RVA: 0x00088ADC File Offset: 0x00086CDC
	public override void read(PooledBinaryReader _reader)
	{
		List<int> list = this.entityIds;
		if (list != null)
		{
			list.Clear();
		}
		List<ulong> list2 = this.discordIds;
		if (list2 != null)
		{
			list2.Clear();
		}
		if (_reader.ReadBoolean())
		{
			this.entityId = _reader.ReadInt32();
			this.remove = _reader.ReadBoolean();
			this.discordId = _reader.ReadUInt64();
			return;
		}
		this.entityId = 0;
		this.remove = false;
		this.discordId = 0UL;
		int num = _reader.ReadInt32();
		if (this.entityIds == null)
		{
			this.entityIds = new List<int>(num);
		}
		if (this.discordIds == null)
		{
			this.discordIds = new List<ulong>(num);
		}
		for (int i = 0; i < num; i++)
		{
			this.entityIds.Add(_reader.ReadInt32());
			this.discordIds.Add(_reader.ReadUInt64());
		}
	}

	// Token: 0x060017D0 RID: 6096 RVA: 0x00088BAC File Offset: 0x00086DAC
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		bool flag = this.entityId > 0;
		_writer.Write(flag);
		if (flag)
		{
			_writer.Write(this.entityId);
			_writer.Write(this.remove);
			_writer.Write(this.discordId);
			return;
		}
		int count = this.entityIds.Count;
		_writer.Write(count);
		for (int i = 0; i < count; i++)
		{
			_writer.Write(this.entityIds[i]);
			_writer.Write(this.discordIds[i]);
		}
	}

	// Token: 0x060017D1 RID: 6097 RVA: 0x00088C3C File Offset: 0x00086E3C
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (this.entityId > 0)
		{
			if (!base.ValidEntityIdForSender(this.entityId, false))
			{
				return;
			}
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				base.Sender.DiscordUserId = this.discordId;
			}
			DiscordManager.Instance.UserMappingReceived(this.entityId, this.remove, this.discordId, false);
			return;
		}
		else
		{
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				Log.Error("[Discord] Received User ID mapping package on server with multiple entries");
				return;
			}
			if (this.entityIds == null || this.discordIds == null || this.entityIds.Count != this.discordIds.Count)
			{
				Log.Error("[Discord] Received invalid User ID mapping package");
				return;
			}
			DiscordManager.Instance.UserMappingsReceived(this.entityIds, this.discordIds);
			return;
		}
	}

	// Token: 0x060017D2 RID: 6098 RVA: 0x00088CFD File Offset: 0x00086EFD
	public override int GetLength()
	{
		return 2 + ((this.entityId > 0) ? 12 : (4 + this.entityIds.Count * 12));
	}

	// Token: 0x04000F66 RID: 3942
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityId;

	// Token: 0x04000F67 RID: 3943
	[PublicizedFrom(EAccessModifier.Private)]
	public bool remove;

	// Token: 0x04000F68 RID: 3944
	[PublicizedFrom(EAccessModifier.Private)]
	public ulong discordId;

	// Token: 0x04000F69 RID: 3945
	[PublicizedFrom(EAccessModifier.Private)]
	public List<int> entityIds;

	// Token: 0x04000F6A RID: 3946
	[PublicizedFrom(EAccessModifier.Private)]
	public List<ulong> discordIds;
}
