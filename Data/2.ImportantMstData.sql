USE [MDK]
GO
/****** Object:  Table [Prod].[MstWorkSchedule]    Script Date: 04/09/2015 14:00:45 ******/
/****** Object:  Table [Prod].[MstMeasurementUnits]    Script Date: 04/09/2015 14:00:45 ******/
SET IDENTITY_INSERT [Prod].[MstMeasurementUnits] ON
INSERT [Prod].[MstMeasurementUnits] ([ID], [Symbol], [Unit]) VALUES (1, N'Kg', N'Kilogram')
INSERT [Prod].[MstMeasurementUnits] ([ID], [Symbol], [Unit]) VALUES (2, N'Ton', N'Ton')
INSERT [Prod].[MstMeasurementUnits] ([ID], [Symbol], [Unit]) VALUES (3, N'Bucket', N'Bucket')
SET IDENTITY_INSERT [Prod].[MstMeasurementUnits] OFF
/****** Object:  Table [Prod].[MstMaterialTypes]    Script Date: 04/09/2015 14:00:45 ******/
/****** Object:  Table [Prod].[MstMachineTypes]    Script Date: 04/09/2015 14:00:45 ******/
SET IDENTITY_INSERT [Prod].[MstMachineTypes] ON
INSERT [Prod].[MstMachineTypes] ([ID], [MachineType], [Description]) VALUES (0, N'-', N'untuk material yang tidak dihasilkan dar mesin apapun')
INSERT [Prod].[MstMachineTypes] ([ID], [MachineType], [Description]) VALUES (1, N'Crusher', N'Mesin penggiling bahan dasar yang mengasilkan material dalam proses')
INSERT [Prod].[MstMachineTypes] ([ID], [MachineType], [Description]) VALUES (2, N'AMP', N'Mesin yang menghasilkan material produk atau Hotmix')
INSERT [Prod].[MstMachineTypes] ([ID], [MachineType], [Description]) VALUES (3, N'Loader', N'Mesin pengangkut material bahan masukan dan keluaran dari mesin crusher dan AMP')
SET IDENTITY_INSERT [Prod].[MstMachineTypes] OFF
/****** Object:  Table [Prod].[MstInventoryStatuses]    Script Date: 04/09/2015 14:00:45 ******/
SET IDENTITY_INSERT [Prod].[MstInventoryStatuses] ON
INSERT [Prod].[MstInventoryStatuses] ([ID], [StatusName]) VALUES (1, N'Stok Awal')
INSERT [Prod].[MstInventoryStatuses] ([ID], [StatusName]) VALUES (2, N'Pembelian')
INSERT [Prod].[MstInventoryStatuses] ([ID], [StatusName]) VALUES (3, N'Penjualan')
INSERT [Prod].[MstInventoryStatuses] ([ID], [StatusName]) VALUES (4, N'Pemakaian')
INSERT [Prod].[MstInventoryStatuses] ([ID], [StatusName]) VALUES (5, N'Hasil Produksi')
SET IDENTITY_INSERT [Prod].[MstInventoryStatuses] OFF
/****** Object:  Table [Prod].[MstDeliveryStatuses]    Script Date: 04/09/2015 14:00:45 ******/
SET IDENTITY_INSERT [Prod].[MstDeliveryStatuses] ON
INSERT [Prod].[MstDeliveryStatuses] ([ID], [Status]) VALUES (1, N'Masuk')
INSERT [Prod].[MstDeliveryStatuses] ([ID], [Status]) VALUES (2, N'Keluar')
SET IDENTITY_INSERT [Prod].[MstDeliveryStatuses] OFF
/****** Object:  Table [Prod].[MstMaterialTypes]    Script Date: 04/09/2015 15:01:52 ******/
INSERT [Prod].[MstMaterialTypes] ([ID], [Type]) VALUES (1, N'Raw')
INSERT [Prod].[MstMaterialTypes] ([ID], [Type]) VALUES (2, N'dalam Proses')
INSERT [Prod].[MstMaterialTypes] ([ID], [Type]) VALUES (3, N'Produk')
INSERT [Prod].[MstMaterialTypes] ([ID], [Type]) VALUES (4, N'Pembantu')
/****** Object:  Table [Prod].[MstWorkSchedule]    Script Date: 04/20/2015 13:08:37 ******/
SET IDENTITY_INSERT [Prod].[MstWorkSchedule] ON
INSERT [Prod].[MstWorkSchedule] ([ID], [Shift]) VALUES (1, N'Shift Pagi')
INSERT [Prod].[MstWorkSchedule] ([ID], [Shift]) VALUES (2, N'Shift Malam')
SET IDENTITY_INSERT [Prod].[MstWorkSchedule] OFF

