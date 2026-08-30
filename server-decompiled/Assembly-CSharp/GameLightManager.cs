using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001325 RID: 4901
public class GameLightManager
{
	// Token: 0x06009A9F RID: 39583 RVA: 0x003A6273 File Offset: 0x003A4473
	public static GameLightManager Create(EntityPlayerLocal player)
	{
		GameLightManager gameLightManager = new GameLightManager();
		gameLightManager.player = player;
		gameLightManager.Init();
		return gameLightManager;
	}

	// Token: 0x06009AA0 RID: 39584 RVA: 0x003A6287 File Offset: 0x003A4487
	[PublicizedFrom(EAccessModifier.Private)]
	public void Init()
	{
		GameLightManager.Instance = this;
		this.UpdateLightInit();
	}

	// Token: 0x06009AA1 RID: 39585 RVA: 0x003A6295 File Offset: 0x003A4495
	public void Destroy()
	{
		this.lights.Clear();
		this.priorityLights.Clear();
		this.removeLights.Clear();
		this.UpdateLightCleanup();
		GameLightManager.Instance = null;
	}

	// Token: 0x06009AA2 RID: 39586 RVA: 0x003A62C4 File Offset: 0x003A44C4
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

	// Token: 0x06009AA3 RID: 39587 RVA: 0x003A64A4 File Offset: 0x003A46A4
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

	// Token: 0x06009AA4 RID: 39588 RVA: 0x003A64DB File Offset: 0x003A46DB
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

	// Token: 0x06009AA5 RID: 39589 RVA: 0x003A650C File Offset: 0x003A470C
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

	// Token: 0x06009AA6 RID: 39590 RVA: 0x003A6565 File Offset: 0x003A4765
	public void MakeLightAPriority(LightLOD lightLOD)
	{
		if (lightLOD.priority <= 0f)
		{
			lightLOD.priority = 1f;
			this.priorityLights.Add(lightLOD);
		}
	}

	// Token: 0x06009AA7 RID: 39591 RVA: 0x003A658B File Offset: 0x003A478B
	public Vector3 CameraPos()
	{
		return this.player.cameraTransform.position;
	}

	// Token: 0x06009AA8 RID: 39592 RVA: 0x003A659D File Offset: 0x003A479D
	public void HandleWaterLevelChanged()
	{
		this.isWaterLevelChanged = true;
	}

	// Token: 0x06009AA9 RID: 39593 RVA: 0x003A65A8 File Offset: 0x003A47A8
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

	// Token: 0x06009AAA RID: 39594 RVA: 0x003A65FC File Offset: 0x003A47FC
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

	// Token: 0x06009AAB RID: 39595 RVA: 0x003A664C File Offset: 0x003A484C
	public void AddUpdateLight(UpdateLight _ul)
	{
		this.newULs.Add(_ul);
	}

	// Token: 0x06009AAC RID: 39596 RVA: 0x003A665C File Offset: 0x003A485C
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

	// Token: 0x06009AAD RID: 39597 RVA: 0x003A66E8 File Offset: 0x003A48E8
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

	// Token: 0x04007478 RID: 29816
	public static GameLightManager Instance;

	// Token: 0x04007479 RID: 29817
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayerLocal player;

	// Token: 0x0400747A RID: 29818
	[PublicizedFrom(EAccessModifier.Private)]
	public List<LightLOD> lights = new List<LightLOD>();

	// Token: 0x0400747B RID: 29819
	[PublicizedFrom(EAccessModifier.Private)]
	public List<LightLOD> priorityLights = new List<LightLOD>();

	// Token: 0x0400747C RID: 29820
	[PublicizedFrom(EAccessModifier.Private)]
	public List<LightLOD> removeLights = new List<LightLOD>();

	// Token: 0x0400747D RID: 29821
	[PublicizedFrom(EAccessModifier.Private)]
	public int lightUpdateIndex;

	// Token: 0x0400747E RID: 29822
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isUpdating;

	// Token: 0x0400747F RID: 29823
	[PublicizedFrom(EAccessModifier.Private)]
	public volatile bool isWaterLevelChanged;

	// Token: 0x04007480 RID: 29824
	[PublicizedFrom(EAccessModifier.Private)]
	public List<UpdateLight> newULs = new List<UpdateLight>(512);

	// Token: 0x04007481 RID: 29825
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cFastULGroups = 4;

	// Token: 0x04007482 RID: 29826
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cFastULGroupMask = 3;

	// Token: 0x04007483 RID: 29827
	[PublicizedFrom(EAccessModifier.Private)]
	public List<UpdateLight>[] fastULs = new List<UpdateLight>[4];

	// Token: 0x04007484 RID: 29828
	[PublicizedFrom(EAccessModifier.Private)]
	public int fastULUpdateIndex;

	// Token: 0x04007485 RID: 29829
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cULGroups = 64;

	// Token: 0x04007486 RID: 29830
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cSlowULGroupMask = 63;

	// Token: 0x04007487 RID: 29831
	[PublicizedFrom(EAccessModifier.Private)]
	public List<UpdateLight>[] slowULs = new List<UpdateLight>[64];

	// Token: 0x04007488 RID: 29832
	[PublicizedFrom(EAccessModifier.Private)]
	public int slowULUpdateIndex;
}
