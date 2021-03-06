USE [MDKSite1]
GO
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
