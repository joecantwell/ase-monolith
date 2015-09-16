USE [INSUREME]
GO

CREATE TABLE [car].[CarInsuranceQuoteRequest]
(
	CarQuoteId INT IDENTITY PRIMARY KEY,
	CarRegistration NVARCHAR(12),
	CarMake NVARCHAR(64),
	CarModel NVARCHAR(64),
	CarDesc NVARCHAR(64),
	CarYear INT,
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