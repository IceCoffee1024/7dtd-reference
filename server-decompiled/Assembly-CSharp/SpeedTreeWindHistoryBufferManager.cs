using System;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;

// Token: 0x02001373 RID: 4979
public class SpeedTreeWindHistoryBufferManager
{
	// Token: 0x06009D00 RID: 40192 RVA: 0x003B4C73 File Offset: 0x003B2E73
	[PublicizedFrom(EAccessModifier.Private)]
	public SpeedTreeWindHistoryBufferManager()
	{
	}

	// Token: 0x17001297 RID: 4759
	// (get) Token: 0x06009D01 RID: 40193 RVA: 0x003B4CB2 File Offset: 0x003B2EB2
	public static SpeedTreeWindHistoryBufferManager Instance
	{
		get
		{
			if (SpeedTreeWindHistoryBufferManager.m_Instance == null)
			{
				SpeedTreeWindHistoryBufferManager.m_Instance = new SpeedTreeWindHistoryBufferManager();
			}
			return SpeedTreeWindHistoryBufferManager.m_Instance;
		}
	}

	// Token: 0x06009D02 RID: 40194 RVA: 0x003B4CCC File Offset: 0x003B2ECC
	public bool TryRegisterActiveRenderer(Renderer renderer)
	{
		bool flag;
		using (SpeedTreeWindHistoryBufferManager.s_ManagerTotal.Auto())
		{
			using (SpeedTreeWindHistoryBufferManager.s_ManagerRegistrations.Auto())
			{
				if (renderer == null)
				{
					Debug.LogError("Cannot register a null renderer.");
					flag = false;
				}
				else
				{
					SpeedTreeWindHistoryBufferManager.SharedMaterialGroup sharedMaterialGroup;
					if (!this.rendererToGroupMap.TryGetValue(renderer, out sharedMaterialGroup))
					{
						this.newMaterialsSet.Clear();
						this.tempMaterialsList.Clear();
						BillboardRenderer billboardRenderer = renderer as BillboardRenderer;
						if (billboardRenderer != null)
						{
							this.tempMaterialsList.Add(billboardRenderer.billboard.material);
						}
						else
						{
							renderer.GetSharedMaterials(this.tempMaterialsList);
						}
						foreach (Material material in this.tempMaterialsList)
						{
							if (!(material == null) && !this.materialToGroupMap.TryGetValue(material, out sharedMaterialGroup))
							{
								this.newMaterialsSet.Add(material);
							}
						}
						if (sharedMaterialGroup == null)
						{
							if (this.newMaterialsSet.Count == 0)
							{
								return false;
							}
							sharedMaterialGroup = new SpeedTreeWindHistoryBufferManager.SharedMaterialGroup();
							this.sharedMaterialGroups.Add(sharedMaterialGroup);
						}
						if (this.newMaterialsSet.Count > 0)
						{
							sharedMaterialGroup.MergeMaterials(this.newMaterialsSet);
							foreach (Material material2 in this.newMaterialsSet)
							{
								this.materialToGroupMap[material2] = sharedMaterialGroup;
							}
						}
						this.rendererToGroupMap[renderer] = sharedMaterialGroup;
						this.newMaterialsSet.Clear();
						this.tempMaterialsList.Clear();
					}
					sharedMaterialGroup.RegisterActiveRenderer(renderer);
					flag = true;
				}
			}
		}
		return flag;
	}

	// Token: 0x06009D03 RID: 40195 RVA: 0x003B4EFC File Offset: 0x003B30FC
	public void DeregisterActiveRenderer(Renderer renderer)
	{
		using (SpeedTreeWindHistoryBufferManager.s_ManagerTotal.Auto())
		{
			using (SpeedTreeWindHistoryBufferManager.s_ManagerDeregistrations.Auto())
			{
				SpeedTreeWindHistoryBufferManager.SharedMaterialGroup sharedMaterialGroup;
				if (this.rendererToGroupMap.TryGetValue(renderer, out sharedMaterialGroup))
				{
					sharedMaterialGroup.DeregisterActiveRenderer(renderer);
				}
			}
		}
	}

	// Token: 0x06009D04 RID: 40196 RVA: 0x003B4F78 File Offset: 0x003B3178
	public void Update()
	{
		using (SpeedTreeWindHistoryBufferManager.s_ManagerTotal.Auto())
		{
			using (SpeedTreeWindHistoryBufferManager.s_ManagerUpdate.Auto())
			{
				foreach (SpeedTreeWindHistoryBufferManager.SharedMaterialGroup sharedMaterialGroup in this.sharedMaterialGroups)
				{
					sharedMaterialGroup.Update();
				}
			}
		}
	}

