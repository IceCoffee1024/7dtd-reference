using System;
using System.Collections.Generic;

// Token: 0x02000539 RID: 1337
public class GameEventVariables
{
	// Token: 0x06002C10 RID: 11280 RVA: 0x001162C4 File Offset: 0x001144C4
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

	// Token: 0x06002C11 RID: 11281 RVA: 0x00116388 File Offset: 0x00114588
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

	// Token: 0x06002C12 RID: 11282 RVA: 0x00116450 File Offset: 0x00114650
	public void SetEventVariable(string name, bool value)
	{
		if (this.EventVariables == null)
		{
			this.EventVariables = new Dictionary<string, object>();
		}
		this.EventVariables[name] = value;
	}

	// Token: 0x06002C13 RID: 11283 RVA: 0x00116477 File Offset: 0x00114677
	public void SetEventVariable(string name, string value)
	{
		if (this.EventVariables == null)
		{
			this.EventVariables = new Dictionary<string, object>();
		}
		this.EventVariables[name] = value;
	}

	// Token: 0x06002C14 RID: 11284 RVA: 0x00116499 File Offset: 0x00114699
	public void ParseVarInt(string varName, ref int optionalValue)
	{
		if (this.EventVariables == null || !this.EventVariables.ContainsKey(varName))
		{
			return;
		}
		optionalValue = (int)this.EventVariables[varName];
	}

	// Token: 0x06002C15 RID: 11285 RVA: 0x001164C5 File Offset: 0x001146C5
	public void ParseVarString(string varName, ref string optionalValue)
	{
		if (this.EventVariables == null || !this.EventVariables.ContainsKey(varName))
		{
			return;
		}
		optionalValue = (string)this.EventVariables[varName];
	}

	// Token: 0x06002C16 RID: 11286 RVA: 0x001164F1 File Offset: 0x001146F1
	public void ParseVarFloat(string varName, ref float optionalValue)
	{
		if (this.EventVariables == null || !this.EventVariables.ContainsKey(varName))
		{
			return;
		}
		optionalValue = (float)this.EventVariables[varName];
	}

	// Token: 0x06002C17 RID: 11287 RVA: 0x001164C5 File Offset: 0x001146C5
	public void ParseString(string varName, ref string optionalValue)
	{
		if (this.EventVariables == null || !this.EventVariables.ContainsKey(varName))
		{
			return;
		}
		optionalValue = (string)this.EventVariables[varName];
	}

	// Token: 0x06002C18 RID: 11288 RVA: 0x0011651D File Offset: 0x0011471D
	public void ParseBool(string varName, ref bool optionalValue)
	{
		if (this.EventVariables == null || !this.EventVariables.ContainsKey(varName))
		{
			return;
		}
		optionalValue = (bool)this.EventVariables[varName];
	}

	// Token: 0x040021B7 RID: 8631
	public Dictionary<string, object> EventVariables = new Dictionary<string, object>();

	// Token: 0x040021B8 RID: 8632
	public GameEventVariables.OperationTypes operationType;

	// Token: 0x0200053A RID: 1338
	public enum OperationTypes
	{
		// Token: 0x040021BA RID: 8634
		Set,
		// Token: 0x040021BB RID: 8635
		Add,
		// Token: 0x040021BC RID: 8636
		Subtract,
		// Token: 0x040021BD RID: 8637
		Multiply
	}
}
