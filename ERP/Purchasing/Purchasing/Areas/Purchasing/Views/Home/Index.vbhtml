@Code
    ViewData("Title") = "Dashboard"
    Dim userprofile = ERPBase.ErpUserProfile.GetUserProfile()
    
    Dim prStaffOrManager = User.IsInRole("Purchasing.Staff") OrElse User.IsInRole("Purchasing.Manager")
    Dim prcEntities = New Purchasing.PurchasingEntities
    Dim UnitRequisition As IList(Of Purchasing.DepartmentPurchaseRequisition)
    If prStaffOrManager Then
        UnitRequisition = (From m In prcEntities.DepartmentPurchaseRequisitions
                              Where m.Archive = False).ToList()
    Else
        UnitRequisition = (From m In prcEntities.DepartmentPurchaseRequisitions
                          Where m.Archive = False AndAlso m.OfficeID = userprofile.WorkUnitId).ToList()
    End If
    

    Dim sumOfRequisition = (From m In UnitRequisition
                           Select If(m.DepartmentPRDetails.Count > 0, m.DepartmentPRDetails.Sum(Function(x) x.TotalEstPrice), 0))
    
   
    
    Dim UnitRequisitionOnApproval = UnitRequisition.Where(Function(m) m.DocState = 1)
    Dim UnitRequisitionApproved = UnitRequisition.Where(Function(m) m.DocState = 2)
    Dim sumOfDepRequisitionOnApproval = 0D
    If UnitRequisitionOnApproval.Count > 0 Then
        sumOfDepRequisitionOnApproval = (From m In UnitRequisitionOnApproval
    Select If(m.DepartmentPRDetails.Count > 0, m.DepartmentPRDetails.Sum(Function(x) x.TotalEstPrice), 0)).Sum()
    End If
    
    
    Dim sumOfDepRequisitionOnApproved = 0D
    If UnitRequisitionApproved.Count > 0 Then
        sumOfDepRequisitionOnApproved = (From m In UnitRequisitionApproved
    Select If(m.DepartmentPRDetails.Count > 0, m.DepartmentPRDetails.Sum(Function(x) x.TotalEstPrice), 0)).Sum()
    End If
    
    
    
    '    Dim ProjectRequisition = From m In prcEntities.ProjectPurchaseRequisitions
    '                             Where m.Archive = False
                               
  
                             
    
    
                             
    '    Dim ProjectRequisitionOnApproval = ProjectRequisition.Where(Function(m) m.DocState = 1)
    '    Dim ProjectRequisitionOnApproved = ProjectRequisition.Where(Function(m) m.DocState = 2)
    
 
    '    Dim sumOfProjRequisitionOnApproval = 0D
    '    If ProjectRequisitionOnApproval.Count > 0 Then
       
    '        sumOfProjRequisitionOnApproval = (From m In ProjectRequisitionOnApproval
    'Select If(m.ProjectPurchaseRequisitionDetails.Count > 0, m.ProjectPurchaseRequisitionDetails.Sum(Function(x) x.TotalEstPrice), 0)).Sum()
    
        
    '    End If
    
    
    
    '    Dim sumOfProjRequisitionOnApproved As Decimal = 0D
    '    If ProjectRequisitionOnApproved.Count > 0 Then
    '        sumOfProjRequisitionOnApproved = (From m In ProjectRequisitionOnApproved
    '            Select If(m.ProjectPurchaseRequisitionDetails.Count > 0, m.ProjectPurchaseRequisitionDetails.Sum(Function(x) x.TotalEstPrice), 0)).Sum()
    
    '    End If
 
    
    Dim PO = From m In prcEntities.PurchaseOrders
             Where m.Archive = False
     
    Dim sumOfPo As Decimal = 0D
    If PO.Count > 0 Then
        sumOfPo = (From m In PO
             Select m.PurchaseOrderDetails.Sum(Function(x) x.TotalPrice)).Sum()
    End If
    
    
    Dim RequestByType = From m In UnitRequisition
                        Group m By RequestName = m.RequestType.Name Into g = Group, Count(), Sum(m.DepartmentPRDetails.Sum(Function(x) x.TotalEstPrice))
                        
                        
                        
    
End Code
<div class="row">
    <div class="col-lg-12 col-sm-12">
        <div class="panel panel-primary">
            <div class="panel-body">
                <ul class="list-group">
                    <li class="list-group-item"><span class="badge bg-danger">@(PO.Count)
                    </span><a href='/Purchasing/PurchaseOrder/Index'>Jumlah Outstanding PO</a></li>
                    <li class="list-group-item"><span class="badge bg-danger">@(sumOfPo.ToString("N2"))
                    </span>Nilai Outstanding PO</li>
                </ul>
            </div>
        </div>
    </div>
</div>
<div class="row">
    <div class="col-sm-12 col-lg-6">
        <div class="panel panel-primary">
            <div class="panel-heading">
                Permintaan
            </div>
            <div class="panel-body">
                <ul class="list-group">
                    <li class="list-group-item"><span class="badge bg-danger">@(UnitRequisitionOnApproval.Count)
                    </span>Permintaan dalam pengajuan </li>
                    <li class="list-group-item"><span class="badge bg-danger">Rp. @(sumOfDepRequisitionOnApproval.ToString("N2"))
                    </span>Nilai Permintaan yang diajukan</li>
                    <li class="list-group-item"><span class="badge bg-primary">@(UnitRequisitionApproved.Count)
                    </span>Permintaan yang sudah disetujui </li>
                    <li class="list-group-item"><span class="badge bg-primary">Rp. @(sumOfDepRequisitionOnApproved.ToString("N2"))
                    </span>Nilai Permintaan Yang disetujui</li>
                </ul>
            </div>
        </div>
    </div>
    <div class="col-sm-12 col-lg-6">
        <div class="panel panel-primary">
            <div class="panel-heading">
                Jenis Permintaan
            </div>
            <div class="panel-body">
                <ul class="list-group">
                @For Each item In RequestByType
                    @<li class="list-group-item"><span class="badge bg-primary">@item.Count 
                    </span>@item.RequestName 
                    </li>
                Next
                    
                  
                </ul>
            </div>
        </div>
    </div>
</div>
