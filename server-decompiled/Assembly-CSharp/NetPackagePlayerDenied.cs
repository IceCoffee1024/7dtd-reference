using System;
using UnityEngine.Scripting;

// Token: 0x02000819 RID: 2073
[Preserve]
public class NetPackagePlayerDenied : NetPackage
{
	// Token: 0x17000656 RID: 1622
	// (get) Token: 0x06003CD9 RID: 15577 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool FlushQueue
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000657 RID: 1623
	// (get) Token: 0x06003CDA RID: 15578 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool AllowedBeforeAuth
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06003CDB RID: 15579 RVA: 0x00183373 File Offset: 0x00181573
	public NetPackagePlayerDenied Setup(GameUtils.KickPlayerData _kickData)
	{
		this.kickData = _kickData;
		return this;
	}

	// Token: 0x06003CDC RID: 15580 RVA: 0x00183380 File Offset: 0x00181580
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

	// Token: 0x06003CDD RID: 15581 RVA: 0x001833FC File Offset: 0x001815FC
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write((int)this.kickData.reason);
		_writer.Write(this.kickData.apiResponseEnum);
		_writer.Write(this.kickData.banUntil.ToBinary());
		_writer.Write(this.kickData.customReason);
	}

	// Token: 0x06003CDE RID: 15582 RVA: 0x00183459 File Offset: 0x00181659
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (!this.processed)
		{
			SingletonMonoBehaviour<ConnectionManager>.Instance.Disconnect();
			_callbacks.ShowMessagePlayerDenied(this.kickData);
			this.processed = true;
		}
	}

	// Token: 0x06003CDF RID: 15583 RVA: 0x000B8CB1 File Offset: 0x000B6EB1
	public override int GetLength()
	{
		return 20;
	}

	// Token: 0x17000658 RID: 1624
	// (get) Token: 0x06003CE0 RID: 15584 RVA: 0x0002EEAC File Offset: 0x0002D0AC
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x0400313B RID: 12603
	public GameUtils.KickPlayerData kickData;

	// Token: 0x0400313C RID: 12604
	[PublicizedFrom(EAccessModifier.Private)]
	public bool processed;
}
