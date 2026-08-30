using System;
using System.Xml.Linq;
using SandboxOptions;
using UnityEngine.Scripting;

// Token: 0x02000621 RID: 1569
[Preserve]
public class LootEntryRequirementSandboxOption : BaseOperationLootEntryRequirement
{
	// Token: 0x060032CF RID: 13007 RVA: 0x0014A57C File Offset: 0x0014877C
	public override void Init(XElement e)
	{
		base.Init(e);
		string text = "";
		e.ParseAttribute("option", ref text);
		this.option = Enum.Parse<global::SandboxOptions.SandboxOptions>(text);
		e.ParseAttribute("value", ref this.value);
	}

	// Token: 0x060032D0 RID: 13008 RVA: 0x0014A5CC File Offset: 0x001487CC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override float LeftSide(EntityPlayer player)
	{
		if (player != null)
		{
			return SandboxOptionManager.GetFloat(this.option);
		}
		return 0f;
	}

	// Token: 0x060032D1 RID: 13009 RVA: 0x0014A5E8 File Offset: 0x001487E8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override float RightSide(EntityPlayer player)
	{
		return this.value;
	}

	// Token: 0x0400286E RID: 10350
	[PublicizedFrom(EAccessModifier.Protected)]
	public global::SandboxOptions.SandboxOptions option;

	// Token: 0x0400286F RID: 10351
	[PublicizedFrom(EAccessModifier.Protected)]
	public float value;
}
