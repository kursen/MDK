Imports System.Runtime.CompilerServices

Namespace HtmlHelpers

#Region "HelpersToolsDeclare"

    Public Module Helpers

        '
        ' Html.AccordionBox
        '------------------------
        <Extension()> _
        Public Function AccordionBox(helper As HtmlHelper, ByVal Id As String, ByVal title As String,
                                        Optional ByVal setMinimize As Boolean = False, Optional ByVal icon As String = "icon-list") As AccordionBox
            Return New AccordionBox(helper, Id, title, setMinimize, icon)
        End Function
        '------------------------

        '
        ' Html.Modal
        '------------------------
        <Extension()> _
        Public Function Modal(helper As HtmlHelper, ByVal Id As String, ByVal title As String, Optional idTitle As String = "") As Modal
            Return New Modal(helper, Id, title, idTitle)
        End Function
        '------------------------

        '
        ' Html.ModalForm
        '------------------------
        <Extension()> _
        Public Function ModalForm(helper As HtmlHelper, ByVal Id As String, ByVal title As String, ByVal url As String _
                       , Optional ByVal o_idTitle As String = "", Optional o_method As String = "POST", Optional o_idForm As String = "", Optional o_classForm As String = "") As ModalForm
            Return New ModalForm(helper, Id, title, url, o_idTitle, o_method, o_idForm, o_classForm)
        End Function
        '------------------------

        '
        ' Html.WriteSummaryBox
        '------------------------
        <Extension()> _
        Public Function RowBox(helper As HtmlHelper, Optional ByVal ClassName As String = "", Optional ByVal Disable_Scrollable As Boolean = False) As RowBox
            Return New RowBox(helper, ClassName, disable_scrollable)
        End Function
        '------------------------

        '
        ' Html.WriteSummaryBox
        '------------------------
        <Extension()> _
        Public Function WriteSummaryBox(helper As HtmlHelper, ByVal title As String,
                                        Optional ByVal icon As String = "icon-list",
                                        Optional idGoTo As String = "#",
                                        Optional ByVal isScrollable As Boolean = True) As WriteSummaryBox
            Return New WriteSummaryBox(helper, title, icon, idGoTo)
        End Function
        '------------------------

        '
        ' Html.Image
        '------------------------
        <Extension()> _
        Public Function Image(ByVal helper As HtmlHelper, ByVal id As String, ByVal url As String, ByVal altText As String) As String
            Return Image(helper, id, url, altText, Nothing)
        End Function

        <Extension()> _
        Public Function Image(ByVal helper As HtmlHelper, ByVal id As String, ByVal url As String, ByVal altText As String, ByVal htmlAttributes As Object) As String
            ' Instantiate a UrlHelper
            Dim urlHelper As New UrlHelper(helper.ViewContext.RequestContext)
            Dim builder As New TagBuilder("img")
            'Create valid id
            builder.GenerateId(id)
            ' Add attributes
            builder.MergeAttribute("src", urlHelper.Content(url))
            builder.MergeAttribute("alt", altText)
            If (htmlAttributes <> Nothing) Then
                builder.MergeAttributes(New RouteValueDictionary(htmlAttributes))
            End If
            'Render tag
            Return builder.ToString(TagRenderMode.SelfClosing)
        End Function
        '------------------------


        <System.Runtime.CompilerServices.Extension()> _
        Public Function CreateFromEnumerable(Of TDerived As {List(Of T), New}, T)(seq As IEnumerable(Of T)) As TDerived
            Dim outList As New TDerived()
            outList.AddRange(seq)
            Return outList
        End Function


        '
        ' Html.DropDownListGroupFor
        '------------------------
        <Extension()> _
        Public Function DropDownGroupListFor(ByVal helper As HtmlHelper, ByVal id As String, ByVal selectList As Dictionary(Of String, SelectList), Optional ByVal htmlAttributes As Object = Nothing) As IHtmlString
            Dim _select As New TagBuilder("select")

            ' Generate Id and add attributes
            _select.GenerateId(id)
            _select.Attributes("name") = id
            If Not IsNothing(htmlAttributes) Then
                _select.MergeAttributes(New RouteValueDictionary(htmlAttributes))
            End If

            Dim optgroups = New StringBuilder()


            For Each item In selectList
                Dim _optgroup = New TagBuilder("optgroup")
                _optgroup.Attributes.Add("label", CStr(item.Key))

                Dim options = New StringBuilder()

                For Each subItem In item.Value
                    Dim _option = New TagBuilder("option")

                    _option.Attributes.Add("value", subItem.Value)
                    _option.SetInnerText(subItem.Text)

                    If subItem.Selected Then
                        _option.Attributes.Add("selected", "selected")
                    End If

                    options.Append(_option.ToString(TagRenderMode.Normal))
                Next

                _optgroup.InnerHtml = options.ToString()

                optgroups.Append(_optgroup.ToString(TagRenderMode.Normal))
            Next

            _select.InnerHtml = optgroups.ToString()

            'Render tag
            Return MvcHtmlString.Create(_select.ToString(TagRenderMode.Normal))
        End Function

        <Extension()> _
        Public Function BreadCrumb(ByVal helper As HtmlHelper, items As Array) As String
            ' Instantiate a UrlHelper
            Dim urlHelper As New UrlHelper(helper.ViewContext.RequestContext)
            Dim builder As New TagBuilder("ul")
            builder.AddCssClass("breadcrumb")

            Dim listdata = New StringBuilder()
            For i As Byte = 0 To (items.Length / 2) - 1
                Dim _listdata = New TagBuilder("li")

                If Not IsNothing(items(i, 1)) Then
                    Dim _anchor = New TagBuilder("a")
                    _anchor.Attributes.Add("href", items(i, 1))
                    _anchor.SetInnerText(items(i, 0))

                    _listdata.InnerHtml = _anchor.ToString()
                Else
                    _listdata.Attributes.Add("class", "active")
                    _listdata.SetInnerText(items(i, 0))
                End If

                listdata.Append(_listdata.ToString(TagRenderMode.Normal))
            Next

            builder.InnerHtml = listdata.ToString()

            'Render tag
            Return builder.ToString(TagRenderMode.Normal)
        End Function

    End Module

