using MelonLoader;
using System.Reflection;


[assembly: AssemblyTitle(ArrowModRenewed.src.BuildInfo.ModName)]
[assembly: AssemblyCopyright($"Created by {ArrowModRenewed.src.BuildInfo.ModAuthor}")]

[assembly: AssemblyVersion(ArrowModRenewed.src.BuildInfo.ModVersion)]
[assembly: AssemblyFileVersion(ArrowModRenewed.src.BuildInfo.ModVersion)]
[assembly: MelonInfo(typeof(ArrowModRenewed.src.ArrowModRenewed), ArrowModRenewed.src.BuildInfo.ModName, ArrowModRenewed.src.BuildInfo.ModVersion, ArrowModRenewed.src.BuildInfo.ModAuthor)]

//This tells MelonLoader that the mod is only for The Long Dark.
[assembly: MelonGame("Hinterland", "TheLongDark")]