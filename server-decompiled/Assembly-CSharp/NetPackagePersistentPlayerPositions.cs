using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020008FD RID: 2301
[Preserve]
public class NetPackagePersistentPlayerPositions : NetPackage
{
	// Token: 0x17000716 RID: 1814
	// (get) Token: 0x060041FF RID: 16895 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x17000717 RID: 1815
	// (get) Token: 0x06004200 RID: 16896 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool FlushQueue
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06004201 RID: 16897 RVA: 0x0019EBD4 File Offset: 0x0019CDD4
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

	// Token: 0x06004202 RID: 16898 RVA: 0x0019EC50 File Offset: 0x0019CE50
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

	// Token: 0x06004203 RID: 16899 RVA: 0x0019EC90 File Offset: 0x0019CE90
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

	// Token: 0x06004204 RID: 16900 RVA: 0x0019ED14 File Offset: 0x0019CF14
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

	// Token: 0x040035C9 RID: 13769
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<PlatformUserIdentifierAbs, Vector3i> positions = new Dictionary<PlatformUserIdentifierAbs, Vector3i>();
}
