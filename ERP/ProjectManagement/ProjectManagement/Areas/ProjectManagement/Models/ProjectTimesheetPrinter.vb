Imports System.Drawing

Public Class ProjectTimesheetPrinter
    Private prm As ProjectManagement_ERPEntities
    Private ProjectId As Integer

    Dim g As Graphics

    Const ROW_HEIGHT = 70.0F
    Const CELLWEEK_COLUMNWIDTH = 110.0F
    Const COLDESCRIPTION_WIDTH = 160.0F
    Const MARGIN_LEFTRIGHT = 100.0F
    Const LINE_SPACING = 30.0F
    Const CELL_MARGIN = 50
    'pen and colors
    Dim blackPen2 = New Pen(Brushes.Black, 2.0F)
    Dim blackPen4 = New Pen(Brushes.Black, 4.0F)
    Dim BrushhtmlBDBDBD As Brush = New SolidBrush(ColorTranslator.FromHtml("#BDBDBD"))

    Dim Arial14 As Font
    Dim Arial9 As Font
    Dim Arial8 As Font
    Dim Arial6 As Font

    Dim CenterMiddleText As New StringFormat
    Dim CenterBottomText As New StringFormat
    Dim RightMiddleText As New StringFormat
    Dim LeftMiddleText As New StringFormat
    Dim romanNumerals() As String = {"0", "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX", "X", "X1"}

    Dim ColHeadWidth() As Single
    Dim numberOfWeekColumns As Integer
    Dim NumberOfTableBodyRow As Integer

    Dim _TableHeaderHeight As Single
    Dim _TableBodyHeight As Single
    Dim _TableFooterHeight As Single
    Dim _tableWidth As Single
    Dim _tableHeight As Single

    'the data
    Dim ProjectInfoModel As ProjectInfo
    Dim FooterWeightData As ProjectTimeSheetViewer.ProjectTimeSheetFooterViewer
    Dim HeaderTimeSheetData As ProjectTimeSheetViewer
    Dim WeightTable As DataTable
    Dim HeaderText() As String
    Public Sub New(ByVal Context As ProjectManagement_ERPEntities, ByVal projectId As Integer)
        prm = Context
        Me.ProjectId = projectId

        Arial14 = New Font("Arial", 14.0F)
        Arial9 = New Font("Arial", 9.0F)
        Arial8 = New Font("Arial", 8.0F, FontStyle.Regular, GraphicsUnit.Point)
        Arial6 = New Font("Arial", 6.0F, FontStyle.Regular, GraphicsUnit.Point)

        CenterMiddleText.Alignment = StringAlignment.Center
        CenterMiddleText.LineAlignment = StringAlignment.Center

        CenterBottomText.Alignment = StringAlignment.Center
        CenterBottomText.LineAlignment = StringAlignment.Far

        RightMiddleText.LineAlignment = StringAlignment.Center
        RightMiddleText.Alignment = StringAlignment.Far

        LeftMiddleText.LineAlignment = StringAlignment.Center
        LeftMiddleText.Alignment = StringAlignment.Near


    End Sub

    Private Sub InitData()
        ProjectInfoModel = prm.ProjectInfoes.Where(Function(m) m.Id = Me.ProjectId).SingleOrDefault()
        If ProjectInfoModel Is Nothing Then
            Throw New HttpException(404, "NOT FOUND")
        End If
        NumberOfTableBodyRow = ProjectInfoModel.ProjectTaskDivisions.Count
        For Each item In ProjectInfoModel.ProjectTaskDivisions
            NumberOfTableBodyRow += item.ProjectTaskDivisionItems.Count
        Next
        WeightTable = ProjectTimeSheetViewer.GetTimesheetTable(ProjectId)

        FooterWeightData = ProjectTimeSheetViewer.CreateTimesheetFooter(ProjectInfoModel, prm)
        HeaderTimeSheetData = ProjectTimeSheetViewer.CreateProjectTimesheetHeader(ProjectInfoModel)
        numberOfWeekColumns = FooterWeightData.WeeklyWeight.Count

    End Sub
    Public Function PrintTimeSheet() As Image

        InitData()
        _TableHeaderHeight = ROW_HEIGHT * 4
        _TableBodyHeight = ROW_HEIGHT * NumberOfTableBodyRow
        _TableFooterHeight = ROW_HEIGHT * 4

        '|MATA PEMBAYARAN | URAIAN |SATUAN|PERKIRAAN KUANTITAS |BOBOT|
        ColHeadWidth = {240.0F, 1100.0F, 140.0F, 300.0F, 200.0F}
        HeaderText = {"No. Mata Pembayaran", "U r a i a n", "Sat", "Perkiraan Kuantitas", "Bobot" & Environment.NewLine & "%"}

        Dim lineY = 100.0F 'start at 100

        _tableWidth = ColHeadWidth.Sum() + numberOfWeekColumns * CELLWEEK_COLUMNWIDTH
        _tableHeight = _TableHeaderHeight + _TableBodyHeight + _TableFooterHeight + ROW_HEIGHT

        ' this is what exactly TABLE height should be measure: 
        ' _TableHeaderHeight +  (ROW_HEIGHT * 0.5) + _TableBodyHeight +  (ROW_HEIGHT * 0.5) +  _TableFooterHeight 
        'there is half of row_height between header-body and body-footer

        Dim ImageHeight As Integer = Convert.ToInt32(_tableHeight) + 450 + (ROW_HEIGHT * 8)
        Dim ImageWidth As Integer = Convert.ToInt32(_tableWidth + MARGIN_LEFTRIGHT + COLDESCRIPTION_WIDTH + MARGIN_LEFTRIGHT)
        Dim Img As New Bitmap(ImageWidth, ImageHeight)
        Img.SetResolution(300, 300)

        g = Graphics.FromImage(Img)
        g.FillRectangle(Brushes.White, 0, 0, Img.Width, Img.Height)

        Dim sectionHeader = DrawTimesheetHeader(lineY, Img.Width)


        Dim tableYPos As Single = sectionHeader.Y + sectionHeader.Height + ROW_HEIGHT
        'draw  table border
        g.DrawRectangle(blackPen4, New Rectangle(MARGIN_LEFTRIGHT, tableYPos, _tableWidth + COLDESCRIPTION_WIDTH, _tableHeight))



        Dim Section1 = DrawTimesheetTableHeaderDefault(tableYPos)
        Dim Section2 = DrawTimesheetTableHeaderWeight(Section1.X + Section1.Width, tableYPos)

        Dim xPos As Single = Section1.X + Section1.Width
        Dim yPos As Single = Section1.Y + Section1.Height + ROW_HEIGHT * 0.5F

        Dim section3 = DrawTimesheetTableBodyDefault(yPos)
        Dim section4 = DrawTableBodyWeight(xPos, yPos)


        Dim section5 = DrawTimesheetTableFooterDefault(section3.Y + section3.Height + ROW_HEIGHT * 0.5F)
        Dim section6 = DrawTabelFooterWeight(section3.X + section3.Width, section3.Y + section3.Height + ROW_HEIGHT * 0.5F)



        DrawTableColumnDefaultLines(MARGIN_LEFTRIGHT, tableYPos, Section1.Width, _tableHeight)
        DrawTableColumnWeekLines(Section2.X, Section2.Y, Section2.Width, _tableHeight)
        DrawTableRowLines(MARGIN_LEFTRIGHT, Section1.Y, Section1.Width, Section2.Width, Section1.Height, _tableHeight)

        Dim section7 = DrawChart(section4.X, section4.Y, section4.Width, section4.Height)
        DrawTimeSheetFooter(section5.Y + section5.Height, ImageWidth)
      
        g.Flush()
        g.Dispose()

        Dim rValue = Resize(Img, 80, 0) ''resize the image by 50%

        Return rValue
    End Function


    Private Sub InitMC0Data()
        ProjectInfoModel = prm.ProjectInfoes.Where(Function(m) m.Id = Me.ProjectId).SingleOrDefault()
        If ProjectInfoModel Is Nothing Then
            Throw New HttpException(404, "NOT FOUND")
        End If
        NumberOfTableBodyRow = ProjectInfoModel.ProjectTaskDivisions.Count
        For Each item In ProjectInfoModel.ProjectTaskDivisions
            NumberOfTableBodyRow += item.ProjectTaskDivisionItems.Count
        Next
        WeightTable = ProjectTimeSheetViewer.GetTimesheetTableMC0(ProjectId)

        FooterWeightData = ProjectTimeSheetViewer.CreateTimesheetFooterMC0(ProjectInfoModel, prm)
        HeaderTimeSheetData = ProjectTimeSheetViewer.CreateProjectTimesheetHeader(ProjectInfoModel)
        numberOfWeekColumns = FooterWeightData.WeeklyWeight.Count
    End Sub

    Public Function PrintTimeSheetRevision(ByVal RevisionNumber As Integer) As Image

        InitMC0Data()
        _TableHeaderHeight = ROW_HEIGHT * 4
        _TableBodyHeight = ROW_HEIGHT * NumberOfTableBodyRow
        _TableFooterHeight = ROW_HEIGHT * 4

        '|MATA PEMBAYARAN | URAIAN |SATUAN|PERKIRAAN KUANTITAS |BOBOT|
        ColHeadWidth = {240.0F, 1100.0F, 140.0F, 300.0F, 200.0F, 300.0F, 200.0F}
        Dim MCORevisionNumber As String = "Volume MC 0"
        HeaderText = {"No. Mata Pembayaran", "U r a i a n", "Sat", "Volume", "Bobot" & Environment.NewLine & "%",
                      MCORevisionNumber, "Bobot" & Environment.NewLine & "%"}

        Dim lineY = 100.0F 'start at 100

        _tableWidth = ColHeadWidth.Sum() + numberOfWeekColumns * CELLWEEK_COLUMNWIDTH
        _tableHeight = _TableHeaderHeight + _TableBodyHeight + _TableFooterHeight + ROW_HEIGHT

        ' this is what exactly TABLE height should be measure: 
        ' _TableHeaderHeight +  (ROW_HEIGHT * 0.5) + _TableBodyHeight +  (ROW_HEIGHT * 0.5) +  _TableFooterHeight 
        'there is half of row_height between header-body and body-footer

        Dim ImageHeight As Integer = Convert.ToInt32(_tableHeight) + 450 + (ROW_HEIGHT * 8)
        Dim ImageWidth As Integer = Convert.ToInt32(_tableWidth + MARGIN_LEFTRIGHT + COLDESCRIPTION_WIDTH + MARGIN_LEFTRIGHT)
        Dim Img As New Bitmap(ImageWidth, ImageHeight)
        Img.SetResolution(300, 300)

        g = Graphics.FromImage(Img)
        g.FillRectangle(Brushes.White, 0, 0, Img.Width, Img.Height)

        Dim sectionHeader = DrawTimesheetHeader(lineY, Img.Width)


        Dim tableYPos As Single = sectionHeader.Y + sectionHeader.Height + ROW_HEIGHT
        'draw  table border
        g.DrawRectangle(blackPen4, New Rectangle(MARGIN_LEFTRIGHT, tableYPos, _tableWidth + COLDESCRIPTION_WIDTH, _tableHeight))



        Dim Section1 = DrawTimesheetTableHeaderDefault(tableYPos)
        Dim Section2 = DrawTimesheetTableHeaderWeight(Section1.X + Section1.Width, tableYPos)

        Dim xPos As Single = Section1.X + Section1.Width
        Dim yPos As Single = Section1.Y + Section1.Height + ROW_HEIGHT * 0.5F

        Dim section3 = DrawTimesheetTableBodyDefault(yPos, True)
        Dim section4 = DrawTableBodyWeight(xPos, yPos)


        Dim section5 = DrawTimesheetTableFooterDefault(section3.Y + section3.Height + ROW_HEIGHT * 0.5F)
        Dim section6 = DrawTabelFooterWeight(section3.X + section3.Width, section3.Y + section3.Height + ROW_HEIGHT * 0.5F)



        DrawTableColumnDefaultLines(MARGIN_LEFTRIGHT, tableYPos, Section1.Width, _tableHeight)
        DrawTableColumnWeekLines(Section2.X, Section2.Y, Section2.Width, _tableHeight)
        DrawTableRowLines(MARGIN_LEFTRIGHT, Section1.Y, Section1.Width, Section2.Width, Section1.Height, _tableHeight)

        Dim section7 = DrawChart(section4.X, section4.Y, section4.Width, section4.Height)
        DrawTimeSheetFooter(section5.Y + section5.Height, ImageWidth)

        g.Flush()
        g.Dispose()

        Dim rValue = Resize(Img, 80, 0) ''resize the image by 50%

        Return rValue
    End Function

    Private Sub drawWeightCell(left As Single, top As Single, weight As Decimal, cellbackgroundColor As System.Drawing.Color,
                               cellTextColor As System.Drawing.Color)
        If weight = 0D Then Return
        Dim rect = New RectangleF(left + 2, top + 1, CELLWEEK_COLUMNWIDTH - 3, ROW_HEIGHT - 2)
        g.FillRectangle(New Drawing.SolidBrush(cellbackgroundColor), rect)

        g.DrawString(weight.ToString("#0.#0"), Arial6, New Drawing.SolidBrush(cellTextColor), rect, CenterMiddleText)
    End Sub

    Private Sub DrawTableColumnDefaultLines(ByVal xPos As Single, ByVal yPos As Single, width As Single, height As Single)


        For Each item In ColHeadWidth
            xPos += item
            g.DrawLine(blackPen2, xPos, yPos, xPos, yPos + height)

        Next
        g.DrawLine(blackPen2, xPos - 4, yPos, xPos - 4, yPos + height)

    End Sub
    Private Sub DrawTableColumnWeekLines(ByVal xPos As Single, yPos As Single, ByVal width As Single, height As Single)

        For Each item In HeaderTimeSheetData.MonthList

            For counter = 2 To item.Count
                xPos += CELLWEEK_COLUMNWIDTH
                g.DrawLine(blackPen2, xPos, yPos + ROW_HEIGHT * 2, xPos, yPos + height - ROW_HEIGHT * 2)
            Next
            xPos += CELLWEEK_COLUMNWIDTH
            If item.Equals(HeaderTimeSheetData.MonthList.Last) Then
                g.DrawLine(blackPen4, xPos, yPos, xPos, yPos + height)
            Else

                g.DrawLine(blackPen4, xPos, yPos + ROW_HEIGHT, xPos, yPos + height)
            End If


        Next
    End Sub

    

    Private Sub DrawTableRowLines(ByVal xPos As Single, ByVal yPos As Single,
                                  ByVal colDefaultWidth As Single,
                                  ByVal colWeekWidth As Single,
                                  ByVal colDefaultHeight As Single, height As Single)

        Dim x2 As Single = xPos + colDefaultWidth + colWeekWidth

        'header
        For counter = 0 To 2
            yPos += ROW_HEIGHT
            g.DrawLine(blackPen2, xPos + colDefaultWidth, yPos, x2, yPos)
        Next
        yPos += ROW_HEIGHT
        g.FillRectangle(Brushes.White, xPos, yPos, colDefaultWidth + COLDESCRIPTION_WIDTH + colWeekWidth, 4)
        g.DrawLine(blackPen2, xPos, yPos, x2 + COLDESCRIPTION_WIDTH, yPos)
        g.DrawLine(blackPen2, xPos, yPos + 4, x2 + COLDESCRIPTION_WIDTH, yPos + 4)

        yPos -= ROW_HEIGHT / 2
        ''body
        For counter = 0 To NumberOfTableBodyRow
            yPos += ROW_HEIGHT
            g.DrawLine(blackPen2, xPos, yPos, x2, yPos)
        Next
        'footer
        yPos += ROW_HEIGHT / 2
        g.FillRectangle(Brushes.White, xPos, yPos, colDefaultWidth + COLDESCRIPTION_WIDTH + colWeekWidth, 4)
        g.DrawLine(blackPen2, xPos, yPos, x2 + COLDESCRIPTION_WIDTH, yPos)
        g.DrawLine(blackPen2, xPos, yPos + 4, x2 + COLDESCRIPTION_WIDTH, yPos + 4)



        For counter = 0 To 2
            yPos += ROW_HEIGHT
            g.DrawLine(blackPen2, xPos, yPos, x2, yPos)
        Next
    End Sub



    Private Sub DrawChartPole(ByVal x As Single, y As Single, w As Single, ByVal h As Single)
        g.DrawRectangle(blackPen2, New Rectangle(x + 10, y, 50, h))
        Dim rectDivision = h / 10
        Dim recPole = New Rectangle(x + 10, y, 50, rectDivision)
        For counter = 0 To 9
            g.DrawString((100 - counter * 10).ToString(), Arial8, Brushes.Black, New RectangleF(recPole.X + 55, recPole.Y - 20, 80, ROW_HEIGHT), RightMiddleText)
            If counter Mod 2 = 0 Then
                g.FillRectangle(Brushes.Black, recPole)
            End If
            recPole.Y += recPole.Height
        Next


    End Sub

   

    Public Function Resize(ByVal imgSource As Image, ByVal intPercent As Integer, ByVal intType As Integer) As Image
        'resize the image by percent
        Dim intX, intY As Integer
        intX = Int(imgSource.Width / 100 * intPercent)
        intY = Int(imgSource.Height / 100 * intPercent)
        Dim bm As Drawing.Bitmap = New System.Drawing.Bitmap(intX, intY)
        Dim painter As System.Drawing.Graphics = Drawing.Graphics.FromImage(bm)

        Select Case intType
            Case 0
                painter.InterpolationMode = Drawing.Drawing2D.InterpolationMode.Default
            Case 1
                painter.InterpolationMode = Drawing.Drawing2D.InterpolationMode.High
            Case 2
                painter.InterpolationMode = Drawing.Drawing2D.InterpolationMode.HighQualityBilinear
            Case 3
                painter.InterpolationMode = Drawing.Drawing2D.InterpolationMode.HighQualityBicubic
        End Select

        painter.DrawImage(imgSource, 0, 0, intX, intY)
        painter.Flush()
        painter.Dispose()
        Return bm

    End Function

    Private Function DrawTimesheetHeader(ByVal yPos As Single, ByVal maxWidth As Single) As RectangleF
        Dim _lineSpace As Single
        Dim titleString = "TIME SCHEDULE RENCANA"
        Dim KontraktorLabel = "KONTRAKTOR"
        Dim Kegiatanlabel = "Kegiatan"
        Dim NamaPaketLabel = "Nama Paket"
        Dim RegionLabel = "Prop / Kab / Kodya"
        Dim LineY As Single = yPos
        g.DrawString(titleString, Arial14, Brushes.Black, New RectangleF(MARGIN_LEFTRIGHT, LineY, maxWidth, 50.0F), CenterMiddleText)

        _lineSpace = Arial9.Height + LINE_SPACING
        LineY += _lineSpace + 100
        g.DrawString(KontraktorLabel, Arial9, Brushes.Black, MARGIN_LEFTRIGHT, LineY)
        g.DrawString(":  " & ProjectInfoModel.CompanyInfo.Name, Arial9, Brushes.Black, MARGIN_LEFTRIGHT + 400, LineY)
        LineY += _lineSpace
        g.DrawString(Kegiatanlabel, Arial9, Brushes.Black, MARGIN_LEFTRIGHT, LineY)
        g.DrawString(":  " & ProjectInfoModel.ProjectCode, Arial9, Brushes.Black, MARGIN_LEFTRIGHT + 400, LineY)
        LineY += _lineSpace
        g.DrawString(NamaPaketLabel, Arial9, Brushes.Black, MARGIN_LEFTRIGHT, LineY)
        g.DrawString(":  " & ProjectInfoModel.ProjectTitle, Arial9, Brushes.Black, MARGIN_LEFTRIGHT + 400, LineY)
        LineY += _lineSpace
        g.DrawString(RegionLabel, Arial9, Brushes.Black, MARGIN_LEFTRIGHT, LineY)
        g.DrawString(":  " & ProjectInfoModel.Location, Arial9, Brushes.Black, MARGIN_LEFTRIGHT + 400, LineY)
        LineY += _lineSpace

        Return New RectangleF(MARGIN_LEFTRIGHT, yPos, maxWidth, LineY - yPos)
    End Function
    Private Function DrawTimeSheetFooter(ByVal yPos As Single, ByVal maxWidth As Single) As RectangleF
        Dim nWeek = FooterWeightData.WeeklyWeight.Count
        Dim numberOfRestantDays = DateDiff(DateInterval.Day, FooterWeightData.WeeklyWeight.Last.Datestart, FooterWeightData.WeeklyWeight.Last.DateEnd)
        Dim restantdaysString = ""
        If numberOfRestantDays > 0 Then
            nWeek -= 1
            restantdaysString = " + " & numberOfRestantDays.ToString() & " Hari"
        End If
        Dim strNb = String.Format("NB: JANGKA WAKTU PELAKSANAAN {0} Hari = {1} Minggu {2}", ProjectInfoModel.NumberOfDays, nWeek, restantdaysString)
        g.DrawString(strNb, Arial9, Brushes.Black, MARGIN_LEFTRIGHT + ColHeadWidth(0) + 100, yPos)

        Dim rect As New RectangleF(maxWidth - 1500, yPos + ROW_HEIGHT, 700, 50)

        g.DrawString(ProjectInfoModel.CompanyInfo.Name, Arial9, Brushes.Black, rect, CenterMiddleText)
        rect.Y += ROW_HEIGHT * 4

        Dim f As New Font("Arial", 9, FontStyle.Underline Or FontStyle.Bold)

        g.DrawString("DIREKTUR UTAMA", Arial9, Brushes.Black, rect, CenterMiddleText)
    End Function

    Private Function DrawTimesheetTableHeaderDefault(ByVal TableAnchorY As Single) As RectangleF
        Dim xPos = MARGIN_LEFTRIGHT
        Dim yPos = TableAnchorY
        Dim ctr As Integer = 0
        Dim _rowHeight As Single = ROW_HEIGHT * 4
        For Each colWidth In ColHeadWidth
            g.DrawString(HeaderText(ctr), Arial8, Brushes.Black, New RectangleF(xPos, yPos, colWidth, _rowHeight), CenterBottomText)
            xPos += colWidth
            ctr += 1
        Next
        Return New RectangleF(MARGIN_LEFTRIGHT, TableAnchorY, ColHeadWidth.Sum(), _rowHeight)
    End Function

    Private Function DrawTimesheetTableHeaderWeight(ByVal xPos As Single, ByVal yPos As Single) As RectangleF
        Dim _lineY As Single
        Dim _width As Single = HeaderTimeSheetData.WeekItems.Count * CELLWEEK_COLUMNWIDTH

        g.FillRectangle(BrushhtmlBDBDBD, xPos, yPos + ROW_HEIGHT * 2, _width - 1, ROW_HEIGHT * 2)
        _lineY = yPos
        Dim r = New RectangleF(xPos, _lineY, _width, ROW_HEIGHT)
        g.DrawString("JADWAL PELAKSANAAN", Arial8, Brushes.Black, r, CenterBottomText)
        g.DrawString("Ket.", Arial8, Brushes.Black,
                     New RectangleF(MARGIN_LEFTRIGHT + ColHeadWidth.Sum() + CELLWEEK_COLUMNWIDTH * numberOfWeekColumns, _lineY, COLDESCRIPTION_WIDTH, ROW_HEIGHT * 5),
                     CenterMiddleText)

        Dim colWidthAnchor = xPos



        For Each item In HeaderTimeSheetData.WeekItems
            g.DrawString(item.WeekNumber.ToString("\W#"), Arial6, Brushes.Black,
                         New RectangleF(colWidthAnchor, yPos + ROW_HEIGHT * 2, CELLWEEK_COLUMNWIDTH, ROW_HEIGHT), CenterMiddleText)
            g.DrawString(item.DayStartEnd, Arial6, Brushes.Black,
                         New RectangleF(colWidthAnchor, yPos + ROW_HEIGHT * 3, CELLWEEK_COLUMNWIDTH, ROW_HEIGHT), CenterMiddleText)
            colWidthAnchor += CELLWEEK_COLUMNWIDTH
        Next

        colWidthAnchor = xPos
        For Each item In HeaderTimeSheetData.MonthList
            Dim textDimension = g.MeasureString(item.MonthName, Arial8)
            Dim textColWidth = item.Count * CELLWEEK_COLUMNWIDTH
            If textColWidth < textDimension.Width Then
                g.DrawString(item.MonthName.Substring(0, 3), Arial8, Brushes.Black,
                         New RectangleF(colWidthAnchor, yPos + ROW_HEIGHT, textColWidth, ROW_HEIGHT), CenterMiddleText)
            Else
                g.DrawString(item.MonthName, Arial8, Brushes.Black,
                         New RectangleF(colWidthAnchor, yPos + ROW_HEIGHT, textColWidth, ROW_HEIGHT), CenterMiddleText)
            End If
            colWidthAnchor += item.Count * CELLWEEK_COLUMNWIDTH
        Next
        Return New RectangleF(xPos, yPos, _width, ROW_HEIGHT * 5)

    End Function

    Private Function DrawTimesheetTableBodyDefault(ByVal yPos As Single, Optional ByVal boolMC0 As Boolean = False) As RectangleF
        Dim divisionRowCell As New RectangleF
        Dim lineY = yPos

        Dim colWidthAnchor = CELL_MARGIN

        For Each item In ProjectInfoModel.ProjectTaskDivisions.OrderBy(Function(m) m.Ordinal)

            divisionRowCell.X = MARGIN_LEFTRIGHT
            divisionRowCell.Y = lineY
            divisionRowCell.Width = ColHeadWidth.Sum()
            divisionRowCell.Height = ROW_HEIGHT

            g.FillRectangle(BrushhtmlBDBDBD, divisionRowCell)

            divisionRowCell.Width = ColHeadWidth(0)

            g.DrawString(romanNumerals(item.DivisionNumber), Arial8, Brushes.Black, divisionRowCell, CenterMiddleText)

            divisionRowCell.X += divisionRowCell.Width + colWidthAnchor
            divisionRowCell.Width = ColHeadWidth(1)
            g.DrawString("DIVISI " & item.DivisionNumber & ". " & item.TaskTitle, Arial8, Brushes.Black, divisionRowCell, LeftMiddleText)

            Dim taskRowCell As New Rectangle

            For Each task In item.ProjectTaskDivisionItems.OrderBy(Function(m) m.Ordinal)
                lineY += ROW_HEIGHT
                taskRowCell.Width = ColHeadWidth(0)
                taskRowCell.X = MARGIN_LEFTRIGHT
                taskRowCell.Y = lineY
                taskRowCell.Height = ROW_HEIGHT
                g.DrawString(task.PaymentNumber, Arial8, Brushes.Black, taskRowCell, CenterMiddleText)

                taskRowCell.X += taskRowCell.Width + CELL_MARGIN
                taskRowCell.Width = ColHeadWidth(1)
                g.DrawString(task.TaskTitle, Arial8, Brushes.Black, taskRowCell, LeftMiddleText)

                taskRowCell.X += taskRowCell.Width - CELL_MARGIN
                taskRowCell.Width = ColHeadWidth(2)
                g.DrawString(task.UnitQuantity, Arial8, Brushes.Black, taskRowCell, CenterMiddleText)

                taskRowCell.X += taskRowCell.Width
                taskRowCell.Width = ColHeadWidth(3) - 20
                g.DrawString(task.Quantity.ToString("#,###.#0"), Arial8, Brushes.Black, taskRowCell, RightMiddleText)


                taskRowCell.X += taskRowCell.Width
                taskRowCell.Width = ColHeadWidth(4) - 20
                Dim row = WeightTable.Rows.Find(task.Id)
                If row IsNot Nothing Then
                    g.DrawString(Convert.ToDecimal(row("taskweight")).ToString("#0.#0"), Arial8, Brushes.Black, taskRowCell, RightMiddleText)

                    If boolMC0 Then
                        taskRowCell.X += taskRowCell.Width
                        taskRowCell.Width = ColHeadWidth(5) - 20
                        g.DrawString(Convert.ToDecimal(row("quantityMC0")).ToString("#0.#0"), Arial8, Brushes.Black, taskRowCell, RightMiddleText)

                        taskRowCell.X += taskRowCell.Width
                        taskRowCell.Width = ColHeadWidth(6) - 20
                        g.DrawString(Convert.ToDecimal(row("taskWeightMC0")).ToString("#0.#0"), Arial8, Brushes.Black, taskRowCell, RightMiddleText)

                    End If


                End If

            Next
            lineY += ROW_HEIGHT
        Next
        Return New RectangleF(MARGIN_LEFTRIGHT, yPos, ColHeadWidth.Sum(), lineY - yPos)
    End Function


    Private Function DrawTableBodyWeight(ByVal xPos As Single, ByVal yPos As Single) As RectangleF

        Dim _cellMargin As Single = 50
        Dim taskRowCell As New Rectangle
        Dim lineY = yPos

        For Each item In ProjectInfoModel.ProjectTaskDivisions.OrderBy(Function(m) m.Ordinal)

            taskRowCell.X = xPos
            taskRowCell.Y = lineY
            taskRowCell.Width = numberOfWeekColumns * CELLWEEK_COLUMNWIDTH
            taskRowCell.Height = ROW_HEIGHT

            g.FillRectangle(BrushhtmlBDBDBD, taskRowCell)

            lineY += ROW_HEIGHT
            For Each task In item.ProjectTaskDivisionItems.OrderBy(Function(m) m.Ordinal)


                Dim row = WeightTable.Rows.Find(task.Id)

                If Not row Is Nothing Then
                    Dim offsetX = xPos


                    Dim cellcolor As Color
                    Dim textcolor As Color
                    Try
                        cellcolor = ColorTranslator.FromHtml("#" & row("CellBackgroundColor"))

                    Catch ex As Exception
                        cellcolor = Color.White
                    End Try
                    Try
                        textcolor = ColorTranslator.FromHtml("#" & row("CellTextColor"))
                    Catch ex As Exception
                        textcolor = System.Drawing.Color.Black
                    End Try

                    For c As Integer = 1 To numberOfWeekColumns

                        drawWeightCell(offsetX, lineY, Convert.ToDecimal(row(c.ToString("00000#"))), cellcolor, textcolor)
                        offsetX += CELLWEEK_COLUMNWIDTH
                    Next
                End If
                lineY += ROW_HEIGHT
            Next

        Next
        Return New RectangleF(xPos, yPos, numberOfWeekColumns * CELLWEEK_COLUMNWIDTH, lineY - yPos)
    End Function

    Private Function DrawTimesheetTableFooterDefault(ByVal yPos As Single) As RectangleF
        Dim _lineY = yPos
        Dim xPos As Single = MARGIN_LEFTRIGHT
        g.FillRectangle(BrushhtmlBDBDBD, xPos, _lineY, ColHeadWidth.Sum(), ROW_HEIGHT * 4)


        Dim footerText() = {"KEMAJUAN PEKERJAAN MINGGUAN",
                           "KEMAJUAN PEKERJAAN KUMULATIF MINGGUAN",
                           "KEMAJUAN PEKERJAAN BULANAN",
                           "KEMAJUAN PEKERJAAN KUMULATIF BULANAN"}

        Dim cellFooter As New RectangleF
        cellFooter.Y = _lineY
        cellFooter.Height = ROW_HEIGHT

        For Each textItem In footerText
            cellFooter.Width = ColHeadWidth(1) - CELL_MARGIN
            cellFooter.X = xPos + ColHeadWidth(0) + CELL_MARGIN
            g.DrawString(textItem, Arial8, Brushes.Black, cellFooter, LeftMiddleText)

            cellFooter.X += cellFooter.Width
            cellFooter.Width = ColHeadWidth(2)
            g.DrawString("%", Arial8, Brushes.Black, cellFooter, CenterMiddleText)

            If textItem.Equals(footerText.First) Then
                cellFooter.X += cellFooter.Width + ColHeadWidth(3)
                cellFooter.Width = ColHeadWidth(4) - CELL_MARGIN
                g.DrawString("100,00", Arial8, Brushes.Black, cellFooter, RightMiddleText)
            End If
            
            cellFooter.Y += ROW_HEIGHT
        Next

        Return New RectangleF(xPos, yPos, ColHeadWidth.Sum(), cellFooter.Y - yPos)
    End Function

    Private Function DrawTabelFooterWeight(ByVal xPos As Single, ByVal yPos As Single) As RectangleF
        Dim lineY As Single = yPos
        Dim cellfooter As New RectangleF

        g.FillRectangle(BrushhtmlBDBDBD, xPos, lineY, numberOfWeekColumns * CELLWEEK_COLUMNWIDTH, ROW_HEIGHT * 4)

        cellfooter.X = xPos
        cellfooter.Y = lineY
        cellfooter.Height = ROW_HEIGHT
        cellfooter.Width = CELLWEEK_COLUMNWIDTH
        For Each item In FooterWeightData.WeeklyWeight
            cellfooter.Y = lineY
            g.DrawString(item.Weight.ToString("#0.#0"), Arial8, Brushes.Black, cellfooter, RightMiddleText)
            cellfooter.Y = lineY + ROW_HEIGHT
            g.DrawString(item.WeightAccumulation.ToString("#0.#0"), Arial8, Brushes.Black, cellfooter, RightMiddleText)
            cellfooter.X += cellfooter.Width
        Next

        lineY += ROW_HEIGHT * 2
        cellfooter.X = xPos
        Dim monthItemcounter As Integer = 0
        For Each item In FooterWeightData.MonthlyWeight
            Dim cwidth = HeaderTimeSheetData.MonthList(monthItemcounter).Count * CELLWEEK_COLUMNWIDTH
            cellfooter.Width = cwidth
            cellfooter.Y = lineY
            g.DrawString(item.Weight.ToString("#0.#0"), Arial8, Brushes.Black, cellfooter, CenterMiddleText)
            cellfooter.Y = lineY + ROW_HEIGHT
            g.DrawString(item.WeightAccumulation.ToString("#0.#0"), Arial8, Brushes.Black, cellfooter, CenterMiddleText)
            cellfooter.X += cellfooter.Width
            monthItemcounter += 1
        Next

        Return New RectangleF(xPos, yPos, numberOfWeekColumns * CELLWEEK_COLUMNWIDTH, lineY - yPos)
    End Function

    Private Function DrawChart(ByVal xpos As Single, ByVal yPos As Single, ByVal width As Single, ByVal height As Single) As RectangleF


        DrawChartPole(xpos + width, yPos, 150, height)

        Dim model = (From n In FooterWeightData.WeeklyWeight
                Order By n.Weeknumber
                Select New PointF() With {.X = (n.Weeknumber) * CELLWEEK_COLUMNWIDTH + xpos,
                                          .Y = yPos + height - (n.WeightAccumulation / 100 * height)}).ToList()
        model.Insert(0, New PointF(xPos, yPos + height))
        g.DrawLines(New Pen(Brushes.Red, 8), model.ToArray())

    End Function


End Class
