using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x020007D7 RID: 2007
[Preserve]
public class NetPackageEAC : NetPackage
{
	// Token: 0x17000650 RID: 1616
	// (get) Token: 0x06003B6C RID: 15212 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool AllowedBeforeAuth
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06003B6D RID: 15213 RVA: 0x0017F931 File Offset: 0x0017DB31
	public NetPackageEAC Setup(int _size, byte[] _data)
	{
		this.data = new byte[_size];
		Array.Copy(_data, this.data, _size);
		return this;
	}

	// Token: 0x06003B6E RID: 15214 RVA: 0x0017F950 File Offset: 0x0017DB50
	public override void read(PooledBinaryReader _reader)
	{
		int num = _reader.ReadInt32();
		this.data = new byte[num];
		for (int i = 0; i < this.data.Length; i++)
		{
			this.data[i] = _reader.ReadByte();
		}
	}

	// Token: 0x06003B6F RID: 15215 RVA: 0x0017F994 File Offset: 0x0017DB94
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.data.Length);
		for (int i = 0; i < this.data.Length; i++)
		{
			_writer.Write(this.data[i]);
		}
	}

	// Token: 0x06003B70 RID: 15216 RVA: 0x0017F9D8 File Offset: 0x0017DBD8
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

	// Token: 0x04003061 RID: 12385
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] data;
}
