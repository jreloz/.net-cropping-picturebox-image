 ''--------------------Crop image-----------------------
    Dim cropX As Integer
    Dim cropY As Integer
    Dim cropWidth As Integer
    Dim cropHeight As Integer

    Dim oCropX As Integer
    Dim oCropY As Integer
    Dim cropBitmap As Bitmap

    Public cropPen As Pen
    Public cropPenSize As Integer = 1 '2
    Public cropDashStyle As Drawing2D.DashStyle = Drawing2D.DashStyle.Solid
    Public cropPenColor As Color = Color.Black

    Dim tmppoint As Point

    Private Sub PictureBox1_MouseMove(sender As Object, e As MouseEventArgs) Handles PictureBox1.MouseMove
        Try

            If PictureBox1.Image Is Nothing Then Exit Sub

            If e.Button = Windows.Forms.MouseButtons.Left Then

                PictureBox1.Refresh()
                cropWidth = e.X - cropX
                cropHeight = e.Y - cropY
                PictureBox1.CreateGraphics.DrawRectangle(cropPen, cropX, cropY, cropWidth, cropHeight)
            End If
            ' GC.Collect()

        Catch exc As Exception

            If Err.Number = 5 Then Exit Sub
        End Try
    End Sub

    Private Sub PictureBox1_MouseDown(sender As Object, e As MouseEventArgs) Handles PictureBox1.MouseDown
        Try

            If e.Button = Windows.Forms.MouseButtons.Left Then

                cropX = e.X
                cropY = e.Y

                cropPen = New Pen(cropPenColor, cropPenSize)
                cropPen.DashStyle = DashStyle.DashDotDot
                Cursor = Cursors.Cross

            End If
            PictureBox1.Refresh()
        Catch exc As Exception
        End Try
    End Sub

    Private Sub PictureBox1_MouseUp(sender As Object, e As MouseEventArgs) Handles PictureBox1.MouseUp
        Try
            Cursor = Cursors.Default
            Try

                If cropWidth < 1 Then
                    Exit Sub
                End If

                Dim rect As Rectangle = New Rectangle(cropX, cropY, cropWidth, cropHeight)
                Dim bit As Bitmap = New Bitmap(PictureBox1.Image, PictureBox1.Width, PictureBox1.Height)

                cropBitmap = New Bitmap(cropWidth, cropHeight)
                Dim g As Graphics = Graphics.FromImage(cropBitmap)
                g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
                g.PixelOffsetMode = Drawing2D.PixelOffsetMode.HighQuality
                g.CompositingQuality = Drawing2D.CompositingQuality.HighQuality
                g.DrawImage(bit, 0, 0, rect, GraphicsUnit.Pixel)

                PreviewPictureBox.Image = cropBitmap



                Dim fd As New SaveFileDialog
                fd.ShowDialog()

                Using mstream As New MemoryStream

                    Using fs = New FileStream(fd.FileName, FileMode.Create, FileAccess.ReadWrite)
                        cropBitmap.Save(mstream, System.Drawing.Imaging.ImageFormat.Jpeg)
                        Dim bytes As Byte() = mstream.ToArray()
                        fs.Write(bytes, 0, bytes.Length)
                    End Using

                End Using




            Catch exc As Exception
            End Try
        Catch exc As Exception
        End Try
    End Sub


    Private Sub PictureBox1_DoubleClick(sender As Object, e As EventArgs) Handles PictureBox1.DoubleClick
        PreviewPictureBox.Image = Nothing
        cropBitmap = Nothing
        PictureBox1.Refresh()

    End Sub
