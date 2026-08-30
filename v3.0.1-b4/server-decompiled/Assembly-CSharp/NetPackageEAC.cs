using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x020007BF RID: 1983
[Preserve]
public class NetPackageEAC : NetPackage
{
	// Token: 0x1700061A RID: 1562
	// (get) Token: 0x06003AC9 RID: 15049 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool AllowedBeforeAuth
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06003ACA RID: 15050 RVA: 0x0017BE85 File Offset: 0x0017A085
	public NetPackageEAC Setup(int _size, byte[] _data)
	{
		this.data = new byte[_size];
		Array.Copy(_data, this.data, _size);
		return this;
	}

	// Token: 0x06003ACB RID: 15051 RVA: 0x0017BEA4 File Offset: 0x0017A0A4
	public override void read(PooledBinaryReader _reader)
	{
		int num = _reader.ReadInt32();
		this.data = new byte[num];
		for (int i = 0; i < this.data.Length; i++)
		{
			this.data[i] = _reader.ReadByte();
		}
	}

	// Token: 0x06003ACC RID: 15052 RVA: 0x0017BEE8 File Offset: 0x0017A0E8
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.data.Length);
		for (int i = 0; i < this.data.Length; i++)
		{
			_writer.Write(this.data[i]);
		}
	}

	// Token: 0x06003ACD RID: 15053 RVA: 0x0017BF2C File Offset: 0x0017A12C
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

	// Token: 0x06003ACE RID: 15054 RVA: 0x0017BF80 File Offset: 0x0017A180
	public override int GetLength()
	{
		return 4 + ((this.data != null) ? this.data.Length : 0);
	}

	// Token: 0x04002FC8 RID: 12232
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] data;
}
