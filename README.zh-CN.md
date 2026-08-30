# 7DTD Reference（中文）

英文文档：[README.md](README.md)

本私有仓库用于分析《7 Days to Die》专用服务器并开发 7DPanel。根目录只保留最新版本的运行时和反编译源码；历史版本作为不可变的 Release 资产保存。

## 目录结构

```text
server-decompiled/       dnSpy 生成的最新源码
runtime/                 最新版本允许同步的服务端文件
assets/                  共享视觉参考资源
metadata.json            当前版本和归档信息
runtime.manifest.json    runtime 目录/根文件允许清单
decompile.manifest.json  传给 dnSpy.Console 的程序集清单
scripts/                 更新、校验、归档和辅助脚本
```

不要混用不同构建的文件。`metadata.json` 是当前版本的唯一来源；Release tag 和归档文件名使用同一个值（例如 `<version>`），Git 中不创建版本目录。

## 快速开始

前置条件：Windows、PowerShell 7、.NET Framework 4.8 Developer Pack、MSBuild/.NET SDK，以及位于允许目录中的 `dnSpy.Console.exe`。复制 `.env.example` 为 `.env`，填写本地或 UNC 路径：

```powershell
Copy-Item .env.example .env
notepad .env
```

先预览，再执行统一更新。省略版本参数时，脚本会从服务端最新的 `output_log*.txt` 自动读取版本和构建号：

```powershell
pwsh ./scripts/Update-Reference.ps1 -WhatIf
pwsh ./scripts/Update-Reference.ps1
```

日志没有可用信息时，显式传入 `-Version <version>` 和 `-GameBuild '<build>'`。脚本会提示先归档当前版本，然后按清单同步 runtime，在临时目录运行 dnSpy，成功后替换 `server-decompiled/`、更新元数据并校验。脚本不会自动提交、推送或创建 Release。

## 替换前归档

替换根目录前必须先归档当前版本：

```powershell
$version = '<version>'
pwsh ./scripts/Verify-Reference.ps1
pwsh ./scripts/New-ReferenceRelease.ps1 -Version $version
gh release create $version `
  "./dist/7dtd-reference-$version.zip" `
  "./dist/7dtd-reference-$version.zip.sha256" `
  --title "7DTD Reference $version"
```

发布前请审核压缩包和校验值。ZIP 包含当前源码、runtime、assets、元数据、清单及中英文 README。Release tag 应保持不可变；更新后只提交新的根目录内容。`dist/` 是本地输出目录，不提交到 Git。

## 脚本职责

| 脚本 | 作用 |
| --- | --- |
| `Update-Reference.ps1` | 统一流程：读取 `.env`、探测版本、归档提示、同步 runtime、运行 dnSpy、替换源码、更新元数据、校验。 |
| `Update-ReferenceRuntime.ps1` | 根据 `runtime.manifest.json` 从本地/UNC 服务端镜像同步 runtime，支持 `-WhatIf`、`-SkipReleasePrompt`。 |
| `Get-ServerVersion.ps1` | 从最新 `output_log*.txt` 读取版本和构建号。 |
| `New-ReferenceRelease.ps1` | 生成 ZIP 和 SHA-256；`-Version` 必须等于 `metadata.json.gameVersion`。 |
| `Verify-Reference.ps1` | 检查元数据、必要目录、目标框架及项目 `HintPath` 文件。 |
| `New-RuntimeManifest.ps1` | 官方文件集合变化后重新生成、再人工审核 runtime 清单。 |
| `Set-DecompiledTargetFramework.ps1` | 将 dnSpy 项目重定向到 .NET Framework 4.8，支持 `-WhatIf`。 |

需要单独诊断某一步时可直接运行辅助脚本：

```powershell
pwsh ./scripts/Get-ServerVersion.ps1 -ServerRoot '\\server\share\7dtd'
pwsh ./scripts/Verify-Reference.ps1
pwsh ./scripts/Set-DecompiledTargetFramework.ps1 -WhatIf
```

## Runtime、源码与兼容性边界

`runtime/` 应与 `metadata.json` 所记录构建的官方服务端分发内容一致。清单允许完整目录，因此目录内部增删文件无需逐文件维护；清单之外的根目录路径不会复制。官方文件集合变化时，审核 `New-RuntimeManifest.ps1` 生成的差异后再提交。机器专用配置应放在仓库外。

dnSpy 源码来自 `decompile.manifest.json` 中的程序集，并先输出到临时目录，校验成功后才替换 `server-decompiled/`。目标框架重定向只影响项目引用；仍须针对 `runtime/7DaysToDieServer_Data/Managed` 的实际程序集，并在游戏 Mono 进程中测试。不要把本仓库的 runtime 或反编译参考文件打入 7DPanel 发布包。

## 安全与法律

保持仓库私有，`.env` 不纳入 Git。脚本支持 UNC 路径，但只会读取配置的服务端目录。游戏文件可能受[《7 Days to Die》EULA](https://7daystodie.com/eula/)约束；分享、镜像或公开任何内容前请确认授权。
