Imports System
Imports System.Windows.Forms

Public Module Program
    <STAThread>
    Public Sub Main()
        Application.EnableVisualStyles()
        Application.SetCompatibleTextRenderingDefault(False)
        Application.Run(New YTPMakerAdvanced.MainForm())
    End Sub
End Module
