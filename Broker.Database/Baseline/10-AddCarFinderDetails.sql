USE [INSUREME]
GO

CREATE TABLE [car].[VehicleDetails]
(
	VehicleId INT IDENTITY PRIMARY KEY,
	ModelName NVARCHAR(64),
	ModelDesc NVARCHAR(256),
	ManufYear INT,
	CurrentRegistration NVARCHAR(16),
	Colour NVARCHAR(64),	
	BodyType NVARCHAR(64),
	FuelType NVARCHAR(64),
	Transmission NVARCHAR(64),
	IsImport BIT DEFAULT (0),
	UTCDateAdded DATETIME DEFAULT (GETUTCDATE())
)
GO
