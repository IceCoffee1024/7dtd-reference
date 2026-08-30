using System;
using System.Threading;
using UnityEngine.Scripting;

// Token: 0x0200079D RID: 1949
[Preserve]
public abstract class NetPackage
{
	// Token: 0x170005F5 RID: 1525
	// (get) Token: 0x060039F6 RID: 14838 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual int Channel
	{
		get
		{
			return 0;
		}
	}

	// Token: 0x170005F6 RID: 1526
	// (get) Token: 0x060039F7 RID: 14839 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool Compress
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170005F7 RID: 1527
	// (get) Token: 0x060039F8 RID: 14840 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool FlushQueue
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170005F8 RID: 1528
	// (get) Token: 0x060039F9 RID: 14841 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.Both;
		}
	}

	// Token: 0x170005F9 RID: 1529
	// (get) Token: 0x060039FA RID: 14842 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool AllowedBeforeAuth
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170005FA RID: 1530
	// (get) Token: 0x060039FB RID: 14843 RVA: 0x0017AAD5 File Offset: 0x00178CD5
	public int PackageId
	{
		get
		{
			return NetPackageManager.GetPackageId(base.GetType());
		}
	}

	// Token: 0x170005FB RID: 1531
	// (get) Token: 0x060039FC RID: 14844 RVA: 0x0017AAE2 File Offset: 0x00178CE2
	// (set) Token: 0x060039FD RID: 14845 RVA: 0x0017AAEA File Offset: 0x00178CEA
	public ClientInfo Sender { get; set; }

	// Token: 0x170005FC RID: 1532
	// (get) Token: 0x060039FE RID: 14846 RVA: 0x00020021 File Offset: 0x0001E221
	public virtual bool ReliableDelivery
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060039FF RID: 14847
	public abstract void read(PooledBinaryReader _reader);

	// Token: 0x06003A00 RID: 14848 RVA: 0x0017AAF3 File Offset: 0x00178CF3
	public virtual void write(PooledBinaryWriter _writer)
	{
		_writer.Write((ushort)this.PackageId);
	}

	// Token: 0x06003A01 RID: 14849 RVA: 0x00020021 File Offset: 0x0001E221
	public virtual bool ShouldProcess(World _world, GameManager _callbacks)
	{
		return true;
	}

	// Token: 0x06003A02 RID: 14850
	public abstract void ProcessPackage(World _world, GameManager _callbacks);

	// Token: 0x06003A03 RID: 14851 RVA: 0x0017AB02 File Offset: 0x00178D02
	public virtual void HandleSkipped(World _world, GameManager _callbacks)
	{
		NetPackageManager.FreePackage(this);
	}

	// Token: 0x06003A04 RID: 14852
	public abstract int GetLength();

	// Token: 0x06003A05 RID: 14853 RVA: 0x0017AB0C File Offset: 0x00178D0C
	public override string ToString()
	{
		string text;
		if ((text = this.classnameCached) == null)
		{
			text = (this.classnameCached = base.GetType().Name);
		}
		return text;
	}

	// Token: 0x06003A06 RID: 14854 RVA: 0x0017AB37 File Offset: 0x00178D37
	public void RegisterSendQueue()
	{
		Interlocked.Increment(ref this.inSendQueuesCount);
	}

	// Token: 0x06003A07 RID: 14855 RVA: 0x0017AB45 File Offset: 0x00178D45
	public void SendQueueHandled()
	{
		if (Interlocked.Decrement(ref this.inSendQueuesCount) == 0)
		{
			NetPackageManager.FreePackage(this);
		}
	}

	// Token: 0x06003A08 RID: 14856 RVA: 0x0017AB5C File Offset: 0x00178D5C
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

	// Token: 0x06003A09 RID: 14857 RVA: 0x0017ABF4 File Offset: 0x00178DF4
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

	// Token: 0x06003A0A RID: 14858 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Protected)]
	public NetPackage()
	{
	}

	// Token: 0x04002FA3 RID: 12195
	[PublicizedFrom(EAccessModifier.Private)]
	public int inSendQueuesCount;

	// Token: 0x04002FA4 RID: 12196
	[PublicizedFrom(EAccessModifier.Private)]
	public string classnameCached;
}
