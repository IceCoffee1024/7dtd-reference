using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001371 RID: 4977
public class GameLightManager
{
	// Token: 0x06009C9E RID: 40094 RVA: 0x003B248B File Offset: 0x003B068B
	public static GameLightManager Create(EntityPlayerLocal player)
	{
		GameLightManager gameLightManager = new GameLightManager();
		gameLightManager.player = player;
		gameLightManager.Init();
		return gameLightManager;
	}

	// Token: 0x06009C9F RID: 40095 RVA: 0x003B249F File Offset: 0x003B069F
	[PublicizedFrom(EAccessModifier.Private)]
	public void Init()
	{
		GameLightManager.Instance = this;
		this.UpdateLightInit();
	}

	// Token: 0x06009CA0 RID: 40096 RVA: 0x003B24AD File Offset: 0x003B06AD
	public void Destroy()
	{
		this.lights.Clear();
		this.priorityLights.Clear();
		this.removeLights.Clear();
		this.UpdateLightCleanup();
		GameLightManager.Instance = null;
	}

	// Token: 0x06009CA1 RID: 40097 RVA: 0x003B24DC File Offset: 0x003B06DC
	public void FrameUpdate()
	{
		if (GameManager.IsDedicatedServer)
		{
			return;
		}
		if (GameManager.Instance.World == null)
		{
			return;
		}
		if (MeshDescription.bDebugStability || LightViewer.IsAllOff)
		{
			return;
		}
		this.isUpdating = true;
		Vector3 position = this.player.cameraTransform.position;
		int count = this.lights.Count;
		int num = (count + 19) / 20;
		if (this.lightUpdateIndex >= count)
		{
			this.lightUpdateIndex = 0;
		}
		for (int i = 0; i < num; i++)
		{
			LightLOD lightLOD = this.lights[this.lightUpdateIndex];
			if (lightLOD.priority <= 0f)
			{
				lightLOD.FrameUpdate(position);
				if (lightLOD.priority > 0f)
				{
					this.priorityLights.Add(lightLOD);
				}
			}
			int num2 = this.lightUpdateIndex + 1;
			this.lightUpdateIndex = num2;
			if (num2 >= count)
			{
				this.lightUpdateIndex = 0;
			}
		}
		int j = 0;
		while (j < this.priorityLights.Count)
		{
			LightLOD lightLOD2 = this.priorityLights[j];
			lightLOD2.FrameUpdate(position);
			if (lightLOD2.priority <= 0f)
			{
				this.priorityLights.RemoveAt(j);
			}
			else
			{
				j++;
			}
		}
		int count2 = this.removeLights.Count;
		if (count2 > 0)
		{
			for (int k = count2 - 1; k >= 0; k--)
			{
				LightLOD lightLOD3 = this.removeLights[k];
				this.RemoveLightFromLists(lightLOD3);
			}
			this.removeLights.Clear();
		}
		if (this.isWaterLevelChanged)
		{
			this.isWaterLevelChanged = false;
			foreach (LightLOD lightLOD4 in this.lights)
			{
				if (!lightLOD4.bWorksUnderwater)
				{
					lightLOD4.WaterLevelDirty = true;
				}
			}
		}
		this.isUpdating = false;
		this.UpdateLightFrameUpdate();
	}

	// Token: 0x06009CA2 RID: 40098 RVA: 0x003B26BC File Offset: 0x003B08BC
	public void AddLight(LightLOD lightLOD)
	{
		this.lights.Add(lightLOD);
		lightLOD.priority = 1f;
		this.priorityLights.Add(lightLOD);
		if (OcclusionManager.Instance.cullLights)
		{
			OcclusionManager.AddLight(lightLOD);
		}
	}

	// Token: 0x06009CA3 RID: 40099 RVA: 0x003B26F3 File Offset: 0x003B08F3
	public void RemoveLight(LightLOD lightLOD)
	{
		if (this.isUpdating)
		{
			this.removeLights.Add(lightLOD);
		}
		else
		{
			this.RemoveLightFromLists(lightLOD);
		}
		if (OcclusionManager.Instance.cullLights)
		{
			OcclusionManager.RemoveLight(lightLOD);
		}
	}

	// Token: 0x06009CA4 RID: 40100 RVA: 0x003B2724 File Offset: 0x003B0924
	[PublicizedFrom(EAccessModifier.Private)]
	public void RemoveLightFromLists(LightLOD lightLOD)
	{
		int num = this.lights.IndexOf(lightLOD);
		if (num < 0)
		{
			Log.Warning("RemoveLightFromLists none");
			return;
		}
		this.lights.RemoveAt(num);
		if (num < this.lightUpdateIndex)
		{
			this.lightUpdateIndex--;
		}
		this.priorityLights.Remove(lightLOD);
	}

	// Token: 0x06009CA5 RID: 40101 RVA: 0x003B277D File Offset: 0x003B097D
	public void MakeLightAPriority(LightLOD lightLOD)
	{
		if (lightLOD.priority <= 0f)
		{
			lightLOD.priority = 1f;
			this.priorityLights.Add(lightLOD);
		}
	}

	// Token: 0x06009CA6 RID: 40102 RVA: 0x003B27A3 File Offset: 0x003B09A3
	public Vector3 CameraPos()
	{
		return this.player.cameraTransform.position;
	}

	// Token: 0x06009CA7 RID: 40103 RVA: 0x003B27B5 File Offset: 0x003B09B5
	public void HandleWaterLevelChanged()
	{
		this.isWaterLevelChanged = true;
	}

	// Token: 0x06009CA8 RID: 40104 RVA: 0x003B27C0 File Offset: 0x003B09C0
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateLightInit()
	{
		for (int i = 0; i < this.fastULs.Length; i++)
		{
			this.fastULs[i] = new List<UpdateLight>(64);
		}
		for (int j = 0; j < this.slowULs.Length; j++)
		{
			this.slowULs[j] = new List<UpdateLight>(256);
		}
	}

