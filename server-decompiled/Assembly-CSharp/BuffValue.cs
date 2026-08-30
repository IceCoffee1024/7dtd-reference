using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x0200064B RID: 1611
public class BuffValue
{
	// Token: 0x17000543 RID: 1347
	// (get) Token: 0x06003416 RID: 13334 RVA: 0x0015A178 File Offset: 0x00158378
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

	// Token: 0x17000544 RID: 1348
	// (get) Token: 0x06003417 RID: 13335 RVA: 0x0015A1C4 File Offset: 0x001583C4
	// (set) Token: 0x06003418 RID: 13336 RVA: 0x0015A1D1 File Offset: 0x001583D1
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

	// Token: 0x17000545 RID: 1349
	// (get) Token: 0x06003419 RID: 13337 RVA: 0x0015A1F7 File Offset: 0x001583F7
	// (set) Token: 0x0600341A RID: 13338 RVA: 0x0015A204 File Offset: 0x00158404
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

	// Token: 0x17000546 RID: 1350
	// (get) Token: 0x0600341B RID: 13339 RVA: 0x0015A22A File Offset: 0x0015842A
	// (set) Token: 0x0600341C RID: 13340 RVA: 0x0015A237 File Offset: 0x00158437
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

	// Token: 0x17000547 RID: 1351
	// (get) Token: 0x0600341D RID: 13341 RVA: 0x0015A25D File Offset: 0x0015845D
	// (set) Token: 0x0600341E RID: 13342 RVA: 0x0015A26B File Offset: 0x0015846B
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

	// Token: 0x17000548 RID: 1352
	// (get) Token: 0x0600341F RID: 13343 RVA: 0x0015A292 File Offset: 0x00158492
	// (set) Token: 0x06003420 RID: 13344 RVA: 0x0015A29F File Offset: 0x0015849F
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

	// Token: 0x17000549 RID: 1353
	// (get) Token: 0x06003421 RID: 13345 RVA: 0x0015A2C5 File Offset: 0x001584C5
	// (set) Token: 0x06003422 RID: 13346 RVA: 0x0015A2D3 File Offset: 0x001584D3
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

	// Token: 0x1700054A RID: 1354
	// (get) Token: 0x06003423 RID: 13347 RVA: 0x0015A2FA File Offset: 0x001584FA
	// (set) Token: 0x06003424 RID: 13348 RVA: 0x0015A302 File Offset: 0x00158502
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

	// Token: 0x1700054B RID: 1355
	// (get) Token: 0x06003425 RID: 13349 RVA: 0x0015A317 File Offset: 0x00158517
	public float DurationInSeconds
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return this.durationTicks / 20f;
		}
	}

	// Token: 0x1700054C RID: 1356
	// (get) Token: 0x06003426 RID: 13350 RVA: 0x0015A327 File Offset: 0x00158527
	// (set) Token: 0x06003427 RID: 13351 RVA: 0x0015A32F File Offset: 0x0015852F
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

	// Token: 0x06003428 RID: 13352 RVA: 0x0015A340 File Offset: 0x00158540
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

	// Token: 0x06003429 RID: 13353 RVA: 0x0015A388 File Offset: 0x00158588
	public void DurationTriggerUpdate()
	{
		this.updateTicks = 32767;
	}

	// Token: 0x1700054D RID: 1357
	// (get) Token: 0x0600342A RID: 13354 RVA: 0x0015A395 File Offset: 0x00158595
	public string BuffName
	{
		get
		{
			return this.buffName;
		}
	}

	// Token: 0x1700054E RID: 1358
	// (get) Token: 0x0600342B RID: 13355 RVA: 0x0015A39D File Offset: 0x0015859D
	public int InstigatorId
	{
		get
		{
			return this.instigatorId;
		}
	}

	// Token: 0x1700054F RID: 1359
	// (get) Token: 0x0600342C RID: 13356 RVA: 0x0015A3A5 File Offset: 0x001585A5
	public Vector3i InstigatorPos
	{
		get
		{
			return this.instigatorPos;
		}
	}

	// Token: 0x0600342D RID: 13357 RVA: 0x0000640C File Offset: 0x0000460C
	public BuffValue()
	{
	}

	// Token: 0x0600342E RID: 13358 RVA: 0x0015A3B0 File Offset: 0x001585B0
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

	// Token: 0x0600342F RID: 13359 RVA: 0x0015A407 File Offset: 0x00158607
	public void ClearBuffClassLink()
	{
		this.cachedBuff = null;
	}

	// Token: 0x06003430 RID: 13360 RVA: 0x0015A410 File Offset: 0x00158610
	[PublicizedFrom(EAccessModifier.Private)]
	public void cacheBuffClassPointer()
	{
		if (!BuffManager.Buffs.TryGetValue(this.buffName, out this.cachedBuff))
		{
			this.Remove = true;
		}
	}

	// Token: 0x06003431 RID: 13361 RVA: 0x0015A434 File Offset: 0x00158634
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

	// Token: 0x06003432 RID: 13362 RVA: 0x0015A45C File Offset: 0x0015865C
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

	// Token: 0x06003433 RID: 13363 RVA: 0x0015A4C0 File Offset: 0x001586C0
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

	// Token: 0x04002957 RID: 10583
	[PublicizedFrom(EAccessModifier.Private)]
	public BuffClass cachedBuff;

	// Token: 0x04002958 RID: 10584
	[PublicizedFrom(EAccessModifier.Private)]
	public string buffName;

	// Token: 0x04002959 RID: 10585
	[PublicizedFrom(EAccessModifier.Private)]
	public byte stackEffectMultiplier;

	// Token: 0x0400295A RID: 10586
	[PublicizedFrom(EAccessModifier.Private)]
	public uint durationTicks;

	// Token: 0x0400295B RID: 10587
	[PublicizedFrom(EAccessModifier.Private)]
	public int instigatorId;

	// Token: 0x0400295C RID: 10588
	[PublicizedFrom(EAccessModifier.Private)]
	public BuffValue.BuffFlags buffFlags;

	// Token: 0x0400295D RID: 10589
	[PublicizedFrom(EAccessModifier.Private)]
	public ushort updateTicks;

	// Token: 0x0400295E RID: 10590
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i instigatorPos;

	// Token: 0x0200064C RID: 1612
	[PublicizedFrom(EAccessModifier.Private)]
	public enum BuffFlags : byte
	{
		// Token: 0x04002960 RID: 10592
		None,
		// Token: 0x04002961 RID: 10593
		Started,
		// Token: 0x04002962 RID: 10594
		Finished,
		// Token: 0x04002963 RID: 10595
		Remove = 4,
		// Token: 0x04002964 RID: 10596
		Update = 8,
		// Token: 0x04002965 RID: 10597
		Invalid = 16,
		// Token: 0x04002966 RID: 10598
		Paused = 32
	}
}
