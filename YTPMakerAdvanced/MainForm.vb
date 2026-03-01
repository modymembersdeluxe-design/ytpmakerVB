Imports System
Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.Drawing
Imports System.IO
Imports System.Linq
Imports System.Text
Imports System.Windows.Forms

Namespace YTPMakerAdvanced
    Public Class MainForm
        Inherits Form

        Private ReadOnly durationTrack As New TrackBar()
        Private ReadOnly clipTrack As New TrackBar()
        Private ReadOnly minClipTrack As New TrackBar()
        Private ReadOnly durationLabel As New Label()
        Private ReadOnly clipLabel As New Label()
        Private ReadOnly minClipLabel As New Label()

        Private ReadOnly longformModeCheck As New CheckBox()
        Private ReadOnly megaYtpCheck As New CheckBox()
        Private ReadOnly adaptiveBeatSyncCheck As New CheckBox()
        Private ReadOnly beatSyncStrength As New NumericUpDown()
        Private ReadOnly cutIntensity As New ComboBox()

        Private ReadOnly effectsChecked As New CheckedListBox()
        Private ReadOnly planPreview As New TextBox()
        Private ReadOnly ffmpegCommandPreview As New TextBox()

        Private ReadOnly ffmpegEnabledCheck As New CheckBox()
        Private ReadOnly ffmpegPathText As New TextBox()
        Private ReadOnly inputVideoText As New TextBox()
        Private ReadOnly outputVideoText As New TextBox()
        Private ReadOnly introClipText As New TextBox()
        Private ReadOnly outroClipText As New TextBox()
        Private ReadOnly codecPresetCombo As New ComboBox()
        Private ReadOnly keyframeSeconds As New NumericUpDown()
        Private ReadOnly shuffleSeedValue As New NumericUpDown()
        Private ReadOnly twoPassCheck As New CheckBox()
        Private ReadOnly normalizeAudioCheck As New CheckBox()
        Private ReadOnly smartTransitionsCheck As New CheckBox()

        Private ReadOnly windowsCompatCombo As New ComboBox()
        Private ReadOnly legacyCodecCheck As New CheckBox()
        Private ReadOnly forceSingleThreadCheck As New CheckBox()

        Public Sub New()
            Text = "YTP Maker Advanced - SharpDevelop VB Edition (Legacy Windows Ready)"
            Size = New Size(1360, 900)
            StartPosition = FormStartPosition.CenterScreen
            BackColor = Color.FromArgb(236, 240, 246)

            BuildUi()
            UpdateRangeLabels()
            UpdatePlanPreview()
            UpdateFfmpegPreview()
        End Sub

        Private Sub BuildUi()
            Dim root As New TableLayoutPanel()
            root.Dock = DockStyle.Fill
            root.ColumnCount = 3
            root.RowCount = 2
            root.Padding = New Padding(12)
            root.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 30.0F))
            root.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 40.0F))
            root.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 30.0F))
            root.RowStyles.Add(New RowStyle(SizeType.Percent, 70.0F))
            root.RowStyles.Add(New RowStyle(SizeType.Percent, 30.0F))
            Controls.Add(root)

            root.Controls.Add(BuildLeftPanel(), 0, 0)
            root.Controls.Add(BuildCenterPanel(), 1, 0)
            root.Controls.Add(BuildRightPanel(), 2, 0)
            root.Controls.Add(BuildBottomPanel(), 0, 1)
            root.SetColumnSpan(root.GetControlFromPosition(0, 1), 3)
        End Sub

        Private Function BuildLeftPanel() As Control
            Dim container As New GroupBox()
            container.Dock = DockStyle.Fill
            container.Text = "Video & Stream Settings"
            container.Font = New Font("Segoe UI", 11, FontStyle.Bold)

            Dim panel As New TableLayoutPanel()
            panel.Dock = DockStyle.Fill
            panel.ColumnCount = 1
            panel.RowCount = 10
            panel.Padding = New Padding(8)

            Dim importVideo As New Button()
            importVideo.Text = "Import Video (MP4/WMV/AVI/MKV)"
            importVideo.Dock = DockStyle.Fill
            importVideo.Height = 40

            Dim importImages As New Button()
            importImages.Text = "Import Images (PNG/JPG/BMP)"
            importImages.Dock = DockStyle.Fill
            importImages.Height = 40

            AddHandler importVideo.Click, AddressOf SelectInputVideo
            AddHandler importImages.Click, Sub(sender, e) MessageBox.Show("Image import hook ready.", "Import", MessageBoxButtons.OK, MessageBoxIcon.Information)

            durationTrack.Minimum = 1
            durationTrack.Maximum = 120
            durationTrack.Value = 25
            durationTrack.TickFrequency = 5
            durationTrack.Dock = DockStyle.Fill

            clipTrack.Minimum = 10
            clipTrack.Maximum = 100000
            clipTrack.Value = 7000
            clipTrack.TickFrequency = 2500
            clipTrack.Dock = DockStyle.Fill

            minClipTrack.Minimum = 1
            minClipTrack.Maximum = 180
            minClipTrack.Value = 8
            minClipTrack.TickFrequency = 10
            minClipTrack.Dock = DockStyle.Fill

            AddHandler durationTrack.ValueChanged, AddressOf OnParameterChanged
            AddHandler clipTrack.ValueChanged, AddressOf OnParameterChanged
            AddHandler minClipTrack.ValueChanged, AddressOf OnParameterChanged

            panel.Controls.Add(importVideo)
            panel.Controls.Add(importImages)
            panel.Controls.Add(durationLabel)
            panel.Controls.Add(durationTrack)
            panel.Controls.Add(clipLabel)
            panel.Controls.Add(clipTrack)
            panel.Controls.Add(minClipLabel)
            panel.Controls.Add(minClipTrack)

            longformModeCheck.Text = "Longform Mode: 25 to 120 minutes"
            longformModeCheck.Font = New Font("Segoe UI", 10, FontStyle.Bold)
            AddHandler longformModeCheck.CheckedChanged, AddressOf OnLongformModeChanged
            panel.Controls.Add(longformModeCheck)

            container.Controls.Add(panel)
            Return container
        End Function

        Private Function BuildCenterPanel() As Control
            Dim container As New GroupBox()
            container.Dock = DockStyle.Fill
            container.Text = "Preview / Generation Plan"
            container.Font = New Font("Segoe UI", 11, FontStyle.Bold)

            Dim panel As New TableLayoutPanel()
            panel.Dock = DockStyle.Fill
            panel.ColumnCount = 1
            panel.RowCount = 5
            panel.Padding = New Padding(8)
            panel.RowStyles.Add(New RowStyle(SizeType.Absolute, 260))
            panel.RowStyles.Add(New RowStyle(SizeType.Absolute, 24))
            panel.RowStyles.Add(New RowStyle(SizeType.Percent, 50))
            panel.RowStyles.Add(New RowStyle(SizeType.Absolute, 24))
            panel.RowStyles.Add(New RowStyle(SizeType.Percent, 50))

            Dim preview As New Panel()
            preview.Dock = DockStyle.Fill
            preview.BackColor = Color.Black

            Dim previewLabel As New Label()
            previewLabel.Dock = DockStyle.Fill
            previewLabel.Text = "Preview Placeholder (render engine hook)"
            previewLabel.ForeColor = Color.White
            previewLabel.TextAlign = ContentAlignment.MiddleCenter
            preview.Controls.Add(previewLabel)

            Dim planTitle As New Label()
            planTitle.Text = "Generation Plan"
            planTitle.Dock = DockStyle.Fill
            planTitle.Font = New Font("Segoe UI", 10, FontStyle.Bold)

            planPreview.Multiline = True
            planPreview.ScrollBars = ScrollBars.Vertical
            planPreview.ReadOnly = True
            planPreview.Dock = DockStyle.Fill
            planPreview.Font = New Font("Consolas", 10)

            Dim ffmpegTitle As New Label()
            ffmpegTitle.Text = "Generated FFmpeg Command"
            ffmpegTitle.Dock = DockStyle.Fill
            ffmpegTitle.Font = New Font("Segoe UI", 10, FontStyle.Bold)

            ffmpegCommandPreview.Multiline = True
            ffmpegCommandPreview.ScrollBars = ScrollBars.Vertical
            ffmpegCommandPreview.ReadOnly = True
            ffmpegCommandPreview.Dock = DockStyle.Fill
            ffmpegCommandPreview.Font = New Font("Consolas", 9)

            panel.Controls.Add(preview)
            panel.Controls.Add(planTitle)
            panel.Controls.Add(planPreview)
            panel.Controls.Add(ffmpegTitle)
            panel.Controls.Add(ffmpegCommandPreview)
            container.Controls.Add(panel)

            Return container
        End Function

        Private Function BuildRightPanel() As Control
            Dim container As New GroupBox()
            container.Dock = DockStyle.Fill
            container.Text = "Advanced + FFmpeg + Legacy Windows"
            container.Font = New Font("Segoe UI", 11, FontStyle.Bold)

            Dim panel As New TableLayoutPanel()
            panel.Dock = DockStyle.Fill
            panel.ColumnCount = 1
            panel.Padding = New Padding(8)
            panel.AutoScroll = True

            megaYtpCheck.Text = "Mega YTP Mode"
            megaYtpCheck.Checked = True

            adaptiveBeatSyncCheck.Text = "Adaptive Beat Sync"
            adaptiveBeatSyncCheck.Checked = True

            Dim beatStrengthLabel As New Label()
            beatStrengthLabel.Text = "Beat Sync Strength (1-10):"
            beatStrengthLabel.AutoSize = True
            beatSyncStrength.Minimum = 1
            beatSyncStrength.Maximum = 10
            beatSyncStrength.Value = 7

            Dim cutLabel As New Label()
            cutLabel.Text = "Cut Intensity:"
            cutLabel.AutoSize = True
            cutIntensity.DropDownStyle = ComboBoxStyle.DropDownList
            cutIntensity.Items.AddRange(New Object() {"Balanced", "Chaotic", "Ultra-chaotic", "Story-biased"})
            cutIntensity.SelectedIndex = 0

            effectsChecked.CheckOnClick = True
            effectsChecked.Items.AddRange(New Object() {
                "Random Sound", "Mute", "Speed Up", "Reverse", "Chorus", "Lagfun",
                "Low Harmony", "Audio Crust", "Pitch Shift", "Scrambling",
                "Invert", "Rainbow", "Mirror", "Spadinner Mix", "Frame Rate Cuts"
            })
            effectsChecked.Height = 220
            effectsChecked.Dock = DockStyle.Fill

            ffmpegEnabledCheck.Text = "Enable FFmpeg Render Pipeline"
            ffmpegEnabledCheck.Checked = True

            ffmpegPathText.Text = "ffmpeg"
            outputVideoText.Text = "output_ytp.mp4"

            codecPresetCombo.DropDownStyle = ComboBoxStyle.DropDownList
            codecPresetCombo.Items.AddRange(New Object() {
                "libx264 veryfast", "libx264 medium", "libx265 medium", "mpeg4 fast"
            })
            codecPresetCombo.SelectedIndex = 0

            windowsCompatCombo.DropDownStyle = ComboBoxStyle.DropDownList
            windowsCompatCombo.Items.AddRange(New Object() {
                "Auto (Modern)", "Windows XP", "Windows Vista", "Windows 7", "Windows 8", "Windows 8.1"
            })
            windowsCompatCombo.SelectedIndex = 0

            keyframeSeconds.Minimum = 1
            keyframeSeconds.Maximum = 10
            keyframeSeconds.Value = 2

            shuffleSeedValue.Minimum = 1
            shuffleSeedValue.Maximum = 999999
            shuffleSeedValue.Value = 1337

            twoPassCheck.Text = "Enable Two-Pass"
            normalizeAudioCheck.Text = "Normalize Audio (loudnorm)"
            smartTransitionsCheck.Text = "Smart Transition Mix"
            smartTransitionsCheck.Checked = True

            legacyCodecCheck.Text = "Legacy Playback Safe Codec"
            forceSingleThreadCheck.Text = "Force Single Thread (Old PC Safe)"

            AddHandler megaYtpCheck.CheckedChanged, AddressOf OnParameterChanged
            AddHandler adaptiveBeatSyncCheck.CheckedChanged, AddressOf OnParameterChanged
            AddHandler beatSyncStrength.ValueChanged, AddressOf OnParameterChanged
            AddHandler cutIntensity.SelectedIndexChanged, AddressOf OnParameterChanged
            AddHandler effectsChecked.ItemCheck, AddressOf OnEffectsChanged

            AddHandler ffmpegEnabledCheck.CheckedChanged, AddressOf OnParameterChanged
            AddHandler ffmpegPathText.TextChanged, AddressOf OnParameterChanged
            AddHandler inputVideoText.TextChanged, AddressOf OnParameterChanged
            AddHandler outputVideoText.TextChanged, AddressOf OnParameterChanged
            AddHandler introClipText.TextChanged, AddressOf OnParameterChanged
            AddHandler outroClipText.TextChanged, AddressOf OnParameterChanged
            AddHandler codecPresetCombo.SelectedIndexChanged, AddressOf OnParameterChanged
            AddHandler windowsCompatCombo.SelectedIndexChanged, AddressOf OnParameterChanged
            AddHandler keyframeSeconds.ValueChanged, AddressOf OnParameterChanged
            AddHandler shuffleSeedValue.ValueChanged, AddressOf OnParameterChanged
            AddHandler twoPassCheck.CheckedChanged, AddressOf OnParameterChanged
            AddHandler normalizeAudioCheck.CheckedChanged, AddressOf OnParameterChanged
            AddHandler smartTransitionsCheck.CheckedChanged, AddressOf OnParameterChanged
            AddHandler legacyCodecCheck.CheckedChanged, AddressOf OnParameterChanged
            AddHandler forceSingleThreadCheck.CheckedChanged, AddressOf OnParameterChanged

            panel.Controls.Add(megaYtpCheck)
            panel.Controls.Add(adaptiveBeatSyncCheck)
            panel.Controls.Add(beatStrengthLabel)
            panel.Controls.Add(beatSyncStrength)
            panel.Controls.Add(cutLabel)
            panel.Controls.Add(cutIntensity)
            panel.Controls.Add(New Label() With {.Text = "Effects:", .AutoSize = True})
            panel.Controls.Add(effectsChecked)

            panel.Controls.Add(New Label() With {.Text = "Windows target profile:", .AutoSize = True})
            panel.Controls.Add(windowsCompatCombo)
            panel.Controls.Add(legacyCodecCheck)
            panel.Controls.Add(forceSingleThreadCheck)

            panel.Controls.Add(ffmpegEnabledCheck)
            panel.Controls.Add(New Label() With {.Text = "FFmpeg executable:", .AutoSize = True})
            panel.Controls.Add(ffmpegPathText)
            panel.Controls.Add(New Label() With {.Text = "Input video path:", .AutoSize = True})
            panel.Controls.Add(inputVideoText)
            panel.Controls.Add(New Label() With {.Text = "Output video path:", .AutoSize = True})
            panel.Controls.Add(outputVideoText)
            panel.Controls.Add(New Label() With {.Text = "Intro clip path (optional):", .AutoSize = True})
            panel.Controls.Add(introClipText)
            panel.Controls.Add(New Label() With {.Text = "Outro clip path (optional):", .AutoSize = True})
            panel.Controls.Add(outroClipText)
            panel.Controls.Add(New Label() With {.Text = "Video codec preset:", .AutoSize = True})
            panel.Controls.Add(codecPresetCombo)
            panel.Controls.Add(New Label() With {.Text = "Keyframe every (seconds):", .AutoSize = True})
            panel.Controls.Add(keyframeSeconds)
            panel.Controls.Add(New Label() With {.Text = "Shuffle seed:", .AutoSize = True})
            panel.Controls.Add(shuffleSeedValue)
            panel.Controls.Add(twoPassCheck)
            panel.Controls.Add(normalizeAudioCheck)
            panel.Controls.Add(smartTransitionsCheck)

            container.Controls.Add(panel)
            Return container
        End Function

        Private Function BuildBottomPanel() As Control
            Dim panel As New FlowLayoutPanel()
            panel.Dock = DockStyle.Fill
            panel.FlowDirection = FlowDirection.LeftToRight
            panel.Padding = New Padding(8)

            Dim quickButtons As String() = {
                "Add Intro/Outro", "Overlay Memes", "Clip Settings", "Save Template", "Export Video"
            }

            For Each buttonText As String In quickButtons
                Dim btn As New Button()
                btn.Text = buttonText
                btn.Width = 165
                btn.Height = 48
                btn.Font = New Font("Segoe UI", 10, FontStyle.Bold)
                AddHandler btn.Click,
                    Sub(sender, e)
                        MessageBox.Show(String.Format("'{0}' workflow hook is ready.", buttonText), "Feature", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End Sub
                panel.Controls.Add(btn)
            Next

            Dim generateCommandButton As New Button()
            generateCommandButton.Text = "Generate FFmpeg Cmd"
            generateCommandButton.Width = 185
            generateCommandButton.Height = 48
            generateCommandButton.BackColor = Color.DodgerBlue
            generateCommandButton.ForeColor = Color.White
            generateCommandButton.Font = New Font("Segoe UI", 10, FontStyle.Bold)
            AddHandler generateCommandButton.Click,
                Sub(sender, e)
                    UpdateFfmpegPreview()
                    MessageBox.Show("FFmpeg command regenerated.", "FFmpeg", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End Sub
            panel.Controls.Add(generateCommandButton)

            Dim runFfmpegButton As New Button()
            runFfmpegButton.Text = "Run FFmpeg (Prototype)"
            runFfmpegButton.Width = 205
            runFfmpegButton.Height = 48
            runFfmpegButton.BackColor = Color.DarkSlateBlue
            runFfmpegButton.ForeColor = Color.White
            runFfmpegButton.Font = New Font("Segoe UI", 10, FontStyle.Bold)
            AddHandler runFfmpegButton.Click, AddressOf RunFfmpegPrototype
            panel.Controls.Add(runFfmpegButton)

            Dim startButton As New Button()
            startButton.Text = "Start Generation!"
            startButton.Width = 220
            startButton.Height = 48
            startButton.BackColor = Color.ForestGreen
            startButton.ForeColor = Color.White
            startButton.Font = New Font("Segoe UI", 11, FontStyle.Bold)
            AddHandler startButton.Click,
                Sub(sender, e)
                    UpdatePlanPreview()
                    UpdateFfmpegPreview()
                    MessageBox.Show("Generation has started based on the current plan.", "YTP Maker Advanced", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End Sub
            panel.Controls.Add(startButton)

            Return panel
        End Function

        Private Sub SelectInputVideo(sender As Object, e As EventArgs)
            Using dialog As New OpenFileDialog()
                dialog.Filter = "Video Files|*.mp4;*.wmv;*.avi;*.mkv|All Files|*.*"
                dialog.Title = "Select source video"
                If dialog.ShowDialog() = DialogResult.OK Then
                    inputVideoText.Text = dialog.FileName
                    If String.IsNullOrWhiteSpace(outputVideoText.Text) Then
                        outputVideoText.Text = Path.Combine(Path.GetDirectoryName(dialog.FileName), "ytp_output.mp4")
                    End If
                End If
            End Using
        End Sub

        Private Sub OnLongformModeChanged(sender As Object, e As EventArgs)
            If longformModeCheck.Checked Then
                durationTrack.Minimum = 25
                durationTrack.Maximum = 120
                If durationTrack.Value < 25 Then
                    durationTrack.Value = 25
                End If
                clipTrack.Maximum = 150000
            Else
                durationTrack.Minimum = 1
                durationTrack.Maximum = 45
                If durationTrack.Value > 45 Then
                    durationTrack.Value = 25
                End If
                clipTrack.Maximum = 70000
                If clipTrack.Value > clipTrack.Maximum Then
                    clipTrack.Value = clipTrack.Maximum
                End If
            End If

            UpdateRangeLabels()
            UpdateAllPreviews()
        End Sub

        Private Sub OnEffectsChanged(sender As Object, e As ItemCheckEventArgs)
            BeginInvoke(New MethodInvoker(AddressOf UpdateAllPreviews))
        End Sub

        Private Sub OnParameterChanged(sender As Object, e As EventArgs)
            UpdateRangeLabels()
            UpdateAllPreviews()
        End Sub

        Private Sub UpdateAllPreviews()
            UpdatePlanPreview()
            UpdateFfmpegPreview()
        End Sub

        Private Sub UpdateRangeLabels()
            durationLabel.Text = String.Format("Stream Duration: {0} minute(s)", durationTrack.Value)
            clipLabel.Text = String.Format("Clip Count: {0}", clipTrack.Value.ToString("N0"))
            minClipLabel.Text = String.Format("Min/Max Clip Length: {0} to {1} sec", minClipTrack.Value, Math.Max(minClipTrack.Value + 12, 30))
        End Sub

        Private Sub UpdatePlanPreview()
            Dim selectedEffects As String() = effectsChecked.CheckedItems.Cast(Of String)().ToArray()
            Dim summary As New StringBuilder()

            summary.AppendLine("=== YTP Maker Advanced Plan ===")
            summary.AppendLine(String.Format("Mode: {0}", If(longformModeCheck.Checked, "Longform", "Standard")))
            summary.AppendLine(String.Format("Duration Target: {0} minute(s)", durationTrack.Value))
            summary.AppendLine(String.Format("Clip Count Target: {0}", clipTrack.Value.ToString("N0")))
            summary.AppendLine(String.Format("Clip Length Window: {0}-{1} seconds", minClipTrack.Value, Math.Max(minClipTrack.Value + 12, 30)))
            summary.AppendLine(String.Format("Mega YTP: {0}", If(megaYtpCheck.Checked, "Enabled", "Disabled")))

            If adaptiveBeatSyncCheck.Checked Then
                summary.AppendLine(String.Format("Adaptive Beat Sync: Enabled (Strength {0}/10)", beatSyncStrength.Value))
            Else
                summary.AppendLine("Adaptive Beat Sync: Disabled")
            End If

            summary.AppendLine(String.Format("Cut Intensity Profile: {0}", cutIntensity.SelectedItem))
            summary.AppendLine(String.Format("Smart Transition Mix: {0}", If(smartTransitionsCheck.Checked, "Enabled", "Disabled")))
            summary.AppendLine(String.Format("Windows Profile: {0}", windowsCompatCombo.SelectedItem))
            summary.AppendLine(String.Format("Legacy Codec Safety: {0}", If(legacyCodecCheck.Checked, "Enabled", "Disabled")))
            summary.AppendLine("Effects: " & If(selectedEffects.Any(), String.Join(", ", selectedEffects), "None"))
            summary.AppendLine()

            If ffmpegEnabledCheck.Checked Then
                summary.AppendLine("FFmpeg Pipeline: Enabled")
                summary.AppendLine(String.Format("Codec Preset: {0}", codecPresetCombo.SelectedItem))
                summary.AppendLine(String.Format("Audio Normalize: {0}", If(normalizeAudioCheck.Checked, "On", "Off")))
                summary.AppendLine(String.Format("Two-pass: {0}", If(twoPassCheck.Checked, "On", "Off")))
                summary.AppendLine(String.Format("Shuffle Seed: {0}", shuffleSeedValue.Value))
            Else
                summary.AppendLine("FFmpeg Pipeline: Disabled")
            End If

            summary.AppendLine("Estimated Render Complexity: " & EstimateComplexity(selectedEffects.Length))
            planPreview.Text = summary.ToString()
        End Sub

        Private Sub UpdateFfmpegPreview()
            ffmpegCommandPreview.Text = BuildFfmpegCommandPreview()
        End Sub

        Private Function BuildFfmpegCommandPreview() As String
            If Not ffmpegEnabledCheck.Checked Then
                Return "FFmpeg pipeline disabled."
            End If

            Dim chosenProfile As String = windowsCompatCombo.SelectedItem.ToString()
            Dim codecTokens As String() = codecPresetCombo.SelectedItem.ToString().Split(" "c)
            Dim codec As String = codecTokens(0)
            Dim preset As String = If(codecTokens.Length > 1, codecTokens(1), "medium")

            If chosenProfile = "Windows XP" OrElse legacyCodecCheck.Checked Then
                codec = "mpeg4"
                preset = ""
            End If

            Dim filterParts As New List(Of String)()
            If smartTransitionsCheck.Checked Then
                filterParts.Add("tblend=all_mode='average':all_opacity=0.15")
            End If
            If effectsChecked.CheckedItems.Contains("Invert") Then
                filterParts.Add("negate")
            End If
            If effectsChecked.CheckedItems.Contains("Mirror") Then
                filterParts.Add("hflip")
            End If
            If effectsChecked.CheckedItems.Contains("Rainbow") Then
                filterParts.Add("hue=s=2")
            End If
            If effectsChecked.CheckedItems.Contains("Frame Rate Cuts") Then
                filterParts.Add("fps=18")
            End If
            If adaptiveBeatSyncCheck.Checked Then
                filterParts.Add(String.Format("setpts=(1/{0})*PTS", Math.Max(1D, CDbl(beatSyncStrength.Value) / 5D).ToString("0.0")))
            End If

            Dim audioFilterParts As New List(Of String)()
            If normalizeAudioCheck.Checked Then
                audioFilterParts.Add("loudnorm")
            End If
            If effectsChecked.CheckedItems.Contains("Reverse") Then
                audioFilterParts.Add("areverse")
            End If
            If effectsChecked.CheckedItems.Contains("Pitch Shift") Then
                audioFilterParts.Add("asetrate=44100*1.12,aresample=44100")
            End If

            Dim builder As New StringBuilder()
            builder.Append(ffmpegPathText.Text.Trim())
            builder.Append(" -y")

            If Not String.IsNullOrWhiteSpace(introClipText.Text) Then
                builder.Append(" -i ").Append(QuotePath(introClipText.Text.Trim()))
            End If

            builder.Append(" -i ").Append(QuotePath(If(String.IsNullOrWhiteSpace(inputVideoText.Text), "input.mp4", inputVideoText.Text.Trim())))

            If Not String.IsNullOrWhiteSpace(outroClipText.Text) Then
                builder.Append(" -i ").Append(QuotePath(outroClipText.Text.Trim()))
            End If

            If filterParts.Count > 0 Then
                builder.Append(" -vf ").Append(QuotePath(String.Join(",", filterParts.ToArray())))
            End If

            If audioFilterParts.Count > 0 Then
                builder.Append(" -af ").Append(QuotePath(String.Join(",", audioFilterParts.ToArray())))
            End If

            builder.Append(" -c:v ").Append(codec)
            If Not String.IsNullOrEmpty(preset) Then
                builder.Append(" -preset ").Append(preset)
            End If

            builder.Append(" -pix_fmt yuv420p")

            If chosenProfile = "Windows Vista" OrElse chosenProfile = "Windows 7" OrElse chosenProfile = "Windows 8" OrElse chosenProfile = "Windows 8.1" Then
                builder.Append(" -profile:v baseline -level 3.0")
            End If

            If forceSingleThreadCheck.Checked OrElse chosenProfile = "Windows XP" Then
                builder.Append(" -threads 1")
            End If

            builder.Append(" -g ").Append(CInt(keyframeSeconds.Value * 30D).ToString())
            builder.Append(" -metadata ytp_seed=").Append(shuffleSeedValue.Value.ToString())

            If twoPassCheck.Checked Then
                builder.Append(" -pass 1")
            End If

            builder.Append(" ").Append(QuotePath(If(String.IsNullOrWhiteSpace(outputVideoText.Text), "output_ytp.mp4", outputVideoText.Text.Trim())))
            Return builder.ToString()
        End Function

        Private Function QuotePath(value As String) As String
            Return "\"" & value.Replace("\"", String.Empty) & "\""
        End Function

        Private Function EstimateComplexity(effectCount As Integer) As String
            Dim score As Integer = effectCount + CInt(Math.Ceiling(durationTrack.Value / 15.0R))
            If longformModeCheck.Checked Then score += 3
            If adaptiveBeatSyncCheck.Checked Then score += 2
            If megaYtpCheck.Checked Then score += 2
            If ffmpegEnabledCheck.Checked Then score += 3
            If twoPassCheck.Checked Then score += 2
            If forceSingleThreadCheck.Checked Then score += 1

            If score <= 6 Then Return "Low"
            If score <= 11 Then Return "Medium"
            If score <= 16 Then Return "High"
            Return "Extreme"
        End Function

        Private Sub RunFfmpegPrototype(sender As Object, e As EventArgs)
            If Not ffmpegEnabledCheck.Checked Then
                MessageBox.Show("Enable FFmpeg pipeline first.", "FFmpeg", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim inputPath As String = inputVideoText.Text.Trim()
            If String.IsNullOrWhiteSpace(inputPath) OrElse Not File.Exists(inputPath) Then
                MessageBox.Show("Select a valid input video file before running FFmpeg.", "FFmpeg", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim cmd As String = BuildFfmpegCommandPreview()
            Try
                Dim processInfo As New ProcessStartInfo()
                processInfo.FileName = "cmd.exe"
                processInfo.Arguments = "/C " & cmd
                processInfo.UseShellExecute = False
                processInfo.CreateNoWindow = True
                Process.Start(processInfo)
                MessageBox.Show("FFmpeg process started (prototype mode).", "FFmpeg", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Catch ex As Exception
                MessageBox.Show("Unable to start FFmpeg process: " & ex.Message, "FFmpeg", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub
    End Class
End Namespace
