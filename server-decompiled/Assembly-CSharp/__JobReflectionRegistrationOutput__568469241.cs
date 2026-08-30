using System;
using Unity.Jobs;
using UnityEngine;

// Token: 0x02001D94 RID: 7572
[DOTSCompilerGenerated]
[PublicizedFrom(EAccessModifier.Internal)]
public class __JobReflectionRegistrationOutput__568469241
{
	// Token: 0x0600DE77 RID: 56951 RVA: 0x004FA544 File Offset: 0x004F8744
	public static void CreateJobReflectionData()
	{
		try
		{
			IJobParallelForExtensions.EarlyJobInit<WaterSimulationApplyFlows>();
			IJobParallelForExtensions.EarlyJobInit<WaterSimulationCalcFlows>();
			IJobExtensions.EarlyJobInit<WaterSimulationPostProcess>();
			IJobExtensions.EarlyJobInit<WaterSimulationPreProcess>();
		}
		catch (Exception ex)
		{
			EarlyInitHelpers.JobReflectionDataCreationFailed(ex);
		}
	}

	// Token: 0x0600DE78 RID: 56952 RVA: 0x004FA588 File Offset: 0x004F8788
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
	public static void EarlyInit()
	{
		__JobReflectionRegistrationOutput__568469241.CreateJobReflectionData();
	}
}
