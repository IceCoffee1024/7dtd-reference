using System;
using UnityEngine.Scripting;

// Token: 0x0200080D RID: 2061
[Preserve]
public class NetPackageLockRequest : NetPackage
{
	// Token: 0x17000653 RID: 1619
	// (get) Token: 0x06003CB7 RID: 15543 RVA: 0x00020021 File Offset: 0x0001E221
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003CB8 RID: 15544 RVA: 0x00183288 File Offset: 0x00181488
	public unsafe NetPackageLockRequest Setup(ReadOnlySpan<ILockTarget> _targets, ILockContext _context, ushort _channel = 0)
	{
		if (_targets == null || _targets.Length == 0)
		{
			Log.Error("[NetPackageLockRequest] No lock targets supplied.");
			return null;
		}
		this.locking = true;
		this.targets = new ILockTarget[_targets.Length];
		for (int i = 0; i < _targets.Length; i++)
		{
			this.targets[i] = *_targets[i];
		}
		this.context = _context;
		this.channel = _channel;
		return this;
	}

	// Token: 0x06003CB9 RID: 15545 RVA: 0x00183303 File Offset: 0x00181503
	public NetPackageLockRequest Setup()
	{
		this.locking = false;
		this.targets = null;
		this.context = null;
		this.channel = 0;
		return this;
	}

	// Token: 0x06003CBA RID: 15546 RVA: 0x00183324 File Offset: 0x00181524
	public override void read(PooledBinaryReader _br)
	{
		this.locking = _br.ReadBoolean();
		this.channel = _br.ReadUInt16();
		int num = _br.ReadInt32();
		if (num > 0)
		{
			this.targets = new ILockTarget[num];
			for (int i = 0; i < num; i++)
			{
				this.targets[i] = ILockTarget.ReadIdentifyingInfo(_br);
			}
		}
		string text = _br.ReadString();
		if (string.IsNullOrEmpty(text))
		{
			this.context = null;
			return;
		}
		Type type = Type.GetType(text);
		if (type == null)
		{
			Log.Error("[NetPackageLockRequest] Unknown context type: " + text);
			this.context = null;
			return;
		}
		this.context = (ILockContext)Activator.CreateInstance(type);
		this.context.Read(_br);
	}

	// Token: 0x06003CBB RID: 15547 RVA: 0x001833D8 File Offset: 0x001815D8
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.locking);
		_bw.Write(this.channel);
		ILockTarget[] array = this.targets;
		_bw.Write((array != null) ? array.Length : 0);
		if (this.targets != null)
		{
			for (int i = 0; i < this.targets.Length; i++)
			{
				ILockTarget.WriteIdentifyingInfo(this.targets[i], _bw);
			}
		}
		if (this.context == null)
		{
			_bw.Write(string.Empty);
			return;
		}
		_bw.Write(this.context.GetType().FullName);
		this.context.Write(_bw);
	}

	// Token: 0x06003CBC RID: 15548 RVA: 0x00183478 File Offset: 0x00181678
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (this.locking)
		{
			LockManager.Instance.LockRequestServer(this.targets.AsSpan<ILockTarget>(), base.Sender.entityId, this.context, this.channel);
			return;
		}
		LockManager.Instance.UnlockRequestServer(base.Sender.entityId, false);
	}

	// Token: 0x06003CBD RID: 15549 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLength()
	{
		return 0;
	}

	// Token: 0x04003146 RID: 12614
	[PublicizedFrom(EAccessModifier.Private)]
	public bool locking;

	// Token: 0x04003147 RID: 12615
	[PublicizedFrom(EAccessModifier.Private)]
	public ILockTarget[] targets;

	// Token: 0x04003148 RID: 12616
	[PublicizedFrom(EAccessModifier.Private)]
	public ILockContext context;

	// Token: 0x04003149 RID: 12617
	[PublicizedFrom(EAccessModifier.Private)]
	public ushort channel;
}