#End Region

#Region "HtmlHelpersClass"

    Public Class AccordionBox
        Implements IDisposable
        Protected _helper As HtmlHelper

        Public Sub New(ByVal helper As HtmlHelper, ByVal Id As String, ByVal title As String,
                                         ByVal setMinimize As Boolean, ByVal icon As String)
            _helper = helper
            Dim Writer = helper.ViewContext.Writer
            Writer.Write("<div id='" & Id & "' class='accordion'>" & vbCrLf)
            Writer.Write("     <div class='accordion-group box sum-box'>" & vbCrLf)
            Writer.Write("         <div class='accordion-heading title box-title'>" & vbCrLf)
            Writer.Write("             <h2>" & vbCrLf)
            Writer.Write("                 <span>" & title & "</span>" & vbCrLf)
            Writer.Write("             </h2>" & vbCrLf)
            Writer.Write("             <a href='#" & Id & "body' data-parent='#" & Id & "' " & vbCrLf)
            Writer.Write("	data-toggle='collapse' class='accordion-toggle minimize pull-right'>" & vbCrLf)
            Writer.Write("             </a>" & vbCrLf)
            Writer.Write("         </div>" & vbCrLf)
            Writer.Write("         <div class='accordion-body collapse " & IIf(setMinimize, "", "in") & "' id='" & Id & "body'>" & vbCrLf)
            Writer.Write("             <div class='content noPad'>" & vbCrLf)
        End Sub

        Public Sub Dispose() Implements IDisposable.Dispose
            Dim Writer = _helper.ViewContext.Writer
            Writer.Write("   </div>" & vbCrLf)
            Writer.Write("   </div>" & vbCrLf)
            Writer.Write("  </div>" & vbCrLf)
            Writer.Write("</div>" & vbCrLf)
        End Sub
    End Class

    Public Class Modal
        Implements IDisposable
        Protected _helper As HtmlHelper

        Public Sub New(ByVal helper As HtmlHelper, ByVal Id As String, ByVal title As String, Optional ByVal idTitle As String = "")
            _helper = helper
            Dim Writer = helper.ViewContext.Writer
            Writer.Write("<div class='modal fade' id='" & Id & "' tabindex='-1'>" & vbCrLf)
            Writer.Write("    <div class='modal-dialog'>" & vbCrLf)
            Writer.Write("        <div class='modal-content'>" & vbCrLf)
            Writer.Write("            <div class='modal-header'>" & vbCrLf)
            Writer.Write("                <button aria-hidden='true' class='close' data-dismiss='modal' type='button' tabindex='-1'>×</button>" & vbCrLf)
            Writer.Write("                <h4 class='modal-title' id='" & idTitle & "'> " & title & " </h4>" & vbCrLf)
            Writer.Write("            </div>" & vbCrLf)
            Writer.Write("            <div class='modal-body'>" & vbCrLf)
        End Sub

        Public Sub Dispose() Implements IDisposable.Dispose
            Dim Writer = _helper.ViewContext.Writer
            Writer.Write("    <div class='modal-footer'>" & vbCrLf)
            Writer.Write("        <button class='btn btn-primary' data-dismiss='modal' type='button'> Tutup </button>" & vbCrLf)
            Writer.Write("    </div>" & vbCrLf)
            Writer.Write("    </div>" & vbCrLf)
            Writer.Write("  </div>" & vbCrLf)
            Writer.Write(" </div>" & vbCrLf)
            Writer.Write("</div>" & vbCrLf)
        End Sub
    End Class

    Public Class ModalForm
        Implements IDisposable
        Protected _helper As HtmlHelper

        Public Sub New(ByVal helper As HtmlHelper, ByVal Id As String, ByVal title As String, ByVal url As String, _
                        ByVal o_idTitle As String, o_method As String, ByVal o_idForm As String, ByVal o_classForm As String)
            _helper = helper
            Dim Writer = helper.ViewContext.Writer
            Writer.Write("<div class='modal fade' id='" & Id & "' tabindex='-1'>" & vbCrLf)
            Writer.Write("    <div class='modal-dialog'>" & vbCrLf)
            Writer.Write("        <div class='modal-content'>" & vbCrLf)
            Writer.Write("            <div class='modal-header'>" & vbCrLf)
            Writer.Write("                <button aria-hidden='true' class='close' data-dismiss='modal' type='button' tabindex='-1'>×</button>" & vbCrLf)
            Writer.Write("                <h4 class='modal-title' id='" & o_idTitle & "'> " & title & " </h4>" & vbCrLf)
            Writer.Write("            </div>" & vbCrLf)
            Writer.Write("            <form action='" & url & "' id='" & o_idForm & "' method='" & o_method & "' class='form " & o_classForm & "' autocomplete='false'>" & vbCrLf)
            Writer.Write("              <div class='modal-body'>" & vbCrLf)
        End Sub

        Public Sub Dispose() Implements IDisposable.Dispose
            Dim Writer = _helper.ViewContext.Writer
            Writer.Write("      </div>" & vbCrLf)
            Writer.Write("      <div class='modal-footer'>" & vbCrLf)
            Writer.Write("        <button class='btn btn-primary' type='submit' id='btn-submit'> Simpan </button>" & vbCrLf)
            Writer.Write("        <button class='btn btn-default' data-dismiss='modal' type='button'> Batal </button>" & vbCrLf)
            Writer.Write("      </div>" & vbCrLf)
            Writer.Write("    </form>" & vbCrLf)
            Writer.Write("  </div>" & vbCrLf)
            Writer.Write(" </div>" & vbCrLf)
            Writer.Write("</div>" & vbCrLf)
        End Sub
    End Class

    Public Class RowBox
        Implements IDisposable
        Protected _helper As HtmlHelper

        Public Sub New(ByVal helper As HtmlHelper, ByVal ClassName As String, ByVal disable_Scrollable As Boolean)
            _helper = helper
            Dim Writer = helper.ViewContext.Writer
            Writer.Write("<div class='" & ClassName & "'>" & vbCrLf)
            Writer.Write("  <div class='box'>" & vbCrLf)
            Writer.Write("    <div class='" + IIf(disable_Scrollable, "", "scrollable-area") + "'>" & vbCrLf)
            Writer.Write("      <div class='box-content'>" & vbCrLf)
        End Sub

        Public Sub Dispose() Implements IDisposable.Dispose
            Dim Writer = _helper.ViewContext.Writer
            Writer.Write("      </div>" & vbCrLf)
            Writer.Write("    </div>" & vbCrLf)
            Writer.Write("  </div>" & vbCrLf)
            Writer.Write("</div>" & vbCrLf)
        End Sub
    End Class

    Public Class WriteSummaryBox
        Implements IDisposable
        Protected _helper As HtmlHelper

        Public Sub New(ByVal helper As HtmlHelper, ByVal title As String,
                                        Optional ByVal icon As String = "",
                                        Optional ByVal idGoTo As String = "",
                                        Optional ByVal isScrollable As Boolean = True)
            _helper = helper
            Dim Writer = helper.ViewContext.Writer
            Writer.Write("<div class='sum-box box'>" & vbCrLf)
            Writer.Write("  <div class='box-title well'>" & vbCrLf)
            Writer.Write("      <a href='#' id='" & idGoTo & "'></a>" & vbCrLf)
            Writer.Write("      <h2><i class='" & icon & "'></i>  " & title & "</h2>" & vbCrLf)
            'Writer.Write(title & vbCrLf)
            Writer.Write("  </div>" & vbCrLf)
            Writer.Write("  <div class='box-content'>" & vbCrLf)
            Writer.Write("    <div class='" & IIf(isScrollable, "scrollable-area", "") & "'>" & vbCrLf)
        End Sub

        Public Sub Dispose() Implements IDisposable.Dispose
            ' Using Writer = _helper.ViewContext.Writer
            Dim Writer = _helper.ViewContext.Writer
            Writer.Write("    </div>" & vbCrLf)
            Writer.Write("  </div>" & vbCrLf)
            Writer.Write("</div>" & vbCrLf)
            'Writer.Flush()
            'End Using
        End Sub
    End Class
#End Region
End Namespace
