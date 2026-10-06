using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001086 RID: 4230
[Preserve]
public class XUiC_SpawnMenu : XUiController
{
	// Token: 0x0600861B RID: 34331 RVA: 0x003344FC File Offset: 0x003326FC
	public override void Init()
	{
		base.Init();
		XUiC_SpawnMenu.ID = base.WindowGroup.Id;
		this.toggleLookAtYou = base.GetChildById("toggleLookAtYou").GetChildByType<XUiC_ToggleButton>();
		this.toggleLookAtYou.OnValueChanged += this.ToggleLookAtYou_OnValueChanged;
		this.toggleSpawn25 = base.GetChildById("toggleSpawn25").GetChildByType<XUiC_ToggleButton>();
		this.toggleSpawn25.OnValueChanged += this.ToggleSpawn25_OnValueChanged;
		this.toggleFromDynamic = base.GetChildById("toggleFromDynamic").GetChildByType<XUiC_ToggleButton>();
		this.toggleFromDynamic.OnValueChanged += this.ToggleFromDynamic_OnValueChanged;
		this.toggleFromStatic = base.GetChildById("toggleFromStatic").GetChildByType<XUiC_ToggleButton>();
		this.toggleFromStatic.OnValueChanged += this.ToggleFromStatic_OnValueChanged;
		this.toggleFromBiome = base.GetChildById("toggleFromBiome").GetChildByType<XUiC_ToggleButton>();
		this.toggleFromBiome.OnValueChanged += this.ToggleFromBiome_OnValueChanged;
		this.spawnFiltered = base.GetChildById("spawnFiltered").GetChildByType<XUiC_SimpleButton>();
		this.spawnFiltered.OnPressed += this.SpawnFiltered_OnPressed;
		this.entitiesList = (XUiC_SpawnEntitiesList)base.GetChildById("entities");
		this.entitiesList.SelectionChanged += this.EntitiesList_SelectionChanged;
		this.toggleFromDynamic.Value = true;
	}

	// Token: 0x0600861C RID: 34332 RVA: 0x00334666 File Offset: 0x00332866
	public override void OnOpen()
	{
		base.OnOpen();
		XUiC_FocusedBlockHealth.SetData(this.xui.playerUI, null, 0f);
	}

	// Token: 0x0600861D RID: 34333 RVA: 0x00334684 File Offset: 0x00332884
	[PublicizedFrom(EAccessModifier.Private)]
	public void EntitiesList_SelectionChanged(XUiC_List<XUiC_SpawnEntitiesList.SpawnEntityEntry> _list, XUiC_SpawnEntitiesList.SpawnEntityEntry _previousEntry, XUiC_SpawnEntitiesList.SpawnEntityEntry _newEntry)
	{
		if (_newEntry != null)
		{
			this.entitiesList.ClearSelection();
			this.BtnSpawns_OnPress(_newEntry.key);
		}
	}

	// Token: 0x0600861E RID: 34334 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void ToggleLookAtYou_OnValueChanged(XUiC_ToggleButton _sender, bool _newValue)
	{
	}

	// Token: 0x0600861F RID: 34335 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void ToggleSpawn25_OnValueChanged(XUiC_ToggleButton _sender, bool _newValue)
	{
	}

	// Token: 0x06008620 RID: 34336 RVA: 0x003346B0 File Offset: 0x003328B0
	[PublicizedFrom(EAccessModifier.Private)]
	public void SpawnFiltered_OnPressed(XUiController _sender, int _mouseButton)
	{
		int num = 0;
		Vector3 hitPoint = this.GetHitPoint();
		float y = GameManager.Instance.World.GetPrimaryPlayer().finalCamera.transform.eulerAngles.y;
		int entryCount = this.entitiesList.EntryCount;
		float num2 = 0f;
		float num3 = 45f;
		float num4 = (float)((entryCount <= 1) ? 0 : 2);
		for (int i = 0; i < entryCount; i++)
		{
			XUiC_SpawnEntitiesList.SpawnEntityEntry entry = this.entitiesList.GetEntry(i);
			Vector3 vector = hitPoint;
			float num5 = (num2 + y) * 0.017453292f;
			vector.x += Mathf.Sin(num5) * num4;
			vector.z += Mathf.Cos(num5) * num4;
			num += this.Spawn(entry.key, vector);
			if (num > 200)
			{
				break;
			}
			num2 += num3;
			if (num2 > 359f)
			{
				num2 = 0f;
				num3 /= 2f;
				num4 += 2f;
			}
		}
		Log.Out("Spawned {0}", new object[] { num });
	}

	// Token: 0x06008621 RID: 34337 RVA: 0x003347CB File Offset: 0x003329CB
	[PublicizedFrom(EAccessModifier.Private)]
	public void ToggleFromDynamic_OnValueChanged(XUiC_ToggleButton _sender, bool _newValue)
	{
		if (_newValue)
		{
			this.toggleFromStatic.Value = false;
			this.toggleFromBiome.Value = false;
			return;
		}
		this.toggleFromDynamic.Value = true;
	}

