USE [INSUREME]
GO

CREATE TABLE [meta].[HouseTypes]
(
	HouseTypeId INT IDENTITY PRIMARY KEY,
	TypeDescription NVARCHAR(128),
	UTCDateAdded DATETIME DEFAULT GetUtcDate()
)
GO

CREATE TABLE [meta].[InsuranceTypes]
(
	InsuranceTypeId INT IDENTITY PRIMARY KEY,
	InsuranceType NVARCHAR(10),
	InsuranceDesc NVARCHAR(64),
	UTCDateAdded DATETIME DEFAULT GetUtcDate()
)
GO





CREATE TABLE [meta].[Counties]
(
	CountyId INT IDENTITY PRIMARY KEY,
	CountyName NVARCHAR(64),
	UTCDateAdded DATETIME DEFAULT GetUtcDate()
)
GO

CREATE TABLE [meta].[Towns]
(
	TownId INT IDENTITY PRIMARY KEY,
	TownName NVARCHAR(64),
	CountyId INT NOT NULL,
	UTCDateAdded DATETIME DEFAULT GetUtcDate()
)
GO



CREATE TABLE [home].[HomeInsuranceQuoteRequest]
(
	HomeQuoteId INT IDENTITY PRIMARY KEY,
	InsuranceTypeId INT NOT NULL,
	IsSublet BIT DEFAULT(0),
	CountyId INT NOT NULL,
	TownId INT NOT NULL,
	YearBuilt INT,
	NumBedrooms INT,
	BuildingCover DECIMAL(18,3),
	ContentsCover DECIMAL(18,3),
	EmailAddress NVARCHAR(64),
	Telephone NVARCHAR(32),
	DateOfBirth DATETIME,
	UTCDateAdded DATETIME DEFAULT GetUtcDate()
)
GO

ALTER TABLE [home].[HomeInsuranceQuoteRequest]
	WITH CHECK ADD CONSTRAINT [FK_HomeInsuranceQuoteRequest_InsuranceTypes] FOREIGN KEY(InsuranceTypeId) REFERENCES [meta].[InsuranceTypes]([InsuranceTypeId])
GO

ALTER TABLE [home].[HomeInsuranceQuoteRequest]
	WITH CHECK ADD CONSTRAINT [FK_HomeInsuranceQuoteRequest_Counties] FOREIGN KEY(CountyId) REFERENCES [meta].[Counties]([CountyId])
GO

ALTER TABLE [home].[HomeInsuranceQuoteRequest]
	WITH CHECK ADD CONSTRAINT [FK_HomeInsuranceQuoteRequest_Towns] FOREIGN KEY(TownId) REFERENCES [meta].[Towns]([TownId])
GO