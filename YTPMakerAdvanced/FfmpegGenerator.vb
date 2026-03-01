Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.Text

Namespace YTPMakerAdvanced
    Public Class FfmpegPipelineResult
        Public Property Commands As List(Of String)
        Public Property ConcatList As String
        Public Property AutoKeyframeData As List(Of String)

        Public Sub New()
            Commands = New List(Of String)()
            ConcatList = String.Empty
            AutoKeyframeData = New List(Of String)()
        End Sub
    End Class

    Public NotInheritable Class FfmpegGenerator
        Private Sub New()
        End Sub

        Public Shared Function BuildPipeline(project As YtpProject, ffmpegPath As String) As FfmpegPipelineResult
            Dim result As New FfmpegPipelineResult()
            Dim safeFfmpeg As String = If(String.IsNullOrWhiteSpace(ffmpegPath), "ffmpeg", ffmpegPath)

            If project.Assets.Count = 0 Then
                result.Commands.Add("# No assets loaded.")
                Return result
            End If

            Dim tempDir As String = project.Output.TempPath
            If String.IsNullOrWhiteSpace(tempDir) Then
                tempDir = "temp_clips"
            End If

            result.Commands.Add(String.Format("mkdir \"{0}\"", tempDir))
            Dim maxClips As Integer = Math.Min(project.Generator.ClipCount, project.Assets.Count)

            For i As Integer = 0 To maxClips - 1
                Dim asset As AssetItem = project.Assets(i Mod project.Assets.Count)
                Dim outClip As String = String.Format("{0}\clip_{1:0000}.mp4", tempDir, i)
                Dim vf As String = BuildVideoFilter(project.SelectedVideoEffects)
                Dim af As String = BuildAudioFilter(project.SelectedAudioEffects)

                Dim cmd As New StringBuilder()
                cmd.Append(safeFfmpeg)
                cmd.Append(" -y -i ").Append(Quote(asset.Path))
                cmd.Append(" -t ").Append(project.Generator.MaxClipSeconds)
                If Not String.IsNullOrWhiteSpace(vf) Then
                    cmd.Append(" -vf ").Append(Quote(vf))
                End If
                If Not String.IsNullOrWhiteSpace(af) Then
                    cmd.Append(" -af ").Append(Quote(af))
                End If
                cmd.Append(" -c:v libx264 -pix_fmt yuv420p ").Append(Quote(outClip))
                result.Commands.Add(cmd.ToString())

                result.AutoKeyframeData.Add(String.Format("clip={0};asset={1};effectCount={2}", i, Path.GetFileName(asset.Path), project.SelectedVideoEffects.Count + project.SelectedAudioEffects.Count))
            Next

            Dim concatPath As String = String.Format("{0}\concat_list.txt", tempDir)
            result.ConcatList = concatPath
            result.Commands.Add(String.Format("# Write concat list to {0}", concatPath))
            result.Commands.Add(String.Format("{0} -y -f concat -safe 0 -i {1} -c copy {2}", safeFfmpeg, Quote(concatPath), Quote(project.Output.OutputPath)))

            If project.Output.EnableFinalRender Then
                result.Commands.Add(String.Format("{0} -y -i {1} -c:v libx264 -preset veryfast {2}", safeFfmpeg, Quote(project.Output.OutputPath), Quote(FinalizeOutputName(project.Output.OutputPath, project.Output.OutputFormat))))
            End If

            Return result
        End Function

        Private Shared Function BuildVideoFilter(selected As List(Of String)) As String
            Dim f As New List(Of String)()
            If selected.Contains("invert") Then f.Add("negate")
            If selected.Contains("rainbow") Then f.Add("hue=s=2")
            If selected.Contains("mirror") Then f.Add("hflip")
            If selected.Contains("framerate reduction") Then f.Add("fps=10")
            If selected.Contains("scrambling") Then f.Add("shuffleframes=0|2|1|3")
            Return String.Join(",", f.ToArray())
        End Function

        Private Shared Function BuildAudioFilter(selected As List(Of String)) As String
            Dim f As New List(Of String)()
            If selected.Contains("mute") Then f.Add("volume=0")
            If selected.Contains("speed up") Then f.Add("atempo=1.25")
            If selected.Contains("reverse") Then f.Add("areverse")
            If selected.Contains("chorus") Then f.Add("aecho=0.8:0.9:40|80:0.4|0.3")
            If selected.Contains("pitch-shifting loop") Then f.Add("asetrate=44100*1.15,aresample=44100")
            Return String.Join(",", f.ToArray())
        End Function

        Private Shared Function FinalizeOutputName(path As String, formatName As String) As String
            Dim ext As String = formatName.ToLowerInvariant()
            If ext = "mp4" OrElse ext = "wmv" OrElse ext = "avi" OrElse ext = "mkv" Then
                Return Path.ChangeExtension(path, ext)
            End If
            Return path
        End Function

        Private Shared Function Quote(value As String) As String
            Return "\"" & value.Replace("\"", "") & "\""
        End Function
    End Class
End Namespace