	// Token: 0x06009CA9 RID: 40105 RVA: 0x003B2814 File Offset: 0x003B0A14
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateLightCleanup()
	{
		this.newULs.Clear();
		for (int i = 0; i < this.fastULs.Length; i++)
		{
			this.fastULs[i] = null;
		}
		for (int j = 0; j < this.slowULs.Length; j++)
		{
			this.slowULs[j] = null;
		}
	}

	// Token: 0x06009CAA RID: 40106 RVA: 0x003B2864 File Offset: 0x003B0A64
	public void AddUpdateLight(UpdateLight _ul)
	{
		this.newULs.Add(_ul);
	}

	// Token: 0x06009CAB RID: 40107 RVA: 0x003B2874 File Offset: 0x003B0A74
	public void RemoveUpdateLight(UpdateLight _ul)
	{
		bool flag;
		if (_ul.IsDynamicObject)
		{
			int num = (_ul.GetHashCode() >> 2) & 3;
			flag = this.fastULs[num].Remove(_ul);
		}
		else
		{
			int num2 = (_ul.GetHashCode() >> 2) & 63;
			flag = this.slowULs[num2].Remove(_ul);
		}
		if (!flag && !this.newULs.Remove(_ul))
		{
			Log.Warning("RemoveUpdateLight {0} dy{1} missing!", new object[]
			{
				_ul.transform.name,
				_ul.IsDynamicObject
			});
		}
	}

	// Token: 0x06009CAC RID: 40108 RVA: 0x003B2900 File Offset: 0x003B0B00
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateLightFrameUpdate()
	{
		if (GameManager.Instance == null || GameManager.Instance.World == null || !GameManager.Instance.gameStateManager.IsGameStarted())
		{
			return;
		}
		float num = Time.deltaTime * 4f;
		this.fastULUpdateIndex = (this.fastULUpdateIndex + 1) & 3;
		List<UpdateLight> list = this.fastULs[this.fastULUpdateIndex];
		for (int i = list.Count - 1; i >= 0; i--)
		{
			UpdateLight updateLight = list[i];
			if (updateLight)
			{
				updateLight.UpdateLighting(num);
			}
			else
			{
				list.RemoveAt(i);
			}
		}
		this.slowULUpdateIndex = (this.slowULUpdateIndex + 1) & 63;
		List<UpdateLight> list2 = this.slowULs[this.slowULUpdateIndex];
		for (int j = list2.Count - 1; j >= 0; j--)
		{
			UpdateLight updateLight2 = list2[j];
			if (updateLight2)
			{
				if (updateLight2.appliedLit < 0f)
				{
					updateLight2.UpdateLighting(1f);
				}
			}
			else
			{
				list2.RemoveAt(j);
			}
		}
		int num2 = Utils.FastMin(160, this.newULs.Count);
		for (int k = 0; k < num2; k++)
		{
			UpdateLight updateLight3 = this.newULs[k];
			if (updateLight3)
			{
				updateLight3.ManagerFirstUpdate();
				int num3 = updateLight3.GetHashCode() >> 2;
				if (updateLight3.IsDynamicObject)
				{
					this.fastULs[num3 & 3].Add(updateLight3);
				}
				else
				{
					this.slowULs[num3 & 63].Add(updateLight3);
				}
			}
		}
		this.newULs.RemoveRange(0, num2);
	}

	// Token: 0x0400760C RID: 30220
	public static GameLightManager Instance;

	// Token: 0x0400760D RID: 30221
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayerLocal player;

	// Token: 0x0400760E RID: 30222
	[PublicizedFrom(EAccessModifier.Private)]
	public List<LightLOD> lights = new List<LightLOD>();

	// Token: 0x0400760F RID: 30223
	[PublicizedFrom(EAccessModifier.Private)]
	public List<LightLOD> priorityLights = new List<LightLOD>();

	// Token: 0x04007610 RID: 30224
	[PublicizedFrom(EAccessModifier.Private)]
	public List<LightLOD> removeLights = new List<LightLOD>();

	// Token: 0x04007611 RID: 30225
	[PublicizedFrom(EAccessModifier.Private)]
	public int lightUpdateIndex;

	// Token: 0x04007612 RID: 30226
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isUpdating;

	// Token: 0x04007613 RID: 30227
	[PublicizedFrom(EAccessModifier.Private)]
	public volatile bool isWaterLevelChanged;

	// Token: 0x04007614 RID: 30228
	[PublicizedFrom(EAccessModifier.Private)]
	public List<UpdateLight> newULs = new List<UpdateLight>(512);

	// Token: 0x04007615 RID: 30229
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cFastULGroups = 4;

	// Token: 0x04007616 RID: 30230
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cFastULGroupMask = 3;

	// Token: 0x04007617 RID: 30231
	[PublicizedFrom(EAccessModifier.Private)]
	public List<UpdateLight>[] fastULs = new List<UpdateLight>[4];

	// Token: 0x04007618 RID: 30232
	[PublicizedFrom(EAccessModifier.Private)]
	public int fastULUpdateIndex;

	// Token: 0x04007619 RID: 30233
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cULGroups = 64;

	// Token: 0x0400761A RID: 30234
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cSlowULGroupMask = 63;

	// Token: 0x0400761B RID: 30235
	[PublicizedFrom(EAccessModifier.Private)]
	public List<UpdateLight>[] slowULs = new List<UpdateLight>[64];

	// Token: 0x0400761C RID: 30236
	[PublicizedFrom(EAccessModifier.Private)]
	public int slowULUpdateIndex;
}
