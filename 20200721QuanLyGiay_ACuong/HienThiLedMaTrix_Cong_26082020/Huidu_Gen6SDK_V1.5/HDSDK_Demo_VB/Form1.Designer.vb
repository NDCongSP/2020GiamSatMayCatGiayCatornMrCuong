Imports System.Runtime.InteropServices
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    Dim m_nSendType As Integer
    Dim m_pSendParams As IntPtr

    'Form 重写 Dispose，以清理组件列表。
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
        Marshal.FreeHGlobal(m_pSendParams)
    End Sub

    'Windows 窗体设计器所必需的
    Private components As System.ComponentModel.IContainer

    '注意: 以下过程是 Windows 窗体设计器所必需的
    '可以使用 Windows 窗体设计器修改它。
    '不要使用代码编辑器修改它。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.ButtonText = New System.Windows.Forms.Button()
        Me.ButtonImage = New System.Windows.Forms.Button()
        Me.ButtonTime = New System.Windows.Forms.Button()
        Me.ButtonRealTimeArea = New System.Windows.Forms.Button()
        Me.ButtonCmd = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'ButtonText
        '
        Me.ButtonText.Location = New System.Drawing.Point(24, 27)
        Me.ButtonText.Name = "ButtonText"
        Me.ButtonText.Size = New System.Drawing.Size(75, 23)
        Me.ButtonText.TabIndex = 0
        Me.ButtonText.Text = "Text"
        Me.ButtonText.UseVisualStyleBackColor = True
        '
        'ButtonImage
        '
        Me.ButtonImage.Location = New System.Drawing.Point(164, 27)
        Me.ButtonImage.Name = "ButtonImage"
        Me.ButtonImage.Size = New System.Drawing.Size(75, 23)
        Me.ButtonImage.TabIndex = 0
        Me.ButtonImage.Text = "Image"
        Me.ButtonImage.UseVisualStyleBackColor = True
        '
        'ButtonTime
        '
        Me.ButtonTime.Location = New System.Drawing.Point(24, 92)
        Me.ButtonTime.Name = "ButtonTime"
        Me.ButtonTime.Size = New System.Drawing.Size(75, 23)
        Me.ButtonTime.TabIndex = 0
        Me.ButtonTime.Text = "Time"
        Me.ButtonTime.UseVisualStyleBackColor = True
        '
        'ButtonRealTimeArea
        '
        Me.ButtonRealTimeArea.Location = New System.Drawing.Point(164, 92)
        Me.ButtonRealTimeArea.Name = "ButtonRealTimeArea"
        Me.ButtonRealTimeArea.Size = New System.Drawing.Size(75, 23)
        Me.ButtonRealTimeArea.TabIndex = 0
        Me.ButtonRealTimeArea.Text = "RealTimeArea"
        Me.ButtonRealTimeArea.UseVisualStyleBackColor = True
        '
        'ButtonCmd
        '
        Me.ButtonCmd.Location = New System.Drawing.Point(24, 154)
        Me.ButtonCmd.Name = "ButtonCmd"
        Me.ButtonCmd.Size = New System.Drawing.Size(75, 23)
        Me.ButtonCmd.TabIndex = 0
        Me.ButtonCmd.Text = "ButtonCmd"
        Me.ButtonCmd.UseVisualStyleBackColor = True
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(284, 261)
        Me.Controls.Add(Me.ButtonRealTimeArea)
        Me.Controls.Add(Me.ButtonCmd)
        Me.Controls.Add(Me.ButtonTime)
        Me.Controls.Add(Me.ButtonImage)
        Me.Controls.Add(Me.ButtonText)
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.ResumeLayout(False)

        InitializeSendParams()
    End Sub

    Friend WithEvents ButtonText As System.Windows.Forms.Button
    Friend WithEvents ButtonImage As System.Windows.Forms.Button
    Friend WithEvents ButtonTime As System.Windows.Forms.Button
    Friend WithEvents ButtonRealTimeArea As System.Windows.Forms.Button
    Friend WithEvents ButtonCmd As System.Windows.Forms.Button

End Class
