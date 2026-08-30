using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000208 RID: 520
[Preserve]
public class ConsoleCmdDebugJiggle : ConsoleCmdAbstract
{
	// Token: 0x06000FDD RID: 4061 RVA: 0x00066524 File Offset: 0x00064724
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "debugjiggle", "dgj" };
	}

	// Token: 0x06000FDE RID: 4062 RVA: 0x00032147 File Offset: 0x00030347
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "";
	}

	// Token: 0x06000FDF RID: 4063 RVA: 0x00032147 File Offset: 0x00030347
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "";
	}

	// Token: 0x17000161 RID: 353
	// (get) Token: 0x06000FE0 RID: 4064 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06000FE1 RID: 4065 RVA: 0x0006653C File Offset: 0x0006473C
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		World world = GameManager.Instance.World;
		if (world == null)
		{
			return;
		}
		if (_params.Count <= 0)
		{
			foreach (EntityAlive entityAlive in world.EntityAlives)
			{
				this.LogEntityJiggles(entityAlive);
			}
			return;
		}
		int num;
		if (!int.TryParse(_params[0], out num))
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Expected an entity id");
			return;
		}
		Entity entity = world.GetEntity(num);
		if (entity == null)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output(string.Format("Could not find entity with id {0}", num));
			return;
		}
		this.LogEntityJiggles(entity);
	}

	// Token: 0x06000FE2 RID: 4066 RVA: 0x00066600 File Offset: 0x00064800
	[PublicizedFrom(EAccessModifier.Private)]
	public void LogEntityJiggles(Entity entity)
	{
		EModelBase emodel = entity.emodel;
		if (emodel == null)
		{
			return;
		}
		emodel.LogJiggles();
	}
}
