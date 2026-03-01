# YTP Maker Advanced (SharpDevelop/VB style)

This repository includes a longer-form **VB.NET WinForms** "YTP Maker Advanced" prototype inspired by your reference UI.

## New update: Legacy Windows support profile

This update adds an **old Windows compatibility layer** focused on:

- Windows XP
- Windows Vista
- Windows 7
- Windows 8
- Windows 8.1

## What's added

- SharpDevelop-friendly project format retargeted to **.NET Framework 4.0** for older systems.
- Updated startup code to legacy WinForms bootstrapping (`EnableVisualStyles` + `SetCompatibleTextRenderingDefault`).
- Advanced generation controls for source input, clip randomization, and effect toggles.
- **Longform Mode** up to 120 minutes.
- **Adaptive Beat Sync** controls.
- **FFmpeg Pipeline Features**
  - FFmpeg path + input/output + intro/outro paths.
  - Codec/preset selection and keyframe/shuffle controls.
  - Audio normalization and two-pass toggle.
  - Generated FFmpeg command preview + prototype run button.
- **Legacy Windows output controls**
  - Windows target profile selector.
  - Legacy codec safety mode.
  - Force single-thread mode for older PCs.
  - Compatibility-focused FFmpeg flags (`yuv420p`, baseline profile rules, XP-safe codec fallback).

## Opening in SharpDevelop

1. Open `YTPMakerAdvanced.sln` in SharpDevelop.
2. Build the project.
3. Run and test settings across the Windows compatibility profiles.

> Note: This remains a UI + command-generation prototype and does not yet implement full timeline compositing.
