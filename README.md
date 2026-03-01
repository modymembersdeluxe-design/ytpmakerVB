# YTP Maker Advanced (SharpDevelop/VB style)

This repository includes a longer-form **VB.NET WinForms** "YTP Maker Advanced" prototype inspired by your reference UI.

## What's added

- Complete VB WinForms solution structure (`.sln` + `.vbproj` + source files).
- Advanced generation controls for source input, clip randomization, and effect toggles.
- **Longform Mode**
  - Extends generation from short clips to long projects (up to 120 minutes).
  - Dynamically updates clip-count and duration ranges.
- **Adaptive Beat Sync**
  - Adds rhythm-aware cut intensity and sync strength controls.
  - Feeds into render complexity estimation.
- **New FFmpeg Pipeline Features**
  - FFmpeg path + input/output configuration fields.
  - Optional intro/outro clip path fields.
  - Codec/preset selection (`libx264`, `libx265`, `mpeg4`).
  - Keyframe interval and shuffle-seed metadata control.
  - Audio normalization (`loudnorm`) and two-pass toggles.
  - Generated command preview box + prototype run button.
- Live “Generation Plan” preview summarizing selected options.

## Opening in SharpDevelop

1. Open `YTPMakerAdvanced.sln` in SharpDevelop.
2. Build the project.
3. Run and experiment with generation + FFmpeg settings.

> Note: This is a UI/command-generation prototype and does not yet implement full timeline-based media compositing.
