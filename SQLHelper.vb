Imports System.Data.SqlClient
Imports System.Data

Public Class SQLHelper
    Public ReadOnly _connectionString As String
    Private _connection As SqlConnection
    Public Sub New(server As String, integratedSecurity As Boolean, Optional uid As String = "", Optional password As String = "", Optional database As String = "")
        If integratedSecurity Then
            If database <> "" Then
                _connectionString = $"Server={server};Database={database};User Id={uid};Password={password};"
            Else
                _connectionString = $"Server={server};User Id={uid};Password={password};"
            End If

        Else
            If database <> "" Then
                _connectionString = $"Server={server};Database={database};User Id={uid};Password={password};Integrated Security=True;"
            Else
                _connectionString = $"Server={server};User Id={uid};Password={password};Integrated Security=True;"
            End If

        End If
    End Sub

    Public Function OpenConnection() As Boolean
        Try
            _connection = New SqlConnection(_connectionString)
            _connection.Open()
            Console.WriteLine("Database connection opened successfully.")
            Return True
        Catch ex As SqlException
            Console.WriteLine($"Error opening connection: {ex.Message}")
            Return False
        End Try
    End Function

    Public Function CloseConnection() As Boolean
        Try
            If _connection IsNot Nothing AndAlso _connection.State = ConnectionState.Open Then
                _connection.Close()
                Console.WriteLine("Database connection closed successfully.")
            End If
            Return True
        Catch ex As SqlException
            Console.WriteLine($"Error closing connection: {ex.Message}")
            Return False
        End Try
    End Function

    Public Function ExecuteNonQuery(sql As String, Optional parameters As SqlParameter() = Nothing) As Integer
        Dim rowsAffected As Integer = -1
        If OpenConnection() Then
            Using cmd As New SqlCommand(sql, _connection)
                If parameters IsNot Nothing Then
                    cmd.Parameters.AddRange(parameters)
                End If
                Try
                    rowsAffected = cmd.ExecuteNonQuery()
                    Console.WriteLine($"Query executed successfully. Rows affected: {rowsAffected}")
                Catch ex As SqlException

                    Console.WriteLine($"Error executing non-query: {ex.Message}")
                Finally
                    CloseConnection()
                End Try
            End Using
        End If
        Return rowsAffected
    End Function



    Public Async Function ExecuteScalarAsync(query As String, params As SqlParameter()) As Task(Of Object)
        Using con As New SqlConnection(_connectionString)
            Using cmd As New SqlCommand(query, con)
                If params IsNot Nothing Then
                    cmd.Parameters.AddRange(params)
                End If
                Await con.OpenAsync()
                Return Await cmd.ExecuteScalarAsync()
            End Using
        End Using
    End Function
    Public Function ExecuteReader(sql As String, Optional parameters As SqlParameter() = Nothing) As DataTable
        Dim dt As New DataTable()

        If OpenConnection() Then
            Using cmd As New SqlCommand(sql, _connection)
                If parameters IsNot Nothing Then
                    cmd.Parameters.AddRange(parameters)
                End If
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    Try
                        dt.Load(reader)
                        Console.WriteLine("Reader query executed successfully.")
                    Catch ex As SqlException
                        Console.WriteLine($"Error executing reader query: {ex.Message}")
                        dt = Nothing
                    Finally
                        CloseConnection()
                    End Try
                End Using
            End Using

        End If

        Return dt
    End Function
    Public Function ExecuteDataTable(query As String, Optional parameters As SqlParameter() = Nothing) As DataTable
        Dim dt As New DataTable()
        ClearDataTable(dt)
        Try
            'CloseConnection()
            If OpenConnection() Then
                Using cmd As New SqlCommand(query, _connection)
                    If parameters IsNot Nothing Then
                        cmd.Parameters.AddRange(parameters)
                    End If
                    ' _connection.Open()
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        dt.Load(reader)
                    End Using
                End Using
            End If
        Catch ex As Exception
            MessageBox.Show("SQLHelper.ExecuteDataTable Error: " & ex.Message)
        End Try

        Return dt

    End Function


    Public Sub ClearDataTable(ByRef dt As DataTable, Optional dispose As Boolean = False)
        If dt IsNot Nothing Then
            dt.Clear()
            If dispose Then
                dt.Dispose()
                dt = Nothing
            End If
        End If
    End Sub

    Public Async Function ExecuteNonQueryAsync(sql As String, parameters As SqlParameter()) As Task
        Using conn As New SqlConnection(_connectionString)
            Await conn.OpenAsync()
            Using cmd As New SqlCommand(sql, conn)
                cmd.Parameters.AddRange(parameters)
                Await cmd.ExecuteNonQueryAsync()
            End Using
        End Using
    End Function


    Public Function ExecuteScalar(sql As String, Optional parameters As SqlParameter() = Nothing) As Object
        Dim result As Object = Nothing

        If OpenConnection() Then
            Using cmd As New SqlCommand(sql, _connection)
                If parameters IsNot Nothing Then
                    cmd.Parameters.AddRange(parameters)
                End If
                Try
                    result = cmd.ExecuteScalar()
                    Console.WriteLine("Scalar query executed successfully.")
                Catch ex As SqlException
                    Console.WriteLine($"Error executing scalar query: {ex.Message}")
                Finally
                    CloseConnection()
                End Try
            End Using
        End If
        Return result
    End Function
    Public Function TableExists(tableName As String) As Boolean
        Dim sql As String = $"IF OBJECT_ID('{tableName}', 'U') IS NOT NULL SELECT 1 ELSE SELECT 0"
        Dim result As Object = ExecuteScalar(sql)
        Return Convert.ToInt32(result) = 1
    End Function

End Class
