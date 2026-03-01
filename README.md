# YTP Maker Advanced (SharpDevelop / VB.NET, Legacy Windows Ready)

A nostalgic **YouTube Poop generator skeleton** for older Windows systems, designed around classic YTP workflows and modernized automation hooks.

## Highlights in this update

- Rebuilt as a modular preview for **automated and semi-automatic YTP creation**.
- Supports retro-oriented workflows on legacy Windows environments (XP, Vista, 7, 8, 8.1 era tooling).
- Adds a clear architecture:
  - **Generator (FFmpeg)**: temporary clip rendering, concat pipeline, format conversion.
  - **Exporter**: `.ytpproj` JSON project descriptor save/load.
  - **Vegas Pro 12 adapter**: script template + keyframe instruction scaffolding.

## New mega feature set

### 1) Nostalgic YTP generator modes

- Generic
- YTP Tennis
- Collab Entry
- YTPMV

### 2) Source Explorer + library management

- Bulk import by folder + manual multi-file import.
- Supported source types:
  - Video: `mp4`, `wmv`, `avi`, `mkv`
  - Images: `png`, `jpg`, `jpeg`, `webp`, `gif`
  - Audio: `mp3`, `wav`, `ogg`
  - Tracker: `xm`, `mod`, `it` (best-effort)
- Asset tagging + audio library split for YTPMV style projects.

### 3) Effects framework (toggleable)

Audio effect list includes nostalgic/meme-style options such as:
`random sound`, `mute`, `speed up`, `speed down`, `reverse`, `chorus`, `vibrato`, `stutter`, `dance`, `squidward`, `sus`, `lagfun`, `low/high harmony`, `confusion`, `random chords`, `trailing reverses`, `low-quality meme`, `audio crust`, `pitch-shifting loop`, `mashup mixing`.

Video effect list includes:
`invert`, `rainbow`, `mirror`, `mirror symmetry`, `screen clip`, `overlay images/sources`, `spadinner`, `sentence mixing`, `shuffle/loop frames`, `framerate reduction`, `random cuts`, `speed loop boost`, `scrambling`.

> Note: Some effects are direct FFmpeg filter mappings, while meme/composite effects are intentionally scaffolded as approximations or metadata hooks.

### 4) FFmpeg pipeline + output formats

- Auto-builds command pipelines from selected assets/effects.
- Generates temporary per-clip renders, concat commands, and final conversion steps.
- Output targets: **WMV, MP4, AVI, MKV**.
- Exports a ready-to-run `.bat` pipeline for large remix batches.

### 5) Vegas Pro 12 helper / adapter preview

- Generates a Vegas script template text file.
- Includes `AutoKeyframeData` instruction lines to assist later edit automation.
- Shows an in-app preview of generated Vegas helper output.

### 6) Project model (`.ytpproj` JSON)

Serialized project data includes:

- `ProjectName`
- `Type` (Generic / YTP Tennis / Collab Entry / YTPMV)
- `Assets` list (`Path`, `Type`, `Tag`)
- `AudioLibrary`
- Selected audio/video effect lists
- Generator settings (clip pool params, seed, beat sync)
- Output settings (format/path/temp path)
- `AutoKeyframeData`

## Project structure

- `YTPMakerAdvanced/MainForm.vb` — WinForms UI + workflow wiring
- `YTPMakerAdvanced/Models.vb` — project/domain models
- `YTPMakerAdvanced/ProjectSerializer.vb` — JSON `.ytpproj` save/load
- `YTPMakerAdvanced/FfmpegGenerator.vb` — FFmpeg command pipeline builder
- `YTPMakerAdvanced/VegasAdapter.vb` — Vegas Pro 12 template/adaptor skeleton

## Opening in SharpDevelop

1. Open `YTPMakerAdvanced.sln`.
2. Build the project.
3. Use tabs in order:
   - Generator
   - Source Explorer
   - Vegas 12 Adapter

## Status

This is a **preview skeleton** intentionally designed for extension:

- good for large-scale remix experimentation (YTP/YTPMV/Tennis/Collab);
- ready for deeper beat detection, advanced effect plugins, and full Vegas scripting integration.
