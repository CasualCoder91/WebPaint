Public Class _Default
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub btnLoadImage_Click(sender As Object, e As EventArgs)
        webPaint.DisplayImage("https://www.simplilearn.com/ice9/free_resources_article_thumb/what_is_image_Processing.jpg")
        webPaint.Visible = True
    End Sub
End Class