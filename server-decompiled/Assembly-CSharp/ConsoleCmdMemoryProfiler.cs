using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000247 RID: 583
[Preserve]
public class ConsoleCmdMemoryProfiler : ConsoleCmdAbstract
{
	// Token: 0x170001BD RID: 445
	// (get) Token: 0x06001175 RID: 4469 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06001176 RID: 4470 RVA: 0x0006EB30 File Offset: 0x0006CD30
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "memprofile", "mprof" };
	}

	// Token: 0x06001177 RID: 4471 RVA: 0x0006EB48 File Offset: 0x0006CD48
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Toggles screen Memory Profiler UI";
	}

	// Token: 0x06001178 RID: 4472 RVA: 0x0006EB50 File Offset: 0x0006CD50
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

	// Token: 0x04000CE9 RID: 3305
	[PublicizedFrom(EAccessModifier.Private)]
	public bool enabled;
}
