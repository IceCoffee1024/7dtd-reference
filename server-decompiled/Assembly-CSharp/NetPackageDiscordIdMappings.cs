using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200033D RID: 829
[Preserve]
public class NetPackageDiscordIdMappings : NetPackage
{
	// Token: 0x060017E3 RID: 6115 RVA: 0x00089602 File Offset: 0x00087802
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

	// Token: 0x060017E4 RID: 6116 RVA: 0x0008963C File Offset: 0x0008783C
	public NetPackageDiscordIdMappings Setup(List<int> _entityIds, List<ulong> _discordIds)
	{
		this.entityId = 0;
		this.remove = false;
		this.discordId = 0UL;
		this.entityIds = _entityIds;
		this.discordIds = _discordIds;
		return this;
	}

	// Token: 0x060017E5 RID: 6117 RVA: 0x00089664 File Offset: 0x00087864
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

	// Token: 0x060017E6 RID: 6118 RVA: 0x00089734 File Offset: 0x00087934
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

	// Token: 0x060017E7 RID: 6119 RVA: 0x000897C4 File Offset: 0x000879C4
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

	// Token: 0x04000F80 RID: 3968
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityId;

	// Token: 0x04000F81 RID: 3969
	[PublicizedFrom(EAccessModifier.Private)]
	public bool remove;

	// Token: 0x04000F82 RID: 3970
	[PublicizedFrom(EAccessModifier.Private)]
	public ulong discordId;

	// Token: 0x04000F83 RID: 3971
	[PublicizedFrom(EAccessModifier.Private)]
	public List<int> entityIds;

	// Token: 0x04000F84 RID: 3972
	[PublicizedFrom(EAccessModifier.Private)]
	public List<ulong> discordIds;
}
