Imports System.Runtime.CompilerServices

Namespace HtmlHelpers
    Public Module FormHelpers
        <Extension()>
        Public Function WriteFormInput(helper As HtmlHelper,
                                    InputControl As MvcHtmlString,
                                    caption As String,
                                    Optional smLabelWidth As Integer = 2,
                                    Optional smControlWidth As Integer = 10,
                                    Optional lgLabelWidth As Integer = 2,
                                    Optional lgControlWidth As Integer = 10) As MvcHtmlString

            Dim divFormGroup = New TagBuilder("div")
            divFormGroup.AddCssClass("form-group")
            Dim label = New TagBuilder("label")
            label.AddCssClass("control-label")
            label.AddCssClass("col-sm-" + smLabelWidth.ToString())
            label.AddCssClass("col-lg-" + lgLabelWidth.ToString())

            label.SetInnerText(caption)
            Dim divInput = New TagBuilder("div")
            divInput.AddCssClass("col-sm-" + smControlWidth.ToString())
            divInput.AddCssClass("col-lg-" + lgControlWidth.ToString())
            divInput.InnerHtml = InputControl.ToString() + vbLf
            divFormGroup.InnerHtml = label.ToString() + vbLf + divInput.ToString()
            Return New MvcHtmlString(vbLf + divFormGroup.ToString(TagRenderMode.Normal))
        End Function

        <Extension()>
        Public Function WriteStaticFormInput(helper As HtmlHelper, LabelCaption As String, Value As String,
                                    Optional smLabelWidth As Integer = 2,
                                    Optional smControlWidth As Integer = 10,
                                    Optional lgLabelWidth As Integer = 2,
                                    Optional lgControlWidth As Integer = 10) As MvcHtmlString

            Dim p = New TagBuilder("p")
            p.AddCssClass("form-control-static")
            p.SetInnerText(Value)

            Return WriteFormInput(helper, New MvcHtmlString(p.ToString(TagRenderMode.Normal)), LabelCaption, smLabelWidth,smControlWidth,lgLabelWidth,lgControlWidth)
        End Function

    End Module
End Namespace