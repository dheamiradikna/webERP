Imports Microsoft.VisualBasic
Namespace [model]
Public Class LikeModel
    Public Property status() As String
            Get
                Return m_status
            End Get
            Set
                m_status = Value
            End Set
        End Property
        Private m_status As String
        Public Property message() As String
            Get
                Return m_message
            End Get
            Set
                m_message = Value
            End Set
        End Property
        Private m_message As String
End Class

End Namespace
