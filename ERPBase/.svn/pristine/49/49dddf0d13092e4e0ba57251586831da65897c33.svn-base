@ModelType TestModel
@Code
    ViewData("Title") = "Playground"
   ' Html.SetEditableDefaultSettings(False, "0", dataurl:="/SavePartial", formPlacement:="left")
    
    
End Code
@Html.WritePageTitle("This is playground",String.Empty)
@If Request.HttpMethod = "POST" Then
    Using Html.BeginJUIBox("Post Result")
    @<dl class="dl-horizontal">
        <dt>MyString</dt>
        <dd>@Model.MyString ( @Html.ValidationMessageFor(Function(m) m.MyString))&nbsp;</dd>
        <dt>MyInteger</dt>
        <dd>@Model.MyInteger ( @Html.ValidationMessageFor(Function(m) m.MyInteger))&nbsp;</dd>
        <dt>MyLong</dt>
        <dd>@Model.MyLong ( @Html.ValidationMessageFor(Function(m) m.MyLong))&nbsp;</dd>
        <dt>MyDate</dt>
        <dd>@Model.MyDate ( @Html.ValidationMessageFor(Function(m) m.MyDate))&nbsp;</dd>
        <dt>MyDecimal</dt>
        <dd>@Model.MyDecimal ( @Html.ValidationMessageFor(Function(m) m.MyDecimal))&nbsp;</dd>
        <dt>MyDouble</dt>
        <dd>@Model.MyDouble ( @Html.ValidationMessageFor(Function(m) m.MyDouble))&nbsp;</dd>
    </dl>
    @Html.ValidationMessageFor(Function(m) m.MyDecimal)
    End Using
End If
@Using Html.BeginJUIBox("This is the form", isMoveable:=True, isCollapsible:=True, isRemovable:=True)
    Using Html.BeginForm("TestSaveData", "Playground", FormMethod.Post, New With {.class = "form-horizontal", .autocomplete = "off"})
  

    @Html.WriteFormInput(Html.TextBoxFor(Function(m) m.MyString, New With {.class = "form-control"}), "MyString", 4, 8, 2, 6)

    @Html.WriteFormInput(Html.IntegerInputFor(Function(m) m.MyInteger), "MyInteger", 4, 8, 2, 6)
    @Html.WriteFormInput(Html.IntegerInputFor(Function(m) m.MyLong), "MyLong", 4, 8, 2, 6)
    @Html.WriteFormInput(Html.DateInputFor(Function(m) m.MyDate, New With {.content = "#content", .format = "dd-mm-yyyy"}), "Tanggal lahir", 4, 4, 2, 2)
    @Html.WriteFormDecimalInputFor(Function(m) m.MyDecimal, "MyDecimal")
    @Html.WriteFormInput(Html.TextBoxFor(Function(m) m.MyDouble, New With {.class = "form-control"}), "MyDouble")

    
    @<div class="well">
        <div class="col-sm-offset-4">
            <button type="submit" class="btn btn-primary btn-label-left">
                <span><i class="fa fa-clock-o"></i></span>Submit
            </button>
        </div>
    </div>
    
    End Using

End Using


<div class="row">
    <div class="col-sm-12 col-lg-12">
        @Using Html.BeginJUIBox("Table goes here")
    
            Using Html.BeginForm("test", "test", FormMethod.Post, New With {.autocomplete = "Off", .class = "form-horizontal"})
                
                @Html.EditableInputTextBox("test", "test", "text", "ini title",datapk:=1,dataurl:="/savedata")
            
            End Using
          
        
        
        End Using
    </div>
</div>

<script type="text/javascript">

    _savepartialresponse = function (data) {
        if (data.stat == 1) {
        } else {
            alert(data);
        }
    }
</script>

@Section  endscript
<script type="text/javascript">
    $.fn.editable.defaults.placement = 'right';
</script>


End Section
