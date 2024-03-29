﻿Public Class _Default
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub btnLoadImage_Click(sender As Object, e As EventArgs)
        webPaint.DisplayImage("https://picsum.photos/200/300")
        webPaint.Visible = True
    End Sub

    Protected Sub btnSaveImage_Click(sender As Object, e As EventArgs)
        Dim path As String = Server.MapPath("~/Images")
        Dim fileName As String = DateTime.Now.ToString().Replace("/", "-").Replace(" ", "- ").Replace(":", "") + ".png"
        Dim fullPath As String = path + "\\" + fileName
        webPaint.UploadImage(fullPath)
        webPaint.Visible = False
    End Sub

    Protected Sub btnUpload_Click(sender As Object, e As EventArgs)
        webPaint.DisplayImage(Me.FileUpload.FileBytes)
        webPaint.Visible = True
    End Sub
End Class