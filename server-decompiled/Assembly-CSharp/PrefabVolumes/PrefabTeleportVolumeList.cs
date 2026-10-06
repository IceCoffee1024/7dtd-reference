using System;
using System.Collections.Generic;
using System.Text;

namespace PrefabVolumes
{
	// Token: 0x020018DC RID: 6364
	public class PrefabTeleportVolumeList : PrefabVolumeListAbs<PrefabTeleportVolumeList, PrefabTeleportVolume>
	{
		// Token: 0x0600C47B RID: 50299 RVA: 0x00486623 File Offset: 0x00484823
		public PrefabTeleportVolumeList(Prefab _owner)
			: base(_owner)
		{
		}

		// Token: 0x1700189C RID: 6300
		// (get) Token: 0x0600C47C RID: 50300 RVA: 0x00020119 File Offset: 0x0001E319
		public override PrefabVolumeAbs.EVolumeType VolumeType
		{
			get
			{
				return PrefabVolumeAbs.EVolumeType.Teleport;
			}
		}

		// Token: 0x1700189D RID: 6301
		// (get) Token: 0x0600C47D RID: 50301 RVA: 0x0048662C File Offset: 0x0048482C
		public override SelectionCategory SelectionCategory
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return SelectionBoxManager.Instance.CategoryTraderTeleport;
			}
		}

		// Token: 0x0600C47E RID: 50302 RVA: 0x00486638 File Offset: 0x00484838
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

		// Token: 0x0600C47F RID: 50303 RVA: 0x00486728 File Offset: 0x00484928
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

		// Token: 0x0600C480 RID: 50304 RVA: 0x00486858 File Offset: 0x00484A58
		public override bool CanCreateVolume(string _prefabInstanceName, Vector3i _bbPos, Vector3i _startPos, Vector3i _size)
		{
			if (this.Owner.bTraderArea)
			{
				return true;
			}
			XUiC_MessageBoxWindowGroup.ShowOk(LocalPlayerUI.GetUIForPrimaryPlayer().xui, Localization.Get("failed", false, null), Localization.Get("xuiPrefabEditorTraderTeleportError", false, null), "", null, false, true, false);
			return false;
		}

		// Token: 0x0600C481 RID: 50305 RVA: 0x004868A5 File Offset: 0x00484AA5
		public void AddExistingVolume(PrefabTeleportVolume _volume)
		{
			this.List.Add(_volume);
		}
	}
}
