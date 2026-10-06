using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x020007CD RID: 1997
[Preserve]
public class NetPackageEAC : NetPackage
{
	// Token: 0x1700064A RID: 1610
	// (get) Token: 0x06003B49 RID: 15177 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool AllowedBeforeAuth
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06003B4A RID: 15178 RVA: 0x0017E82D File Offset: 0x0017CA2D
	public NetPackageEAC Setup(int _size, byte[] _data)
	{
		this.data = new byte[_size];
		Array.Copy(_data, this.data, _size);
		return this;
	}

	// Token: 0x06003B4B RID: 15179 RVA: 0x0017E84C File Offset: 0x0017CA4C
	public override void read(PooledBinaryReader _reader)
	{
		int num = _reader.ReadInt32();
		this.data = new byte[num];
		for (int i = 0; i < this.data.Length; i++)
		{
			this.data[i] = _reader.ReadByte();
		}
	}

	// Token: 0x06003B4C RID: 15180 RVA: 0x0017E890 File Offset: 0x0017CA90
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.data.Length);
		for (int i = 0; i < this.data.Length; i++)
		{
			_writer.Write(this.data[i]);
		}
	}

	// Token: 0x06003B4D RID: 15181 RVA: 0x0017E8D4 File Offset: 0x0017CAD4
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

	// Token: 0x04003041 RID: 12353
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] data;
}
