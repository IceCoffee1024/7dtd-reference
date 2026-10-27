using System;
using System.Xml.Linq;
using SandboxOptions;
using UnityEngine.Scripting;

// Token: 0x02000633 RID: 1587
[Preserve]
public class LootEntryRequirementSandboxOption : BaseOperationLootEntryRequirement
{
	// Token: 0x06003373 RID: 13171 RVA: 0x0014DBFC File Offset: 0x0014BDFC
	public override void Init(XElement e)
	{
		base.Init(e);
		string text = "";
		e.ParseAttribute("option", ref text);
		this.option = Enum.Parse<global::SandboxOptions.SandboxOptions>(text);
		e.ParseAttribute("value", ref this.value);
	}

	// Token: 0x06003374 RID: 13172 RVA: 0x0014DC4C File Offset: 0x0014BE4C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override float LeftSide(EntityPlayer player)
	{
		if (player != null)
		{
			return SandboxOptionManager.GetFloat(this.option);
		}
		return 0f;
	}

	// Token: 0x06003375 RID: 13173 RVA: 0x0014DC68 File Offset: 0x0014BE68
	[PublicizedFrom(EAccessModifier.Protected)]
	public override float RightSide(EntityPlayer player)
	{
		return this.value;
	}

	// Token: 0x040028F5 RID: 10485
	[PublicizedFrom(EAccessModifier.Protected)]
	public global::SandboxOptions.SandboxOptions option;

	// Token: 0x040028F6 RID: 10486
	[PublicizedFrom(EAccessModifier.Protected)]
	public float value;
}
