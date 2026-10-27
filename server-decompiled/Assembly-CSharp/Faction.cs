using System;
using UnityEngine;

// Token: 0x0200052C RID: 1324
public class Faction
{
	// Token: 0x06002B48 RID: 11080 RVA: 0x00112E7B File Offset: 0x0011107B
	public Faction()
	{
	}

	// Token: 0x06002B49 RID: 11081 RVA: 0x00112E94 File Offset: 0x00111094
	public Faction(string _name, bool _playerFaction = false, string _icon = "")
	{
		this.Name = _name;
		this.Icon = _icon;
		this.IsPlayerFaction = _playerFaction;
		for (int i = 0; i < 255; i++)
		{
			this.Relationships[i] = 400f;
		}
	}

	// Token: 0x06002B4A RID: 11082 RVA: 0x00112EEC File Offset: 0x001110EC
	public void ModifyRelationship(byte _factionId, float _modifier)
	{
		float num = this.Relationships[(int)_factionId];
		if (num != 255f)
		{
			num = Mathf.Clamp(num + _modifier, 0f, 1000f);
		}
		this.Relationships[(int)_factionId] = num;
	}

	// Token: 0x06002B4B RID: 11083 RVA: 0x00112F26 File Offset: 0x00111126
	public void SetRelationship(byte _factionId, float _value)
	{
		this.Relationships[(int)_factionId] = (float)((byte)Mathf.Clamp(_value, 0f, 1000f));
	}

	// Token: 0x06002B4C RID: 11084 RVA: 0x00112F42 File Offset: 0x00111142
	public float GetRelationship(byte _factionId)
	{
		return this.Relationships[(int)_factionId];
	}

	// Token: 0x06002B4D RID: 11085 RVA: 0x00112F4C File Offset: 0x0011114C
	public void SetAlly(byte _factionId)
	{
		this.Relationships[(int)_factionId] = 1000f;
	}

	// Token: 0x06002B4E RID: 11086 RVA: 0x00112F5C File Offset: 0x0011115C
	public void Write(PooledBinaryWriter bw)
	{
		for (int i = 0; i < 255; i++)
		{
			bw.Write(this.Relationships[i]);
		}
		bw.Write(this.IsPlayerFaction);
	}

	// Token: 0x06002B4F RID: 11087 RVA: 0x00112F94 File Offset: 0x00111194
	public void Read(PooledBinaryReader br)
	{
		this.Relationships = new float[255];
		for (int i = 0; i < 255; i++)
		{
			this.Relationships[i] = br.ReadSingle();
		}
		this.IsPlayerFaction = br.ReadBoolean();
	}

	// Token: 0x06002B50 RID: 11088 RVA: 0x00112FDC File Offset: 0x001111DC
	public override string ToString()
	{
		return string.Format("{0} : {1}", this.Name, string.Join(", ", Array.ConvertAll<float, string>(this.Relationships, (float x) => x.ToCultureInvariantString())));
	}

	// Token: 0x04002139 RID: 8505
	public byte ID;

	// Token: 0x0400213A RID: 8506
	public string Name;

	// Token: 0x0400213B RID: 8507
	public string Icon;

	// Token: 0x0400213C RID: 8508
	public bool IsPlayerFaction;

	// Token: 0x0400213D RID: 8509
	public float[] Relationships = new float[255];
}
