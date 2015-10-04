﻿/*
WARNING - DO NOT RUN ON PRODUCTION SERVER!
*/
USE [MASTER]
GO

IF EXISTS (
		SELECT NAME
		FROM master.sys.databases
		WHERE NAME = N'INSUREME'
		)
	ALTER DATABASE [INSUREME]

SET SINGLE_USER
WITH

ROLLBACK IMMEDIATE
GO

IF EXISTS (
		SELECT NAME
		FROM master.sys.databases
		WHERE NAME = N'INSUREME'
		)
	DROP DATABASE INSUREME;
GO

CREATE DATABASE [INSUREME]
GO

USE [INSUREME]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE SCHEMA car
GO

CREATE SCHEMA meta
GO
