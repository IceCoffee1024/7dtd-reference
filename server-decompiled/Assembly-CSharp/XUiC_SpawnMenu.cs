using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001075 RID: 4213
[Preserve]
public class XUiC_SpawnMenu : XUiController
{
	// Token: 0x060085D5 RID: 34261 RVA: 0x003308F0 File Offset: 0x0032EAF0
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

	// Token: 0x060085D6 RID: 34262 RVA: 0x00330A5A File Offset: 0x0032EC5A
	public override void OnOpen()
	{
		base.OnOpen();
		XUiC_FocusedBlockHealth.SetData(this.xui.playerUI, null, 0f);
	}

	// Token: 0x060085D7 RID: 34263 RVA: 0x00330A78 File Offset: 0x0032EC78
	[PublicizedFrom(EAccessModifier.Private)]
	public void EntitiesList_SelectionChanged(XUiC_List<XUiC_SpawnEntitiesList.SpawnEntityEntry> _list, XUiC_SpawnEntitiesList.SpawnEntityEntry _previousEntry, XUiC_SpawnEntitiesList.SpawnEntityEntry _newEntry)
	{
		if (_newEntry != null)
		{
			this.entitiesList.ClearSelection();
			this.BtnSpawns_OnPress(_newEntry.key);
		}
	}

	// Token: 0x060085D8 RID: 34264 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void ToggleLookAtYou_OnValueChanged(XUiC_ToggleButton _sender, bool _newValue)
	{
	}

	// Token: 0x060085D9 RID: 34265 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void ToggleSpawn25_OnValueChanged(XUiC_ToggleButton _sender, bool _newValue)
	{
	}

	// Token: 0x060085DA RID: 34266 RVA: 0x00330AA4 File Offset: 0x0032ECA4
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

	// Token: 0x060085DB RID: 34267 RVA: 0x00330BBF File Offset: 0x0032EDBF
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

	// Token: 0x060085DC RID: 34268 RVA: 0x00330BE9 File Offset: 0x0032EDE9
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

	// Token: 0x060085DD RID: 34269 RVA: 0x00330C13 File Offset: 0x0032EE13
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

	// Token: 0x060085DE RID: 34270 RVA: 0x00330C40 File Offset: 0x0032EE40
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnSpawns_OnPress(int _key)
	{
		Vector3 hitPoint = this.GetHitPoint();
		this.Spawn(_key, hitPoint);
	}

	// Token: 0x060085DF RID: 34271 RVA: 0x00330C60 File Offset: 0x0032EE60
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

	// Token: 0x060085E0 RID: 34272 RVA: 0x00330D14 File Offset: 0x0032EF14
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

	// Token: 0x04006376 RID: 25462
	public static string ID = "";

	// Token: 0x04006377 RID: 25463
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SpawnEntitiesList entitiesList;

	// Token: 0x04006378 RID: 25464
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ToggleButton toggleLookAtYou;

	// Token: 0x04006379 RID: 25465
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ToggleButton toggleSpawn25;

	// Token: 0x0400637A RID: 25466
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ToggleButton toggleFromDynamic;

	// Token: 0x0400637B RID: 25467
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ToggleButton toggleFromStatic;

	// Token: 0x0400637C RID: 25468
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ToggleButton toggleFromBiome;

	// Token: 0x0400637D RID: 25469
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton spawnFiltered;
}
