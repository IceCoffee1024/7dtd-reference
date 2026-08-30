using System;
using System.Collections.Generic;
using System.Text;

namespace PrefabVolumes
{
	// Token: 0x020018B0 RID: 6320
	public class PrefabTeleportVolumeList : PrefabVolumeListAbs<PrefabTeleportVolumeList, PrefabTeleportVolume>
	{
		// Token: 0x0600C323 RID: 49955 RVA: 0x004820DB File Offset: 0x004802DB
		public PrefabTeleportVolumeList(Prefab _owner)
			: base(_owner)
		{
		}

		// Token: 0x170017FA RID: 6138
		// (get) Token: 0x0600C324 RID: 49956 RVA: 0x00020021 File Offset: 0x0001E221
		public override PrefabVolumeAbs.EVolumeType VolumeType
		{
			get
			{
				return PrefabVolumeAbs.EVolumeType.Teleport;
			}
		}

		// Token: 0x170017FB RID: 6139
		// (get) Token: 0x0600C325 RID: 49957 RVA: 0x004820E4 File Offset: 0x004802E4
		public override SelectionCategory SelectionCategory
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return SelectionBoxManager.Instance.CategoryTraderTeleport;
			}
		}

		// Token: 0x0600C326 RID: 49958 RVA: 0x004820F0 File Offset: 0x004802F0
		public override void ReadFromProperties(DynamicProperties _properties)
		{
			this.List.Clear();
			Dictionary<string, string> values = _properties.Values;
			if (!values.ContainsKey("TeleportVolumeSize") || !values.ContainsKey("TeleportVolumeStart"))
			{
				return;
			}
			List<Vector3i> list = StringParsers.ParseList<Vector3i>(values["TeleportVolumeSize"], '#', (string _s, int _start, int _end) => StringParsers.ParseVector3i(_s, _start, _end, false));
			List<Vector3i> list2 = StringParsers.ParseList<Vector3i>(values["TeleportVolumeStart"], '#', (string _s, int _start, int _end) => StringParsers.ParseVector3i(_s, _start, _end, false));
			for (int i = 0; i < list2.Count; i++)
			{
				Vector3i vector3i = list2[i];
				Vector3i vector3i2 = ((i < list.Count) ? list[i] : Vector3i.one);
				PrefabTeleportVolume prefabTeleportVolume = new PrefabTeleportVolume();
				prefabTeleportVolume.Use(vector3i, vector3i2);
				this.List.Add(prefabTeleportVolume);
			}
		}

		// Token: 0x0600C327 RID: 49959 RVA: 0x004821E0 File Offset: 0x004803E0
		public override void WriteToProperties(DynamicProperties _properties)
		{
			StringBuilder stringBuilder = new StringBuilder();
			StringBuilder stringBuilder2 = new StringBuilder();
			foreach (PrefabTeleportVolume prefabTeleportVolume in this.List)
			{
				if (prefabTeleportVolume.Used)
				{
					if (stringBuilder.Length > 0)
					{
						stringBuilder.Append('#');
						stringBuilder2.Append('#');
					}
					stringBuilder.Append(prefabTeleportVolume.size.ToString());
					stringBuilder2.Append(prefabTeleportVolume.startPos.ToString());
				}
			}
			_properties.Values["TraderArea"] = (stringBuilder.Length > 0).ToString();
			if (stringBuilder.Length > 0)
			{
				_properties.Values["TeleportVolumeSize"] = stringBuilder.ToString();
				_properties.Values["TeleportVolumeStart"] = stringBuilder2.ToString();
				return;
			}
			_properties.Values.Remove("TeleportVolumeSize");
			_properties.Values.Remove("TeleportVolumeStart");
		}

		// Token: 0x0600C328 RID: 49960 RVA: 0x00482310 File Offset: 0x00480510
		public override bool CanCreateVolume(string _prefabInstanceName, Vector3i _bbPos, Vector3i _startPos, Vector3i _size)
		{
			if (this.Owner.bTraderArea)
			{
				return true;
			}
			XUiC_MessageBoxWindowGroup.ShowOk(LocalPlayerUI.GetUIForPrimaryPlayer().xui, Localization.Get("failed", false, null), Localization.Get("xuiPrefabEditorTraderTeleportError", false, null), "", null, false, true, false);
			return false;
		}

		// Token: 0x0600C329 RID: 49961 RVA: 0x0048235D File Offset: 0x0048055D
		public void AddExistingVolume(PrefabTeleportVolume _volume)
		{
			this.List.Add(_volume);
		}
	}
}
