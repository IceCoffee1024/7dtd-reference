using System;
using System.Xml.Linq;
using SandboxOptions;
using UnityEngine.Scripting;

// Token: 0x0200062A RID: 1578
[Preserve]
public class LootEntryRequirementSandboxOption : BaseOperationLootEntryRequirement
{
	// Token: 0x06003322 RID: 13090 RVA: 0x0014C698 File Offset: 0x0014A898
	public override void Init(XElement e)
	{
		base.Init(e);
		string text = "";
		e.ParseAttribute("option", ref text);
		this.option = Enum.Parse<global::SandboxOptions.SandboxOptions>(text);
		e.ParseAttribute("value", ref this.value);
	}

	// Token: 0x06003323 RID: 13091 RVA: 0x0014C6E8 File Offset: 0x0014A8E8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override float LeftSide(EntityPlayer player)
	{
		if (player != null)
		{
			return SandboxOptionManager.GetFloat(this.option);
		}
		return 0f;
	}

	// Token: 0x06003324 RID: 13092 RVA: 0x0014C704 File Offset: 0x0014A904
	[PublicizedFrom(EAccessModifier.Protected)]
	public override float RightSide(EntityPlayer player)
	{
		return this.value;
	}

	// Token: 0x040028E1 RID: 10465
	[PublicizedFrom(EAccessModifier.Protected)]
	public global::SandboxOptions.SandboxOptions option;

	// Token: 0x040028E2 RID: 10466
	[PublicizedFrom(EAccessModifier.Protected)]
	public float value;
}
