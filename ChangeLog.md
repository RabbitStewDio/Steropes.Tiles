# Changelog

## 2021-04-21 - v2.1.0

    [???]     Updating version info for development branch 1.1.1-unstable.0

    [???]     Reorganize project to split samples, src and test into separate directories. Migrate project format to new csproj files

    [API]     Refactored project to enable new Unity2D renderer engine

              Tile set handling, blending, texture operations and texture atlas building
              logic are now part of the core library with service interfaces connecting
              the core with the actual rendering system.
              
              The sample core logic has been moved to a shared library to demonstrate that
              renderers are replaceable without much effort.
              
    [API]     Changed readonly struct usages to pass-by-ref

    [Bug]     Fixed various rendering problems caused by backport of Unity-Code into mainline.

    [Bug]     Anchor handling and tile rendering for larger tiles was wrong after upgrade from WinForms

    [Build]   Updated project to MonoGame 3.8, DotNet core and a modern structure.

    [Build]   Upgraded build scripts to Nuke

    [Build]   Fixed package downgrades to allow build scripts to pass.

    [Build]   Change debug type to portable so that snuget packages work.

    [Build]   Added change log for build scripts to auto-fill.

    [Build]   Exclude projects from creating nuget packages.

    [Build]   Ensure a failed build returns to a valid branch.

    [CleanUp] Code cleanup to be in line with the language defaults.

    [CleanUp] Removed uses of System.Diagnostic.Trace logging, its just horrible to use and configure.

    [CleanUp] Removed Console messages.

    [Feature] Rewrite of template generator in Avalonia

              The template generator is no longer a WinForms app and uses modern AvaloniaUI
              for its presentation. Fixed several bugs in the code along the way.
              Also massive cleanup of warnings, as it was hard to see real errors in the
              sea of angry warnings.
              
              
    [Feature] Added better highlighting for cell-map mode in template generator

              The existing cell-map generator randomly selected colors based on some
              obscure internal logic around the implicit dictionary order of keys. This
              better implementation now lets designers directly select the relevant keys,
              add documentation and most importantly set a color for each cell-map choice.
              