	// Token: 0x040076D2 RID: 30418
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerMarker s_GetMatProps = new ProfilerMarker("SpeedTreeWindPropertyBuffer.GetMatProps");

	// Token: 0x040076D3 RID: 30419
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerMarker s_SetMatProps = new ProfilerMarker("SpeedTreeWindPropertyBuffer.SetMatProps");

	// Token: 0x040076D4 RID: 30420
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerMarker s_CheckVis = new ProfilerMarker("SpeedTreeWindPropertyBuffer.CheckVis");

	// Token: 0x040076D5 RID: 30421
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerMarker s_ManagerUpdate = new ProfilerMarker("SpeedTreeWindPropertyBuffer.ManagerUpdate");

	// Token: 0x040076D6 RID: 30422
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerMarker s_ManagerRegistrations = new ProfilerMarker("SpeedTreeWindPropertyBuffer.ManagerRegistrations");

	// Token: 0x040076D7 RID: 30423
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerMarker s_ManagerDeregistrations = new ProfilerMarker("SpeedTreeWindPropertyBuffer.ManagerDeregistrations");

	// Token: 0x040076D8 RID: 30424
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerMarker s_ManagerGetFirstRenderer = new ProfilerMarker("SpeedTreeWindPropertyBuffer.ManagerGetFirstRenderer");

	// Token: 0x040076D9 RID: 30425
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerMarker s_ManagerTotal = new ProfilerMarker("SpeedTreeWindPropertyBuffer.ManagerTotal");

	// Token: 0x040076DA RID: 30426
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_WindVector = Shader.PropertyToID("_ST_WindVector");

	// Token: 0x040076DB RID: 30427
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_WindGlobal = Shader.PropertyToID("_ST_WindGlobal");

	// Token: 0x040076DC RID: 30428
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_WindBranch = Shader.PropertyToID("_ST_WindBranch");

	// Token: 0x040076DD RID: 30429
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_WindBranchTwitch = Shader.PropertyToID("_ST_WindBranchTwitch");

	// Token: 0x040076DE RID: 30430
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_WindBranchWhip = Shader.PropertyToID("_ST_WindBranchWhip");

	// Token: 0x040076DF RID: 30431
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_WindBranchAnchor = Shader.PropertyToID("_ST_WindBranchAnchor");

	// Token: 0x040076E0 RID: 30432
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_WindBranchAdherences = Shader.PropertyToID("_ST_WindBranchAdherences");

	// Token: 0x040076E1 RID: 30433
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_WindTurbulences = Shader.PropertyToID("_ST_WindTurbulences");

	// Token: 0x040076E2 RID: 30434
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_WindLeaf1Ripple = Shader.PropertyToID("_ST_WindLeaf1Ripple");

	// Token: 0x040076E3 RID: 30435
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_WindLeaf1Tumble = Shader.PropertyToID("_ST_WindLeaf1Tumble");

	// Token: 0x040076E4 RID: 30436
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_WindLeaf1Twitch = Shader.PropertyToID("_ST_WindLeaf1Twitch");

	// Token: 0x040076E5 RID: 30437
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_WindLeaf2Ripple = Shader.PropertyToID("_ST_WindLeaf2Ripple");

	// Token: 0x040076E6 RID: 30438
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_WindLeaf2Tumble = Shader.PropertyToID("_ST_WindLeaf2Tumble");

	// Token: 0x040076E7 RID: 30439
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_WindLeaf2Twitch = Shader.PropertyToID("_ST_WindLeaf2Twitch");

	// Token: 0x040076E8 RID: 30440
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_WindFrondRipple = Shader.PropertyToID("_ST_WindFrondRipple");

	// Token: 0x040076E9 RID: 30441
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_WindAnimation = Shader.PropertyToID("_ST_WindAnimation");

	// Token: 0x040076EA RID: 30442
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_PF_WindVector = Shader.PropertyToID("_ST_PF_WindVector");

	// Token: 0x040076EB RID: 30443
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_PF_WindGlobal = Shader.PropertyToID("_ST_PF_WindGlobal");

	// Token: 0x040076EC RID: 30444
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_PF_WindBranch = Shader.PropertyToID("_ST_PF_WindBranch");

	// Token: 0x040076ED RID: 30445
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_PF_WindBranchTwitch = Shader.PropertyToID("_ST_PF_WindBranchTwitch");

	// Token: 0x040076EE RID: 30446
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_PF_WindBranchWhip = Shader.PropertyToID("_ST_PF_WindBranchWhip");

	// Token: 0x040076EF RID: 30447
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_PF_WindBranchAnchor = Shader.PropertyToID("_ST_PF_WindBranchAnchor");

