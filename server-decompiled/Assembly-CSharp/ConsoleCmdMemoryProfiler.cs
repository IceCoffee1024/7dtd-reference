using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200024A RID: 586
[Preserve]
public class ConsoleCmdMemoryProfiler : ConsoleCmdAbstract
{
	// Token: 0x170001B5 RID: 437
	// (get) Token: 0x06001172 RID: 4466 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06001173 RID: 4467 RVA: 0x0006EDDC File Offset: 0x0006CFDC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "memprofile", "mprof" };
	}

	// Token: 0x06001174 RID: 4468 RVA: 0x0006EDF4 File Offset: 0x0006CFF4
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Toggles screen Memory Profiler UI";
	}

	// Token: 0x06001175 RID: 4469 RVA: 0x0006EDFC File Offset: 0x0006CFFC
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		UnityMemoryProfilerLabel[] array2;
		if (!this.enabled)
		{
			this.enabled = true;
			UnityMemoryProfilerLabel[] array = global::UnityEngine.Object.FindObjectsByType<UnityMemoryProfilerLabel>(FindObjectsSortMode.None);
			if (array == null || array.Length == 0)
			{
				global::UnityEngine.Object @object = Resources.Load("GUI/Prefabs/Debug_ProfilerLabel");
				using (List<UIRoot>.Enumerator enumerator = UIRoot.list.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						UIRoot uiroot = enumerator.Current;
						Transform transform = uiroot.gameObject.transform;
						if (uiroot.gameObject.GetComponentInChildren<UIAnchor>() != null)
						{
							transform = uiroot.gameObject.GetComponentInChildren<UIAnchor>().transform;
						}
						global::UnityEngine.Object.Instantiate(@object, transform);
					}
					return;
				}
			}
			array2 = global::UnityEngine.Object.FindObjectsByType<UnityMemoryProfilerLabel>(FindObjectsSortMode.None);
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i].gameObject.SetActive(true);
			}
			return;
		}
		this.enabled = false;
		array2 = global::UnityEngine.Object.FindObjectsByType<UnityMemoryProfilerLabel>(FindObjectsSortMode.None);
		for (int i = 0; i < array2.Length; i++)
		{
			array2[i].gameObject.SetActive(false);
		}
	}

	// Token: 0x04000CE2 RID: 3298
	[PublicizedFrom(EAccessModifier.Private)]
	public bool enabled;
}
