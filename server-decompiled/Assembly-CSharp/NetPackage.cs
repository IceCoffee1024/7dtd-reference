using System;
using System.Threading;
using UnityEngine.Scripting;

// Token: 0x02000793 RID: 1939
[Preserve]
public abstract class NetPackage
{
	// Token: 0x170005EB RID: 1515
	// (get) Token: 0x0600399E RID: 14750 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual int Channel
	{
		get
		{
			return 0;
		}
	}

	// Token: 0x170005EC RID: 1516
	// (get) Token: 0x0600399F RID: 14751 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool Compress
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170005ED RID: 1517
	// (get) Token: 0x060039A0 RID: 14752 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool FlushQueue
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170005EE RID: 1518
	// (get) Token: 0x060039A1 RID: 14753 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.Both;
		}
	}

	// Token: 0x170005EF RID: 1519
	// (get) Token: 0x060039A2 RID: 14754 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool AllowedBeforeAuth
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170005F0 RID: 1520
	// (get) Token: 0x060039A3 RID: 14755 RVA: 0x00178855 File Offset: 0x00176A55
	public int PackageId
	{
		get
		{
			return NetPackageManager.GetPackageId(base.GetType());
		}
	}

	// Token: 0x170005F1 RID: 1521
	// (get) Token: 0x060039A4 RID: 14756 RVA: 0x00178862 File Offset: 0x00176A62
	// (set) Token: 0x060039A5 RID: 14757 RVA: 0x0017886A File Offset: 0x00176A6A
	public ClientInfo Sender { get; set; }

	// Token: 0x170005F2 RID: 1522
	// (get) Token: 0x060039A6 RID: 14758 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public virtual bool ReliableDelivery
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060039A7 RID: 14759
	public abstract void read(PooledBinaryReader _reader);

	// Token: 0x060039A8 RID: 14760 RVA: 0x00178873 File Offset: 0x00176A73
	public virtual void write(PooledBinaryWriter _writer)
	{
		_writer.Write((ushort)this.PackageId);
	}

	// Token: 0x060039A9 RID: 14761 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public virtual bool ShouldProcess(World _world, GameManager _callbacks)
	{
		return true;
	}

	// Token: 0x060039AA RID: 14762
	public abstract void ProcessPackage(World _world, GameManager _callbacks);

	// Token: 0x060039AB RID: 14763 RVA: 0x00178882 File Offset: 0x00176A82
	public virtual void HandleSkipped(World _world, GameManager _callbacks)
	{
		NetPackageManager.FreePackage(this);
	}

	// Token: 0x060039AC RID: 14764
	public abstract int GetLength();

	// Token: 0x060039AD RID: 14765 RVA: 0x0017888C File Offset: 0x00176A8C
	public override string ToString()
	{
		string text;
		if ((text = this.classnameCached) == null)
		{
			text = (this.classnameCached = base.GetType().Name);
		}
		return text;
	}

	// Token: 0x060039AE RID: 14766 RVA: 0x001788B7 File Offset: 0x00176AB7
	public void RegisterSendQueue()
	{
		Interlocked.Increment(ref this.inSendQueuesCount);
	}

	// Token: 0x060039AF RID: 14767 RVA: 0x001788C5 File Offset: 0x00176AC5
	public void SendQueueHandled()
	{
		if (Interlocked.Decrement(ref this.inSendQueuesCount) == 0)
		{
			NetPackageManager.FreePackage(this);
		}
	}

	// Token: 0x060039B0 RID: 14768 RVA: 0x001788DC File Offset: 0x00176ADC
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool ValidEntityIdForSender(int _entityId, bool _allowAttachedToEntity = false)
	{
		if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			return true;
		}
		if (_entityId == this.Sender.entityId)
		{
			return true;
		}
		if (_allowAttachedToEntity)
		{
			EntityPlayer entityPlayer = GameManager.Instance.World.GetEntity(this.Sender.entityId) as EntityPlayer;
			if (entityPlayer != null && entityPlayer.AttachedToEntity != null && entityPlayer.AttachedToEntity.entityId == _entityId)
			{
				return true;
			}
		}
		Log.Warning(string.Format("Received {0} with invalid entityId {1} from {2}", this.ToString(), _entityId, this.Sender));
		return false;
	}

	// Token: 0x060039B1 RID: 14769 RVA: 0x00178974 File Offset: 0x00176B74
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool ValidUserIdForSender(PlatformUserIdentifierAbs _userId)
	{
		if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			return true;
		}
		if (object.Equals(_userId, this.Sender.PlatformId) || object.Equals(_userId, this.Sender.CrossplatformId))
		{
			return true;
		}
		Log.Warning(string.Format("Received {0} with invalid userId {1} from {2}", this.ToString(), _userId, this.Sender));
		return false;
	}

	// Token: 0x060039B2 RID: 14770 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Protected)]
	public NetPackage()
	{
	}

	// Token: 0x04002F2A RID: 12074
	[PublicizedFrom(EAccessModifier.Private)]
	public int inSendQueuesCount;

	// Token: 0x04002F2B RID: 12075
	[PublicizedFrom(EAccessModifier.Private)]
	public string classnameCached;
}
