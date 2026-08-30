using System;
using UnityEngine.Scripting;

// Token: 0x02000803 RID: 2051
[Preserve]
public class NetPackageLockResponse : NetPackage
{
	// Token: 0x17000649 RID: 1609
	// (get) Token: 0x06003C60 RID: 15456 RVA: 0x0002EEAC File Offset: 0x0002D0AC
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003C61 RID: 15457 RVA: 0x00181100 File Offset: 0x0017F300
	public unsafe NetPackageLockResponse Setup(bool _success, string _errorMsg, ReadOnlySpan<ILockTarget> _targets, ILockContext _context, ushort _channel = 0)
	{
		if (_targets == null || _targets.Length == 0)
		{
			Log.Error("[NetPackageLockResponse] No lock targets supplied.");
			return null;
		}
		this.locking = true;
		this.success = _success;
		this.errorMsg = _errorMsg ?? string.Empty;
		this.targets = new ILockTarget[_targets.Length];
		for (int i = 0; i < _targets.Length; i++)
		{
			this.targets[i] = *_targets[i];
		}
		this.context = _context;
		this.channel = _channel;
		return this;
	}

	// Token: 0x06003C62 RID: 15458 RVA: 0x00181194 File Offset: 0x0017F394
	public NetPackageLockResponse Setup(bool _success, string _errorMsg, bool _isForceUnlocked)
	{
		this.locking = false;
		this.success = _success;
		this.errorMsg = _errorMsg ?? string.Empty;
		this.isForceUnlocked = _isForceUnlocked;
		this.targets = null;
		this.context = null;
		this.channel = 0;
		return this;
	}

	// Token: 0x06003C63 RID: 15459 RVA: 0x001811D4 File Offset: 0x0017F3D4
	public override void read(PooledBinaryReader _br)
	{
		this.locking = _br.ReadBoolean();
		this.success = _br.ReadBoolean();
		this.errorMsg = _br.ReadString();
		this.isForceUnlocked = _br.ReadBoolean();
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
			Log.Error("[NetPackageLockResponse] Unknown context type: " + text);
			this.context = null;
			return;
		}
		this.context = (ILockContext)Activator.CreateInstance(type);
		this.context.Read(_br);
	}

	// Token: 0x06003C64 RID: 15460 RVA: 0x001812AC File Offset: 0x0017F4AC
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.locking);
		_bw.Write(this.success);
		_bw.Write(this.errorMsg);
		_bw.Write(this.isForceUnlocked);
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

	// Token: 0x06003C65 RID: 15461 RVA: 0x00181370 File Offset: 0x0017F570
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (!this.locking)
		{
			LockManager.Instance.UnlockResponse(this.success, this.errorMsg, this.isForceUnlocked);
			return;
		}
		LockManager.Instance.LockResponse(this.success, this.errorMsg, this.targets.AsSpan<ILockTarget>(), this.context, this.channel);
	}

	// Token: 0x06003C66 RID: 15462 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLength()
	{
		return 0;
	}

	// Token: 0x040030CB RID: 12491
	[PublicizedFrom(EAccessModifier.Private)]
	public bool locking;

	// Token: 0x040030CC RID: 12492
	[PublicizedFrom(EAccessModifier.Private)]
	public bool success;

	// Token: 0x040030CD RID: 12493
	[PublicizedFrom(EAccessModifier.Private)]
	public string errorMsg = string.Empty;

	// Token: 0x040030CE RID: 12494
	[PublicizedFrom(EAccessModifier.Private)]
	public ILockTarget[] targets;

	// Token: 0x040030CF RID: 12495
	[PublicizedFrom(EAccessModifier.Private)]
	public ILockContext context;

	// Token: 0x040030D0 RID: 12496
	[PublicizedFrom(EAccessModifier.Private)]
	public ushort channel;

	// Token: 0x040030D1 RID: 12497
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isForceUnlocked;
}
