using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x020007CA RID: 1994
[Preserve]
public class NetPackageEAC : NetPackage
{
	// Token: 0x17000625 RID: 1573
	// (get) Token: 0x06003B28 RID: 15144 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool AllowedBeforeAuth
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06003B29 RID: 15145 RVA: 0x0017E25D File Offset: 0x0017C45D
	public NetPackageEAC Setup(int _size, byte[] _data)
	{
		this.data = new byte[_size];
		Array.Copy(_data, this.data, _size);
		return this;
	}

	// Token: 0x06003B2A RID: 15146 RVA: 0x0017E27C File Offset: 0x0017C47C
	public override void read(PooledBinaryReader _reader)
	{
		int num = _reader.ReadInt32();
		this.data = new byte[num];
		for (int i = 0; i < this.data.Length; i++)
		{
			this.data[i] = _reader.ReadByte();
		}
	}

	// Token: 0x06003B2B RID: 15147 RVA: 0x0017E2C0 File Offset: 0x0017C4C0
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.data.Length);
		for (int i = 0; i < this.data.Length; i++)
		{
			_writer.Write(this.data[i]);
		}
	}

	// Token: 0x06003B2C RID: 15148 RVA: 0x0017E304 File Offset: 0x0017C504
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			IAntiCheatServer antiCheatServer = PlatformManager.MultiPlatform.AntiCheatServer;
			if (antiCheatServer == null)
			{
				return;
			}
			antiCheatServer.HandleMessageFromClient(base.Sender, this.data);
			return;
		}
		else
		{
			IAntiCheatClient antiCheatClient = PlatformManager.MultiPlatform.AntiCheatClient;
			if (antiCheatClient == null)
			{
				return;
			}
			antiCheatClient.HandleMessageFromServer(this.data);
			return;
		}
	}

	// Token: 0x06003B2D RID: 15149 RVA: 0x0017E358 File Offset: 0x0017C558
	public override int GetLength()
	{
		return 4 + ((this.data != null) ? this.data.Length : 0);
	}

	// Token: 0x04003047 RID: 12359
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] data;
}
