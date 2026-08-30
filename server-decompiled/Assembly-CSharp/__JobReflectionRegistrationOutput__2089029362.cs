using System;
using Unity.Jobs;
using UnityEngine;

// Token: 0x02001D6D RID: 7533
[DOTSCompilerGenerated]
[PublicizedFrom(EAccessModifier.Internal)]
public class __JobReflectionRegistrationOutput__2089029362
{
	// Token: 0x0600DCD6 RID: 56534 RVA: 0x004F3AD0 File Offset: 0x004F1CD0
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

	// Token: 0x0600DCD7 RID: 56535 RVA: 0x004F3B14 File Offset: 0x004F1D14
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
	public static void EarlyInit()
	{
		__JobReflectionRegistrationOutput__2089029362.CreateJobReflectionData();
	}
}
