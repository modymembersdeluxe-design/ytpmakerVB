Imports System
Imports System.IO
Imports System.Web.Script.Serialization

Namespace YTPMakerAdvanced
    Public NotInheritable Class ProjectSerializer
        Private Sub New()
        End Sub

        Public Shared Sub SaveProject(path As String, project As YtpProject)
            Dim serializer As New JavaScriptSerializer()
            serializer.MaxJsonLength = Integer.MaxValue
            Dim json As String = serializer.Serialize(project)
            File.WriteAllText(path, json)
        End Sub

        Public Shared Function LoadProject(path As String) As YtpProject
            Dim serializer As New JavaScriptSerializer()
            serializer.MaxJsonLength = Integer.MaxValue
            Dim json As String = File.ReadAllText(path)
            Dim loaded As YtpProject = serializer.Deserialize(Of YtpProject)(json)
            If loaded Is Nothing Then
                Return New YtpProject()
            End If
            Return loaded
        End Function
    End Class
End Namespace
