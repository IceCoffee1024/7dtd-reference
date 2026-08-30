using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000243 RID: 579
[Preserve]
public class ConsoleCmdMemoryProfiler : ConsoleCmdAbstract
{
	// Token: 0x170001BC RID: 444
	// (get) Token: 0x06001163 RID: 4451 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06001164 RID: 4452 RVA: 0x0006E4D4 File Offset: 0x0006C6D4
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "memprofile", "mprof" };
	}

	// Token: 0x06001165 RID: 4453 RVA: 0x0006E4EC File Offset: 0x0006C6EC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Toggles screen Memory Profiler UI";
	}

	// Token: 0x06001166 RID: 4454 RVA: 0x0006E4F4 File Offset: 0x0006C6F4
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

	// Token: 0x04000CD8 RID: 3288
	[PublicizedFrom(EAccessModifier.Private)]
	public bool enabled;
}