	// Token: 0x040076F0 RID: 30448
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_PF_WindBranchAdherences = Shader.PropertyToID("_ST_PF_WindBranchAdherences");

	// Token: 0x040076F1 RID: 30449
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_PF_WindTurbulences = Shader.PropertyToID("_ST_PF_WindTurbulences");

	// Token: 0x040076F2 RID: 30450
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_PF_WindLeaf1Ripple = Shader.PropertyToID("_ST_PF_WindLeaf1Ripple");

	// Token: 0x040076F3 RID: 30451
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_PF_WindLeaf1Tumble = Shader.PropertyToID("_ST_PF_WindLeaf1Tumble");

	// Token: 0x040076F4 RID: 30452
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_PF_WindLeaf1Twitch = Shader.PropertyToID("_ST_PF_WindLeaf1Twitch");

	// Token: 0x040076F5 RID: 30453
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_PF_WindLeaf2Ripple = Shader.PropertyToID("_ST_PF_WindLeaf2Ripple");

	// Token: 0x040076F6 RID: 30454
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_PF_WindLeaf2Tumble = Shader.PropertyToID("_ST_PF_WindLeaf2Tumble");

	// Token: 0x040076F7 RID: 30455
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_PF_WindLeaf2Twitch = Shader.PropertyToID("_ST_PF_WindLeaf2Twitch");

	// Token: 0x040076F8 RID: 30456
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_PF_WindFrondRipple = Shader.PropertyToID("_ST_PF_WindFrondRipple");

	// Token: 0x040076F9 RID: 30457
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _ST_PF_WindAnimation = Shader.PropertyToID("_ST_PF_WindAnimation");

	// Token: 0x040076FA RID: 30458
	[PublicizedFrom(EAccessModifier.Private)]
	public static SpeedTreeWindHistoryBufferManager m_Instance;

	// Token: 0x040076FB RID: 30459
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<Renderer, SpeedTreeWindHistoryBufferManager.SharedMaterialGroup> rendererToGroupMap = new Dictionary<Renderer, SpeedTreeWindHistoryBufferManager.SharedMaterialGroup>();

	// Token: 0x040076FC RID: 30460
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<Material, SpeedTreeWindHistoryBufferManager.SharedMaterialGroup> materialToGroupMap = new Dictionary<Material, SpeedTreeWindHistoryBufferManager.SharedMaterialGroup>();

	// Token: 0x040076FD RID: 30461
	[PublicizedFrom(EAccessModifier.Private)]
	public List<Material> tempMaterialsList = new List<Material>();

	// Token: 0x040076FE RID: 30462
	[PublicizedFrom(EAccessModifier.Private)]
	public HashSet<Material> newMaterialsSet = new HashSet<Material>();

	// Token: 0x040076FF RID: 30463
	[PublicizedFrom(EAccessModifier.Private)]
	public HashSet<SpeedTreeWindHistoryBufferManager.SharedMaterialGroup> sharedMaterialGroups = new HashSet<SpeedTreeWindHistoryBufferManager.SharedMaterialGroup>();

	// Token: 0x02001374 RID: 4980
	public class SharedMaterialGroup
	{
		// Token: 0x06009D06 RID: 40198 RVA: 0x003B527D File Offset: 0x003B347D
		public SharedMaterialGroup()
		{
			this.previousProperties = new MaterialPropertyBlock();
		}

		// Token: 0x06009D07 RID: 40199 RVA: 0x003B52A6 File Offset: 0x003B34A6
		public void MergeMaterials(HashSet<Material> newMaterialsSet)
		{
			this.sharedMaterials.UnionWith(newMaterialsSet);
		}

		// Token: 0x06009D08 RID: 40200 RVA: 0x003B52B4 File Offset: 0x003B34B4
		public void RegisterActiveRenderer(Renderer renderer)
		{
			this.activeRenderers.Add(renderer);
		}

		// Token: 0x06009D09 RID: 40201 RVA: 0x003B52C3 File Offset: 0x003B34C3
		public void DeregisterActiveRenderer(Renderer renderer)
		{
			this.activeRenderers.Remove(renderer);
		}

