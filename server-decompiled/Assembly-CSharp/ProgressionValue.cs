using System;
using System.IO;
using UnityEngine;

// Token: 0x02000AE7 RID: 2791
public class ProgressionValue
{
	// Token: 0x06005318 RID: 21272 RVA: 0x001FE258 File Offset: 0x001FC458
	public float GetCalculatedLevel(EntityAlive _ea)
	{
		if (this.calculatedFrame == Time.frameCount)
		{
			return this.calculatedLevel;
		}
		ProgressionClass progressionClass = this.ProgressionClass;
		if (progressionClass == null)
		{
			return 0f;
		}
		float num = (float)this.Level;
		PassiveEffects passiveEffects;
		switch (progressionClass.Type)
		{
		case ProgressionType.Attribute:
			passiveEffects = PassiveEffects.AttributeLevel;
			break;
		case ProgressionType.Skill:
			passiveEffects = PassiveEffects.SkillLevel;
			break;
		case ProgressionType.Perk:
			passiveEffects = PassiveEffects.PerkLevel;
			break;
		default:
			passiveEffects = PassiveEffects.None;
			break;
		}
		PassiveEffects passiveEffects2 = passiveEffects;
		if (passiveEffects2 != PassiveEffects.None)
		{
			num = EffectManager.GetValue(passiveEffects2, null, num, _ea, null, progressionClass.NameTag, true, true, true, true, true, 1, true, false);
		}
		num = Mathf.Min(num, (float)ProgressionClass.GetCalculatedMaxLevel(_ea, this));
		num = Mathf.Max(num, (float)progressionClass.MinLevel);
		this.calculatedFrame = Time.frameCount;
		this.calculatedLevel = num;
		return num;
	}

	// Token: 0x06005319 RID: 21273 RVA: 0x0000640C File Offset: 0x0000460C
	public ProgressionValue()
	{
	}

	// Token: 0x0600531A RID: 21274 RVA: 0x001FE30F File Offset: 0x001FC50F
	public ProgressionValue(string _name)
	{
		this.name = _name;
	}

	// Token: 0x17000922 RID: 2338
	// (get) Token: 0x0600531B RID: 21275 RVA: 0x001FE31E File Offset: 0x001FC51E
	public string Name
	{
		get
		{
			return this.name;
		}
	}

	// Token: 0x17000923 RID: 2339
	// (get) Token: 0x0600531C RID: 21276 RVA: 0x001FE328 File Offset: 0x001FC528
	public ProgressionClass ProgressionClass
	{
		get
		{
			if (this.cachedProgressionClass == null && !Progression.ProgressionClasses.TryGetValue(this.name, out this.cachedProgressionClass))
			{
				Log.Error("ProgressionValue ProgressionClasses missing {0}", new object[] { this.name });
			}
			return this.cachedProgressionClass;
		}
	}

	// Token: 0x17000924 RID: 2340
	// (get) Token: 0x0600531D RID: 21277 RVA: 0x001FE374 File Offset: 0x001FC574
	// (set) Token: 0x0600531E RID: 21278 RVA: 0x001FE39E File Offset: 0x001FC59E
	public int CostForNextLevel
	{
		get
		{
			if (this.ProgressionClass.CurrencyType == ProgressionCurrencyType.SP)
			{
				return this.ProgressionClass.CalculatedCostForLevel(this.Level + 1);
			}
			return this.costForNextLevel;
		}
		set
		{
			if (this.ProgressionClass.CurrencyType != ProgressionCurrencyType.SP)
			{
				this.costForNextLevel = value;
			}
		}
	}

	// Token: 0x17000925 RID: 2341
	// (get) Token: 0x0600531F RID: 21279 RVA: 0x001FE3B8 File Offset: 0x001FC5B8
	// (set) Token: 0x06005320 RID: 21280 RVA: 0x001FE3EB File Offset: 0x001FC5EB
	public int Level
	{
		get
		{
			ProgressionClass progressionClass = this.ProgressionClass;
			if (progressionClass == null)
			{
				return this.level;
			}
			if (progressionClass.IsSkill)
			{
				return progressionClass.MaxLevel;
			}
			return this.level;
		}
		set
		{
			this.calculatedFrame = -1;
			if (this.ProgressionClass == null)
			{
				this.level = value;
				return;
			}
			if (this.ProgressionClass.IsSkill)
			{
				this.level = this.ProgressionClass.MaxLevel;
				return;
			}
			this.level = value;
		}
	}

