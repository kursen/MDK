USE [MDKSite1]
GO
/****** Object:  Trigger [Imp].[TransferMaterialData]    Script Date: 11/18/2015 13:28:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Abu Rifqi Hanif Al Muyassar (Rano Tino Pandu)
-- Create date: 22 Jumadil Ula 1436 H (13-3-2015)
-- Description:	Transfer data barang ke tabel material
-- =============================================
ALTER TRIGGER [Imp].[TransferMaterialData]
   ON  [Imp].[TblMst_Barang]
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	----Untuk Hapus
	--Delete [Prod].[MstMaterials] 
	--	From [Prod].[MstMaterials] pm, deleted d 
	--	Where pm.Code = d.KodeBrg
	
	--Untuk Update
	Update 
		[Prod].[MstMaterials]
		Set
			Code		= t.KodeBrg
			, Symbol	= t.NoBrg
			, Name		= t.NamaBrg
		From
			[Prod].[MstMaterials] pmm
			, inserted t
		Where
			pmm.Code	= t.KodeBrg
	--Untuk Insert
	Insert Into 
		[Prod].[MstMaterials]
		(
			Code
			, Symbol
			, Name
		)
		Select 
			KodeBrg
			, NoBrg
			, NamaBrg
		from 
			inserted t
		Where
			t.KodeBrg Not In (Select Code From [Prod].[MstMaterials])
END
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




/****** Object:  Trigger [Imp].[TransferMaterialData]    Script Date: 11/18/2015 11:59:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Abu Rifqi Hanif Al Muyassar (Rano Tino Pandu)
-- Create date: 22 Jumadil Ula 1436 H (13-3-2015)
-- Description:	Transfer data barang ke tabel material
-- =============================================
CREATE TRIGGER [Prod].[ReSyncMaterialData]
   ON Prod.MstMaterials
   AFTER DELETE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	--Untuk Hapus
    Delete Imp.TblMst_Barang
		From Imp.TblMst_Barang d, deleted  pm
		Where pm.Code = d.KodeBrg
		
	----Untuk Insert
	--Insert Into 
	--	Imp.TblMst_Barang
	--	(
	--		KodeBrg
	--		, NoBrg
	--		, NamaBrg
	--	)
	--	Select 
	--		Code
	--		, Symbol
	--		, Name
	--	from 
	--		inserted t
	--	Where
	--		t.Code Not In (Select Code From Imp.TblMst_Barang)
END
GO

SELECT *
  FROM [Imp].[TblTrans_Penimbangan2]
WHERE KodeBrg = 12
GO

/*
DELETE
  FROM [Imp].[TblTrans_Penimbangan2]
WHERE KodeBrg = 12

GO

SELECT *
  FROM [Imp].[TblTrans_Penimbangan2]
WHERE KodeBrg = 12
GO
*/

