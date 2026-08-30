using System;
using System.Collections.Generic;

// Token: 0x02000534 RID: 1332
public class GameEventVariables
{
	// Token: 0x06002BE6 RID: 11238 RVA: 0x0011524C File Offset: 0x0011344C
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

	// Token: 0x06002BE7 RID: 11239 RVA: 0x00115310 File Offset: 0x00113510
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

	// Token: 0x06002BE8 RID: 11240 RVA: 0x001153D8 File Offset: 0x001135D8
	public void SetEventVariable(string name, bool value)
	{
		if (this.EventVariables == null)
		{
			this.EventVariables = new Dictionary<string, object>();
		}
		this.EventVariables[name] = value;
	}

	// Token: 0x06002BE9 RID: 11241 RVA: 0x001153FF File Offset: 0x001135FF
	public void SetEventVariable(string name, string value)
	{
		if (this.EventVariables == null)
		{
			this.EventVariables = new Dictionary<string, object>();
		}
		this.EventVariables[name] = value;
	}

	// Token: 0x06002BEA RID: 11242 RVA: 0x00115421 File Offset: 0x00113621
	public void ParseVarInt(string varName, ref int optionalValue)
	{
		if (this.EventVariables == null || !this.EventVariables.ContainsKey(varName))
		{
			return;
		}
		optionalValue = (int)this.EventVariables[varName];
	}

	// Token: 0x06002BEB RID: 11243 RVA: 0x0011544D File Offset: 0x0011364D
	public void ParseVarString(string varName, ref string optionalValue)
	{
		if (this.EventVariables == null || !this.EventVariables.ContainsKey(varName))
		{
			return;
		}
		optionalValue = (string)this.EventVariables[varName];
	}

	// Token: 0x06002BEC RID: 11244 RVA: 0x00115479 File Offset: 0x00113679
	public void ParseVarFloat(string varName, ref float optionalValue)
	{
		if (this.EventVariables == null || !this.EventVariables.ContainsKey(varName))
		{
			return;
		}
		optionalValue = (float)this.EventVariables[varName];
	}

	// Token: 0x06002BED RID: 11245 RVA: 0x0011544D File Offset: 0x0011364D
	public void ParseString(string varName, ref string optionalValue)
	{
		if (this.EventVariables == null || !this.EventVariables.ContainsKey(varName))
		{
			return;
		}
		optionalValue = (string)this.EventVariables[varName];
	}

	// Token: 0x06002BEE RID: 11246 RVA: 0x001154A5 File Offset: 0x001136A5
	public void ParseBool(string varName, ref bool optionalValue)
	{
		if (this.EventVariables == null || !this.EventVariables.ContainsKey(varName))
		{
			return;
		}
		optionalValue = (bool)this.EventVariables[varName];
	}

	// Token: 0x04002188 RID: 8584
	public Dictionary<string, object> EventVariables = new Dictionary<string, object>();

	// Token: 0x04002189 RID: 8585
	public GameEventVariables.OperationTypes operationType;

	// Token: 0x02000535 RID: 1333
	public enum OperationTypes
	{
		// Token: 0x0400218B RID: 8587
		Set,
		// Token: 0x0400218C RID: 8588
		Add,
		// Token: 0x0400218D RID: 8589
		Subtract,
		// Token: 0x0400218E RID: 8590
		Multiply
	}
}