	// Token: 0x06008622 RID: 34338 RVA: 0x003347F5 File Offset: 0x003329F5
	[PublicizedFrom(EAccessModifier.Private)]
	public void ToggleFromStatic_OnValueChanged(XUiC_ToggleButton _sender, bool _newValue)
	{
		if (_newValue)
		{
			this.toggleFromDynamic.Value = false;
			this.toggleFromBiome.Value = false;
			return;
		}
		this.toggleFromStatic.Value = true;
	}

	// Token: 0x06008623 RID: 34339 RVA: 0x0033481F File Offset: 0x00332A1F
	[PublicizedFrom(EAccessModifier.Private)]
	public void ToggleFromBiome_OnValueChanged(XUiC_ToggleButton _sender, bool _newValue)
	{
		if (_newValue)
		{
			this.toggleFromDynamic.Value = false;
			this.toggleFromStatic.Value = false;
			return;
		}
		this.toggleFromBiome.Value = true;
	}

	// Token: 0x06008624 RID: 34340 RVA: 0x0033484C File Offset: 0x00332A4C
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnSpawns_OnPress(int _key)
	{
		Vector3 hitPoint = this.GetHitPoint();
		this.Spawn(_key, hitPoint);
	}

	// Token: 0x06008625 RID: 34341 RVA: 0x0033486C File Offset: 0x00332A6C
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 GetHitPoint()
	{
		EntityPlayerLocal primaryPlayer = GameManager.Instance.World.GetPrimaryPlayer();
		float num = (float)(primaryPlayer.AttachedToEntity ? 2 : 0);
		Vector3 vector = XUiC_LevelTools3Window.getRaycastHitPoint(100f, num);
		if (vector.Equals(Vector3.zero))
		{
			Ray ray = primaryPlayer.finalCamera.ScreenPointToRay(new Vector3((float)Screen.width * 0.5f, (float)Screen.height * 0.5f, 0f));
			vector = ray.origin + ray.direction * 10f + Origin.position;
		}
		vector.y += 0.25f;
		return vector;
	}

	// Token: 0x06008626 RID: 34342 RVA: 0x00334920 File Offset: 0x00332B20
	[PublicizedFrom(EAccessModifier.Private)]
	public int Spawn(int _key, Vector3 _spawnPos)
	{
		Camera finalCamera = GameManager.Instance.World.GetPrimaryPlayer().finalCamera;
		Vector3 vector = new Vector3(0f, this.toggleLookAtYou.Value ? (finalCamera.transform.eulerAngles.y + 180f) : finalCamera.transform.eulerAngles.y, 0f);
		EnumSpawnerSource enumSpawnerSource = EnumSpawnerSource.Unknown;
		if (this.toggleFromDynamic.Value)
		{
			enumSpawnerSource = EnumSpawnerSource.Dynamic;
		}
		if (this.toggleFromStatic.Value)
		{
			enumSpawnerSource = EnumSpawnerSource.StaticSpawner;
		}
		if (this.toggleFromBiome.Value)
		{
			enumSpawnerSource = EnumSpawnerSource.Biome;
		}
		int num = (this.toggleSpawn25.Value ? 25 : 1);
		if (InputUtils.ShiftKeyPressed)
		{
			num = 5;
		}
		Vector3 vector2 = finalCamera.transform.right;
		if (!InputUtils.AltKeyPressed)
		{
			vector2 *= 0.01f;
		}
		if (_key == EntityClass.junkDroneClass)
		{
			num = 1;
		}
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			_spawnPos -= vector2 * ((float)(num - 1) * 0.5f);
			for (int i = 0; i < num; i++)
			{
				Entity entity = EntityFactory.CreateEntity(_key, _spawnPos, vector);
				entity.SetSpawnerSource(enumSpawnerSource);
				GameManager.Instance.World.SpawnEntityInWorld(entity);
				_spawnPos += vector2;
			}
		}
		else
		{
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageConsoleCmdServer>().Setup(string.Concat(new string[]
			{
				"spawnentityat \"",
				EntityClass.list[_key].entityClassName,
				"\" ",
				_spawnPos.x.ToCultureInvariantString(),
				" ",
				_spawnPos.y.ToCultureInvariantString(),
				" ",
				_spawnPos.z.ToCultureInvariantString(),
				" ",
				num.ToString(),
				" ",
				vector.x.ToCultureInvariantString(),
				" ",
				vector.y.ToCultureInvariantString(),
				" ",
				vector.z.ToCultureInvariantString(),
				" ",
				vector2.x.ToCultureInvariantString(),
				" ",
				vector2.y.ToCultureInvariantString(),
				" ",
				vector2.z.ToCultureInvariantString(),
				" ",
				enumSpawnerSource.ToStringCached<EnumSpawnerSource>()
			})), false);
		}
		return num;
	}

	// Token: 0x040063BB RID: 25531
	public static string ID = "";

	// Token: 0x040063BC RID: 25532
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SpawnEntitiesList entitiesList;

	// Token: 0x040063BD RID: 25533
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ToggleButton toggleLookAtYou;

	// Token: 0x040063BE RID: 25534
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ToggleButton toggleSpawn25;

	// Token: 0x040063BF RID: 25535
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ToggleButton toggleFromDynamic;

	// Token: 0x040063C0 RID: 25536
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ToggleButton toggleFromStatic;

	// Token: 0x040063C1 RID: 25537
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ToggleButton toggleFromBiome;

	// Token: 0x040063C2 RID: 25538
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton spawnFiltered;
}
