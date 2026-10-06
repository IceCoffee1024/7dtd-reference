using System;
using UnityEngine.Scripting;

// Token: 0x02000822 RID: 2082
[Preserve]
public class NetPackagePlayerDenied : NetPackage
{
	// Token: 0x17000680 RID: 1664
	// (get) Token: 0x06003CF7 RID: 15607 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool FlushQueue
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000681 RID: 1665
	// (get) Token: 0x06003CF8 RID: 15608 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool AllowedBeforeAuth
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06003CF9 RID: 15609 RVA: 0x00185316 File Offset: 0x00183516
	public NetPackagePlayerDenied Setup(GameUtils.KickPlayerData _kickData)
	{
		this.kickData = _kickData;
		return this;
	}

	// Token: 0x06003CFA RID: 15610 RVA: 0x00185320 File Offset: 0x00183520
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

	// Token: 0x06003CFB RID: 15611 RVA: 0x0018539C File Offset: 0x0018359C
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write((int)this.kickData.reason);
		_writer.Write(this.kickData.apiResponseEnum);
		_writer.Write(this.kickData.banUntil.ToBinary());
		_writer.Write(this.kickData.customReason);
	}

	// Token: 0x06003CFC RID: 15612 RVA: 0x001853F9 File Offset: 0x001835F9
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (!this.processed)
		{
			SingletonMonoBehaviour<ConnectionManager>.Instance.Disconnect();
			_callbacks.ShowMessagePlayerDenied(this.kickData);
			this.processed = true;
		}
	}

	// Token: 0x17000682 RID: 1666
	// (get) Token: 0x06003CFD RID: 15613 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x0400319E RID: 12702
	public GameUtils.KickPlayerData kickData;

	// Token: 0x0400319F RID: 12703
	[PublicizedFrom(EAccessModifier.Private)]
	public bool processed;
}
