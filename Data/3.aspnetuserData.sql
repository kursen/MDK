USE [MDK]
GO
/****** Object:  Table [dbo].[aspnet_WebEvent_Events]    Script Date: 04/13/2015 15:14:11 ******/
/****** Object:  Table [dbo].[aspnet_SchemaVersions]    Script Date: 04/13/2015 15:14:11 ******/
INSERT [dbo].[aspnet_SchemaVersions] ([Feature], [CompatibleSchemaVersion], [IsCurrentVersion]) VALUES (N'common', N'1', 1)
INSERT [dbo].[aspnet_SchemaVersions] ([Feature], [CompatibleSchemaVersion], [IsCurrentVersion]) VALUES (N'health monitoring', N'1', 1)
INSERT [dbo].[aspnet_SchemaVersions] ([Feature], [CompatibleSchemaVersion], [IsCurrentVersion]) VALUES (N'membership', N'1', 1)
INSERT [dbo].[aspnet_SchemaVersions] ([Feature], [CompatibleSchemaVersion], [IsCurrentVersion]) VALUES (N'personalization', N'1', 1)
INSERT [dbo].[aspnet_SchemaVersions] ([Feature], [CompatibleSchemaVersion], [IsCurrentVersion]) VALUES (N'profile', N'1', 1)
INSERT [dbo].[aspnet_SchemaVersions] ([Feature], [CompatibleSchemaVersion], [IsCurrentVersion]) VALUES (N'role manager', N'1', 1)
/****** Object:  Table [dbo].[aspnet_Applications]    Script Date: 04/13/2015 15:14:11 ******/
INSERT [dbo].[aspnet_Applications] ([ApplicationName], [LoweredApplicationName], [ApplicationId], [Description]) VALUES (N'MDK_ERP', N'mdk_erp', N'27bf7a46-4754-4d44-99d9-50cc8313926b', NULL)
/****** Object:  Table [dbo].[aspnet_Paths]    Script Date: 04/13/2015 15:14:11 ******/
/****** Object:  Table [dbo].[aspnet_Users]    Script Date: 04/13/2015 15:14:11 ******/
INSERT [dbo].[aspnet_Users] ([ApplicationId], [UserId], [UserName], [LoweredUserName], [MobileAlias], [IsAnonymous], [LastActivityDate]) VALUES (N'27bf7a46-4754-4d44-99d9-50cc8313926b', N'a2eb303e-e1cd-45b4-a44b-15380789e1ea', N'admin', N'admin', NULL, 0, CAST(0x0000A4790035C5C7 AS DateTime))
INSERT [dbo].[aspnet_Users] ([ApplicationId], [UserId], [UserName], [LoweredUserName], [MobileAlias], [IsAnonymous], [LastActivityDate]) VALUES (N'27bf7a46-4754-4d44-99d9-50cc8313926b', N'e7894502-7a7e-4e98-a912-132ab2790dae', N'prd', N'prd', NULL, 0, CAST(0x0000A44E010AAFDB AS DateTime))
/****** Object:  Table [dbo].[aspnet_Roles]    Script Date: 04/13/2015 15:14:11 ******/
INSERT [dbo].[aspnet_Roles] ([ApplicationId], [RoleId], [RoleName], [LoweredRoleName], [Description]) VALUES (N'27bf7a46-4754-4d44-99d9-50cc8313926b', N'44fa00eb-78c8-4e8d-8867-f0328643b1d0', N'Administrator', N'administrator', NULL)
INSERT [dbo].[aspnet_Roles] ([ApplicationId], [RoleId], [RoleName], [LoweredRoleName], [Description]) VALUES (N'27bf7a46-4754-4d44-99d9-50cc8313926b', N'51ebbe91-2c0a-4248-96bb-36e54b99e0b6', N'Production', N'production', NULL)
/****** Object:  Table [dbo].[aspnet_UsersInRoles]    Script Date: 04/13/2015 15:14:11 ******/
INSERT [dbo].[aspnet_UsersInRoles] ([UserId], [RoleId]) VALUES (N'e7894502-7a7e-4e98-a912-132ab2790dae', N'51ebbe91-2c0a-4248-96bb-36e54b99e0b6')
INSERT [dbo].[aspnet_UsersInRoles] ([UserId], [RoleId]) VALUES (N'a2eb303e-e1cd-45b4-a44b-15380789e1ea', N'44fa00eb-78c8-4e8d-8867-f0328643b1d0')
/****** Object:  Table [dbo].[aspnet_Profile]    Script Date: 04/13/2015 15:14:11 ******/
/****** Object:  Table [dbo].[aspnet_PersonalizationPerUser]    Script Date: 04/13/2015 15:14:11 ******/
/****** Object:  Table [dbo].[aspnet_PersonalizationAllUsers]    Script Date: 04/13/2015 15:14:11 ******/
/****** Object:  Table [dbo].[aspnet_Membership]    Script Date: 04/13/2015 15:14:11 ******/
INSERT [dbo].[aspnet_Membership] ([ApplicationId], [UserId], [Password], [PasswordFormat], [PasswordSalt], [MobilePIN], [Email], [LoweredEmail], [PasswordQuestion], [PasswordAnswer], [IsApproved], [IsLockedOut], [CreateDate], [LastLoginDate], [LastPasswordChangedDate], [LastLockoutDate], [FailedPasswordAttemptCount], [FailedPasswordAttemptWindowStart], [FailedPasswordAnswerAttemptCount], [FailedPasswordAnswerAttemptWindowStart], [Comment]) VALUES (N'27bf7a46-4754-4d44-99d9-50cc8313926b', N'a2eb303e-e1cd-45b4-a44b-15380789e1ea', N'7hK7iLmB7ySnZb3GrQet3HGPLIs=', 1, N'Aj8z0mJYCwyf9xi/O8Vk3g==', NULL, N'admin@halotec.com', N'admin@halotec.com', NULL, NULL, 1, 0, CAST(0x0000A44E010227E4 AS DateTime), CAST(0x0000A4790035C5C7 AS DateTime), CAST(0x0000A44E010227E4 AS DateTime), CAST(0xFFFF2FB300000000 AS DateTime), 0, CAST(0xFFFF2FB300000000 AS DateTime), 0, CAST(0xFFFF2FB300000000 AS DateTime), NULL)
INSERT [dbo].[aspnet_Membership] ([ApplicationId], [UserId], [Password], [PasswordFormat], [PasswordSalt], [MobilePIN], [Email], [LoweredEmail], [PasswordQuestion], [PasswordAnswer], [IsApproved], [IsLockedOut], [CreateDate], [LastLoginDate], [LastPasswordChangedDate], [LastLockoutDate], [FailedPasswordAttemptCount], [FailedPasswordAttemptWindowStart], [FailedPasswordAnswerAttemptCount], [FailedPasswordAnswerAttemptWindowStart], [Comment]) VALUES (N'27bf7a46-4754-4d44-99d9-50cc8313926b', N'e7894502-7a7e-4e98-a912-132ab2790dae', N'6aK8Pw4SGbf40oPgd3GggSsnmXI=', 1, N'8FMRDxHBR5PlT00s5N2dzA==', NULL, N'shsh@jdjd.com', N'shsh@jdjd.com', NULL, NULL, 1, 0, CAST(0x0000A44E010AAF18 AS DateTime), CAST(0x0000A44E010AAFDB AS DateTime), CAST(0x0000A44E010AAF18 AS DateTime), CAST(0xFFFF2FB300000000 AS DateTime), 0, CAST(0xFFFF2FB300000000 AS DateTime), 0, CAST(0xFFFF2FB300000000 AS DateTime), NULL)
