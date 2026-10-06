using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Platform;

// Token: 0x020012C4 RID: 4804
public class PermissionsManager
{
	// Token: 0x06009832 RID: 38962 RVA: 0x00396850 File Offset: 0x00394A50
	public static EUserPerms GetPermissions(PermissionsManager.PermissionSources _sources = PermissionsManager.PermissionSources.All)
	{
		EUserPerms euserPerms = EUserPerms.All;
		if (_sources.HasFlag(PermissionsManager.PermissionSources.Platform) && !GameManager.IsDedicatedServer)
		{
			euserPerms &= PlatformManager.MultiPlatform.User.Permissions;
		}
		if (_sources.HasFlag(PermissionsManager.PermissionSources.GamePrefs))
		{
			if (euserPerms.HasFlag(EUserPerms.Communication) && !GamePrefs.GetBool(EnumGamePrefs.OptionsChatCommunication))
			{
				euserPerms &= ~EUserPerms.Communication;
			}
			if (euserPerms.HasFlag(EUserPerms.Crossplay) && !GamePrefs.GetBool(EnumGamePrefs.OptionsCrossplay))
			{
				euserPerms &= ~EUserPerms.Crossplay;
			}
		}
		if (_sources.HasFlag(PermissionsManager.PermissionSources.LaunchPrefs) && euserPerms.HasFlag(EUserPerms.Crossplay) && !LaunchPrefs.AllowCrossplay.Value)
		{
			euserPerms &= ~EUserPerms.Crossplay;
		}
		if (_sources.HasFlag(PermissionsManager.PermissionSources.DebugMask))
		{
			euserPerms &= PermissionsManager.DebugPermissionsMask;
		}
		if (_sources.HasFlag(PermissionsManager.PermissionSources.TitleStorage) && euserPerms.HasFlag(EUserPerms.Crossplay) && !PermissionsManager.tsOverrides.Crossplay)
		{
			euserPerms &= ~EUserPerms.Crossplay;
		}
		return euserPerms;
	}

	// Token: 0x06009833 RID: 38963 RVA: 0x00396972 File Offset: 0x00394B72
	public static IEnumerator ResolvePermissions(EUserPerms _perms, bool _canPrompt, CoroutineCancellationToken _cancellationToken = null)
	{
		PermissionsManager.<>c__DisplayClass5_0 CS$<>8__locals1 = new PermissionsManager.<>c__DisplayClass5_0();
		CS$<>8__locals1._perms = _perms;
		CS$<>8__locals1._canPrompt = _canPrompt;
		Log.Out(string.Format("[PermissionsManager] {0}({1}: [{2}], {3}: {4})", new object[] { "ResolvePermissions", "_perms", CS$<>8__locals1._perms, "_canPrompt", CS$<>8__locals1._canPrompt }));
		yield return null;
		if (_cancellationToken != null && _cancellationToken.IsCancelled())
		{
			yield break;
		}
		bool needsWait = PermissionsManager.resolvingPermissions;
		if (needsWait)
		{
			Log.Out(string.Format("[PermissionsManager] {0}({1}: [{2}], {3}: {4}) Waiting on existing resolve...", new object[] { "ResolvePermissions", "_perms", CS$<>8__locals1._perms, "_canPrompt", CS$<>8__locals1._canPrompt }));
			while (PermissionsManager.resolvingPermissions)
			{
				yield return null;
				if (_cancellationToken != null && _cancellationToken.IsCancelled())
				{
					yield break;
				}
			}
		}
		try
		{
			PermissionsManager.resolvingPermissions = true;
			if (needsWait)
			{
				Log.Out(string.Format("[PermissionsManager] {0}({1}: [{2}], {3}: {4}) Finished waiting. Executing resolve.", new object[] { "ResolvePermissions", "_perms", CS$<>8__locals1._perms, "_canPrompt", CS$<>8__locals1._canPrompt }));
			}
			CS$<>8__locals1.tsFetchComplete = false;
			if (CS$<>8__locals1._perms.HasCrossplay())
			{
				Log.Out(string.Format("[PermissionsManager] {0}({1}: [{2}], {3}: {4}) Fetching Title Storage Overrides...", new object[] { "ResolvePermissions", "_perms", CS$<>8__locals1._perms, "_canPrompt", CS$<>8__locals1._canPrompt }));
				TitleStorageOverridesManager.Instance.FetchFromSource(new Action<TitleStorageOverridesManager.TSOverrides>(CS$<>8__locals1.<ResolvePermissions>g__FetchComplete|0));
			}
			else
			{
				CS$<>8__locals1.tsFetchComplete = true;
			}
			if (!GameManager.IsDedicatedServer)
			{
				yield return PlatformManager.MultiPlatform.User.ResolvePermissions(CS$<>8__locals1._perms, CS$<>8__locals1._canPrompt, _cancellationToken);
				if (_cancellationToken != null && _cancellationToken.IsCancelled())
				{
					PermissionsManager.resolvingPermissions = false;
					yield break;
				}
			}
			while (!CS$<>8__locals1.tsFetchComplete)
			{
				if (_cancellationToken != null && _cancellationToken.IsCancelled())
				{
					yield break;
				}
				yield return null;
			}
		}
		finally
		{
			PermissionsManager.resolvingPermissions = false;
		}
		yield break;
		yield break;
	}

