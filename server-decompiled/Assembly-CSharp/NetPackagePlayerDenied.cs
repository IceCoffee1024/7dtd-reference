using System;
using UnityEngine.Scripting;

// Token: 0x0200082C RID: 2092
[Preserve]
public class NetPackagePlayerDenied : NetPackage
{
	// Token: 0x17000686 RID: 1670
	// (get) Token: 0x06003D1A RID: 15642 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool FlushQueue
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000687 RID: 1671
	// (get) Token: 0x06003D1B RID: 15643 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool AllowedBeforeAuth
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06003D1C RID: 15644 RVA: 0x0018641A File Offset: 0x0018461A
	public NetPackagePlayerDenied Setup(GameUtils.KickPlayerData _kickData)
	{
		this.kickData = _kickData;
		return this;
	}

	// Token: 0x06003D1D RID: 15645 RVA: 0x00186424 File Offset: 0x00184624
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

	// Token: 0x06003D1E RID: 15646 RVA: 0x001864A0 File Offset: 0x001846A0
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write((int)this.kickData.reason);
		_writer.Write(this.kickData.apiResponseEnum);
		_writer.Write(this.kickData.banUntil.ToBinary());
		_writer.Write(this.kickData.customReason);
	}

	// Token: 0x06003D1F RID: 15647 RVA: 0x001864FD File Offset: 0x001846FD
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (!this.processed)
		{
			SingletonMonoBehaviour<ConnectionManager>.Instance.Disconnect();
			_callbacks.ShowMessagePlayerDenied(this.kickData);
			this.processed = true;
		}
	}

	// Token: 0x17000688 RID: 1672
	// (get) Token: 0x06003D20 RID: 15648 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x040031BE RID: 12734
	public GameUtils.KickPlayerData kickData;

	// Token: 0x040031BF RID: 12735
	[PublicizedFrom(EAccessModifier.Private)]
	public bool processed;
}
