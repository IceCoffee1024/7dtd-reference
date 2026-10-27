using System;
using System.Threading;
using UnityEngine.Scripting;

// Token: 0x020007A8 RID: 1960
[Preserve]
public abstract class NetPackage
{
	// Token: 0x1700061E RID: 1566
	// (get) Token: 0x06003A50 RID: 14928 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual int Channel
	{
		get
		{
			return 0;
		}
	}

	// Token: 0x1700061F RID: 1567
	// (get) Token: 0x06003A51 RID: 14929 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool Compress
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000620 RID: 1568
	// (get) Token: 0x06003A52 RID: 14930 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool FlushQueue
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000621 RID: 1569
	// (get) Token: 0x06003A53 RID: 14931 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.Both;
		}
	}

	// Token: 0x17000622 RID: 1570
	// (get) Token: 0x06003A54 RID: 14932 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool AllowedBeforeAuth
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000623 RID: 1571
	// (get) Token: 0x06003A55 RID: 14933 RVA: 0x0017C225 File Offset: 0x0017A425
	public int PackageId
	{
		get
		{
			return NetPackageManager.GetPackageId(base.GetType());
		}
	}

	// Token: 0x17000624 RID: 1572
	// (get) Token: 0x06003A56 RID: 14934 RVA: 0x0017C232 File Offset: 0x0017A432
	// (set) Token: 0x06003A57 RID: 14935 RVA: 0x0017C23A File Offset: 0x0017A43A
	public ClientInfo Sender { get; set; }

	// Token: 0x17000625 RID: 1573
	// (get) Token: 0x06003A58 RID: 14936 RVA: 0x00020119 File Offset: 0x0001E319
	public virtual bool ReliableDelivery
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06003A59 RID: 14937
	public abstract void read(PooledBinaryReader _reader);

	// Token: 0x06003A5A RID: 14938 RVA: 0x0017C243 File Offset: 0x0017A443
	public virtual void write(PooledBinaryWriter _writer)
	{
		_writer.Write((ushort)this.PackageId);
	}

	// Token: 0x06003A5B RID: 14939 RVA: 0x00020119 File Offset: 0x0001E319
	public virtual bool ShouldProcess(World _world, GameManager _callbacks)
	{
		return true;
	}

	// Token: 0x06003A5C RID: 14940
	public abstract void ProcessPackage(World _world, GameManager _callbacks);

	// Token: 0x06003A5D RID: 14941 RVA: 0x0017C252 File Offset: 0x0017A452
	public virtual void HandleSkipped(World _world, GameManager _callbacks)
	{
		NetPackageManager.FreePackage(this);
	}

	// Token: 0x06003A5E RID: 14942 RVA: 0x0017C25C File Offset: 0x0017A45C
	public override string ToString()
	{
		string text;
		if ((text = this.classnameCached) == null)
		{
			text = (this.classnameCached = base.GetType().Name);
		}
		return text;
	}

	// Token: 0x06003A5F RID: 14943 RVA: 0x0017C287 File Offset: 0x0017A487
	public void RegisterSendQueue()
	{
		Interlocked.Increment(ref this.inSendQueuesCount);
	}

	// Token: 0x06003A60 RID: 14944 RVA: 0x0017C295 File Offset: 0x0017A495
	public void SendQueueHandled()
	{
		if (Interlocked.Decrement(ref this.inSendQueuesCount) == 0)
		{
			NetPackageManager.FreePackage(this);
		}
	}

	// Token: 0x06003A61 RID: 14945 RVA: 0x0017C2AC File Offset: 0x0017A4AC
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

	// Token: 0x06003A62 RID: 14946 RVA: 0x0017C344 File Offset: 0x0017A544
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

	// Token: 0x06003A63 RID: 14947 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Protected)]
	public NetPackage()
	{
	}

	// Token: 0x04002FB9 RID: 12217
	[PublicizedFrom(EAccessModifier.Private)]
	public int inSendQueuesCount;

	// Token: 0x04002FBA RID: 12218
	[PublicizedFrom(EAccessModifier.Private)]
	public string classnameCached;
}
