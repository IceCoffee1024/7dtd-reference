using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020008EE RID: 2286
[Preserve]
public class NetPackagePersistentPlayerPositions : NetPackage
{
	// Token: 0x170006F1 RID: 1777
	// (get) Token: 0x06004248 RID: 16968 RVA: 0x0002F168 File Offset: 0x0002D368
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x170006F2 RID: 1778
	// (get) Token: 0x06004249 RID: 16969 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool FlushQueue
	{
		get
		{
			return true;
		}
	}

	// Token: 0x0600424A RID: 16970 RVA: 0x0019DA18 File Offset: 0x0019BC18
	public NetPackagePersistentPlayerPositions Setup(PersistentPlayerList _ppl)
	{
		foreach (KeyValuePair<PlatformUserIdentifierAbs, PersistentPlayerData> keyValuePair in _ppl.Players)
		{
			PlatformUserIdentifierAbs platformUserIdentifierAbs;
			PersistentPlayerData persistentPlayerData;
			keyValuePair.Deconstruct(out platformUserIdentifierAbs, out persistentPlayerData);
			PlatformUserIdentifierAbs platformUserIdentifierAbs2 = platformUserIdentifierAbs;
			PersistentPlayerData persistentPlayerData2 = persistentPlayerData;
			if (persistentPlayerData2.EntityId != -1)
			{
				persistentPlayerData2.UpdatePositionFromEntity();
				this.positions[platformUserIdentifierAbs2] = persistentPlayerData2.Position;
			}
		}
		return this;
	}

	// Token: 0x0600424B RID: 16971 RVA: 0x0019DA94 File Offset: 0x0019BC94
	public override void read(PooledBinaryReader _reader)
	{
		int num = _reader.ReadInt32();
		for (int i = 0; i < num; i++)
		{
			PlatformUserIdentifierAbs platformUserIdentifierAbs = PlatformUserIdentifierAbs.FromStream(_reader, false, false);
			Vector3i vector3i = StreamUtils.ReadVector3i(_reader);
			this.positions[platformUserIdentifierAbs] = vector3i;
		}
	}

	// Token: 0x0600424C RID: 16972 RVA: 0x0019DAD4 File Offset: 0x0019BCD4
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.positions.Count);
		foreach (KeyValuePair<PlatformUserIdentifierAbs, Vector3i> keyValuePair in this.positions)
		{
			PlatformUserIdentifierAbs platformUserIdentifierAbs;
			Vector3i vector3i;
			keyValuePair.Deconstruct(out platformUserIdentifierAbs, out vector3i);
			PlatformUserIdentifierAbs platformUserIdentifierAbs2 = platformUserIdentifierAbs;
			Vector3i vector3i2 = vector3i;
			platformUserIdentifierAbs2.ToStream(_writer, false);
			StreamUtils.Write(_writer, vector3i2);
		}
	}

	// Token: 0x0600424D RID: 16973 RVA: 0x0019DB58 File Offset: 0x0019BD58
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		PersistentPlayerList persistentPlayers = _callbacks.persistentPlayers;
		if (persistentPlayers == null)
		{
			return;
		}
		foreach (KeyValuePair<PlatformUserIdentifierAbs, Vector3i> keyValuePair in this.positions)
		{
			PlatformUserIdentifierAbs platformUserIdentifierAbs;
			Vector3i vector3i;
			keyValuePair.Deconstruct(out platformUserIdentifierAbs, out vector3i);
			PlatformUserIdentifierAbs platformUserIdentifierAbs2 = platformUserIdentifierAbs;
			Vector3i vector3i2 = vector3i;
			PersistentPlayerData playerData = persistentPlayers.GetPlayerData(platformUserIdentifierAbs2);
			if (playerData != null)
			{
				playerData.Position = vector3i2;
			}
		}
		XUiC_SpawnNearFriendsList.Instance.UpdatePlayers();
	}

	// Token: 0x0600424E RID: 16974 RVA: 0x0019DBE0 File Offset: 0x0019BDE0
	public override int GetLength()
	{
		return this.positions.Count * 44;
	}

	// Token: 0x040035B5 RID: 13749
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<PlatformUserIdentifierAbs, Vector3i> positions = new Dictionary<PlatformUserIdentifierAbs, Vector3i>();
}