	// Token: 0x06005321 RID: 21281 RVA: 0x001FE42A File Offset: 0x001FC62A
	public int CalculatedLevel(EntityAlive _ea)
	{
		return (int)this.GetCalculatedLevel(_ea);
	}

	// Token: 0x06005322 RID: 21282 RVA: 0x001FE434 File Offset: 0x001FC634
	public int CalculatedMaxLevel(EntityAlive _ea)
	{
		return ProgressionClass.GetCalculatedMaxLevel(_ea, this);
	}

	// Token: 0x06005323 RID: 21283 RVA: 0x001FE43D File Offset: 0x001FC63D
	public bool IsLocked(EntityAlive _ea)
	{
		return ProgressionClass.GetCalculatedMaxLevel(_ea, this) == 0;
	}

	// Token: 0x17000926 RID: 2342
	// (get) Token: 0x06005324 RID: 21284 RVA: 0x001FE449 File Offset: 0x001FC649
	public float PercToNextLevel
	{
		get
		{
			return 1f - (float)this.CostForNextLevel / (float)this.ProgressionClass.CalculatedCostForLevel(this.level + 1);
		}
	}

	// Token: 0x06005325 RID: 21285 RVA: 0x001FE46D File Offset: 0x001FC66D
	public void ClearProgressionClassLink()
	{
		this.cachedProgressionClass = null;
	}

	// Token: 0x06005326 RID: 21286 RVA: 0x001FE476 File Offset: 0x001FC676
	public bool CanPurchase(EntityAlive _ea, int _level)
	{
		return _level <= this.ProgressionClass.MaxLevel;
	}

	// Token: 0x06005327 RID: 21287 RVA: 0x001FE489 File Offset: 0x001FC689
	public void CopyFrom(ProgressionValue _pv)
	{
		this.name = _pv.name;
		this.level = _pv.level;
		this.costForNextLevel = _pv.costForNextLevel;
	}

	// Token: 0x06005328 RID: 21288 RVA: 0x001FE4AF File Offset: 0x001FC6AF
	public void Read(BinaryReader _reader)
	{
		_reader.ReadByte();
		this.name = _reader.ReadString();
		this.level = (int)_reader.ReadByte();
		this.costForNextLevel = _reader.ReadInt32();
	}

	// Token: 0x06005329 RID: 21289 RVA: 0x001FE4DC File Offset: 0x001FC6DC
	public void Write(BinaryWriter _writer, bool _IsNetwork)
	{
		_writer.Write(1);
		_writer.Write(this.name);
		_writer.Write((byte)this.level);
		_writer.Write(this.costForNextLevel);
	}

	// Token: 0x040040F4 RID: 16628
	[PublicizedFrom(EAccessModifier.Private)]
	public const byte Version = 1;

	// Token: 0x040040F5 RID: 16629
	[PublicizedFrom(EAccessModifier.Private)]
	public string name;

	// Token: 0x040040F6 RID: 16630
	[PublicizedFrom(EAccessModifier.Private)]
	public int level;

	// Token: 0x040040F7 RID: 16631
	[PublicizedFrom(EAccessModifier.Private)]
	public int costForNextLevel;

	// Token: 0x040040F8 RID: 16632
	[PublicizedFrom(EAccessModifier.Private)]
	public int calculatedFrame;

	// Token: 0x040040F9 RID: 16633
	[PublicizedFrom(EAccessModifier.Private)]
	public float calculatedLevel;

	// Token: 0x040040FA RID: 16634
	[PublicizedFrom(EAccessModifier.Private)]
	public ProgressionClass cachedProgressionClass;
}
