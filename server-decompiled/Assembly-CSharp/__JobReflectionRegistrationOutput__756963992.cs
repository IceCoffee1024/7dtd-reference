using System;
using Unity.Jobs;
using UnityEngine;

// Token: 0x02001DC6 RID: 7622
[DOTSCompilerGenerated]
[PublicizedFrom(EAccessModifier.Internal)]
public class __JobReflectionRegistrationOutput__756963992
{
	// Token: 0x0600DFF0 RID: 57328 RVA: 0x004FF100 File Offset: 0x004FD300
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

	// Token: 0x0600DFF1 RID: 57329 RVA: 0x004FF144 File Offset: 0x004FD344
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
	public static void EarlyInit()
	{
		__JobReflectionRegistrationOutput__756963992.CreateJobReflectionData();
	}
}
