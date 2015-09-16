USE [INSUREME]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

IF NOT EXISTS (
		SELECT *
		FROM sys.columns
		WHERE NAME = N'HouseTypeId'
			AND Object_ID = Object_ID(N'home.HomeInsuranceQuoteRequest')
		)
BEGIN
	ALTER TABLE [home].[HomeInsuranceQuoteRequest] 
			ADD [HouseTypeId] [INT]

	ALTER TABLE [home].[HomeInsuranceQuoteRequest] 
	WITH CHECK ADD CONSTRAINT [FK_HomeInsuranceQuoteRequest_HouseTypes] FOREIGN KEY(HouseTypeId) REFERENCES [meta].[HouseTypes]([HouseTypeId])

END
GO