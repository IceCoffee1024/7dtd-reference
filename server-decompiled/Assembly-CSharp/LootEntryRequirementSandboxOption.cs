using System;
using System.Xml.Linq;
using SandboxOptions;
using UnityEngine.Scripting;

// Token: 0x0200062C RID: 1580
[Preserve]
public class LootEntryRequirementSandboxOption : BaseOperationLootEntryRequirement
{
	// Token: 0x06003365 RID: 13157 RVA: 0x0014D008 File Offset: 0x0014B208
	public override void Init(XElement e)
	{
		base.Init(e);
		string text = "";
		e.ParseAttribute("option", ref text);
		this.option = Enum.Parse<global::SandboxOptions.SandboxOptions>(text);
		e.ParseAttribute("value", ref this.value);
	}

	// Token: 0x06003366 RID: 13158 RVA: 0x0014D058 File Offset: 0x0014B258
	[PublicizedFrom(EAccessModifier.Protected)]
	public override float LeftSide(EntityPlayer player)
	{
		if (player != null)
		{
			return SandboxOptionManager.GetFloat(this.option);
		}
		return 0f;
	}

	// Token: 0x06003367 RID: 13159 RVA: 0x0014D074 File Offset: 0x0014B274
	[PublicizedFrom(EAccessModifier.Protected)]
	public override float RightSide(EntityPlayer player)
	{
		return this.value;
	}

	// Token: 0x040028DA RID: 10458
	[PublicizedFrom(EAccessModifier.Protected)]
	public global::SandboxOptions.SandboxOptions option;

	// Token: 0x040028DB RID: 10459
	[PublicizedFrom(EAccessModifier.Protected)]
	public float value;
}
