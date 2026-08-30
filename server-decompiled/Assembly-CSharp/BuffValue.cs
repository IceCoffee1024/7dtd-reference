using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000654 RID: 1620
public class BuffValue
{
	// Token: 0x1700054C RID: 1356
	// (get) Token: 0x06003469 RID: 13417 RVA: 0x0015C294 File Offset: 0x0015A494
	public BuffClass BuffClass
	{
		get
		{
			if (this.cachedBuff == null && !BuffManager.Buffs.TryGetValue(this.buffName, out this.cachedBuff))
			{
				Log.Error("Buff Class not found for '{0}'", new object[] { this.buffName });
			}
			return this.cachedBuff;
		}
	}

	// Token: 0x1700054D RID: 1357
	// (get) Token: 0x0600346A RID: 13418 RVA: 0x0015C2E0 File Offset: 0x0015A4E0
	// (set) Token: 0x0600346B RID: 13419 RVA: 0x0015C2ED File Offset: 0x0015A4ED
	public bool Remove
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return (this.buffFlags & BuffValue.BuffFlags.Remove) > BuffValue.BuffFlags.None;
		}
		set
		{
			if (value)
			{
				this.buffFlags |= BuffValue.BuffFlags.Remove;
				return;
			}
			this.buffFlags &= (BuffValue.BuffFlags)251;
		}
	}

	// Token: 0x1700054E RID: 1358
	// (get) Token: 0x0600346C RID: 13420 RVA: 0x0015C313 File Offset: 0x0015A513
	// (set) Token: 0x0600346D RID: 13421 RVA: 0x0015C320 File Offset: 0x0015A520
	public bool Finished
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return (this.buffFlags & BuffValue.BuffFlags.Finished) > BuffValue.BuffFlags.None;
		}
		set
		{
			if (value)
			{
				this.buffFlags |= BuffValue.BuffFlags.Finished;
				return;
			}
			this.buffFlags &= (BuffValue.BuffFlags)253;
		}
	}

	// Token: 0x1700054F RID: 1359
	// (get) Token: 0x0600346E RID: 13422 RVA: 0x0015C346 File Offset: 0x0015A546
	// (set) Token: 0x0600346F RID: 13423 RVA: 0x0015C353 File Offset: 0x0015A553
	public bool Started
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return (this.buffFlags & BuffValue.BuffFlags.Started) > BuffValue.BuffFlags.None;
		}
		set
		{
			if (value)
			{
				this.buffFlags |= BuffValue.BuffFlags.Started;
				return;
			}
			this.buffFlags &= (BuffValue.BuffFlags)254;
		}
	}

	// Token: 0x17000550 RID: 1360
	// (get) Token: 0x06003470 RID: 13424 RVA: 0x0015C379 File Offset: 0x0015A579
	// (set) Token: 0x06003471 RID: 13425 RVA: 0x0015C387 File Offset: 0x0015A587
	public bool Invalid
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return (this.buffFlags & BuffValue.BuffFlags.Invalid) > BuffValue.BuffFlags.None;
		}
		set
		{
			if (value)
			{
				this.buffFlags |= BuffValue.BuffFlags.Invalid;
				return;
			}
			this.buffFlags &= (BuffValue.BuffFlags)239;
		}
	}

	// Token: 0x17000551 RID: 1361
	// (get) Token: 0x06003472 RID: 13426 RVA: 0x0015C3AE File Offset: 0x0015A5AE
	// (set) Token: 0x06003473 RID: 13427 RVA: 0x0015C3BB File Offset: 0x0015A5BB
	public bool Update
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return (this.buffFlags & BuffValue.BuffFlags.Update) > BuffValue.BuffFlags.None;
		}
		set
		{
			if (value)
			{
				this.buffFlags |= BuffValue.BuffFlags.Update;
				return;
			}
			this.buffFlags &= (BuffValue.BuffFlags)247;
		}
	}

	// Token: 0x17000552 RID: 1362
	// (get) Token: 0x06003474 RID: 13428 RVA: 0x0015C3E1 File Offset: 0x0015A5E1
	// (set) Token: 0x06003475 RID: 13429 RVA: 0x0015C3EF File Offset: 0x0015A5EF
	public bool Paused
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return (this.buffFlags & BuffValue.BuffFlags.Paused) > BuffValue.BuffFlags.None;
		}
		set
		{
			if (value)
			{
				this.buffFlags |= BuffValue.BuffFlags.Paused;
				return;
			}
			this.buffFlags &= (BuffValue.BuffFlags)223;
		}
	}

	// Token: 0x17000553 RID: 1363
	// (get) Token: 0x06003476 RID: 13430 RVA: 0x0015C416 File Offset: 0x0015A616
	// (set) Token: 0x06003477 RID: 13431 RVA: 0x0015C41E File Offset: 0x0015A61E
	public int StackEffectMultiplier
	{
		get
		{
			return (int)this.stackEffectMultiplier;
		}
		set
		{
			this.stackEffectMultiplier = (byte)Mathf.Clamp(value, 0, 255);
		}
	}

	// Token: 0x17000554 RID: 1364
	// (get) Token: 0x06003478 RID: 13432 RVA: 0x0015C433 File Offset: 0x0015A633
	public float DurationInSeconds
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return this.durationTicks / 20f;
		}
	}

	// Token: 0x17000555 RID: 1365
	// (get) Token: 0x06003479 RID: 13433 RVA: 0x0015C443 File Offset: 0x0015A643
	// (set) Token: 0x0600347A RID: 13434 RVA: 0x0015C44B File Offset: 0x0015A64B
	public uint DurationInTicks
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return this.durationTicks;
		}
		set
		{
			this.durationTicks = value;
			this.updateTicks = (ushort)value;
		}
	}

	// Token: 0x0600347B RID: 13435 RVA: 0x0015C45C File Offset: 0x0015A65C
	public void DurationTick()
	{
		this.durationTicks += 1U;
		ushort num = this.updateTicks + 1;
		this.updateTicks = num;
		if ((int)num >= this.BuffClass.UpdateRateTicks)
		{
			this.Update = true;
			this.updateTicks = 0;
		}
	}

	// Token: 0x0600347C RID: 13436 RVA: 0x0015C4A4 File Offset: 0x0015A6A4
	public void DurationTriggerUpdate()
	{
		this.updateTicks = 32767;
	}

	// Token: 0x17000556 RID: 1366
	// (get) Token: 0x0600347D RID: 13437 RVA: 0x0015C4B1 File Offset: 0x0015A6B1
	public string BuffName
	{
		get
		{
			return this.buffName;
		}
	}

	// Token: 0x17000557 RID: 1367
	// (get) Token: 0x0600347E RID: 13438 RVA: 0x0015C4B9 File Offset: 0x0015A6B9
	public int InstigatorId
	{
		get
		{
			return this.instigatorId;
		}
	}

	// Token: 0x17000558 RID: 1368
	// (get) Token: 0x0600347F RID: 13439 RVA: 0x0015C4C1 File Offset: 0x0015A6C1
	public Vector3i InstigatorPos
	{
		get
		{
			return this.instigatorPos;
		}
	}

	// Token: 0x06003480 RID: 13440 RVA: 0x0000640C File Offset: 0x0000460C
	public BuffValue()
	{
	}

	// Token: 0x06003481 RID: 13441 RVA: 0x0015C4CC File Offset: 0x0015A6CC
	public BuffValue(string _buffEffectGroupId, Vector3i _instigatorPos, int _instigatorId = -1, BuffClass _buffClass = null)
	{
		this.buffName = _buffEffectGroupId;
		this.stackEffectMultiplier = 1;
		this.durationTicks = 0U;
		this.instigatorId = _instigatorId;
		this.buffFlags = BuffValue.BuffFlags.None;
		this.updateTicks = 0;
		this.instigatorPos = _instigatorPos;
		if (_buffClass == null)
		{
			this.cacheBuffClassPointer();
			return;
		}
		this.cachedBuff = _buffClass;
	}

	// Token: 0x06003482 RID: 13442 RVA: 0x0015C523 File Offset: 0x0015A723
	public void ClearBuffClassLink()
	{
		this.cachedBuff = null;
	}

	// Token: 0x06003483 RID: 13443 RVA: 0x0015C52C File Offset: 0x0015A72C
	[PublicizedFrom(EAccessModifier.Private)]
	public void cacheBuffClassPointer()
	{
		if (!BuffManager.Buffs.TryGetValue(this.buffName, out this.cachedBuff))
		{
			this.Remove = true;
		}
	}

	// Token: 0x06003484 RID: 13444 RVA: 0x0015C550 File Offset: 0x0015A750
	public void Tick()
	{
		BuffClass buffClass = this.BuffClass;
		if (buffClass != null)
		{
			buffClass.Tick(this);
			return;
		}
		this.Remove = true;
	}

	// Token: 0x06003485 RID: 13445 RVA: 0x0015C578 File Offset: 0x0015A778
	public void Write(BinaryWriter _bw)
	{
		_bw.Write(this.buffName);
		_bw.Write(this.stackEffectMultiplier);
		_bw.Write(this.durationTicks);
		_bw.Write(this.instigatorId);
		_bw.Write((byte)this.buffFlags);
		_bw.Write(this.updateTicks);
		StreamUtils.Write(_bw, this.instigatorPos);
	}

	// Token: 0x06003486 RID: 13446 RVA: 0x0015C5DC File Offset: 0x0015A7DC
	public void Read(BinaryReader _br, int _version)
	{
		if (_version < 2)
		{
			int num = _br.ReadInt32();
			using (Dictionary<string, BuffClass>.KeyCollection.Enumerator enumerator = BuffManager.Buffs.Keys.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					string text = enumerator.Current;
					if (text.GetHashCode() == num)
					{
						this.buffName = BuffManager.Buffs[text].Name;
						break;
					}
				}
				goto IL_0070;
			}
		}
		this.buffName = _br.ReadString().ToLower();
		IL_0070:
		this.stackEffectMultiplier = _br.ReadByte();
		this.durationTicks = _br.ReadUInt32();
		this.instigatorId = _br.ReadInt32();
		this.buffFlags = (BuffValue.BuffFlags)_br.ReadByte();
		if (_version == 0)
		{
			this.updateTicks = (ushort)_br.ReadByte();
		}
		else
		{
			this.updateTicks = _br.ReadUInt16();
		}
		if (_version >= 3)
		{
			this.instigatorPos = StreamUtils.ReadVector3i(_br);
		}
		this.cacheBuffClassPointer();
	}

	// Token: 0x040029CA RID: 10698
	[PublicizedFrom(EAccessModifier.Private)]
	public BuffClass cachedBuff;

	// Token: 0x040029CB RID: 10699
	[PublicizedFrom(EAccessModifier.Private)]
	public string buffName;

	// Token: 0x040029CC RID: 10700
	[PublicizedFrom(EAccessModifier.Private)]
	public byte stackEffectMultiplier;

	// Token: 0x040029CD RID: 10701
	[PublicizedFrom(EAccessModifier.Private)]
	public uint durationTicks;

	// Token: 0x040029CE RID: 10702
	[PublicizedFrom(EAccessModifier.Private)]
	public int instigatorId;

	// Token: 0x040029CF RID: 10703
	[PublicizedFrom(EAccessModifier.Private)]
	public BuffValue.BuffFlags buffFlags;

	// Token: 0x040029D0 RID: 10704
	[PublicizedFrom(EAccessModifier.Private)]
	public ushort updateTicks;

	// Token: 0x040029D1 RID: 10705
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i instigatorPos;

	// Token: 0x02000655 RID: 1621
	[PublicizedFrom(EAccessModifier.Private)]
	public enum BuffFlags : byte
	{
		// Token: 0x040029D3 RID: 10707
		None,
		// Token: 0x040029D4 RID: 10708
		Started,
		// Token: 0x040029D5 RID: 10709
		Finished,
		// Token: 0x040029D6 RID: 10710
		Remove = 4,
		// Token: 0x040029D7 RID: 10711
		Update = 8,
		// Token: 0x040029D8 RID: 10712
		Invalid = 16,
		// Token: 0x040029D9 RID: 10713
		Paused = 32
	}
}
