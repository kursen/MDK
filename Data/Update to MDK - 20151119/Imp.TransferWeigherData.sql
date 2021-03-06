USE [MDKSite1]
GO
/****** Object:  Trigger [Imp].[TransferWeigherData]    Script Date: 11/18/2015 13:49:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Abu Rifqi Hanif Al Muyassar (Rano Tino Pandu)
-- Create date: 14 Jumadil Akhir 1436H (4/4/2015)
-- Description:	Pindahkan data dari tabel Timbangan2 ke tabel Distribusi
-- =============================================
ALTER TRIGGER [Imp].[TransferWeigherData]
   ON  [Imp].[TblTrans_Penimbangan2]
   AFTER INSERT,UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	Declare @Netto float = (Select Berat1-Berat2 from inserted)
	Declare @NoRecord Varchar(10) = (Select NoRecord from inserted)
	Declare @KodeBrg Varchar(25) = (Select KodeBrg from inserted)
	--DECLARE @countNoRec int = (Select Count(NoRec) From [Prod].[DistributionJournals] WHERE NoRec = @NoRecord)
	
	IF @KodeBrg = 12
	BEGIN
		SET @KodeBrg = @KodeBrg - 1
	END
	
	
	IF Exists (Select NoRec From [Prod].[DistributionJournals] WHERE NoRec = @NoRecord)
	BEGIN
		DELETE FROM [Prod].[DistributionJournals] WHERE NoRec = @NoRecord
	END
	
  	Insert into [Prod].[DistributionJournals](
      [NoRec]
      ,[InTime]
      ,[OutTime]
      ,[PoliceLicense]
      ,[DriverName]
      ,[IdDeliveryStatus]
      ,[IdCompany]
      ,[IdMaterial]
      ,[Weight1]
      ,[Weight2]
      ,[Place]
      ,[Clerk1]
      ,[Clerk2]
      ,[Copy]
	)
  	SELECT 
	   tp.[NoRecord]
      ,tp.[TglMasuk]
      ,tp.[TglKeluar]
      ,tp.[NoPolisi]
      ,tp.[Sopir]
	  ,Case When(@Netto >= 0) then 1 else 2 end
      ,cl.ID
      ,mm.ID
      ,tp.[Berat1]
      ,tp.[Berat2]
      ,tp.[DeliveryNote]
      ,tp.[Clerk1]
      ,tp.[Clerk2]
      ,tp.[Copy]
	FROM inserted tp
	Inner Join Prod.CompanyLists cl on cl.Code =  tp.KodePeru
	Inner Join Prod.MstMaterials mm on mm.Code =  @KodeBrg--tp.KodeBrg
	--Where
	--	tp.NoRecord Not In (Select NoRec From [Prod].[DistributionJournals])
	
	
	IF Exists (Select NoRecordWeigher From [Prod].MaterialInventories WHERE NoRecordWeigher = @NoRecord)
	BEGIN
		DELETE FROM [Prod].MaterialInventories WHERE NoRecordWeigher = @NoRecord
	END
	
	--IF @countNoRec = 1
	--BEGIN
		Insert Into prod.MaterialInventories(
			IdInventoryStatus
			,IsPlus
			,NoRecordWeigher)
		Values(
			Case When(@Netto>=0) then 2 else 3 end
			,Case When(@Netto>=0) then 1 else 0 end
			,@NoRecord)
	--END
  END

