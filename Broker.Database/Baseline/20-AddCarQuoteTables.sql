USE [INSUREME]
GO

CREATE TABLE [car].[CarInsuranceQuoteRequest]
(
	CarQuoteId INT IDENTITY PRIMARY KEY,
	VehicleId INT NOT NULL,
	CountyId INT,
	NoClaimsDiscountYears INT DEFAULT(0),
	VehicleValue DECIMAL(18,3),
	DriverAge INT,
	EmailAddress NVARCHAR(64),
	Telephone NVARCHAR(32),
	UTCDateAdded DATETIME DEFAULT GetUtcDate()
)
GO

ALTER TABLE [car].[CarInsuranceQuoteRequest]
	WITH CHECK ADD CONSTRAINT [FK_CarInsuranceQuoteRequest_Counties] FOREIGN KEY(CountyId) REFERENCES [meta].[Counties]([CountyId])
GO

ALTER TABLE [car].[CarInsuranceQuoteRequest]
	WITH CHECK ADD CONSTRAINT [FK_CarInsuranceQuoteRequest_VehicleDetails] FOREIGN KEY(VehicleId) REFERENCES [car].[VehicleDetails]([VehicleId])
GO

CREATE TABLE [car].[CarInsuranceQuoteResponse]
(
	ResponseId INT IDENTITY PRIMARY KEY,
	CarQuoteId INT NOT NULL,	
	Insurer NVARCHAR(64),
	QuoteType NVARCHAR(32),
	QuoteValue DECIMAL(18,3),
	UTCDateAdded DATETIME DEFAULT GetUtcDate()
)
GO

ALTER TABLE [car].[CarInsuranceQuoteResponse]
	WITH CHECK ADD CONSTRAINT [FK_CarInsuranceQuoteResponse_CarInsuranceQuoteRequest] FOREIGN KEY(CarQuoteId) REFERENCES [car].[CarInsuranceQuoteRequest]([CarQuoteId])
GO