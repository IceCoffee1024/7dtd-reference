using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020008E2 RID: 2274
[Preserve]
public class NetPackagePersistentPlayerPositions : NetPackage
{
	// Token: 0x170006E5 RID: 1765
	// (get) Token: 0x060041E3 RID: 16867 RVA: 0x0002EEAC File Offset: 0x0002D0AC
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x170006E6 RID: 1766
	// (get) Token: 0x060041E4 RID: 16868 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool FlushQueue
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060041E5 RID: 16869 RVA: 0x0019B940 File Offset: 0x00199B40
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

	// Token: 0x060041E6 RID: 16870 RVA: 0x0019B9BC File Offset: 0x00199BBC
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

	// Token: 0x060041E7 RID: 16871 RVA: 0x0019B9FC File Offset: 0x00199BFC
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

	// Token: 0x060041E8 RID: 16872 RVA: 0x0019BA80 File Offset: 0x00199C80
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

	// Token: 0x060041E9 RID: 16873 RVA: 0x0019BB08 File Offset: 0x00199D08
	public override int GetLength()
	{
		return this.positions.Count * 44;
	}

	// Token: 0x04003534 RID: 13620
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<PlatformUserIdentifierAbs, Vector3i> positions = new Dictionary<PlatformUserIdentifierAbs, Vector3i>();
}