	// Token: 0x06009834 RID: 38964 RVA: 0x00396990 File Offset: 0x00394B90
	public static string GetPermissionDenyReason(EUserPerms _perms, PermissionsManager.PermissionSources _sources = PermissionsManager.PermissionSources.All)
	{
		if (_sources.HasFlag(PermissionsManager.PermissionSources.DebugMask))
		{
			if (_perms.HasFlag(EUserPerms.Communication) && !PermissionsManager.DebugPermissionsMask.HasFlag(EUserPerms.Communication))
			{
				return Localization.Get("permissionsMissing_communication", false, null);
			}
			if (_perms.HasFlag(EUserPerms.Crossplay) && !PermissionsManager.DebugPermissionsMask.HasFlag(EUserPerms.Crossplay))
			{
				return Localization.Get("permissionsMissing_crossplay", false, null);
			}
		}
		if (_sources.HasFlag(PermissionsManager.PermissionSources.GamePrefs))
		{
			if (_perms.HasFlag(EUserPerms.Communication) && !GamePrefs.GetBool(EnumGamePrefs.OptionsChatCommunication))
			{
				return Localization.Get("permissionsMissing_communication", false, null);
			}
			if (_perms.HasFlag(EUserPerms.Crossplay) && !GamePrefs.GetBool(EnumGamePrefs.OptionsCrossplay))
			{
				return Localization.Get("permissionsMissing_crossplay", false, null);
			}
		}
		if (_sources.HasFlag(PermissionsManager.PermissionSources.LaunchPrefs) && _perms.HasFlag(EUserPerms.Crossplay) && !LaunchPrefs.AllowCrossplay.Value)
		{
			return Localization.Get("auth_noCrossplay", false, null);
		}
		if (_sources.HasFlag(PermissionsManager.PermissionSources.TitleStorage) && _perms.HasFlag(EUserPerms.Crossplay) && !PermissionsManager.tsOverrides.Crossplay)
		{
			return Localization.Get("auth_noCrossplayOverridden", false, null);
		}
		if (_sources.HasFlag(PermissionsManager.PermissionSources.Platform) && !GameManager.IsDedicatedServer)
		{
			string permissionDenyReason = PlatformManager.MultiPlatform.User.GetPermissionDenyReason(_perms);
			if (!string.IsNullOrEmpty(permissionDenyReason))
			{
				return permissionDenyReason;
			}
		}
		return null;
	}

	// Token: 0x06009835 RID: 38965 RVA: 0x00396B3E File Offset: 0x00394D3E
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsAllowed(EUserPerms _checkPerms)
	{
		return PermissionsManager.GetPermissions(PermissionsManager.PermissionSources.All).HasFlag(_checkPerms);
	}

	// Token: 0x06009836 RID: 38966 RVA: 0x00396B57 File Offset: 0x00394D57
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsMultiplayerAllowed()
	{
		return PermissionsManager.GetPermissions(PermissionsManager.PermissionSources.All).HasMultiplayer();
	}

	// Token: 0x06009837 RID: 38967 RVA: 0x00396B65 File Offset: 0x00394D65
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsCommunicationAllowed()
	{
		return PermissionsManager.GetPermissions(PermissionsManager.PermissionSources.All).HasCommunication();
	}

	// Token: 0x06009838 RID: 38968 RVA: 0x00396B73 File Offset: 0x00394D73
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsCrossplayAllowed()
	{
		return PermissionsManager.GetPermissions(PermissionsManager.PermissionSources.All).HasCrossplay();
	}

	// Token: 0x06009839 RID: 38969 RVA: 0x00396B84 File Offset: 0x00394D84
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool CanHostMultiplayer()
	{
		EUserPerms permissions = PermissionsManager.GetPermissions(PermissionsManager.PermissionSources.All);
		return permissions.HasMultiplayer() && permissions.HasHostMultiplayer();
	}

	// Token: 0x04007215 RID: 29205
	public static EUserPerms DebugPermissionsMask = EUserPerms.All;

	// Token: 0x04007216 RID: 29206
	[PublicizedFrom(EAccessModifier.Private)]
	public static TitleStorageOverridesManager.TSOverrides tsOverrides = default(TitleStorageOverridesManager.TSOverrides);

	// Token: 0x04007217 RID: 29207
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool resolvingPermissions;

	// Token: 0x020012C5 RID: 4805
	[Flags]
	public enum PermissionSources
	{
		// Token: 0x04007219 RID: 29209
		Platform = 1,
		// Token: 0x0400721A RID: 29210
		GamePrefs = 2,
		// Token: 0x0400721B RID: 29211
		LaunchPrefs = 4,
		// Token: 0x0400721C RID: 29212
		DebugMask = 8,
		// Token: 0x0400721D RID: 29213
		TitleStorage = 16,
		// Token: 0x0400721E RID: 29214
		All = 31
	}
}
