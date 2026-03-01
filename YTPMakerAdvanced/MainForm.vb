Imports System
Imports System.Collections.Generic
Imports System.Drawing
Imports System.IO
Imports System.Linq
Imports System.Text
Imports System.Windows.Forms

Namespace YTPMakerAdvanced
    Public Class MainForm
        Inherits Form

        Private _project As YtpProject

        Private ReadOnly projectNameText As New TextBox()
        Private ReadOnly projectTypeCombo As New ComboBox()
        Private ReadOnly outputFormatCombo As New ComboBox()
        Private ReadOnly outputPathText As New TextBox()
        Private ReadOnly ffmpegPathText As New TextBox()

        Private ReadOnly clipCountUpDown As New NumericUpDown()
        Private ReadOnly minClipUpDown As New NumericUpDown()
        Private ReadOnly maxClipUpDown As New NumericUpDown()
        Private ReadOnly beatSyncCheck As New CheckBox()
        Private ReadOnly seedUpDown As New NumericUpDown()

        Private ReadOnly audioEffects As New CheckedListBox()
        Private ReadOnly videoEffects As New CheckedListBox()

        Private ReadOnly sourceList As New ListView()
        Private ReadOnly sourceTagText As New TextBox()
        Private ReadOnly sourceFolderText As New TextBox()
        Private ReadOnly audioLibraryList As New ListBox()

        Private ReadOnly planPreview As New TextBox()
        Private ReadOnly commandPreview As New TextBox()
        Private ReadOnly vegasPreview As New TextBox()

        Public Sub New()
            _project = New YtpProject()

            Text = "YTP Maker Advanced - Nostalgic Generator (XP to 8.1)"
            Size = New Size(1500, 950)
            StartPosition = FormStartPosition.CenterScreen

            BuildUi()
            LoadDefaults()
            RefreshAllViews()
        End Sub

        Private Sub BuildUi()
            Dim root As New TableLayoutPanel()
            root.Dock = DockStyle.Fill
            root.RowCount = 2
            root.ColumnCount = 1
            root.RowStyles.Add(New RowStyle(SizeType.Percent, 84.0F))
            root.RowStyles.Add(New RowStyle(SizeType.Percent, 16.0F))
            Controls.Add(root)

            Dim tabs As New TabControl()
            tabs.Dock = DockStyle.Fill
            tabs.TabPages.Add(BuildGeneratorTab())
            tabs.TabPages.Add(BuildSourceExplorerTab())
            tabs.TabPages.Add(BuildVegasTab())
            root.Controls.Add(tabs, 0, 0)

            root.Controls.Add(BuildBottomActions(), 0, 1)
        End Sub

        Private Function BuildGeneratorTab() As TabPage
            Dim page As New TabPage("Generator")
            Dim split As New TableLayoutPanel()
            split.Dock = DockStyle.Fill
            split.ColumnCount = 3
            split.RowCount = 1
            split.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 34.0F))
            split.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 33.0F))
            split.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 33.0F))
            page.Controls.Add(split)

            split.Controls.Add(BuildProjectSettingsPanel(), 0, 0)
            split.Controls.Add(BuildEffectsPanel(), 1, 0)
            split.Controls.Add(BuildPreviewPanel(), 2, 0)
            Return page
        End Function

        Private Function BuildProjectSettingsPanel() As Control
            Dim box As New GroupBox()
            box.Text = "Project / Render Settings"
            box.Dock = DockStyle.Fill

            Dim p As New TableLayoutPanel()
            p.Dock = DockStyle.Fill
            p.ColumnCount = 1
            p.AutoScroll = True
            p.Padding = New Padding(8)

            projectTypeCombo.DropDownStyle = ComboBoxStyle.DropDownList
            projectTypeCombo.Items.AddRange(New Object() {"Generic", "YTP Tennis", "Collab Entry", "YTPMV"})

            outputFormatCombo.DropDownStyle = ComboBoxStyle.DropDownList
            outputFormatCombo.Items.AddRange(New Object() {"MP4", "WMV", "AVI", "MKV"})

            clipCountUpDown.Minimum = 10
            clipCountUpDown.Maximum = 10000
            minClipUpDown.Minimum = 1
            minClipUpDown.Maximum = 120
            maxClipUpDown.Minimum = 2
            maxClipUpDown.Maximum = 180
            seedUpDown.Minimum = 1
            seedUpDown.Maximum = 999999

            beatSyncCheck.Text = "Beat Sync (for YTPMV/Tennis)"

            ffmpegPathText.Text = "ffmpeg"

            p.Controls.Add(New Label() With {.Text = "Project Name", .AutoSize = True})
            p.Controls.Add(projectNameText)
            p.Controls.Add(New Label() With {.Text = "Project Type", .AutoSize = True})
            p.Controls.Add(projectTypeCombo)
            p.Controls.Add(New Label() With {.Text = "Clip Count", .AutoSize = True})
            p.Controls.Add(clipCountUpDown)
            p.Controls.Add(New Label() With {.Text = "Min Clip Seconds", .AutoSize = True})
            p.Controls.Add(minClipUpDown)
            p.Controls.Add(New Label() With {.Text = "Max Clip Seconds", .AutoSize = True})
            p.Controls.Add(maxClipUpDown)
            p.Controls.Add(beatSyncCheck)
            p.Controls.Add(New Label() With {.Text = "Shuffle Seed", .AutoSize = True})
            p.Controls.Add(seedUpDown)
            p.Controls.Add(New Label() With {.Text = "Output Format", .AutoSize = True})
            p.Controls.Add(outputFormatCombo)
            p.Controls.Add(New Label() With {.Text = "Output Path", .AutoSize = True})
            p.Controls.Add(outputPathText)
            p.Controls.Add(New Label() With {.Text = "FFmpeg Path", .AutoSize = True})
            p.Controls.Add(ffmpegPathText)

            Dim handlers As Control() = {projectNameText, outputPathText, ffmpegPathText, projectTypeCombo, outputFormatCombo, clipCountUpDown, minClipUpDown, maxClipUpDown, seedUpDown, beatSyncCheck}
            For Each c As Control In handlers
                AddHandlerByType(c)
            Next

            box.Controls.Add(p)
            Return box
        End Function

        Private Function BuildEffectsPanel() As Control
            Dim box As New GroupBox()
            box.Text = "Nostalgic YTP Effects"
            box.Dock = DockStyle.Fill

            Dim p As New TableLayoutPanel()
            p.Dock = DockStyle.Fill
            p.ColumnCount = 1
            p.RowCount = 4
            p.RowStyles.Add(New RowStyle(SizeType.Absolute, 24))
            p.RowStyles.Add(New RowStyle(SizeType.Percent, 50.0F))
            p.RowStyles.Add(New RowStyle(SizeType.Absolute, 24))
            p.RowStyles.Add(New RowStyle(SizeType.Percent, 50.0F))

            audioEffects.CheckOnClick = True
            audioEffects.Items.AddRange(New Object() {
                "random sound", "mute", "speed up", "speed down", "reverse", "chorus", "vibrato", "stutter", "dance", "squidward", "sus", "lagfun", "low harmony", "high harmony", "confusion", "random chords", "trailing reverses", "low-quality meme", "audio crust", "pitch-shifting loop", "mashup mixing"
            })
            videoEffects.CheckOnClick = True
            videoEffects.Items.AddRange(New Object() {
                "invert", "rainbow", "mirror", "mirror symmetry", "screen clip", "overlay images/sources", "spadinner", "sentence mixing", "shuffle/loop frames", "framerate reduction", "random cuts", "speed loop boost", "scrambling"
            })

            AddHandler audioEffects.ItemCheck, AddressOf OnEffectsChanged
            AddHandler videoEffects.ItemCheck, AddressOf OnEffectsChanged

            p.Controls.Add(New Label() With {.Text = "Audio Effects", .AutoSize = True}, 0, 0)
            p.Controls.Add(audioEffects, 0, 1)
            p.Controls.Add(New Label() With {.Text = "Video Effects", .AutoSize = True}, 0, 2)
            p.Controls.Add(videoEffects, 0, 3)

            box.Controls.Add(p)
            Return box
        End Function

        Private Function BuildPreviewPanel() As Control
            Dim box As New GroupBox()
            box.Text = "Plan + FFmpeg Command"
            box.Dock = DockStyle.Fill

            Dim p As New TableLayoutPanel()
            p.Dock = DockStyle.Fill
            p.ColumnCount = 1
            p.RowCount = 4
            p.RowStyles.Add(New RowStyle(SizeType.Absolute, 22))
            p.RowStyles.Add(New RowStyle(SizeType.Percent, 42.0F))
            p.RowStyles.Add(New RowStyle(SizeType.Absolute, 22))
            p.RowStyles.Add(New RowStyle(SizeType.Percent, 58.0F))

            planPreview.Multiline = True
            planPreview.Dock = DockStyle.Fill
            planPreview.ScrollBars = ScrollBars.Vertical
            planPreview.ReadOnly = True
            planPreview.Font = New Font("Consolas", 9.5F)

            commandPreview.Multiline = True
            commandPreview.Dock = DockStyle.Fill
            commandPreview.ScrollBars = ScrollBars.Vertical
            commandPreview.ReadOnly = True
            commandPreview.Font = New Font("Consolas", 8.5F)

            p.Controls.Add(New Label() With {.Text = "Generation Plan", .AutoSize = True}, 0, 0)
            p.Controls.Add(planPreview, 0, 1)
            p.Controls.Add(New Label() With {.Text = "FFmpeg Pipeline Preview", .AutoSize = True}, 0, 2)
            p.Controls.Add(commandPreview, 0, 3)

            box.Controls.Add(p)
            Return box
        End Function

        Private Function BuildSourceExplorerTab() As TabPage
            Dim page As New TabPage("Source Explorer")
            Dim split As New TableLayoutPanel()
            split.Dock = DockStyle.Fill
            split.ColumnCount = 2
            split.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 65.0F))
            split.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 35.0F))
            page.Controls.Add(split)

            sourceList.Dock = DockStyle.Fill
            sourceList.View = View.Details
            sourceList.Columns.Add("Path", 520)
            sourceList.Columns.Add("Type", 90)
            sourceList.Columns.Add("Tag", 120)
            sourceList.FullRowSelect = True

            Dim leftPanel As New TableLayoutPanel()
            leftPanel.Dock = DockStyle.Fill
            leftPanel.RowCount = 3
            leftPanel.RowStyles.Add(New RowStyle(SizeType.Absolute, 28))
            leftPanel.RowStyles.Add(New RowStyle(SizeType.Absolute, 28))
            leftPanel.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))

            leftPanel.Controls.Add(sourceFolderText, 0, 0)
            Dim browseBtn As New Button() With {.Text = "Browse Folder + Bulk Add"}
            AddHandler browseBtn.Click, AddressOf BrowseAndBulkAdd
            leftPanel.Controls.Add(browseBtn, 0, 1)
            leftPanel.Controls.Add(sourceList, 0, 2)

            Dim rightPanel As New TableLayoutPanel()
            rightPanel.Dock = DockStyle.Fill
            rightPanel.RowCount = 10

            rightPanel.Controls.Add(New Label() With {.Text = "Tag selected assets", .AutoSize = True})
            rightPanel.Controls.Add(sourceTagText)
            Dim tagBtn As New Button() With {.Text = "Apply Tag"}
            AddHandler tagBtn.Click, AddressOf ApplyTagToSelected
            rightPanel.Controls.Add(tagBtn)

            Dim addAudioBtn As New Button() With {.Text = "Add Selected to Audio Library"}
            AddHandler addAudioBtn.Click, AddressOf AddSelectedToAudioLibrary
            rightPanel.Controls.Add(addAudioBtn)

            rightPanel.Controls.Add(New Label() With {.Text = "Audio Library (YTPMV)", .AutoSize = True})
            audioLibraryList.Dock = DockStyle.Fill
            rightPanel.Controls.Add(audioLibraryList)

            Dim addFileBtn As New Button() With {.Text = "Add Files (Manual)"}
            AddHandler addFileBtn.Click, AddressOf AddFilesManual
            rightPanel.Controls.Add(addFileBtn)

            Dim clearBtn As New Button() With {.Text = "Clear Source Library"}
            AddHandler clearBtn.Click,
                Sub(sender, e)
                    _project.Assets.Clear()
                    _project.AudioLibrary.Clear()
                    RefreshAllViews()
                End Sub
            rightPanel.Controls.Add(clearBtn)

            split.Controls.Add(leftPanel, 0, 0)
            split.Controls.Add(rightPanel, 1, 0)
            Return page
        End Function

        Private Function BuildVegasTab() As TabPage
            Dim page As New TabPage("Vegas 12 Adapter")
            Dim p As New TableLayoutPanel()
            p.Dock = DockStyle.Fill
            p.RowCount = 3
            p.RowStyles.Add(New RowStyle(SizeType.Absolute, 42))
            p.RowStyles.Add(New RowStyle(SizeType.Absolute, 42))
            p.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))

            Dim btn1 As New Button() With {.Text = "Generate Vegas Script Template (.txt)"}
            AddHandler btn1.Click, AddressOf ExportVegasTemplate
            p.Controls.Add(btn1, 0, 0)

            Dim btn2 As New Button() With {.Text = "Generate .ytpproj JSON"}
            AddHandler btn2.Click, AddressOf SaveProjectJson
            p.Controls.Add(btn2, 0, 1)

            vegasPreview.Multiline = True
            vegasPreview.ScrollBars = ScrollBars.Vertical
            vegasPreview.Dock = DockStyle.Fill
            vegasPreview.ReadOnly = True
            vegasPreview.Font = New Font("Consolas", 9.0F)
            p.Controls.Add(vegasPreview, 0, 2)

            page.Controls.Add(p)
            Return page
        End Function

        Private Function BuildBottomActions() As Control
            Dim panel As New FlowLayoutPanel()
            panel.Dock = DockStyle.Fill

            Dim saveBtn As New Button() With {.Text = "Save .ytpproj", .Width = 140, .Height = 36}
            AddHandler saveBtn.Click, AddressOf SaveProjectJson
            panel.Controls.Add(saveBtn)

            Dim loadBtn As New Button() With {.Text = "Load .ytpproj", .Width = 140, .Height = 36}
            AddHandler loadBtn.Click, AddressOf LoadProjectJson
            panel.Controls.Add(loadBtn)

            Dim exportBatBtn As New Button() With {.Text = "Export FFmpeg .bat", .Width = 160, .Height = 36}
            AddHandler exportBatBtn.Click, AddressOf ExportPipelineBat
            panel.Controls.Add(exportBatBtn)

            Dim refreshBtn As New Button() With {.Text = "Refresh Preview", .Width = 140, .Height = 36}
            AddHandler refreshBtn.Click, Sub(sender, e) RefreshAllViews()
            panel.Controls.Add(refreshBtn)

            Dim skeletonLbl As New Label() With {.Text = "Ready skeleton for automated + semi-automatic YTP/YTPMV/Tennis/Collab workflows", .AutoSize = True, .Padding = New Padding(18, 10, 0, 0)}
            panel.Controls.Add(skeletonLbl)

            Return panel
        End Function

        Private Sub LoadDefaults()
            projectNameText.Text = _project.ProjectName
            projectTypeCombo.SelectedIndex = 0
            outputFormatCombo.SelectedIndex = 0
            outputPathText.Text = _project.Output.OutputPath
            clipCountUpDown.Value = _project.Generator.ClipCount
            minClipUpDown.Value = _project.Generator.MinClipSeconds
            maxClipUpDown.Value = _project.Generator.MaxClipSeconds
            beatSyncCheck.Checked = _project.Generator.EnableBeatSync
            seedUpDown.Value = _project.Generator.ShuffleSeed
        End Sub

        Private Sub AddHandlerByType(ctrl As Control)
            If TypeOf ctrl Is TextBox Then
                AddHandler DirectCast(ctrl, TextBox).TextChanged, AddressOf OnAnySettingChanged
            ElseIf TypeOf ctrl Is ComboBox Then
                AddHandler DirectCast(ctrl, ComboBox).SelectedIndexChanged, AddressOf OnAnySettingChanged
            ElseIf TypeOf ctrl Is NumericUpDown Then
                AddHandler DirectCast(ctrl, NumericUpDown).ValueChanged, AddressOf OnAnySettingChanged
            ElseIf TypeOf ctrl Is CheckBox Then
                AddHandler DirectCast(ctrl, CheckBox).CheckedChanged, AddressOf OnAnySettingChanged
            End If
        End Sub

        Private Sub OnAnySettingChanged(sender As Object, e As EventArgs)
            PullUiIntoProject()
            RefreshAllViews()
        End Sub

        Private Sub OnEffectsChanged(sender As Object, e As ItemCheckEventArgs)
            BeginInvoke(New MethodInvoker(AddressOf HandleEffectRefresh))
        End Sub

        Private Sub HandleEffectRefresh()
            PullUiIntoProject()
            RefreshAllViews()
        End Sub

        Private Sub PullUiIntoProject()
            _project.ProjectName = projectNameText.Text.Trim()
            _project.Type = CType(projectTypeCombo.SelectedIndex, ProjectType)
            _project.Generator.ClipCount = CInt(clipCountUpDown.Value)
            _project.Generator.MinClipSeconds = CInt(minClipUpDown.Value)
            _project.Generator.MaxClipSeconds = CInt(maxClipUpDown.Value)
            _project.Generator.EnableBeatSync = beatSyncCheck.Checked
            _project.Generator.ShuffleSeed = CInt(seedUpDown.Value)
            _project.Output.OutputFormat = outputFormatCombo.SelectedItem.ToString()
            _project.Output.OutputPath = outputPathText.Text.Trim()

            _project.SelectedAudioEffects = audioEffects.CheckedItems.Cast(Of String)().ToList()
            _project.SelectedVideoEffects = videoEffects.CheckedItems.Cast(Of String)().ToList()
        End Sub

        Private Sub RefreshAllViews()
            planPreview.Text = BuildPlanText()

            Dim pipeline As FfmpegPipelineResult = FfmpegGenerator.BuildPipeline(_project, ffmpegPathText.Text)
            _project.AutoKeyframeData = pipeline.AutoKeyframeData
            commandPreview.Text = String.Join(Environment.NewLine, pipeline.Commands.ToArray())
            vegasPreview.Text = VegasAdapter.BuildVegas12Template(_project)

            sourceList.Items.Clear()
            For Each a As AssetItem In _project.Assets
                Dim item As New ListViewItem(New String() {a.Path, a.Type.ToString(), a.Tag})
                sourceList.Items.Add(item)
            Next

            audioLibraryList.Items.Clear()
            For Each a As AssetItem In _project.AudioLibrary
                audioLibraryList.Items.Add(a.Path)
            Next
        End Sub

        Private Function BuildPlanText() As String
            Dim sb As New StringBuilder()
            sb.AppendLine("=== Nostalgic YTP Generator Plan ===")
            sb.AppendLine("Project: " & _project.ProjectName)
            sb.AppendLine("Mode: " & _project.Type.ToString())
            sb.AppendLine(String.Format("Assets: {0} | Audio Library: {1}", _project.Assets.Count, _project.AudioLibrary.Count))
            sb.AppendLine(String.Format("Clip Count: {0} ({1}-{2} sec)", _project.Generator.ClipCount, _project.Generator.MinClipSeconds, _project.Generator.MaxClipSeconds))
            sb.AppendLine("Beat Sync: " & If(_project.Generator.EnableBeatSync, "On", "Off"))
            sb.AppendLine("Output: " & _project.Output.OutputFormat & " -> " & _project.Output.OutputPath)
            sb.AppendLine("Audio Effects: " & If(_project.SelectedAudioEffects.Count = 0, "None", String.Join(", ", _project.SelectedAudioEffects.ToArray())))
            sb.AppendLine("Video Effects: " & If(_project.SelectedVideoEffects.Count = 0, "None", String.Join(", ", _project.SelectedVideoEffects.ToArray())))
            sb.AppendLine()
            sb.AppendLine("Workflow: Source Explorer -> Auto/Manual FFmpeg remix -> Vegas 12 keyframe assist")
            sb.AppendLine("Project Outputs: .ytpproj JSON + concat pipeline + optional final WMV/MP4/AVI/MKV")
            Return sb.ToString()
        End Function

        Private Sub BrowseAndBulkAdd(sender As Object, e As EventArgs)
            Using dlg As New FolderBrowserDialog()
                If dlg.ShowDialog() = DialogResult.OK Then
                    sourceFolderText.Text = dlg.SelectedPath
                    BulkAddFromFolder(dlg.SelectedPath)
                End If
            End Using
        End Sub

        Private Sub BulkAddFromFolder(folder As String)
            Dim extMap As New Dictionary(Of String, AssetType)(StringComparer.OrdinalIgnoreCase)
            For Each ex As String In New String() {".mp4", ".wmv", ".avi", ".mkv"}
                extMap(ex) = AssetType.Video
            Next
            For Each ex As String In New String() {".png", ".jpg", ".jpeg", ".webp", ".gif"}
                extMap(ex) = AssetType.Image
            Next
            For Each ex As String In New String() {".mp3", ".wav", ".ogg"}
                extMap(ex) = AssetType.Audio
            Next
            For Each ex As String In New String() {".xm", ".mod", ".it"}
                extMap(ex) = AssetType.Tracker
            Next

            For Each path As String In Directory.GetFiles(folder)
                Dim ext As String = Path.GetExtension(path)
                If extMap.ContainsKey(ext) Then
                    Dim a As New AssetItem()
                    a.Path = path
                    a.Type = extMap(ext)
                    a.Tag = Path.GetFileNameWithoutExtension(path)
                    _project.Assets.Add(a)
                    If a.Type = AssetType.Audio OrElse a.Type = AssetType.Tracker Then
                        _project.AudioLibrary.Add(a)
                    End If
                End If
            Next

            RefreshAllViews()
        End Sub

        Private Sub AddFilesManual(sender As Object, e As EventArgs)
            Using dlg As New OpenFileDialog()
                dlg.Multiselect = True
                dlg.Filter = "Media|*.mp4;*.wmv;*.avi;*.mkv;*.png;*.jpg;*.jpeg;*.webp;*.gif;*.mp3;*.wav;*.ogg;*.xm;*.mod;*.it|All files|*.*"
                If dlg.ShowDialog() = DialogResult.OK Then
                    For Each p As String In dlg.FileNames
                        Dim a As New AssetItem()
                        a.Path = p
                        a.Tag = Path.GetFileNameWithoutExtension(p)
                        a.Type = GuessAssetType(p)
                        _project.Assets.Add(a)
                        If a.Type = AssetType.Audio OrElse a.Type = AssetType.Tracker Then
                            _project.AudioLibrary.Add(a)
                        End If
                    Next
                    RefreshAllViews()
                End If
            End Using
        End Sub

        Private Function GuessAssetType(path As String) As AssetType
            Dim ext As String = Path.GetExtension(path).ToLowerInvariant()
            If ext = ".mp3" OrElse ext = ".wav" OrElse ext = ".ogg" Then Return AssetType.Audio
            If ext = ".xm" OrElse ext = ".mod" OrElse ext = ".it" Then Return AssetType.Tracker
            If ext = ".gif" Then Return AssetType.Gif
            If ext = ".png" OrElse ext = ".jpg" OrElse ext = ".jpeg" OrElse ext = ".webp" Then Return AssetType.Image
            Return AssetType.Video
        End Function

        Private Sub ApplyTagToSelected(sender As Object, e As EventArgs)
            Dim tagVal As String = sourceTagText.Text.Trim()
            If String.IsNullOrWhiteSpace(tagVal) Then
                Return
            End If

            For Each selected As ListViewItem In sourceList.SelectedItems
                Dim pathVal As String = selected.SubItems(0).Text
                For Each a As AssetItem In _project.Assets
                    If String.Equals(a.Path, pathVal, StringComparison.OrdinalIgnoreCase) Then
                        a.Tag = tagVal
                    End If
                Next
            Next
            RefreshAllViews()
        End Sub

        Private Sub AddSelectedToAudioLibrary(sender As Object, e As EventArgs)
            For Each selected As ListViewItem In sourceList.SelectedItems
                Dim pathVal As String = selected.SubItems(0).Text
                Dim asset As AssetItem = _project.Assets.FirstOrDefault(Function(a) String.Equals(a.Path, pathVal, StringComparison.OrdinalIgnoreCase))
                If asset IsNot Nothing Then
                    If Not _project.AudioLibrary.Any(Function(a) String.Equals(a.Path, asset.Path, StringComparison.OrdinalIgnoreCase)) Then
                        _project.AudioLibrary.Add(asset)
                    End If
                End If
            Next
            RefreshAllViews()
        End Sub

        Private Sub SaveProjectJson(sender As Object, e As EventArgs)
            PullUiIntoProject()
            Using dlg As New SaveFileDialog()
                dlg.Filter = "YTP Project|*.ytpproj|JSON|*.json"
                dlg.FileName = _project.ProjectName.Replace(" ", "_") & ".ytpproj"
                If dlg.ShowDialog() = DialogResult.OK Then
                    ProjectSerializer.SaveProject(dlg.FileName, _project)
                    MessageBox.Show("Project saved.", "YTP", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            End Using
        End Sub

        Private Sub LoadProjectJson(sender As Object, e As EventArgs)
            Using dlg As New OpenFileDialog()
                dlg.Filter = "YTP Project|*.ytpproj;*.json|All|*.*"
                If dlg.ShowDialog() = DialogResult.OK Then
                    _project = ProjectSerializer.LoadProject(dlg.FileName)
                    PushProjectToUi()
                    RefreshAllViews()
                End If
            End Using
        End Sub

        Private Sub PushProjectToUi()
            projectNameText.Text = _project.ProjectName
            projectTypeCombo.SelectedIndex = CInt(_project.Type)
            outputFormatCombo.SelectedItem = _project.Output.OutputFormat
            outputPathText.Text = _project.Output.OutputPath

            clipCountUpDown.Value = Math.Max(clipCountUpDown.Minimum, Math.Min(clipCountUpDown.Maximum, _project.Generator.ClipCount))
            minClipUpDown.Value = Math.Max(minClipUpDown.Minimum, Math.Min(minClipUpDown.Maximum, _project.Generator.MinClipSeconds))
            maxClipUpDown.Value = Math.Max(maxClipUpDown.Minimum, Math.Min(maxClipUpDown.Maximum, _project.Generator.MaxClipSeconds))
            seedUpDown.Value = Math.Max(seedUpDown.Minimum, Math.Min(seedUpDown.Maximum, _project.Generator.ShuffleSeed))
            beatSyncCheck.Checked = _project.Generator.EnableBeatSync

            SetCheckedItems(audioEffects, _project.SelectedAudioEffects)
            SetCheckedItems(videoEffects, _project.SelectedVideoEffects)
        End Sub

        Private Sub SetCheckedItems(list As CheckedListBox, selected As List(Of String))
            For i As Integer = 0 To list.Items.Count - 1
                Dim name As String = list.Items(i).ToString()
                list.SetItemChecked(i, selected.Any(Function(s) String.Equals(s, name, StringComparison.OrdinalIgnoreCase)))
            Next
        End Sub

        Private Sub ExportPipelineBat(sender As Object, e As EventArgs)
            PullUiIntoProject()
            Dim pipeline As FfmpegPipelineResult = FfmpegGenerator.BuildPipeline(_project, ffmpegPathText.Text)
            Using dlg As New SaveFileDialog()
                dlg.Filter = "Batch file|*.bat|Text file|*.txt"
                dlg.FileName = "render_ytp_pipeline.bat"
                If dlg.ShowDialog() = DialogResult.OK Then
                    File.WriteAllLines(dlg.FileName, pipeline.Commands.ToArray())
                    MessageBox.Show("Pipeline batch exported.", "YTP", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            End Using
        End Sub

        Private Sub ExportVegasTemplate(sender As Object, e As EventArgs)
            PullUiIntoProject()
            Dim template As String = VegasAdapter.BuildVegas12Template(_project)
            Using dlg As New SaveFileDialog()
                dlg.Filter = "Text|*.txt|All|*.*"
                dlg.FileName = "vegas12_ytp_template.txt"
                If dlg.ShowDialog() = DialogResult.OK Then
                    File.WriteAllText(dlg.FileName, template)
                    MessageBox.Show("Vegas template exported.", "Vegas Adapter", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            End Using
            vegasPreview.Text = template
        End Sub
    End Class
End Namespace
