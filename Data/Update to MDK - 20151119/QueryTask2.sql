/****** Script for SelectTopNRows command from SSMS  ******/
SELECT *
  -- DELETE
  FROM [MDKSite1].[Imp].[TblTrans_Penimbangan2]
WHERE KodeBrg = 12


/****** Script for SelectTopNRows command from SSMS  *****
SELECT TOP 1000 *
  FROM [MDKSite1].[Prod].[MaterialInventories]
  --WHERE 
  --ID = 5553
   -- NoRecordWeigher = '15000037'
  ORDER BY ID DESC
  
  */
  
  /****** Script for SelectTopNRows command from SSMS  ******/
SELECT *
  FROM [MDKSite1].[Prod].[DistributionJournals]
  WHERE  
  ID > 5540
  --NoRec like '%0036'
  ORDER BY ID DESC
  
  
  --DELETE FROM [MDKSite1].[Prod].[DistributionJournals] WHERE ID > 5540