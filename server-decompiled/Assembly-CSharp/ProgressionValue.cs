using System;
using System.IO;
using UnityEngine;

// Token: 0x02000AD5 RID: 2773
public class ProgressionValue
{
	// Token: 0x060052FF RID: 21247 RVA: 0x001FAFF8 File Offset: 0x001F91F8
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

	// Token: 0x06005300 RID: 21248 RVA: 0x0000640C File Offset: 0x0000460C
	public ProgressionValue()
	{
	}

	// Token: 0x06005301 RID: 21249 RVA: 0x001FB0AF File Offset: 0x001F92AF
	public ProgressionValue(string _name)
	{
		this.name = _name;
	}

	// Token: 0x17000900 RID: 2304
	// (get) Token: 0x06005302 RID: 21250 RVA: 0x001FB0BE File Offset: 0x001F92BE
	public string Name
	{
		get
		{
			return this.name;
		}
	}

	// Token: 0x17000901 RID: 2305
	// (get) Token: 0x06005303 RID: 21251 RVA: 0x001FB0C8 File Offset: 0x001F92C8
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

	// Token: 0x17000902 RID: 2306
	// (get) Token: 0x06005304 RID: 21252 RVA: 0x001FB114 File Offset: 0x001F9314
	// (set) Token: 0x06005305 RID: 21253 RVA: 0x001FB13E File Offset: 0x001F933E
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

	// Token: 0x17000903 RID: 2307
	// (get) Token: 0x06005306 RID: 21254 RVA: 0x001FB158 File Offset: 0x001F9358
	// (set) Token: 0x06005307 RID: 21255 RVA: 0x001FB18B File Offset: 0x001F938B
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

	// Token: 0x06005308 RID: 21256 RVA: 0x001FB1CA File Offset: 0x001F93CA
	public int CalculatedLevel(EntityAlive _ea)
	{
		return (int)this.GetCalculatedLevel(_ea);
	}

	// Token: 0x06005309 RID: 21257 RVA: 0x001FB1D4 File Offset: 0x001F93D4
	public int CalculatedMaxLevel(EntityAlive _ea)
	{
		return ProgressionClass.GetCalculatedMaxLevel(_ea, this);
	}

	// Token: 0x0600530A RID: 21258 RVA: 0x001FB1DD File Offset: 0x001F93DD
	public bool IsLocked(EntityAlive _ea)
	{
		return ProgressionClass.GetCalculatedMaxLevel(_ea, this) == 0;
	}

	// Token: 0x17000904 RID: 2308
	// (get) Token: 0x0600530B RID: 21259 RVA: 0x001FB1E9 File Offset: 0x001F93E9
	public float PercToNextLevel
	{
		get
		{
			return 1f - (float)this.CostForNextLevel / (float)this.ProgressionClass.CalculatedCostForLevel(this.level + 1);
		}
	}

	// Token: 0x0600530C RID: 21260 RVA: 0x001FB20D File Offset: 0x001F940D
	public void ClearProgressionClassLink()
	{
		this.cachedProgressionClass = null;
	}

	// Token: 0x0600530D RID: 21261 RVA: 0x001FB216 File Offset: 0x001F9416
	public bool CanPurchase(EntityAlive _ea, int _level)
	{
		return _level <= this.ProgressionClass.MaxLevel;
	}

	// Token: 0x0600530E RID: 21262 RVA: 0x001FB229 File Offset: 0x001F9429
	public void CopyFrom(ProgressionValue _pv)
	{
		this.name = _pv.name;
		this.level = _pv.level;
		this.costForNextLevel = _pv.costForNextLevel;
	}

	// Token: 0x0600530F RID: 21263 RVA: 0x001FB24F File Offset: 0x001F944F
	public void Read(BinaryReader _reader)
	{
		_reader.ReadByte();
		this.name = _reader.ReadString();
		this.level = (int)_reader.ReadByte();
		this.costForNextLevel = _reader.ReadInt32();
	}

	// Token: 0x06005310 RID: 21264 RVA: 0x001FB27C File Offset: 0x001F947C
	public void Write(BinaryWriter _writer, bool _IsNetwork)
	{
		_writer.Write(1);
		_writer.Write(this.name);
		_writer.Write((byte)this.level);
		_writer.Write(this.costForNextLevel);
	}

	// Token: 0x040040B7 RID: 16567
	[PublicizedFrom(EAccessModifier.Private)]
	public const byte Version = 1;

	// Token: 0x040040B8 RID: 16568
	[PublicizedFrom(EAccessModifier.Private)]
	public string name;

	// Token: 0x040040B9 RID: 16569
	[PublicizedFrom(EAccessModifier.Private)]
	public int level;

	// Token: 0x040040BA RID: 16570
	[PublicizedFrom(EAccessModifier.Private)]
	public int costForNextLevel;

	// Token: 0x040040BB RID: 16571
	[PublicizedFrom(EAccessModifier.Private)]
	public int calculatedFrame;

	// Token: 0x040040BC RID: 16572
	[PublicizedFrom(EAccessModifier.Private)]
	public float calculatedLevel;

	// Token: 0x040040BD RID: 16573
	[PublicizedFrom(EAccessModifier.Private)]
	public ProgressionClass cachedProgressionClass;
}
