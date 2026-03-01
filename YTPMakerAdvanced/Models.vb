Imports System
Imports System.Collections.Generic

Namespace YTPMakerAdvanced
    Public Enum AssetType
        Video
        Image
        Gif
        Audio
        Tracker
    End Enum

    Public Enum ProjectType
        Generic
        YTPTennis
        CollabEntry
        YTPMV
    End Enum

    <Serializable>
    Public Class AssetItem
        Public Property Path As String
        Public Property Type As AssetType
        Public Property Tag As String

        Public Sub New()
            Path = String.Empty
            Tag = String.Empty
            Type = AssetType.Video
        End Sub
    End Class

    <Serializable>
    Public Class GeneratorSettings
        Public Property MinClipSeconds As Integer
        Public Property MaxClipSeconds As Integer
        Public Property ClipCount As Integer
        Public Property EnableBeatSync As Boolean
        Public Property ShuffleSeed As Integer

        Public Sub New()
            MinClipSeconds = 2
            MaxClipSeconds = 12
            ClipCount = 150
            EnableBeatSync = False
            ShuffleSeed = 1337
        End Sub
    End Class

    <Serializable>
    Public Class OutputSettings
        Public Property OutputFormat As String
        Public Property OutputPath As String
        Public Property TempPath As String
        Public Property EnableFinalRender As Boolean

        Public Sub New()
            OutputFormat = "MP4"
            OutputPath = "output_ytp.mp4"
            TempPath = "temp_clips"
            EnableFinalRender = True
        End Sub
    End Class

    <Serializable>
    Public Class YtpProject
        Public Property ProjectName As String
        Public Property Type As ProjectType
        Public Property Assets As List(Of AssetItem)
        Public Property AudioLibrary As List(Of AssetItem)
        Public Property SelectedAudioEffects As List(Of String)
        Public Property SelectedVideoEffects As List(Of String)
        Public Property Generator As GeneratorSettings
        Public Property Output As OutputSettings
        Public Property AutoKeyframeData As List(Of String)

        Public Sub New()
            ProjectName = "Untitled YTP Project"
            Type = ProjectType.Generic
            Assets = New List(Of AssetItem)()
            AudioLibrary = New List(Of AssetItem)()
            SelectedAudioEffects = New List(Of String)()
            SelectedVideoEffects = New List(Of String)()
            Generator = New GeneratorSettings()
            Output = New OutputSettings()
            AutoKeyframeData = New List(Of String)()
        End Sub
    End Class
End Namespace
