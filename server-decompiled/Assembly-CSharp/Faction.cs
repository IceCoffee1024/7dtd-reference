using System;
using UnityEngine;

// Token: 0x02000528 RID: 1320
public class Faction
{
	// Token: 0x06002B40 RID: 11072 RVA: 0x001128EF File Offset: 0x00110AEF
	public Faction()
	{
	}

	// Token: 0x06002B41 RID: 11073 RVA: 0x00112908 File Offset: 0x00110B08
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

	// Token: 0x06002B42 RID: 11074 RVA: 0x00112960 File Offset: 0x00110B60
	public void ModifyRelationship(byte _factionId, float _modifier)
	{
		float num = this.Relationships[(int)_factionId];
		if (num != 255f)
		{
			num = Mathf.Clamp(num + _modifier, 0f, 1000f);
		}
		this.Relationships[(int)_factionId] = num;
	}

	// Token: 0x06002B43 RID: 11075 RVA: 0x0011299A File Offset: 0x00110B9A
	public void SetRelationship(byte _factionId, float _value)
	{
		this.Relationships[(int)_factionId] = (float)((byte)Mathf.Clamp(_value, 0f, 1000f));
	}

	// Token: 0x06002B44 RID: 11076 RVA: 0x001129B6 File Offset: 0x00110BB6
	public float GetRelationship(byte _factionId)
	{
		return this.Relationships[(int)_factionId];
	}

	// Token: 0x06002B45 RID: 11077 RVA: 0x001129C0 File Offset: 0x00110BC0
	public void SetAlly(byte _factionId)
	{
		this.Relationships[(int)_factionId] = 1000f;
	}

	// Token: 0x06002B46 RID: 11078 RVA: 0x001129D0 File Offset: 0x00110BD0
	public void Write(PooledBinaryWriter bw)
	{
		for (int i = 0; i < 255; i++)
		{
			bw.Write(this.Relationships[i]);
		}
		bw.Write(this.IsPlayerFaction);
	}

	// Token: 0x06002B47 RID: 11079 RVA: 0x00112A08 File Offset: 0x00110C08
	public void Read(PooledBinaryReader br)
	{
		this.Relationships = new float[255];
		for (int i = 0; i < 255; i++)
		{
			this.Relationships[i] = br.ReadSingle();
		}
		this.IsPlayerFaction = br.ReadBoolean();
	}

	// Token: 0x06002B48 RID: 11080 RVA: 0x00112A50 File Offset: 0x00110C50
	public override string ToString()
	{
		return string.Format("{0} : {1}", this.Name, string.Join(", ", Array.ConvertAll<float, string>(this.Relationships, (float x) => x.ToCultureInvariantString())));
	}

	// Token: 0x04002122 RID: 8482
	public byte ID;

	// Token: 0x04002123 RID: 8483
	public string Name;

	// Token: 0x04002124 RID: 8484
	public string Icon;

	// Token: 0x04002125 RID: 8485
	public bool IsPlayerFaction;

	// Token: 0x04002126 RID: 8486
	public float[] Relationships = new float[255];
}
