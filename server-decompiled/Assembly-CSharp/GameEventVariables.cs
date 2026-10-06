using System;
using System.Collections.Generic;

// Token: 0x0200053F RID: 1343
public class GameEventVariables
{
	// Token: 0x06002C06 RID: 11270 RVA: 0x00117094 File Offset: 0x00115294
	public void ModifyEventVariable(string name, GameEventVariables.OperationTypes operation, int value, int minValue = -2147483648, int maxValue = 2147483647)
	{
		if (this.EventVariables == null)
		{
			this.EventVariables = new Dictionary<string, object>();
		}
		if (this.operationType == GameEventVariables.OperationTypes.Set)
		{
			this.EventVariables[name] = Utils.FastClamp(value, minValue, maxValue);
			return;
		}
		int num = 0;
		this.ParseVarInt(name, ref num);
		switch (this.operationType)
		{
		case GameEventVariables.OperationTypes.Add:
			this.EventVariables[name] = Utils.FastClamp(num + value, minValue, maxValue);
			return;
		case GameEventVariables.OperationTypes.Subtract:
			this.EventVariables[name] = Utils.FastClamp(num - value, minValue, maxValue);
			return;
		case GameEventVariables.OperationTypes.Multiply:
			this.EventVariables[name] = Utils.FastClamp(num * value, minValue, maxValue);
			return;
		default:
			return;
		}
	}

	// Token: 0x06002C07 RID: 11271 RVA: 0x00117158 File Offset: 0x00115358
	public void ModifyEventVariable(string name, GameEventVariables.OperationTypes operation, float value, float minValue = -3.4028235E+38f, float maxValue = 3.4028235E+38f)
	{
		if (this.EventVariables == null)
		{
			this.EventVariables = new Dictionary<string, object>();
		}
		if (this.operationType == GameEventVariables.OperationTypes.Set)
		{
			this.EventVariables[name] = Utils.FastClamp(value, minValue, maxValue);
			return;
		}
		float num = 0f;
		this.ParseVarFloat(name, ref num);
		switch (this.operationType)
		{
		case GameEventVariables.OperationTypes.Add:
			this.EventVariables[name] = Utils.FastClamp(num + value, minValue, maxValue);
			return;
		case GameEventVariables.OperationTypes.Subtract:
			this.EventVariables[name] = Utils.FastClamp(num - value, minValue, maxValue);
			return;
		case GameEventVariables.OperationTypes.Multiply:
			this.EventVariables[name] = Utils.FastClamp(num * value, minValue, maxValue);
			return;
		default:
			return;
		}
	}

	// Token: 0x06002C08 RID: 11272 RVA: 0x00117220 File Offset: 0x00115420
	public void SetEventVariable(string name, bool value)
	{
		if (this.EventVariables == null)
		{
			this.EventVariables = new Dictionary<string, object>();
		}
		this.EventVariables[name] = value;
	}

	// Token: 0x06002C09 RID: 11273 RVA: 0x00117247 File Offset: 0x00115447
	public void SetEventVariable(string name, string value)
	{
		if (this.EventVariables == null)
		{
			this.EventVariables = new Dictionary<string, object>();
		}
		this.EventVariables[name] = value;
	}

	// Token: 0x06002C0A RID: 11274 RVA: 0x00117269 File Offset: 0x00115469
	public void ParseVarInt(string varName, ref int optionalValue)
	{
		if (this.EventVariables == null || !this.EventVariables.ContainsKey(varName))
		{
			return;
		}
		optionalValue = (int)this.EventVariables[varName];
	}

	// Token: 0x06002C0B RID: 11275 RVA: 0x00117295 File Offset: 0x00115495
	public void ParseVarString(string varName, ref string optionalValue)
	{
		if (this.EventVariables == null || !this.EventVariables.ContainsKey(varName))
		{
			return;
		}
		optionalValue = (string)this.EventVariables[varName];
	}

	// Token: 0x06002C0C RID: 11276 RVA: 0x001172C1 File Offset: 0x001154C1
	public void ParseVarFloat(string varName, ref float optionalValue)
	{
		if (this.EventVariables == null || !this.EventVariables.ContainsKey(varName))
		{
			return;
		}
		optionalValue = (float)this.EventVariables[varName];
	}

	// Token: 0x06002C0D RID: 11277 RVA: 0x00117295 File Offset: 0x00115495
	public void ParseString(string varName, ref string optionalValue)
	{
		if (this.EventVariables == null || !this.EventVariables.ContainsKey(varName))
		{
			return;
		}
		optionalValue = (string)this.EventVariables[varName];
	}

	// Token: 0x06002C0E RID: 11278 RVA: 0x001172ED File Offset: 0x001154ED
	public void ParseBool(string varName, ref bool optionalValue)
	{
		if (this.EventVariables == null || !this.EventVariables.ContainsKey(varName))
		{
			return;
		}
		optionalValue = (bool)this.EventVariables[varName];
	}

	// Token: 0x040021BF RID: 8639
	public Dictionary<string, object> EventVariables = new Dictionary<string, object>();

	// Token: 0x040021C0 RID: 8640
	public GameEventVariables.OperationTypes operationType;

	// Token: 0x02000540 RID: 1344
	public enum OperationTypes
	{
		// Token: 0x040021C2 RID: 8642
		Set,
		// Token: 0x040021C3 RID: 8643
		Add,
		// Token: 0x040021C4 RID: 8644
		Subtract,
		// Token: 0x040021C5 RID: 8645
		Multiply
	}
}
