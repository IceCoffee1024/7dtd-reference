using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200135F RID: 4959
public class GameLightManager
{
	// Token: 0x06009C46 RID: 40006 RVA: 0x003B0087 File Offset: 0x003AE287
	public static GameLightManager Create(EntityPlayerLocal player)
	{
		GameLightManager gameLightManager = new GameLightManager();
		gameLightManager.player = player;
		gameLightManager.Init();
		return gameLightManager;
	}

	// Token: 0x06009C47 RID: 40007 RVA: 0x003B009B File Offset: 0x003AE29B
	[PublicizedFrom(EAccessModifier.Private)]
	public void Init()
	{
		GameLightManager.Instance = this;
		this.UpdateLightInit();
	}

	// Token: 0x06009C48 RID: 40008 RVA: 0x003B00A9 File Offset: 0x003AE2A9
	public void Destroy()
	{
		this.lights.Clear();
		this.priorityLights.Clear();
		this.removeLights.Clear();
		this.UpdateLightCleanup();
		GameLightManager.Instance = null;
	}

	// Token: 0x06009C49 RID: 40009 RVA: 0x003B00D8 File Offset: 0x003AE2D8
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

	// Token: 0x06009C4A RID: 40010 RVA: 0x003B02B8 File Offset: 0x003AE4B8
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

	// Token: 0x06009C4B RID: 40011 RVA: 0x003B02EF File Offset: 0x003AE4EF
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

	// Token: 0x06009C4C RID: 40012 RVA: 0x003B0320 File Offset: 0x003AE520
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

	// Token: 0x06009C4D RID: 40013 RVA: 0x003B0379 File Offset: 0x003AE579
	public void MakeLightAPriority(LightLOD lightLOD)
	{
		if (lightLOD.priority <= 0f)
		{
			lightLOD.priority = 1f;
			this.priorityLights.Add(lightLOD);
		}
	}

	// Token: 0x06009C4E RID: 40014 RVA: 0x003B039F File Offset: 0x003AE59F
	public Vector3 CameraPos()
	{
		return this.player.cameraTransform.position;
	}

	// Token: 0x06009C4F RID: 40015 RVA: 0x003B03B1 File Offset: 0x003AE5B1
	public void HandleWaterLevelChanged()
	{
		this.isWaterLevelChanged = true;
	}

	// Token: 0x06009C50 RID: 40016 RVA: 0x003B03BC File Offset: 0x003AE5BC
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

	// Token: 0x06009C51 RID: 40017 RVA: 0x003B0410 File Offset: 0x003AE610
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

	// Token: 0x06009C52 RID: 40018 RVA: 0x003B0460 File Offset: 0x003AE660
	public void AddUpdateLight(UpdateLight _ul)
	{
		this.newULs.Add(_ul);
	}

	// Token: 0x06009C53 RID: 40019 RVA: 0x003B0470 File Offset: 0x003AE670
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

	// Token: 0x06009C54 RID: 40020 RVA: 0x003B04FC File Offset: 0x003AE6FC
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

	// Token: 0x040075CB RID: 30155
	public static GameLightManager Instance;

	// Token: 0x040075CC RID: 30156
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayerLocal player;

	// Token: 0x040075CD RID: 30157
	[PublicizedFrom(EAccessModifier.Private)]
	public List<LightLOD> lights = new List<LightLOD>();

	// Token: 0x040075CE RID: 30158
	[PublicizedFrom(EAccessModifier.Private)]
	public List<LightLOD> priorityLights = new List<LightLOD>();

	// Token: 0x040075CF RID: 30159
	[PublicizedFrom(EAccessModifier.Private)]
	public List<LightLOD> removeLights = new List<LightLOD>();

	// Token: 0x040075D0 RID: 30160
	[PublicizedFrom(EAccessModifier.Private)]
	public int lightUpdateIndex;

	// Token: 0x040075D1 RID: 30161
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isUpdating;

	// Token: 0x040075D2 RID: 30162
	[PublicizedFrom(EAccessModifier.Private)]
	public volatile bool isWaterLevelChanged;

	// Token: 0x040075D3 RID: 30163
	[PublicizedFrom(EAccessModifier.Private)]
	public List<UpdateLight> newULs = new List<UpdateLight>(512);

	// Token: 0x040075D4 RID: 30164
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cFastULGroups = 4;

	// Token: 0x040075D5 RID: 30165
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cFastULGroupMask = 3;

	// Token: 0x040075D6 RID: 30166
	[PublicizedFrom(EAccessModifier.Private)]
	public List<UpdateLight>[] fastULs = new List<UpdateLight>[4];

	// Token: 0x040075D7 RID: 30167
	[PublicizedFrom(EAccessModifier.Private)]
	public int fastULUpdateIndex;

	// Token: 0x040075D8 RID: 30168
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cULGroups = 64;

	// Token: 0x040075D9 RID: 30169
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cSlowULGroupMask = 63;

	// Token: 0x040075DA RID: 30170
	[PublicizedFrom(EAccessModifier.Private)]
	public List<UpdateLight>[] slowULs = new List<UpdateLight>[64];

	// Token: 0x040075DB RID: 30171
	[PublicizedFrom(EAccessModifier.Private)]
	public int slowULUpdateIndex;
}