		// Token: 0x06009D0A RID: 40202 RVA: 0x003B52D4 File Offset: 0x003B34D4
		public void Update()
		{
			using (SpeedTreeWindHistoryBufferManager.s_CheckVis.Auto())
			{
				if (this.activeRenderers.Count == 0)
				{
					return;
				}
			}
			Renderer renderer = null;
			using (SpeedTreeWindHistoryBufferManager.s_ManagerGetFirstRenderer.Auto())
			{
				using (HashSet<Renderer>.Enumerator enumerator = this.activeRenderers.GetEnumerator())
				{
					if (!enumerator.MoveNext())
					{
						return;
					}
					renderer = enumerator.Current;
				}
			}
			using (SpeedTreeWindHistoryBufferManager.s_GetMatProps.Auto())
			{
				renderer.GetPropertyBlock(this.previousProperties);
			}
			using (SpeedTreeWindHistoryBufferManager.s_SetMatProps.Auto())
			{
				foreach (Material material in this.sharedMaterials)
				{
					material.SetVector(SpeedTreeWindHistoryBufferManager._ST_PF_WindVector, this.previousProperties.GetVector(SpeedTreeWindHistoryBufferManager._ST_WindVector));
					material.SetVector(SpeedTreeWindHistoryBufferManager._ST_PF_WindGlobal, this.previousProperties.GetVector(SpeedTreeWindHistoryBufferManager._ST_WindGlobal));
					material.SetVector(SpeedTreeWindHistoryBufferManager._ST_PF_WindBranch, this.previousProperties.GetVector(SpeedTreeWindHistoryBufferManager._ST_WindBranch));
					material.SetVector(SpeedTreeWindHistoryBufferManager._ST_PF_WindBranchTwitch, this.previousProperties.GetVector(SpeedTreeWindHistoryBufferManager._ST_WindBranchTwitch));
					material.SetVector(SpeedTreeWindHistoryBufferManager._ST_PF_WindBranchWhip, this.previousProperties.GetVector(SpeedTreeWindHistoryBufferManager._ST_WindBranchWhip));
					material.SetVector(SpeedTreeWindHistoryBufferManager._ST_PF_WindBranchAnchor, this.previousProperties.GetVector(SpeedTreeWindHistoryBufferManager._ST_WindBranchAnchor));
					material.SetVector(SpeedTreeWindHistoryBufferManager._ST_PF_WindBranchAdherences, this.previousProperties.GetVector(SpeedTreeWindHistoryBufferManager._ST_WindBranchAdherences));
					material.SetVector(SpeedTreeWindHistoryBufferManager._ST_PF_WindTurbulences, this.previousProperties.GetVector(SpeedTreeWindHistoryBufferManager._ST_WindTurbulences));
					material.SetVector(SpeedTreeWindHistoryBufferManager._ST_PF_WindLeaf1Ripple, this.previousProperties.GetVector(SpeedTreeWindHistoryBufferManager._ST_WindLeaf1Ripple));
					material.SetVector(SpeedTreeWindHistoryBufferManager._ST_PF_WindLeaf1Tumble, this.previousProperties.GetVector(SpeedTreeWindHistoryBufferManager._ST_WindLeaf1Tumble));
					material.SetVector(SpeedTreeWindHistoryBufferManager._ST_PF_WindLeaf1Twitch, this.previousProperties.GetVector(SpeedTreeWindHistoryBufferManager._ST_WindLeaf1Twitch));
					material.SetVector(SpeedTreeWindHistoryBufferManager._ST_PF_WindLeaf2Ripple, this.previousProperties.GetVector(SpeedTreeWindHistoryBufferManager._ST_WindLeaf2Ripple));
					material.SetVector(SpeedTreeWindHistoryBufferManager._ST_PF_WindLeaf2Tumble, this.previousProperties.GetVector(SpeedTreeWindHistoryBufferManager._ST_WindLeaf2Tumble));
					material.SetVector(SpeedTreeWindHistoryBufferManager._ST_PF_WindLeaf2Twitch, this.previousProperties.GetVector(SpeedTreeWindHistoryBufferManager._ST_WindLeaf2Twitch));
					material.SetVector(SpeedTreeWindHistoryBufferManager._ST_PF_WindFrondRipple, this.previousProperties.GetVector(SpeedTreeWindHistoryBufferManager._ST_WindFrondRipple));
					material.SetVector(SpeedTreeWindHistoryBufferManager._ST_PF_WindAnimation, this.previousProperties.GetVector(SpeedTreeWindHistoryBufferManager._ST_WindAnimation));
				}
			}
		}

		// Token: 0x04007700 RID: 30464
		[PublicizedFrom(EAccessModifier.Private)]
		public HashSet<Renderer> activeRenderers = new HashSet<Renderer>();

		// Token: 0x04007701 RID: 30465
		[PublicizedFrom(EAccessModifier.Private)]
		public HashSet<Material> sharedMaterials = new HashSet<Material>();

		// Token: 0x04007702 RID: 30466
		[PublicizedFrom(EAccessModifier.Private)]
		public MaterialPropertyBlock previousProperties;
	}
}
