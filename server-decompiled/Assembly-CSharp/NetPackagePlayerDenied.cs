using System;
using UnityEngine.Scripting;

// Token: 0x02000824 RID: 2084
[Preserve]
public class NetPackagePlayerDenied : NetPackage
{
	// Token: 0x17000661 RID: 1633
	// (get) Token: 0x06003D38 RID: 15672 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool FlushQueue
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000662 RID: 1634
	// (get) Token: 0x06003D39 RID: 15673 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool AllowedBeforeAuth
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06003D3A RID: 15674 RVA: 0x0018574B File Offset: 0x0018394B
	public NetPackagePlayerDenied Setup(GameUtils.KickPlayerData _kickData)
	{
		this.kickData = _kickData;
		return this;
	}

	// Token: 0x06003D3B RID: 15675 RVA: 0x00185758 File Offset: 0x00183958
	public override void read(PooledBinaryReader _reader)
	{
		this.kickData.reason = (GameUtils.EKickReason)_reader.ReadInt32();
		this.kickData.apiResponseEnum = _reader.ReadInt32();
		this.kickData.banUntil = DateTime.FromBinary(_reader.ReadInt64());
		this.kickData.customReason = _reader.ReadString();
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsClient)
		{
			ThreadManager.AddSingleTaskMainThread("PlayerDenied.ProcessPackage", delegate(object _taskInfo)
			{
				this.ProcessPackage(GameManager.Instance.World, GameManager.Instance);
			}, null);
		}
	}

	// Token: 0x06003D3C RID: 15676 RVA: 0x001857D4 File Offset: 0x001839D4
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write((int)this.kickData.reason);
		_writer.Write(this.kickData.apiResponseEnum);
		_writer.Write(this.kickData.banUntil.ToBinary());
		_writer.Write(this.kickData.customReason);
	}

	// Token: 0x06003D3D RID: 15677 RVA: 0x00185831 File Offset: 0x00183A31
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (!this.processed)
		{
			SingletonMonoBehaviour<ConnectionManager>.Instance.Disconnect();
			_callbacks.ShowMessagePlayerDenied(this.kickData);
			this.processed = true;
		}
	}

	// Token: 0x06003D3E RID: 15678 RVA: 0x000B93F5 File Offset: 0x000B75F5
	public override int GetLength()
	{
		return 20;
	}

	// Token: 0x17000663 RID: 1635
	// (get) Token: 0x06003D3F RID: 15679 RVA: 0x0002F168 File Offset: 0x0002D368
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x040031BA RID: 12730
	public GameUtils.KickPlayerData kickData;

	// Token: 0x040031BB RID: 12731
	[PublicizedFrom(EAccessModifier.Private)]
	public bool processed;
}
