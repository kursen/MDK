#Useage:  .\GenerateScript.ps1 "SERVERNAME" "DATABASE" "C:\OutputPath\"


###Start file
Set-ExecutionPolicy RemoteSigned
#Set-ExecutionPolicy -ExecutionPolicy:Unrestricted -Scope:LocalMachine
function GenerateDBScript([string]$serverName, [string]$dbname, [string]$scriptpath)
{
[System.Reflection.Assembly]::LoadWithPartialName("Microsoft.SqlServer.SMO") | out-null
[System.Reflection.Assembly]::LoadWithPartialName("System.Data") | out-null
#$error.clear()
#$erroractionpreference = "Continue"
$srv = new-object "Microsoft.SqlServer.Management.SMO.Server" $serverName
$srv.SetDefaultInitFields([Microsoft.SqlServer.Management.SMO.View], "IsSystemObject")
$db = New-Object "Microsoft.SqlServer.Management.SMO.Database"
$db = $srv.Databases[$dbname]
$scr = New-Object "Microsoft.SqlServer.Management.Smo.Scripter"
$deptype = New-Object "Microsoft.SqlServer.Management.Smo.DependencyType"
$scr.Server = $srv
$options = New-Object "Microsoft.SqlServer.Management.SMO.ScriptingOptions"
$options.AllowSystemObjects = $true
$options.IncludeDatabaseContext = $false
$options.IncludeIfNotExists = $false
$options.ClusteredIndexes = $true
$options.Default = $true
$options.DriAll = $true
$options.Indexes = $true
$options.NonClusteredIndexes = $true
$options.IncludeHeaders = $false
$options.ToFileOnly = $true
$options.AppendToFile = $false
$options.ScriptDrops = $false 
$options.ExtendedProperties = $true

#Set options for SMO.Scripter
$scr.Options = $options


"Creating tables...."
#Tables
Foreach ($tb in $db.Tables)
{
   If ($tb.IsSystemObject -eq $FALSE)
   {
        if ($tb.Name -NotLike "*aspnet_*")
        {
    	  $smoObjects = New-Object Microsoft.SqlServer.Management.Smo.UrnCollection
          $smoObjects.Add($tb.Urn)
    	  $options.FileName = $scriptpath +"\Tables_" + $tb.Schema + "."+ $tb.Name + ".sql"
    	  $scr.Script($smoObjects)
        }
   }
}
"Done"
"Creating views...."

#Views
$views = $db.Views | where {$_.IsSystemObject -eq $false}
Foreach ($view in $views)
{
	if ($views -ne $null)
	{
        if ($view.Name -NotLike "*aspnet_*")
        {
    	       $options.FileName = $scriptpath +"\Views_" + $view.Schema +"." + $view.Name + ".sql"
	           $scr.Script($view)
         }
	}
}
"Done"
"Creating StoredProcedures...."

#StoredProcedures
$StoredProcedures = $db.StoredProcedures | where {$_.IsSystemObject -eq $false}
Foreach ($StoredProcedure in $StoredProcedures)
{
	if ($StoredProcedures -ne $null)
	{
        if ($StoredProcedures.Name -NotLike "*aspnet_*")
        {
	       $options.FileName = $scriptpath +"\Procedures_" + $StoredProcedure.Schema +"."+$StoredProcedure.Name + ".sql"
	       $scr.Script($StoredProcedure)
         }
	}
}	

"Done"
"Creating Functions...."

#Functions
$UserDefinedFunctions = $db.UserDefinedFunctions | where {$_.IsSystemObject -eq $false}
Foreach ($function in $UserDefinedFunctions)
{
	if ($UserDefinedFunctions -ne $null)
	{
	 $options.FileName = $scriptpath +"\Functions_"+ $function.Schema + "." + $function.Name + ".sql"
	 $scr.Script($function)
	}
}	
"Done"
"Creating DBTriggers...."

#DBTriggers
$DBTriggers = $db.Triggers
foreach ($trigger in $db.triggers)
{
	if ($DBTriggers -ne $null)
	{
		$options.FileName = $scriptpath +"\TriggersDB_"+ $function.Schema + "." + $function.Name + ".sql"
		$scr.Script($DBTriggers)
	}
}

"Done"
"Creating triggers...."

Foreach ($tb in $db.Tables)
{			
	if($tb.triggers -ne $null)
	{
		foreach ($trigger in $tb.triggers)
		{
		$options.FileName = $scriptpath +"\Triggers_"+ $trigger.Name + ".sql"
		$scr.Script($trigger)
		}
	}
} 


}
#GenerateDBScript $args[0] $args[1] $args[2]
"Generate Script for LOCAL Database"
GenerateDBScript  ".\sqlserver2008r2" "MDK" "D:\database\compare\local"
"Generate Script for halotecserver Database"
GenerateDBScript  ".\sqlserver2008r2" "MDK_PRODUCTION" "D:\database\compare\production"
"ALL DONE, HUMAN"	