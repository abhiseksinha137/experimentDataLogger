Imports System.IO

Public Class Form1

    Dim paramDict As New Dictionary(Of String, String)

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If DGV1.RowCount < 2 Then
            MsgBox("Nothing to Save")
        Else
            saveData()
        End If
    End Sub

    Public Function saveData()
        If updateDict() = 1 Then
            writeDict(paramDict)
        Else
            MsgBox("Can't Save")
        End If
    End Function
    Public Function updateDict()
        Dim paramDictDummy As New Dictionary(Of String, String)
        For i = 0 To DGV1.RowCount - 2
            Dim param As String
            Dim val As String
            Try
                param = DGV1.Rows(i).Cells(0).Value.ToString
                val = DGV1.Rows(i).Cells(1).Value.ToString
                paramDictDummy.Add(param, val)
            Catch ex As Exception
                Return 0
            End Try
            paramDict = paramDictDummy
        Next

        Return 1
    End Function

    Public Sub writeDict(ByVal dic As Dictionary(Of String, String))
        Dim s As String = String.Join(vbNewLine, dic.Select(Function(pair) String.Format("{0}={1}", pair.Key, pair.Value)).ToArray())
        Try
            SaveFileDialog1.Filter = "TXT Files (*.txt*)|*.txt"
            SaveFileDialog1.FileName = "untitled.txt"
            SaveFileDialog1.InitialDirectory = "D:\VBNet"
            If SaveFileDialog1.ShowDialog = DialogResult.OK Then
                Using writer As StreamWriter = New StreamWriter(SaveFileDialog1.FileName)
                    writer.Write(s)
                End Using
                MsgBox("Saved")
            End If
            'Using writer As StreamWriter = New StreamWriter("myfile.txt")
            '    writer.Write(s)
            'End Using
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        clearData()
    End Sub
    Public Sub clearData()
        paramDict = New Dictionary(Of String, String)
        DGV1.Rows.Clear()
    End Sub



    Private Sub btnOpen_Click(sender As Object, e As EventArgs) Handles btnOpen.Click
        openDataFile()
    End Sub
    Public Function openDataFile() As Boolean

        If OpenFileDialog1.ShowDialog() = DialogResult.OK Then
            clearData()
            Dim fileName As String = OpenFileDialog1.FileName
            ' Create new StreamReader instance with Using block.
            For Each line As String In File.ReadLines(fileName)
                ' Read one line from file
                Dim words As String() = line.Split(New Char() {"="c})
                paramDict.Add(words(0), words(1))
            Next line
            updateDGV()
            Return True
        End If
        Return False
    End Function

    Public Sub updateDGV()
        DGV1.Rows.Clear()

        For Each kvp As KeyValuePair(Of String, String) In paramDict
            Dim param As String = kvp.Key
            Dim val As String = kvp.Value
            DGV1.Rows.Add(param, val)
        Next
        DGV1.Update()
    End Sub

    Private Sub btnRemove_Click(sender As Object, e As EventArgs) Handles btnRemove.Click
        Dim sel As DataGridViewSelectedRowCollection = DGV1.SelectedRows
        For Each row As DataGridViewRow In sel
            Dim param As String = row.Cells(0).Value.ToString
            Dim res As Boolean = paramDict.Remove(param)
        Next
        updateDGV()
    End Sub

    Private Sub btnLoad_Click(sender As Object, e As EventArgs) Handles btnLoad.Click
        Dim paramDictDummy As Dictionary(Of String, String) = paramDict
        Dim loadData As Boolean = openDataFile()
        If loadData = True Then
            For Each kvp As KeyValuePair(Of String, String) In paramDict
                Dim param As String = kvp.Key
                paramDictDummy(param) = " "
            Next
            paramDict = paramDictDummy
            updateDGV()
        End If

    End Sub

    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        DGV1.Refresh()
        DGV1.Update()
    End Sub



    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        DGV1.Columns(1).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        DGV1.Update()
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        Dim isUpdated As Boolean = updateDict()
        If isUpdated = False Then
            MsgBox("Error! Null Value or Number of rows < 2")
        End If
    End Sub


End Class
