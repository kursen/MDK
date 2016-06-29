USE MDKSite1
--clean data for Imp
  TRUNCATE TABLE Imp.tblMisc_History
  TRUNCATE TABLE Imp.TblMisc_logo
  TRUNCATE TABLE Imp.TblMst_Barang
  TRUNCATE TABLE Imp.TblMst_Customer
  TRUNCATE TABLE Imp.TblMst_Login
  TRUNCATE TABLE Imp.TblMst_NoRec
  TRUNCATE TABLE Imp.TblMst_Perusahaan
  TRUNCATE TABLE Imp.TblTrans_Penimbangan1
  TRUNCATE TABLE Imp.TblTrans_Penimbangan2
  
--clean data Prod
  TRUNCATE TABLE Prod.DistributionJournals
  TRUNCATE TABLE Prod.CrusherJournalDetails
  TRUNCATE TABLE Prod.AMPJournals
  TRUNCATE TABLE Prod.MaterialInitialStock
  DELETE FROM Prod.ProjectRoadList
	DBCC CHECKIDENT ("Prod.ProjectRoadList", RESEED, 0);
  DELETE FROM Prod.CrusherJournals
	DBCC CHECKIDENT ("Prod.CrusherJournals", RESEED, 0);
  DELETE FROM Prod.ProjectList
	DBCC CHECKIDENT ("Prod.ProjectList", RESEED, 0);
  DELETE FROM Prod.CompanyLists
	DBCC CHECKIDENT ("Prod.CompanyLists", RESEED, 0);
  DELETE FROM Prod.MaterialInventories
	DBCC CHECKIDENT ("Prod.MaterialInventories", RESEED, 0);
  DELETE FROM Prod.MaterialUseJournal
    DBCC CHECKIDENT ("Prod.MaterialUseJournal", RESEED, 0);
	
--clean Prod.Mst
 -- TRUNCATE TABLE Prod.MstWeighers
  DELETE FROM Prod.ProjectList
	DBCC CHECKIDENT ("Prod.ProjectList", RESEED, 0);
 -- DELETE FROM Prod.MstInventoryStatuses
	--DBCC CHECKIDENT ("Prod.MstInventoryStatuses", RESEED, 0);
 -- DELETE FROM Prod.MstDeliveryStatuses
	--DBCC CHECKIDENT ("Prod.MstDeliveryStatuses", RESEED, 0);
  DELETE FROM Prod.MstMaterialCompositions
	DBCC CHECKIDENT ("Prod.MstMaterialCompositions", RESEED, 0);
 -- DELETE FROM Prod.MstWorkSchedule
	--DBCC CHECKIDENT ("Prod.MstWorkSchedule", RESEED, 0);
 -- DELETE FROM Prod.MstMeasurementUnits
	--DBCC CHECKIDENT ("Prod.MstMeasurementUnits", RESEED, 0);
  DELETE FROM Prod.MstMaterialRatioUnits
	DBCC CHECKIDENT ("Prod.MstMaterialRatioUnits", RESEED, 0);
  DELETE FROM Prod.MstMaterials
	DBCC CHECKIDENT ("Prod.MstMaterials", RESEED, 0);
 -- DELETE FROM Prod.MstMaterialTypes
 -- DELETE FROM Prod.MstMachines
	--DBCC CHECKIDENT ("Prod.MstMachines", RESEED, 0);
 -- DELETE FROM Prod.MstMachineTypes
	--DBCC CHECKIDENT ("Prod.MstMachineTypes", RESEED, 0);
	
	