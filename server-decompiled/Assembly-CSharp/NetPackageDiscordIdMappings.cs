using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200033B RID: 827
[Preserve]
public class NetPackageDiscordIdMappings : NetPackage
{
	// Token: 0x060017DF RID: 6111 RVA: 0x000893CE File Offset: 0x000875CE
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

	// Token: 0x060017E0 RID: 6112 RVA: 0x00089408 File Offset: 0x00087608
	public NetPackageDiscordIdMappings Setup(List<int> _entityIds, List<ulong> _discordIds)
	{
		this.entityId = 0;
		this.remove = false;
		this.discordId = 0UL;
		this.entityIds = _entityIds;
		this.discordIds = _discordIds;
		return this;
	}

	// Token: 0x060017E1 RID: 6113 RVA: 0x00089430 File Offset: 0x00087630
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

	// Token: 0x060017E2 RID: 6114 RVA: 0x00089500 File Offset: 0x00087700
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

	// Token: 0x060017E3 RID: 6115 RVA: 0x00089590 File Offset: 0x00087790
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

	// Token: 0x04000F70 RID: 3952
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityId;

	// Token: 0x04000F71 RID: 3953
	[PublicizedFrom(EAccessModifier.Private)]
	public bool remove;

	// Token: 0x04000F72 RID: 3954
	[PublicizedFrom(EAccessModifier.Private)]
	public ulong discordId;

	// Token: 0x04000F73 RID: 3955
	[PublicizedFrom(EAccessModifier.Private)]
	public List<int> entityIds;

	// Token: 0x04000F74 RID: 3956
	[PublicizedFrom(EAccessModifier.Private)]
	public List<ulong> discordIds;
}
