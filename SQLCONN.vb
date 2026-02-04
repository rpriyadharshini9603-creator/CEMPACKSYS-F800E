Public Class SQLCONN
    Dim constr As String
    Dim conSuccess As Boolean
    Public sqlhostname As String
    Public sqlusername As String
    Public sqlPassword As String
    Public SQLDATABASE As String
    Public sqlportname As String
    Private Sub SQLCONN_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadMySQLSettings()
    End Sub

    Private Sub LoadMySQLSettings()
        SetTextAndVar(txtsqlhost, sqlhostname, "SQLHOSTNAME")
        SetTextAndVar(txtsqlUser, sqlusername, "SQLUSERNAME")
        SetTextAndVar(txtsqlPwd, sqlPassword, "SQLPASSWORD")
        SetTextAndVar(txtsqlDB, SQLDATABASE, "SQLDATABASE")
    End Sub

    Private Sub SetTextAndVar(tb As TextBox, ByRef var As String, key As String)
        Dim x = RestoreSettings(TITLE1, "Properties", key)
        If x <> "" Then
            var = x
            tb.Text = x
        Else
            var = tb.Text
        End If
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim X

        conSuccess = False
        If Trim(txtsqlhost.Text) <> "" And Trim(txtsqlUser.Text) <> "" And Trim(txtsqlPwd.Text) <> "" And Trim(txtsqlDB.Text) <> "" Then
            DBHostName = Trim(txtsqlhost.Text)
            DBUserId = Trim(txtsqlUser.Text)
            DBPassword = Trim(txtsqlPwd.Text)
            DBName = Trim(txtsqlDB.Text)

            Try
                Dim SQLhelper = New SQLHelper(DBHostName, True, uid:=DBUserId, password:=DBPassword)
                Dim checkSql As String = $"SELECT DB_ID('{DBName}')"
                Dim result = SQLhelper.ExecuteScalar(checkSql)
                NewDB = False
                If result Is Nothing OrElse IsDBNull(result) Then
                    Dim createSql As String = $"CREATE DATABASE [{DBName}]"
                    SQLhelper.ExecuteScalar(createSql)
                    NewDB = True
                End If


                '  Dim sql As String = $"IF DB_ID('{DBName}') IS NULL CREATE DATABASE [{DBName}];"

            Catch ex As Exception
                MessageBox.Show("Error while checking/creating database: " & ex.Message)
            End Try

            helper = New SQLHelper(DBHostName, True, uid:=DBUserId, password:=DBPassword, database:=DBName)
            Try
                If helper.OpenConnection() Then
                    X = SaveProfile_Setting(TITLE1, "Properties", "DBHostName", Trim(txtsqlhost.Text))
                    X = SaveProfile_Setting(TITLE1, "Properties", "DBUserId", Trim(txtsqlUser.Text))
                    X = SaveProfile_Setting(TITLE1, "Properties", "DBPassword", Trim(txtsqlPwd.Text))
                    X = SaveProfile_Setting(TITLE1, "Properties", "DBName", Trim(txtsqlDB.Text))
                    conSuccess = True

                Else
                    MessageBox.Show("Database connection could not succeed!!!")
                End If
            Catch ex As Exception
                MessageBox.Show("Error: " & ex.Message)
            Finally
                helper.CloseConnection()
                If conSuccess Then
                    Me.Hide()
                    frmSplash.Show()
                End If
            End Try
        Else
            MsgBox("Please Enter the All the Details...", vbOKOnly, "TMS SETTING MISSING ERROR")
            Exit Sub
        End If
    End Sub

    Private Sub frmMYSQLCONN_Load(sender As Object, e As EventArgs) Handles Me.Load
        setupAppInitialisation()
        txtsqlhost.Text = DBHostName
        txtsqlUser.Text = DBUserId
        txtsqlPwd.Text = DBPassword
        txtsqlDB.Text = DBName
    End Sub
    Private Sub setupAppInitialisation()
        DBHostName = GetSetting("DBHostName", "localhost")
        DBPort = GetSetting("DBPort", "3306")
        DBUserId = GetSetting("DBUserId", "root")
        DBPassword = GetSetting("DBPassword", "abc123!@")
        DBName = GetSetting("DBName", "UTCLAWPR4PKF8ETCP")
    End Sub
    Public Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.Close()
        Application.Exit()
    End Sub
End Class
