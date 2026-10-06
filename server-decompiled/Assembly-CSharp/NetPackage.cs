using System;
using System.Threading;
using UnityEngine.Scripting;

// Token: 0x020007A0 RID: 1952
[Preserve]
public abstract class NetPackage
{
	// Token: 0x1700061A RID: 1562
	// (get) Token: 0x06003A39 RID: 14905 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual int Channel
	{
		get
		{
			return 0;
		}
	}

	// Token: 0x1700061B RID: 1563
	// (get) Token: 0x06003A3A RID: 14906 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool Compress
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700061C RID: 1564
	// (get) Token: 0x06003A3B RID: 14907 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool FlushQueue
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700061D RID: 1565
	// (get) Token: 0x06003A3C RID: 14908 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.Both;
		}
	}

	// Token: 0x1700061E RID: 1566
	// (get) Token: 0x06003A3D RID: 14909 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool AllowedBeforeAuth
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700061F RID: 1567
	// (get) Token: 0x06003A3E RID: 14910 RVA: 0x0017B28D File Offset: 0x0017948D
	public int PackageId
	{
		get
		{
			return NetPackageManager.GetPackageId(base.GetType());
		}
	}

	// Token: 0x17000620 RID: 1568
	// (get) Token: 0x06003A3F RID: 14911 RVA: 0x0017B29A File Offset: 0x0017949A
	// (set) Token: 0x06003A40 RID: 14912 RVA: 0x0017B2A2 File Offset: 0x001794A2
	public ClientInfo Sender { get; set; }

	// Token: 0x17000621 RID: 1569
	// (get) Token: 0x06003A41 RID: 14913 RVA: 0x00020119 File Offset: 0x0001E319
	public virtual bool ReliableDelivery
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06003A42 RID: 14914
	public abstract void read(PooledBinaryReader _reader);

	// Token: 0x06003A43 RID: 14915 RVA: 0x0017B2AB File Offset: 0x001794AB
	public virtual void write(PooledBinaryWriter _writer)
	{
		_writer.Write((ushort)this.PackageId);
	}

	// Token: 0x06003A44 RID: 14916 RVA: 0x00020119 File Offset: 0x0001E319
	public virtual bool ShouldProcess(World _world, GameManager _callbacks)
	{
		return true;
	}

	// Token: 0x06003A45 RID: 14917
	public abstract void ProcessPackage(World _world, GameManager _callbacks);

	// Token: 0x06003A46 RID: 14918 RVA: 0x0017B2BA File Offset: 0x001794BA
	public virtual void HandleSkipped(World _world, GameManager _callbacks)
	{
		NetPackageManager.FreePackage(this);
	}

	// Token: 0x06003A47 RID: 14919 RVA: 0x0017B2C4 File Offset: 0x001794C4
	public override string ToString()
	{
		string text;
		if ((text = this.classnameCached) == null)
		{
			text = (this.classnameCached = base.GetType().Name);
		}
		return text;
	}

	// Token: 0x06003A48 RID: 14920 RVA: 0x0017B2EF File Offset: 0x001794EF
	public void RegisterSendQueue()
	{
		Interlocked.Increment(ref this.inSendQueuesCount);
	}

	// Token: 0x06003A49 RID: 14921 RVA: 0x0017B2FD File Offset: 0x001794FD
	public void SendQueueHandled()
	{
		if (Interlocked.Decrement(ref this.inSendQueuesCount) == 0)
		{
			NetPackageManager.FreePackage(this);
		}
	}

	// Token: 0x06003A4A RID: 14922 RVA: 0x0017B314 File Offset: 0x00179514
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

	// Token: 0x06003A4B RID: 14923 RVA: 0x0017B3AC File Offset: 0x001795AC
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

	// Token: 0x06003A4C RID: 14924 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Protected)]
	public NetPackage()
	{
	}

	// Token: 0x04002F9D RID: 12189
	[PublicizedFrom(EAccessModifier.Private)]
	public int inSendQueuesCount;

	// Token: 0x04002F9E RID: 12190
	[PublicizedFrom(EAccessModifier.Private)]
	public string classnameCached;
}
