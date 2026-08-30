using System;
using System.Collections.Generic;
using System.Text;

namespace PrefabVolumes
{
	// Token: 0x0200188A RID: 6282
	public class PrefabTeleportVolumeList : PrefabVolumeListAbs<PrefabTeleportVolumeList, PrefabTeleportVolume>
	{
		// Token: 0x0600C19C RID: 49564 RVA: 0x0047BFAF File Offset: 0x0047A1AF
		public PrefabTeleportVolumeList(Prefab _owner)
			: base(_owner)
		{
		}

		// Token: 0x17001788 RID: 6024
		// (get) Token: 0x0600C19D RID: 49565 RVA: 0x0001FF5D File Offset: 0x0001E15D
		public override PrefabVolumeAbs.EVolumeType VolumeType
		{
			get
			{
				return PrefabVolumeAbs.EVolumeType.Teleport;
			}
		}

		// Token: 0x17001789 RID: 6025
		// (get) Token: 0x0600C19E RID: 49566 RVA: 0x0047BFB8 File Offset: 0x0047A1B8
		public override SelectionCategory SelectionCategory
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return SelectionBoxManager.Instance.CategoryTraderTeleport;
			}
		}

		// Token: 0x0600C19F RID: 49567 RVA: 0x0047BFC4 File Offset: 0x0047A1C4
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

		// Token: 0x0600C1A0 RID: 49568 RVA: 0x0047C0B4 File Offset: 0x0047A2B4
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

		// Token: 0x0600C1A1 RID: 49569 RVA: 0x0047C1E4 File Offset: 0x0047A3E4
		public override bool CanCreateVolume(string _prefabInstanceName, Vector3i _bbPos, Vector3i _startPos, Vector3i _size)
		{
			if (this.Owner.bTraderArea)
			{
				return true;
			}
			XUiC_MessageBoxWindowGroup.ShowOk(LocalPlayerUI.GetUIForPrimaryPlayer().xui, Localization.Get("failed", false, null), Localization.Get("xuiPrefabEditorTraderTeleportError", false, null), "", null, false, true, false);
			return false;
		}

		// Token: 0x0600C1A2 RID: 49570 RVA: 0x0047C231 File Offset: 0x0047A431
		public void AddExistingVolume(PrefabTeleportVolume _volume)
		{
			this.List.Add(_volume);
		}
	}
}
