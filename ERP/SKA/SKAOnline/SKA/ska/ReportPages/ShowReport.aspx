<%@ Page Language="C#" AutoEventWireup="true" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<script runat="server">
    protected void Page_Load(object sender, EventArgs e)
  {
        if (!IsPostBack)
        {
            ReportViewerControl.ProcessingMode = ProcessingMode.Remote;
            ServerReport serverReport = ReportViewerControl.ServerReport;

            string reportPath = System.Configuration.ConfigurationManager.AppSettings["ReportPath"];
            string reportServerUrl = System.Configuration.ConfigurationManager.AppSettings["ReportServerUrl"];
            string userName = System.Configuration.ConfigurationManager.AppSettings["ReportCredentialUserName"];
            string password = System.Configuration.ConfigurationManager.AppSettings["ReportCredentialPassword"];
            string domain = System.Configuration.ConfigurationManager.AppSettings["ReportCredentialDomain"];
            
            serverReport.ReportPath = reportPath + Request.QueryString["name"];
            serverReport.ReportServerUrl = new Uri(reportServerUrl);

            SKA.Helpers.ReportCredential credential = new SKA.Helpers.ReportCredential(userName, password, domain);
            
            credential.ReportServerUrl = serverReport.ReportServerUrl;
            serverReport.ReportServerCredentials = credential;

            //ReportParameter[] reportParameterCollection = new ReportParameter[1];
            //reportParameterCollection[0] = new Microsoft.Reporting.WebForms.ReportParameter();
            //reportParameterCollection[0].Name = "UserAccount";
            //reportParameterCollection[0].Values.Add(User.Identity.Name);
            //reportParameterCollection[0].Visible = false;

            //serverReport.SetParameters(reportParameterCollection);

            if (!string.IsNullOrEmpty(Request.QueryString["parametername"]) && !string.IsNullOrEmpty(Request.QueryString["parametervalue"]))
            {
                // Set the parameter
                // .parametername = 'kodecabang',.parametername = 'kodecabang'

                string parametername = Request.QueryString["parametername"];
                string[] listParamNames = parametername.Split(',');

                string parametervalue = Request.QueryString["parametervalue"];
                string[] listParamValues = parametervalue.Split(',');

                ReportParameter[] reportParameterCollection = new ReportParameter[listParamNames.Length];
                for (int i = 0; i < listParamNames.Length; i++)
                {
                    reportParameterCollection[i] = new Microsoft.Reporting.WebForms.ReportParameter();
                    reportParameterCollection[i].Name = listParamNames[i];
                    if (listParamValues[i].Contains("|"))
                    {
                        string itemvalue = listParamValues[i].ToString().Replace("|", ",");
                        reportParameterCollection[i].Values.Add(itemvalue);
                    }
                    else
                    {
                        reportParameterCollection[i].Values.Add(listParamValues[i]);
                    }
                    reportParameterCollection[i].Visible = false;
                }

                serverReport.SetParameters(reportParameterCollection);
            }
            
            serverReport.Refresh();
        }
    }
</script>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="scriptmn" runat="server"></asp:ScriptManager>
    <div>
        <rsweb:ReportViewer id="ReportViewerControl" runat="server" width="100%" height="650px" BorderStyle="None" InternalBorderStyle="Solid" ProcessingMode="Remote" >
        </rsweb:ReportViewer>
    </div>
    </form>
</body>
</html>
