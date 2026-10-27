using System;
using System.Collections.Generic;
using System.Globalization;

// Token: 0x02000547 RID: 1351
public class DamageMultiplier
{
	// Token: 0x06002C24 RID: 11300 RVA: 0x00117DE5 File Offset: 0x00115FE5
	public DamageMultiplier()
	{
	}

	// Token: 0x06002C25 RID: 11301 RVA: 0x00117DF8 File Offset: 0x00115FF8
	public DamageMultiplier(DynamicProperties _properties)
	{
		if (_properties.Classes.ContainsKey("DamageBonus"))
		{
			foreach (KeyValuePair<string, string> keyValuePair in _properties.Classes["DamageBonus"].Values)
			{
				float num = StringParsers.ParseFloat(keyValuePair.Value, 0, -1, NumberStyles.Any);
				this.addMultiplier(keyValuePair.Key, num);
			}
		}
	}

	// Token: 0x06002C26 RID: 11302 RVA: 0x00117E98 File Offset: 0x00116098
	[PublicizedFrom(EAccessModifier.Private)]
	public void addMultiplier(string _name, float _value)
	{
		this.damageMultiplier[_name] = _value;
	}

	// Token: 0x06002C27 RID: 11303 RVA: 0x00117EA7 File Offset: 0x001160A7
	public float Get(string _group)
	{
		if (_group == null || this.damageMultiplier == null || !this.damageMultiplier.ContainsKey(_group))
		{
			return 1f;
		}
		return this.damageMultiplier[_group];
	}

	// Token: 0x06002C28 RID: 11304 RVA: 0x00117ED4 File Offset: 0x001160D4
	public void Read(PooledBinaryReader _br)
	{
		this.damageMultiplier.Clear();
		int num = (int)_br.ReadInt16();
		for (int i = 0; i < num; i++)
		{
			string text = _br.ReadString();
			float num2 = _br.ReadSingle();
			this.damageMultiplier.Add(text, num2);
		}
	}

	// Token: 0x06002C29 RID: 11305 RVA: 0x00117F1C File Offset: 0x0011611C
	public void Write(PooledBinaryWriter _bw)
	{
		_bw.Write((short)this.damageMultiplier.Count);
		foreach (KeyValuePair<string, float> keyValuePair in this.damageMultiplier)
		{
			_bw.Write(keyValuePair.Key);
			_bw.Write(keyValuePair.Value);
		}
	}

	// Token: 0x040021E2 RID: 8674
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<string, float> damageMultiplier = new Dictionary<string, float>();
}
