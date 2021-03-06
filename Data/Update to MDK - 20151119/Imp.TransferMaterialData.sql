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
